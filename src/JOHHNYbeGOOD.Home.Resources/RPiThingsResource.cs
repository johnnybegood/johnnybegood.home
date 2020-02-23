using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Threading;
using JOHHNYbeGOOD.Home.Exceptions;
using JOHHNYbeGOOD.Home.Resources.Connectors;
using JOHHNYbeGOOD.Home.Resources.Devices;
using JOHHNYbeGOOD.Home.Resources.Entities;
using JOHNNYbeGOOD.Home.Model.Devices;
using JOHNNYbeGOOD.Home.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JOHHNYbeGOOD.Home.Resources
{
    /// <summary>
    /// <see cref="IThingsResource"/> implementation for the Raspberry PI (ARM)
    /// </summary>
    public class RPiThingsResource : IThingsResource
    {
        private readonly ILogger<RPiThingsResource> _logger;
        private readonly ThingsOptions _options;
        private readonly IRpiConnectionFactory _factory;
        private readonly Lazy<GpioController> _gpioController;
        private readonly ConcurrentDictionary<string, IDevice> _activeDevices;

        /// <summary>
        /// Default construcstor for <see cref="RPiThingsResource"/>
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public RPiThingsResource(IOptionsSnapshot<ThingsOptions> options, ILogger<RPiThingsResource> logger, IRpiConnectionFactory rpiConnectionFactory)
        {
            _logger = logger;
            _options = options.Value;
            _factory = rpiConnectionFactory;
            _activeDevices = new ConcurrentDictionary<string, IDevice>();
            _gpioController = new Lazy<GpioController>(() => new GpioController(PinNumberingScheme.Logical), LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <inheritdoc />
        public TDevice GetDevice<TDevice>(string id) where TDevice : IDevice
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id is required", nameof(id));
            }

            _logger.LogDebug("Retrieving device {id}", id);

            var device = _activeDevices.GetOrAdd(id, id => ConnectToDevice<TDevice>(id));

            if (device is TDevice typedDevice)
            {
                return typedDevice;
            }

            throw new UnkownDeviceException<TDevice>(id);
        }

        /// <inheritdoc />
        public IReadOnlyCollection<T> GetDevices<T>() where T : IDevice
        {
            return _activeDevices
                .Where(d => d.GetType().GenericTypeArguments.Contains(typeof(T)))
                .Select(d => d.Value)
                .OfType<T>()
                .ToArray();
        }

        /// <summary>
        /// Connect to named device based on Func in the configured options of the default constructor
        /// </summary>
        /// <typeparam name="TDevice">Type of device</typeparam>
        /// <param name="name">Name of the device</param>
        /// <returns></returns>
        private TDevice ConnectToDevice<TDevice>(string name) where TDevice : IDevice
        {
            TDevice device;

            if (_options.Things.TryGetValue(name, out ThingOptions options))
            {
                var deviceFromOptions = options.Device.Invoke();

                if (deviceFromOptions is TDevice typedDeviceFromOptions)
                {
                    device = typedDeviceFromOptions;
                }
                else
                {
                    throw new UnkownDeviceException<TDevice>(name);
                }
            }
            else
            {
                device = default;
            }

            if (device is IRpiDevice rpiDevice)
            {
                rpiDevice.Connect(_factory);
            }

            return device;
        }
    }
}

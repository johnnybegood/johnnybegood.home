using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JOHHNYbeGOOD.Home.Exceptions;
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
        private static ConcurrentDictionary<string, IDevice> _devices = new ConcurrentDictionary<string, IDevice>();

        /// <summary>
        /// Default construcstor for <see cref="RPiThingsResource"/>
        /// </summary>
        /// <param name="logger"></param>
        public RPiThingsResource(IOptionsSnapshot<ThingsOptions> options, ILogger<RPiThingsResource> logger)
        {
            _logger = logger;
            _options = options.Value;
        }

        /// <inheritdoc />
        public T GetDevice<T>(string id) where T : IDevice
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id is required", nameof(id));
            }

            _logger.LogDebug("Retrieving device {id}", id);

            var deviceOptions = _options.Things.SingleOrDefault(t => id.Equals(t.Id, StringComparison.InvariantCultureIgnoreCase));

            if (deviceOptions?.Device is T tDevice)
            {
                return tDevice;
            }

            throw new UnkownDeviceException<T>(id);
        }

        /// <inheritdoc />
        public IReadOnlyCollection<T> GetDevices<T>() where T : IDevice
        {
            throw new NotImplementedException();
        }
    }
}

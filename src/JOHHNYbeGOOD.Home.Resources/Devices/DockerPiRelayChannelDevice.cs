using System.Device.I2c;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Model.Devices;

namespace JOHHNYbeGOOD.Home.Resources.Devices
{
    public class DockerPiRelayChannelDevice : IGateDevice
    {
        private readonly I2cDevice _i2cDevice;
        private readonly byte[] _turnOnCommand;
        private readonly byte[] _turnOffCommand;

        public int Duration { get; set; } = 200;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="i2cDevice"></param>
        /// <param name="channel"></param>
        public DockerPiRelayChannelDevice(byte channel, I2cDevice i2cDevice)
        {
            _i2cDevice = i2cDevice;

            _turnOnCommand = new byte[]{ channel, 0xFF };
            _turnOffCommand = new byte[] { channel, 0x00 };
        }

        /// <inheritdoc />
        public bool IsConnected()
        {
            return true;
        }

        /// <inheritdoc />
        public async Task OpenGateAsync()
        {
            if (Duration > 1000)
            {
                Duration = 1000;
            };

            SendCommand(_turnOnCommand);

            await Task.Delay(1000);

            SendCommand(_turnOffCommand);
        }

        /// <summary>
        /// Send command via I2C
        /// </summary>
        /// <param name="command"></param>
        private void SendCommand(byte[] command)
        {
            _i2cDevice.Write(command);
        }
    }
}

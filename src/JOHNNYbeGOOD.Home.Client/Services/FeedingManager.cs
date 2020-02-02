using System;
using System.Device.I2c;
using System.Threading.Tasks;

namespace JOHNNYbeGOOD.Home.Client.Services
{
    public class FeedingManager : IDisposable
    {
        private I2cDevice _i2cDevice;

        public FeedingManager()
        {
            _i2cDevice = I2cDevice.Create(new I2cConnectionSettings(1, 0x10));
        }

        public async Task Feed()
        {
            byte[] turnOn = { 0x01, 0xFF };
            _i2cDevice.Write(turnOn);

            await Task.Delay(1000);

            byte[] turnOff = { 0x01, 0x00 };
            _i2cDevice.Write(turnOff);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _i2cDevice.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}

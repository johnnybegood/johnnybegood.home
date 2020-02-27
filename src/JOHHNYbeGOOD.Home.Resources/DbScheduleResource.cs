using System;
using System.Threading.Tasks;
using JOHHNYbeGOOD.Home.Resources.Entities;
using JOHNNYbeGOOD.Home.Model;
using JOHNNYbeGOOD.Home.Resources;
using LiteDB;
using Microsoft.Extensions.Options;

namespace JOHHNYbeGOOD.Home.Resources
{
    public class DbScheduleResource : IScheduleResource, IDisposable
    {
        private readonly LiteDatabase _db;
        private readonly ILiteCollection<Schedule> _collection;

        /// <summary>
        /// Default constructor for <see cref="DbScheduleResource"/>
        /// </summary>
        /// <param name="options"></param>
        public DbScheduleResource(IOptionsSnapshot<ScheduleResourceOptions> options)
        {
            _db = new LiteDatabase(options.Value.ConnectionString);
            _collection = _db.GetCollection<Schedule>();
        }

        /// <inheritdoc />
        public Task<Schedule> RetrieveSchedule(string id)
        {
            return Task.Run(() => _collection.FindById(id) ?? new Schedule());
        }

        /// <inheritdoc />
        public Task StoreSchedule(string id, Schedule schedule)
        {
            return Task.Run(() => _collection.Insert(id, schedule));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _db.Dispose();
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

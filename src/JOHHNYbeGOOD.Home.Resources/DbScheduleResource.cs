using System;
using System.Linq;
using System.Threading.Tasks;
using JOHHNYbeGOOD.Home.Resources.Entities;
using JOHNNYbeGOOD.Home.Model;
using JOHNNYbeGOOD.Home.Models;
using JOHNNYbeGOOD.Home.Resources;
using LiteDB;
using Microsoft.Extensions.Options;

namespace JOHHNYbeGOOD.Home.Resources
{
    public class DbScheduleResource : IScheduleResource, IDisposable
    {
        private readonly LiteDatabase _db;
        private readonly ILiteCollection<Schedule> _scheduleCollection;
        private readonly ILiteCollection<FeedingLog> _logCollection;

        /// <summary>
        /// Default constructor for <see cref="DbScheduleResource"/>
        /// </summary>
        /// <param name="options"></param>
        public DbScheduleResource(IOptionsSnapshot<ScheduleResourceOptions> options)
        {
            _db = new LiteDatabase(options.Value.ConnectionString);
            _scheduleCollection = _db.GetCollection<Schedule>();
            _logCollection = _db.GetCollection<FeedingLog>();
        }

        /// <inheritdoc />
        public Task<Schedule> RetrieveSchedule(string id)
        {
            return Task.Run(() => _scheduleCollection.FindById(id) ?? new Schedule());
        }

        /// <inheritdoc />
        public Task StoreSchedule(string id, Schedule schedule)
        {
            return Task.Run(() => _scheduleCollection.Insert(id, schedule));
        }

        /// <inheritdoc />
        public Task<FeedingLogCollection> RetrieveFeedingLog()
        {
            return Task.Run(() =>
            {
                var items = _logCollection
                    .FindAll()
                    .OrderByDescending(c => c.Timestamp)
                    .ToList();

                return new FeedingLogCollection(items);
            });
        }

        /// <inheritdoc />
        public Task LogFeeding(string description, DateTime dateTime)
        {
            return LogFeeding(new FeedingLog

            {
                Id = Guid.NewGuid().ToString(),
                Description = description,
                Result = FeedingLogResult.Successfull,
                Timestamp = dateTime.ToUniversalTime()
            });
        }

        /// <inheritdoc />
        public Task LogFailedFeeding(string description, DateTime dateTime, string cause = null)
        {
            return LogFeeding(new FeedingLog

            {
                Id = Guid.NewGuid().ToString(),
                Description = description,
                Result = FeedingLogResult.Failed,
                Timestamp = dateTime.ToUniversalTime(),
                Cause = cause
            });
        }

        /// <inheritdoc />
        public Task<FeedingLog> LastFeeding(DateTimeOffset beforeDateTime)
        {
            return Task.Run(() => _logCollection
                .Find(l => l.Timestamp <= beforeDateTime.ToUniversalTime())
                .OrderByDescending(l => l.Timestamp)
                .FirstOrDefault());
        }

        /// <summary>
        /// Log a feeding
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Task LogFeeding(FeedingLog item)
        {
            return Task.Run(() => _logCollection.Insert(item));
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

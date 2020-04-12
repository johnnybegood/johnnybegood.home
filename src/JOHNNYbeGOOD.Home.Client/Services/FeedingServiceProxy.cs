using System.Net.Http;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Api.Contracts;
using JOHNNYbeGOOD.Home.Api.Contracts.Models;
using Microsoft.AspNetCore.Components;

namespace JOHNNYbeGOOD.Home.Client.Services
{
    public class FeedingServiceProxy : IFeedingService, ISystemService
    {
        private readonly HttpClient _httpClient;

        public FeedingServiceProxy(HttpClient client)
        {
            _httpClient = client;
        }

        /// <inheritdoc />
        public Task<ScheduleDTO> GetCurrentSchedule()
        {
            return _httpClient.GetJsonAsync<ScheduleDTO>("api/feeding/schedule");
        }

        /// <inheritdoc />
        public Task<LogResponse[]> GetLog()
        {
            return _httpClient.GetJsonAsync<LogResponse[]>("api/feeding/log");
        }

        /// <inheritdoc />
        public Task<NextFeedingSlotResponse> GetNextFeedingAsync()
        {
            return _httpClient.GetJsonAsync<NextFeedingSlotResponse>("api/feeding/next");
        }

        /// <inheritdoc />
        public Task<StatusResponse[]> GetStatus()
        {
            return _httpClient.GetJsonAsync<StatusResponse[]>("api/system/status");
        }

        /// <inheritdoc />
        public Task<FeedingSummaryResponse> GetSummaryAsync()
        {
            return _httpClient.GetJsonAsync<FeedingSummaryResponse>("api/feeding");
        }

        /// <inheritdoc />
        public Task<FeedResponse> PostFeed()
        {
            return _httpClient.PostJsonAsync<FeedResponse>("api/feeding/feed", null);
        }

        /// <inheritdoc />
        public Task PutCurrentSchedule(ScheduleDTO schedule)
        {
            return _httpClient.PutJsonAsync("api/feeding/schedule", schedule);
        }
    }
}

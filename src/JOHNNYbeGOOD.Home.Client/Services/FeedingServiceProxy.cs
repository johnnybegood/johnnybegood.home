using System.Net.Http;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Api.Contracts;
using JOHNNYbeGOOD.Home.Api.Contracts.Models;
using Microsoft.AspNetCore.Components;

namespace JOHNNYbeGOOD.Home.Client.Services
{
    public class FeedingServiceProxy : IFeedingService
    {
        private readonly HttpClient _httpClient;

        public FeedingServiceProxy(HttpClient client)
        {
            _httpClient = client;
        }

        public Task<NextFeedingSlotResponse> GetNextFeedingAsync()
        {
            return _httpClient.GetJsonAsync<NextFeedingSlotResponse>("api/feeding");
        }

        public Task<FeedingSummaryResponse> GetSummaryAsync()
        {
            return _httpClient.GetJsonAsync<FeedingSummaryResponse>("api/feeding/next");
        }

        public Task<FeedResponse> PostFeed()
        {
            return _httpClient.PostJsonAsync<FeedResponse>("api/feeding/feed", null);
        }
    }
}

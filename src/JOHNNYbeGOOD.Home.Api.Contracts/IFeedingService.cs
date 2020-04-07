using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Api.Contracts.Models;

namespace JOHNNYbeGOOD.Home.Api.Contracts
{
    /// <summary>
    /// Summary of the feeding
    /// </summary>
    public interface IFeedingService
    {
        Task<NextFeedingSlotResponse> GetNextFeedingAsync();
        Task<FeedResponse> PostFeed();
        Task<FeedingSummaryResponse> GetSummaryAsync();
        Task<LogResponse[]> GetLog();
    }
}
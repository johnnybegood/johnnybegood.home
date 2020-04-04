namespace JOHNNYbeGOOD.Home.Api.Contracts.Models
{
    public class FeedResponse : NextFeedingSlotResponse
    {
        public bool Succeeded { get; set; }
        public NextFeedingSlotResponse NextFeeding { get; set; }
    }
}
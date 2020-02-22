namespace JOHNNYbeGOOD.Home.Api.Models
{
    public class FeedResponse : NextFeedingSlotResponse
    {
        public bool Succeeded { get; internal set; }
        public NextFeedingSlotResponse NextFeeding { get; internal set; }
    }
}
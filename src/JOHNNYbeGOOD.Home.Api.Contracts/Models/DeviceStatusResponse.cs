namespace JOHNNYbeGOOD.Home.Api.Contracts.Models
{
    public class DeviceStatusResponse
    {
        public string Device { get; set; }
        public string DeviceType { get; set; }
        public bool Connected { get; set; }
        public string State { get; set; }
        public string Description { get; set; }
    }
}

using System;
namespace JOHNNYbeGOOD.Home.Model
{
    public interface IFeedingSlot
    {
        string Name { get; }

        bool CanOpen();
    }
}

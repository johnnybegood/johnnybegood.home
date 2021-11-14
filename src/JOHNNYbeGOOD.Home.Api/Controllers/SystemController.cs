using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Api.Contracts;
using JOHNNYbeGOOD.Home.Api.Contracts.Models;
using JOHNNYbeGOOD.Home.Model.Devices;
using JOHNNYbeGOOD.Home.Resources;
using Microsoft.AspNetCore.Mvc;

namespace JOHNNYbeGOOD.Home.Api.Controllers
{
    [Route("/api/system")]
    public class SystemController : Controller, ISystemService
    {
        private IThingsResource _thingsResource;
        private readonly IScheduleResource _scheduleResource;
        private readonly IFeedingManager _feedingManager;

        public SystemController(IThingsResource thingsResource, IScheduleResource scheduleResource, IFeedingManager feedingManager)
        {
            _thingsResource = thingsResource;
            _scheduleResource = scheduleResource;
            _feedingManager = feedingManager;
        }

        [HttpGet("status")]
        public Task<StatusResponse> GetStatus()
        {
            var response = new StatusResponse
            {
                Devices = _thingsResource
                .FullDeviceSummary()
                .Select(s => new DeviceStatusResponse
                {
                    Device = s.Id,
                    DeviceType = s.DeviceType.Name,
                    Connected = s.Status.IsConnected,
                    State = Enum.GetName(typeof(DeviceStateCode), s.Status.Code),
                    Description = s.Status.Description
                })
                .ToArray(),

                FeedingSlot = _feedingManager
                    .GetDiagnostics()
                    .Select(s => new FeedingSlotStatusResponse
                    {
                        Slot = s.Id,
                        CanOpen = s.CanOpen
                    })
                    .ToArray()
            };

            return Task.FromResult(response);
        }
    }
}

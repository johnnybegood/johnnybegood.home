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

        public SystemController(IThingsResource thingsResource, IScheduleResource scheduleResource)
        {
            _thingsResource = thingsResource;
            _scheduleResource = scheduleResource;
        }

        [HttpGet("status")]
        public Task<StatusResponse[]> GetStatus()
        {
            var status = _thingsResource
                .FullDeviceSummary()
                .Select(s => new StatusResponse
                {
                    Device = s.Id,
                    DeviceType = s.DeviceType.Name,
                    Connected = s.Status.IsConnected,
                    State = Enum.GetName(typeof(DeviceStateCode), s.Status.Code),
                    Description = s.Status.Description
                })
                .ToArray();

            return Task.FromResult(status);
        }
    }
}

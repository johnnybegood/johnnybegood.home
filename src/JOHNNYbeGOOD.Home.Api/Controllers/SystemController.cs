using System;
using System.Linq;
using JOHNNYbeGOOD.Home.Api.Models;
using JOHNNYbeGOOD.Home.Model.Devices;
using JOHNNYbeGOOD.Home.Resources;
using Microsoft.AspNetCore.Mvc;

namespace JOHNNYbeGOOD.Home.Api.Controllers
{
    [Route("/api/system")]
    public class SystemController : Controller
    {
        private IThingsResource _thingsResource;

        public SystemController(IThingsResource thingsResource)
        {
            _thingsResource = thingsResource;
        }

        [HttpGet("Status")]
        public ActionResult<StatusResponse> GetStatus()
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

            return Ok(status);
        }
    }
}

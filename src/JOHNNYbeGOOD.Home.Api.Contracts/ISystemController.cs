using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Api.Contracts.Models;

namespace JOHNNYbeGOOD.Home.Api.Contracts
{
    public interface ISystemService
    {
        Task<StatusResponse[]> GetStatus();
    }
}
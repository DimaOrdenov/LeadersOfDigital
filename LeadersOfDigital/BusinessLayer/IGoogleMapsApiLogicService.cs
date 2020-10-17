using System;
using LeadersOfDigital.Definitions.Models.GoogleApi;
using System.Threading.Tasks;
using System.Threading;

namespace LeadersOfDigital.BusinessLayer
{
    public interface IGoogleMapsApiLogicService
    {
        Task<GoogleDirection> GetDirections(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude, CancellationToken token);
    }
}

using System;
using LeadersOfDigital.Definitions.Models.GoogleApi;
using System.Threading.Tasks;
using System.Threading;
using LeadersOfDigital.Definitions.Requests;

namespace LeadersOfDigital.BusinessLayer
{
    public interface IGoogleMapsApiLogicService
    {
        Task<GoogleDirection> GetDirections(GoogleApiDirectionsRequest googleApiDirectionsRequest, CancellationToken token);
    }
}

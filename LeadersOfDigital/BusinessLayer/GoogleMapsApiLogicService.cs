using System;
using System.Threading;
using System.Threading.Tasks;
using LeadersOfDigital.Definitions.Responses.GoogleApi;
using LeadersOfDigital.Definitions.Requests;
using LeadersOfDigital.Helpers;
using NoTryCatch.BL.Core;
using NoTryCatch.Core.Services;
using RestSharp;
using System.Linq;

namespace LeadersOfDigital.BusinessLayer
{
    public class GoogleMapsApiLogicService : BaseLogic<GoogleDirection>, IGoogleMapsApiLogicService
    {
        public GoogleMapsApiLogicService(IRestClient client, UserContext context, IDebuggerService debuggerService)
            : base(client, context, debuggerService)
        {
        }

        protected override string Route => throw new NotImplementedException("Service not supported for REST requests");

        public Task<GoogleDirection> GetDirections(GoogleApiDirectionsRequest googleApiDirectionsRequest, CancellationToken token)
        {
            IRestRequest request = new RestRequest("api/directions/json", Method.GET);

            request.AddParameter("mode", googleApiDirectionsRequest.TravelMode);
            request.AddParameter("origin", $"{googleApiDirectionsRequest.Origin.Latitude},{googleApiDirectionsRequest.Origin.Longitude}");
            request.AddParameter("destination", $"{googleApiDirectionsRequest.Destination.Latitude},{googleApiDirectionsRequest.Destination.Longitude}");
            request.AddParameter("waypoints", $"{googleApiDirectionsRequest.Waypoint.Latitude},{googleApiDirectionsRequest.Waypoint.Longitude}");
            request.AddParameter("key", $"{Secrets.GoogleApiKey}");

            return ExecuteAsync<GoogleDirection>(request, token);
        }
    }
}

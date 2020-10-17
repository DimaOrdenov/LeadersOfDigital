using System;
using System.Threading;
using System.Threading.Tasks;
using LeadersOfDigital.Definitions.Models.GoogleApi;
using LeadersOfDigital.Helpers;
using NoTryCatch.BL.Core;
using NoTryCatch.Core.Services;
using RestSharp;

namespace LeadersOfDigital.BusinessLayer
{
    public class GoogleMapsApiLogicService : BaseLogic<GoogleDirection>, IGoogleMapsApiLogicService
    {
        public GoogleMapsApiLogicService(IRestClient client, UserContext context, IDebuggerService debuggerService)
            : base(client, context, debuggerService)
        {
        }

        protected override string Route => throw new NotImplementedException("Service not supported for REST requests");

        public Task<GoogleDirection> GetDirections(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude, CancellationToken token)
        {
            IRestRequest request = new RestRequest("api/directions/json", Method.GET);

            request.AddParameter("mode", "driving");
            request.AddParameter("transit_routing_preference", "less_driving");
            request.AddParameter("origin", $"{originLatitude},{originLongitude}");
            request.AddParameter("destination", $"{destinationLatitude},{destinationLongitude}");
            request.AddParameter("key", $"{Secrets.GoogleApiKey}");

            return ExecuteAsync<GoogleDirection>(request, token);
        }
    }
}

using DataModels.Requests;
using DataModels.Responses;
using NoTryCatch.BL.Core;
using NoTryCatch.Core.Services;
using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace LeadersOfDigital.BusinessLayer
{
    internal class FacilitiesLogic : BaseLogic<FacilityResponse>, IFacilitiesLogic
    {
        public FacilitiesLogic(IRestClient client, UserContext context, IDebuggerService debuggerService) :
            base(client, context, debuggerService)
        {
        }

        protected override string Route => "Facilities";

        public Task<FacilityResponse> AddFacility(FacilityRequest facilityRequest, CancellationToken token)
        {
            IRestRequest request = new RestRequest(Route, Method.POST);
            request.AddJsonBody(facilityRequest);

            return ExecuteAsync<FacilityResponse>(request, token);
        }
    }
}

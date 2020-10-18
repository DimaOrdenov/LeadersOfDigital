using System.Threading;
using System.Threading.Tasks;
using DataModels.Requests;
using DataModels.Responses;
using NoTryCatch.BL.Core;

namespace LeadersOfDigital.BusinessLayer
{
    public interface IFacilitiesLogic : IBaseLogic<FacilityResponse>
    {
        Task<FacilityResponse> AddFacility(FacilityRequest facilityRequest, CancellationToken token);
    }
}
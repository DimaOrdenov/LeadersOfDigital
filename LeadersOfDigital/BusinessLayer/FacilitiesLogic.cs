using DataModels.Responses;
using NoTryCatch.BL.Core;
using NoTryCatch.Core.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadersOfDigital.BusinessLayer
{
    internal class FacilitiesLogic : BaseLogic<FacilityResponse>, IFacilitiesLogic
    {
        public FacilitiesLogic(IRestClient client, UserContext context, IDebuggerService debuggerService) :
            base(client, context, debuggerService)
        {
        }

        protected override string Route => "Facilities";
    }
}

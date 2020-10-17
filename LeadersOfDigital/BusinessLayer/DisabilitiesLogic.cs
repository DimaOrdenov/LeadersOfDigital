using DataModels.Responses;
using NoTryCatch.BL.Core;
using NoTryCatch.Core.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadersOfDigital.BusinessLayer
{
    internal class DisabilitiesLogic : BaseLogic<DisabilityResponse>, IDisabilitiesLogic
    {
        public DisabilitiesLogic(IRestClient client, UserContext context, IDebuggerService debuggerService) :
            base(client, context, debuggerService)
        {
        }

        protected override string Route => "Disabilities";
    }
}

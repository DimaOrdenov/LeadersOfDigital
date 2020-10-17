using DataModels.Responses;
using NoTryCatch.BL.Core;
using NoTryCatch.Core.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadersOfDigital.Logic
{
    internal class BarriersLogic : BaseLogic<BarrierResponse>, IBarriersLogic
    {
        public BarriersLogic(IRestClient client, UserContext context, IDebuggerService debuggerService) :
            base(client, context, debuggerService)
        {
        }

        protected override string Route => "Barriers";
    }
}

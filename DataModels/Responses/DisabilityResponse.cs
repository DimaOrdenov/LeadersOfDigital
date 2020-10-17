using DataModels.Responses.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.Responses
{
    public class DisabilityResponse
    {
        public int DisabilityId { get; set; }
        public DisabilityType DisabilityType { get; set; }
    }
}

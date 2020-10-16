using DataModels.Responses.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.Responses
{
    public class BarrierResponse
    {
        public int BarrierId { get; set; }

        public BarrierType BarrierType { get; set; }
        public byte[] Photo { get; set; }

        public string Comment { get; set; }
        public List<DisabilityResponse> AvailableFor { get; set; } 
    }
}

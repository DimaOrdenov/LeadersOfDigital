using DataModels.Responses.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.Responses
{
    public class FacilityResponse
    {
        public int FacilityId { get; set; }
        public Subcategory Subcategory { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public double Latitude { get; set; }
        public double Longitute { get; set; }
        public List<BarrierResponse> Barriers { get; set; } 
        public float Rating { get; set; }
    }
}

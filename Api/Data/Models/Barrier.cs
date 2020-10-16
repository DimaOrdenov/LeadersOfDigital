using DataModels.Responses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data.Models
{
    public class Barrier
    {
        public int BarrierId { get; set; }
        public BarrierType BarrierType { get; set; }
        public string Comment { get; set; }
        public byte[] Photo { get; set; }
        public List<Disability> AvailableFor { get; } = new List<Disability>(); 
        public int FacilityId { get; set; }
        public Facility Facility { get; set; }
    }
}

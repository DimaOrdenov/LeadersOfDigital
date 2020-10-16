using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data.Models
{
    public class Facility
    {
        public int FacilityId { get; set; }
        public Subcategory Subcategory {get; set;}
        public string Name { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public double Latitude { get; set; }
        public double Longitute { get; set; }
        public List<Barrier> Barriers { get; } = new List<Barrier>();
        public float Rating { get; set; }
    }
}

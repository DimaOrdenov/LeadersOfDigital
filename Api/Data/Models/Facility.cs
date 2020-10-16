using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data.Models
{
    public class Facility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public double Latitude { get; set; }
        public double Longitute { get; set; }

        public IEnumerable<Barrier> Barriers { get; set; } 

        public float Rating { get; set; }
    }
}

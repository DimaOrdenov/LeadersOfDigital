using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data.Models
{
    public class Barrier
    {
        public int Id { get; set; }

        public BarrierType BarrierType { get; set; }
        public string Comment { get; set; }

        public IEnumerable<Disability> AvailableFor { get; set; }
    }
}

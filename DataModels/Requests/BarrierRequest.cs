using DataModels.Responses.Enums;

namespace DataModels.Requests
{
    public class BarrierRequest
    {
        public BarrierType BarrierType { get; set; }

        public string Comment { get; set; }

        public byte[] Photo { get; set; }

        public int FacilityId { get; set; }
    }
}

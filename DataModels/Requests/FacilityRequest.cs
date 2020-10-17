using DataModels.Responses.Enums;

namespace DataModels.Requests
{
    public class FacilityRequest
    {
        public FacilityRequest()
        {
            Subcategory = Subcategory.Pharmacy;
        }

        public int Id { get; set; }

        public Subcategory Subcategory { get; set; }

        public string Street { get; set; }

        public double Latitude { get; set; }

        public double Longitute { get; set; }
    }
}

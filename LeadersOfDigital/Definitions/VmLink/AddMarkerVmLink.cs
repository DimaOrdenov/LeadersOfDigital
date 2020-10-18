using System;
namespace LeadersOfDigital.Definitions.VmLink
{
    public class AddMarkerVmLink
    {
        public AddMarkerVmLink(double latitude, double longitute, string address)
        {
            Latitude = latitude;
            Longitute = longitute;
            Address = address;
        }

        public double Latitude { get; }

        public double Longitute { get; }

        public string Address { get; }
    }
}

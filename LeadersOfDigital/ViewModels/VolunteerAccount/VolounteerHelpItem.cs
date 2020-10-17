using System;
using NoTryCatch.Xamarin.Portable.ViewModels;

namespace LeadersOfDigital.ViewModels.VolunteerAccount
{
    public class VolounteerHelpItem : BaseViewModel
    {
        public string FullName { get; set; }

        public string Description { get; set; }

        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        public DateTime PlannedAt { get; set; }

        public bool IsLast { get; set; }
    }
}

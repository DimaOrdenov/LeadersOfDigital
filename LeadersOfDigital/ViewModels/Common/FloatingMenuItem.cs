using System;
using System.Windows.Input;
using NoTryCatch.Xamarin.Portable.ViewModels;

namespace LeadersOfDigital.ViewModels.Common
{
    public class FloatingMenuItem : BaseViewModel
    {
        private bool _isActive;

        public ICommand TapCommand { get; set; }

        public FloatingMenuItem(string title)
        {
            Title = title;
        }

        public string Title { get; }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }
    }
}

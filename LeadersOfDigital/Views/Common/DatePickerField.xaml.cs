using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace LeadersOfDigital.Views.Common
{
    public partial class DatePickerField : ContentView
    {
        public DatePickerField()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty DateProperty = BindableProperty.Create(
           nameof(Date),
           typeof(DateTime),
           typeof(DatePickerField),
           DateTime.MinValue,
           defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
         nameof(Placeholder),
         typeof(string),
         typeof(DatePickerField));

        public static readonly BindableProperty DateSelectedCommandProperty = BindableProperty.Create(
         nameof(DateSelectedCommand),
         typeof(ICommand),
         typeof(DatePickerField));

        public DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public ICommand DateSelectedCommand
        {
            get => (ICommand)GetValue(DateSelectedCommandProperty);
            set => SetValue(DateSelectedCommandProperty, value);
        }
    }
}

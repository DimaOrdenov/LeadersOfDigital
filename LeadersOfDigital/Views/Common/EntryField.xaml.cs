using Xamarin.Forms;

namespace LeadersOfDigital.Views.Common
{
    public partial class EntryField : ContentView
    {
        public EntryField()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
           nameof(Text),
           typeof(string),
           typeof(EntryField),
           defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
         nameof(Placeholder),
         typeof(string),
         typeof(EntryField));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }
    }
}

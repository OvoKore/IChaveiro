using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace IChaveiro.Views.Configurations.Address
{
    public partial class AddressView : ContentPage
    {
        public AddressView()
        {
            InitializeComponent();
            Title = "Address";
            IconImageSource = ImageSource.FromFile("services.png");
            NavigationPage.SetHasNavigationBar(this, true);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}
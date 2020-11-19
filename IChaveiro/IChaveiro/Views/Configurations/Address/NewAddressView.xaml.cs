using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace IChaveiro.Views.Configurations.Address
{
    public partial class NewAddressView : ContentPage
    {
        public NewAddressView()
        {
            InitializeComponent();
            Title = "Address";
            NavigationPage.SetHasNavigationBar(this, true);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}
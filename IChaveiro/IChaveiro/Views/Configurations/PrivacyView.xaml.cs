using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace IChaveiro.Views.Configurations
{
    public partial class PrivacyView : ContentPage
    {
        public PrivacyView()
        {
            InitializeComponent();
            Title = "Privacy Policy";
            NavigationPage.SetHasNavigationBar(this, true);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}
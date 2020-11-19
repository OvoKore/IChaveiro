using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace IChaveiro.Views.Services
{
    public partial class NewServiceView : ContentPage
    {
        public NewServiceView()
        {
            InitializeComponent();
            Title = "Service";
            NavigationPage.SetHasNavigationBar(this, true);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}
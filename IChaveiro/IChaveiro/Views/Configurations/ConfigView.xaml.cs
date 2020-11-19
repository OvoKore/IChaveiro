using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace IChaveiro.Views.Configurations
{
    public partial class ConfigView : ContentPage
    {
        public ConfigView()
        {
            InitializeComponent();
            Title = "Configurations";
            IconImageSource = ImageSource.FromFile("config.png");
            NavigationPage.SetHasNavigationBar(this, false);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}
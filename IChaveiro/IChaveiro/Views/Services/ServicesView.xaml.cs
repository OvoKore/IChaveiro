using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace IChaveiro.Views.Services
{
    public partial class ServicesView : ContentPage
    {
        public ServicesView()
        {
            InitializeComponent();
            Title = "Serviços";
            IconImageSource = ImageSource.FromFile("services.png");
            NavigationPage.SetHasNavigationBar(this, false);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}
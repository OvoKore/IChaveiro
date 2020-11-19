using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace IChaveiro.Views.LoginSignUp
{
    public partial class SignUpView : ContentPage
    {
        public SignUpView()
        {
            InitializeComponent();
            Title = "Sign Up";
            NavigationPage.SetHasNavigationBar(this, true);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}
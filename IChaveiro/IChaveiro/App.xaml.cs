using IChaveiro.ViewModels.Base;
using IChaveiro.ViewModels.Configurations;
using IChaveiro.ViewModels.Configurations.Address;
using IChaveiro.ViewModels.LoginSignUp;
using IChaveiro.ViewModels.Services;
using IChaveiro.Views;
using IChaveiro.Views.Configurations;
using IChaveiro.Views.Configurations.Address;
using IChaveiro.Views.LoginSignUp;
using IChaveiro.Views.Services;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace IChaveiro
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            VersionTracking.Track();

            await NavigationService.NavigateAsync("/NavigationPage/LoginView");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();

            containerRegistry.RegisterForNavigation<MenuView, ViewModelBase>();

            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<SignUpView, SignUpViewModel>();

            containerRegistry.RegisterForNavigation<ServicesView, ServicesViewModel>();
            containerRegistry.RegisterForNavigation<NewServiceView, NewServicesViewModel>();

            containerRegistry.RegisterForNavigation<ConfigView, ConfigViewModel>();

            containerRegistry.RegisterForNavigation<ProfileView, ProfileViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordView, ChangePasswordViewModel>();

            containerRegistry.RegisterForNavigation<AddressView, AddressViewModel>();
            containerRegistry.RegisterForNavigation<NewAddressView, NewAddressViewModel>();

            containerRegistry.RegisterForNavigation<AboutView, AboutViewModel>();
            containerRegistry.RegisterForNavigation<PrivacyView, ViewModelBase>();
            containerRegistry.RegisterForNavigation<TermsUseView, ViewModelBase>();

        }
    }
}
using IChaveiro.Controls;
using IChaveiro.Views.Configurations;
using IChaveiro.Views.Services;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace IChaveiro.Views
{
    public partial class MenuView : TabbedPage
    {
        public MenuView()
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            On<Android>().SetIsSmoothScrollEnabled(false);

            Children.Add(new CustomNavigationPage(new ServicesView()));
            Children.Add(new CustomNavigationPage(new ConfigView()));
        }
    }
}
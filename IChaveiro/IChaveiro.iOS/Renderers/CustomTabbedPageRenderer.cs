using IChaveiro.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(CustomTabbedPageRenderer))]
namespace IChaveiro.iOS.Renderers
{
    public class CustomTabbedPageRenderer : TabbedRenderer
    {
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            if (Element is TabbedPage)
                if (TabBar?.Items != null)
                    for (int i = 0; i < TabBar.Items.Length; i++)
                        TabBar.Items[i].ImageInsets = new UIEdgeInsets(25, 25, 25, 25);
        }
    }
}
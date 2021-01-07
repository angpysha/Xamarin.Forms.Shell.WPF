using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ShellWpfApp.WPF.Shell;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;
using Xamarin.Forms.Platform.WPF.Controls;
using Color = System.Windows.Media.Color;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;

[assembly:ExportRenderer(typeof(Shell),typeof(ShellRenderer))]
namespace ShellWpfApp.WPF.Shell
{
    public class ShellRenderer : VisualPageRenderer<Xamarin.Forms.Shell,WpfFlyoutPage>
    {
        VisualElement _currentView;
        private IVisualElementRenderer HeaderRenderer { get; set; }
        private IVisualElementRenderer FooterRenderer { get; set; }
        private IShellController shellController => Element;

        public ShellItemRenderer ShellItem { get; set; }
        public ShellItem SelectedItem { get; set; }

        public ShellRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Shell> e)
        {
            if (e.NewElement != null)
            {
                if (Control == null)
                {

                      SetNativeControl(new WpfFlyoutPage());

                    //  Control.Content = 
                    if (Control.Shell == null)
                    {
                        Control.Shell = new WeakReference<Xamarin.Forms.Shell>(Element);
                    }
                    OnElementSet();



                }
            }
            base.OnElementChanged(e);
        }

        private void ShowFlaoutButton()
        {

            var items = Element.Items.OfType<FlyoutItem>();
            if (items.Any())
            {
                Control.HasBackButton = true;
                Control.PrimaryTopBarCommands.Add(new FormsAppBarButton()
                {
                    Width = 30,
                    Height = 30,
                    Background = new SolidColorBrush(Colors.BlueViolet)
                });
                //Control.SecondaryTopBarCommands.Add(new FormsAppBarButton()
                //{
                //    Width = 30,
                //    Height = 30,
                //    Background = new SolidColorBrush(Colors.BlueViolet)
                //});

            }
        }

        protected override void Appearing()
        {
            base.Appearing();

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Xamarin.Forms.Shell.CurrentItemProperty.PropertyName)
            {
               // int iii = 0;
               SwitchShellItem(Element.CurrentItem);
            }
        }

        private void OnElementSet()
        {
            ShowFlaoutButton();
            SetFlyoutHeader();
            Control.UpdateFlayoutItems();
            Control.ItemContent.Content = ShellItem = new ShellItemRenderer();
            ShellItem.ShellContext = this;
            ShellItem.InitShellData();
            SwitchShellItem(Element.CurrentItem);
           // SetHeader();
        }

        private void SwitchShellItem(ShellItem elementCurrentItem)
        {
            SelectedItem = elementCurrentItem;
            ShellItem.NavigateToShellItem(elementCurrentItem);
            ShellItem.UpdateData();
        }

        private View FlyoutHeaderView;
        private void SetFlyoutHeader()
        {
            FlyoutHeaderView = Element.FlyoutHeader as View;
            
            var headerRenderer = Platform.GetOrCreateRenderer(FlyoutHeaderView);
            
            var measet = FlyoutHeaderView.Measure(double.MaxValue, double.MaxValue);
            var nativeContent = headerRenderer.GetNativeElement();
            Control.HeaderContent.Content = nativeContent;
            nativeContent.Loaded -= FlyoutHeaderLoaded;
            nativeContent.Loaded += FlyoutHeaderLoaded;
            //Control.HeaderContent.Width = Control.FlyoutView.ActualWidth;
            //Control.HeaderContent.Height = measet.Request.Height;
            //nativeContent.Width = Control.HeaderContent.Width;
            //nativeContent.Height = Control.HeaderContent.Height;
            //(Element.FlyoutHeader as View)?.LayoutTo(new Rectangle(0, 0, nativeContent.ActualWidth,
            //    nativeContent.ActualHeight));

            //Control.HeaderContent.Content = nativeContent;


        }

        private void FlyoutHeaderLoaded(object sender, RoutedEventArgs e)
        {
            var el = sender as FrameworkElement;
            FlyoutHeaderView.Layout(new Rectangle(0,0,el.ActualWidth,FlyoutHeaderView.HeightRequest > 0 ? FlyoutHeaderView.HeightRequest : el.ActualHeight));

        }
        private View FlyoutFooterView;

        //private void SetFlayoutFooter()
        //{
        //    FlyoutFooterView = Element.Flyout
        //}
    }
}

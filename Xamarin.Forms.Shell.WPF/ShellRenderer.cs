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
using Xamarin.Forms.Platform.WPF.Extensions;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;
using Stretch = System.Windows.Media.Stretch;
using WBrush = System.Windows.Media.Brush;
[assembly:ExportRenderer(typeof(Shell),typeof(ShellRenderer))]
namespace ShellWpfApp.WPF.Shell
{
    public class ShellRenderer : VisualPageRenderer<Xamarin.Forms.Shell,WpfFlyoutPage>, IAppearanceObserver, IFlyoutBehaviorObserver
    {
        public Brush FlyoutBackground { get; set; }
        VisualElement _currentView;
        private IVisualElementRenderer HeaderRenderer { get; set; }
        private IVisualElementRenderer FooterRenderer { get; set; }
        private IShellController shellController => Element;

        public ShellItemRenderer ShellItem { get; set; }
        public ShellItem SelectedItem { get; set; }

        public ShellRenderer()
        {
            VerifyExperimentalFlagExit();
        }

        private void VerifyExperimentalFlagExit()
        {
            var flags = Xamarin.Forms.Forms.Flags;
            var contains = flags.Contains("Shell_WPF_Experimental");
            if (contains == false)
            {
                throw new InvalidOperationException("Shell for WPF is experimental. Please, add Shell_UWP_Experimental flag");
            }
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
            } else if (e.PropertyName == Xamarin.Forms.Shell.FlyoutIsPresentedProperty.PropertyName)
            {

            }
        }

        private void OnElementSet()
        {
            ShowFlaoutButton();
            SetFlyoutHeader();
            SetFlayoutFooter();
            SetFlyoutBackground();
            Control.UpdateFlayoutItems();
            UpdateTitleBarBackground();
            shellController.AddAppearanceObserver(this,Element);
            shellController.AddFlyoutBehaviorObserver(this);
            Control.ItemContent.Content = ShellItem = new ShellItemRenderer();
            ShellItem.SetShellContext(this);
            ShellItem.InitShellData();
            SwitchShellItem(Element.CurrentItem);
           // SetHeader();
        }

        private void UpdateTitleBarBackground()
        {
            
        }

        private async void SetFlyoutBackground()
        {
            if (Element.FlyoutBackgroundImage != null)
            {
                var source = await Element.FlyoutBackgroundImage.ToWindowsImageSourceAsync();
                var imageBrush = new ImageBrush(source);
                imageBrush.Stretch = ToWStretch(Element.FlyoutBackgroundImageAspect);
                Control.FlyoutBackground = imageBrush;
            }
            else
            {
                Control.FlyoutBackground = Element.Background != null?
                    Element.FlyoutBackground.ToBrush():
                    Element.FlyoutBackgroundColor.ToBrush();
            }
        }

        private Stretch ToWStretch(Aspect aspect)
        {
            return aspect switch
            {
                Aspect.AspectFit => Stretch.Uniform,
                Aspect.AspectFill => Stretch.UniformToFill,
                Aspect.Fill => Stretch.Fill
            };
        }

        private void SwitchShellItem(ShellItem elementCurrentItem)
        {
            SelectedItem = elementCurrentItem;
            UpdateHamburgerIconVisibility();
            ShellItem.NavigateToShellItem(elementCurrentItem);
            ShellItem.UpdateData();
        }

        private void UpdateHamburgerIconVisibility()
        {
           // Control.hamburger.Visibility

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


        }

        private void FlyoutHeaderLoaded(object sender, RoutedEventArgs e)
        {
            var el = sender as FrameworkElement;
            FlyoutHeaderView.Layout(new Rectangle(0,0,el.ActualWidth,FlyoutHeaderView.HeightRequest > 0 ? FlyoutHeaderView.HeightRequest : el.ActualHeight));

        }
        private View FlyoutFooterView;

        private void SetFlayoutFooter()
        {
            if (Element.FlyoutFooter == null)
                return;
            
            var flyoutFooterView = FlyoutFooterView = Element.FlyoutFooter as View;

            var footerRenderer = Platform.GetOrCreateRenderer(flyoutFooterView);

            var nativeContent = footerRenderer.GetNativeElement();

            Control.FooterControl.Content = nativeContent;
            nativeContent.Loaded -= FlyoutFooterLoaded;
            nativeContent.Loaded += FlyoutFooterLoaded;

        }

        private void FlyoutFooterLoaded(object sender, RoutedEventArgs e)
        {
            var el = sender as FrameworkElement;
            FlyoutFooterView.Layout(new Rectangle(0, 0, el.ActualWidth, FlyoutFooterView.HeightRequest > 0 ? FlyoutFooterView.HeightRequest : el.ActualHeight));
        }

        //private void SetFlayoutFooter()
        //{
        //    FlyoutFooterView = Element.Flyout
        //}
        public void OnAppearanceChanged(ShellAppearance appearance)
        {
            if (appearance != null)
            {
                //TODO: Impleemtn
            }
        }

        public void OnFlyoutBehaviorChanged(FlyoutBehavior behavior)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShellAppWPF.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;
using Xamarin.Forms.Platform.WPF.Controls;
using Grid = System.Windows.Controls.Grid;
using SelectionChangedEventArgs = System.Windows.Controls.SelectionChangedEventArgs;
using WBrush = System.Windows.Media.Brush;

namespace ShellWpfApp.WPF.Shell
{
    /// <summary>
    /// Interaction logic for WpfFlyoutPage.xaml
    /// </summary>
    public partial class WpfFlyoutPage : FormsContentPage
    {
        public static readonly DependencyProperty FlyoutBackgroundProperty = DependencyProperty.Register(nameof(FlyoutBackground),
            typeof(WBrush),
            typeof(WpfFlyoutPage),
            new PropertyMetadata(new System.Windows.Media.SolidColorBrush(Colors.White)));

        public static readonly DependencyProperty FlyoutIsOpenedProperty = DependencyProperty.Register(nameof(FlyoutIsOpened),
            typeof(bool),
            typeof(WpfFlyoutPage),
            new PropertyMetadata(false,OnFlyoutOpenChangedStatic));

        //public static readonly DependencyProperty ToolbarItemsProperty = DependencyProperty.Register(nameof(ToolbarItems),
        //    typeof(IEnumerable<object>),
        //    typeof(WpfFlyoutPage),
        //    new PropertyMetadata(default));



        private static void OnFlyoutOpenChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var page = d as WpfFlyoutPage;
            var bNew = ((bool)e.NewValue);
            var bOld = (bool) e.OldValue;

            if (bNew != bOld)
            {
                page?.UpdatePaneVisibility();
            }
        }


        //public IEnumerable<object> ToolbarItems
        //{
        //    get => (IEnumerable<object>) GetValue(ToolbarItemsProperty);
        //    set => SetValue(ToolbarItemsProperty, value);
        //}
        public ObservableCollection<object> ToolbarItems { get; set; }
        public bool FlyoutIsOpened
        {
            get => (bool) GetValue(FlyoutIsOpenedProperty);
            set => SetValue(FlyoutIsOpenedProperty, value);
        }
        public WBrush FlyoutBackground
        {
            get => (WBrush) GetValue(FlyoutBackgroundProperty);
            set => SetValue(FlyoutBackgroundProperty, value);
        }
        public ContentControl ItemContent => ContentControl;
        public ObservableCollection<FlyoutItem> FlyoutItems { get; set; }

        public WeakReference<Xamarin.Forms.Shell> Shell { get; set; }

        public WpfFlyoutPage()
        {
            InitializeComponent();
            FlyoutItems = new ObservableCollection<FlyoutItem>();
            ParentWindow.Loaded += Window_LOaded;
            ToolbarItems = new ObservableCollection<object>();
            //  DataContext = this;
        }

        private void Window_LOaded(object sender, RoutedEventArgs e)
        {
            var Parentt = ParentWindow;
        }


        public void UpdateFlayoutItems()
        {
            if (Shell.TryGetTarget(out var shell))
            {
                var items = shell.Items.OfType<FlyoutItem>().ToList();
                FlyoutItems.Clear();
                foreach (var flyoutItem in items)
                {
                    FlyoutItems.Add(flyoutItem);
                    
                }

                var appbarInfo = (ParentWindow).GetType().BaseType.BaseType
                    .GetField("topAppBar", BindingFlags.Instance | BindingFlags.NonPublic);
                var appbar = appbarInfo.GetValue(ParentWindow) as FormsAppBar;
                //appbar.Background = new System.Windows.Media.SolidColorBrush(Colors.Red);
                appbar.Visibility = Visibility.Collapsed;
                //currentTitle
                //var currTileInfo = ParentWindow.GetType().BaseType.BaseType.GetProperty("CurrentTitle");
                //var borderInfo = (ParentWindow).GetType().BaseType.BaseType.GetField("BorderWindow", BindingFlags.Instance | BindingFlags.NonPublic);
                var tempalte = ParentWindow.Template;
                var border = tempalte.FindName("BorderWindow",ParentWindow) as Border;
                //var itemmm = ParentWindow.InternalChildren;
                border.BorderBrush = ResourcesHelper.PrimaryColor.ToBrush();
                var commandBar = tempalte.FindName("PART_CommandsBar", ParentWindow) as Grid;
                commandBar.Background = ResourcesHelper.PrimaryColor.ToBrush();



            }
        }

        private void Hambrger_OnClick(object sender, RoutedEventArgs e)
        {

            FlyoutIsOpened = !FlyoutIsOpened;
        }

        private void UpdatePaneVisibility()
        {
            if (FlyoutView.Visibility == Visibility.Collapsed)
            {
                FlyoutView.Visibility = Visibility.Visible;
                COntainerGrid.Visibility = Visibility.Visible;
                var animation = new ThicknessAnimation();
                var thickness = FlyoutView.Margin;
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));
                thickness.Left = 0;
                animation.To = thickness;

                var sb = new Storyboard();
                sb.Children.Add(animation);

                Storyboard.SetTarget(sb, FlyoutView);
                Storyboard.SetTargetProperty(sb, new PropertyPath(Control.MarginProperty));

                sb.Begin();


            }
            else
            {
                var animation = new ThicknessAnimation();
                var thickness = FlyoutView.Margin;
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));
                thickness.Left = -250;
                animation.To = thickness;

                var sb = new Storyboard();
                sb.Children.Add(animation);

                Storyboard.SetTarget(sb, FlyoutView);
                Storyboard.SetTargetProperty(sb, new PropertyPath(Control.MarginProperty));
                sb.Completed += (o, args) =>
                {
                    FlyoutView.Visibility = Visibility.Collapsed;
                    COntainerGrid.Visibility = Visibility.Collapsed;

                };
                sb.Begin();
                //sb. += (o, args) =>
                //{
                //    FlyoutView.Visibility = Visibility.Collapsed;
                //};
            }
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var item = e.AddedItems[0] as ShellItem;

            if (Shell.TryGetTarget(out var shell))
            {
                var shectr = ((IShellController) shell);
                shectr.OnFlyoutItemSelected(item);
                Hambrger_OnClick(this,null);
            }
        }

        public void UpdateTitle(string title)
        {
         //   if (Xamarin.Forms.Shell.GetTitleView(Page))
            PageTitleLabel.Content = title;
            CustomTitleContent.Visibility = Visibility.Collapsed;
            PageTitleLabel.Visibility = Visibility.Visible;
        }

        public void UpdateTitleContent(FrameworkElement element)
        {
            CustomTitleContent.Visibility = Visibility.Visible;
            PageTitleLabel.Visibility = Visibility.Collapsed;
            CustomTitleContent.Content = element;
        }

        private void COntainerGrid_OnTouchDown(object sender, TouchEventArgs e)
        {
            
            Hambrger_OnClick(this,null);
        }

        private void COntainerGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Hambrger_OnClick(this, null);

        }

        private void OnBackPressedMouse(object sender, MouseButtonEventArgs e)
        {

            Xamarin.Forms.Shell.Current.Navigation.PopAsync();
        }

        private void OnPrevButtonClick(object sender, RoutedEventArgs e)
        {
            Xamarin.Forms.Shell.Current.Navigation.PopAsync();


        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
        }
    }
}

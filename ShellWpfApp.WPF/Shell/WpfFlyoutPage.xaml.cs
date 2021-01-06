using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF.Controls;
using SelectionChangedEventArgs = System.Windows.Controls.SelectionChangedEventArgs;

namespace ShellWpfApp.WPF.Shell
{
    /// <summary>
    /// Interaction logic for WpfFlyoutPage.xaml
    /// </summary>
    public partial class WpfFlyoutPage
    {
        public ContentControl ItemContent => ContentControl;
        public ObservableCollection<FlyoutItem> FlyoutItems { get; set; }

        public WeakReference<Xamarin.Forms.Shell> Shell { get; set; }

        public WpfFlyoutPage()
        {
            InitializeComponent();
            FlyoutItems = new ObservableCollection<FlyoutItem>();
          //  DataContext = this;
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

                //var varInfo = (ParentWindow).GetType().BaseType.BaseType
                //    .GetField("hamburgerButton", BindingFlags.Instance | BindingFlags.NonPublic);
                //var hunInfo = varInfo.GetValue(ParentWindow) as System.Windows.Controls.Button;
                //var humBut = Template.FindName("PART_Hamburger", this) as System.Windows.Controls.Button;

                //topAppBar
                var appbarInfo = (ParentWindow).GetType().BaseType.BaseType
                    .GetField("topAppBar", BindingFlags.Instance | BindingFlags.NonPublic);
                var appbar = appbarInfo.GetValue(ParentWindow) as FormsAppBar;
                //appbar.Background = new System.Windows.Media.SolidColorBrush(Colors.Red);
                appbar.Visibility = Visibility.Collapsed;
                //currentTitle
                var currTileInfo = ParentWindow.GetType().BaseType.BaseType.GetProperty("CurrentTitle");
               // currTileInfo.SetValue(ParentWindow, "Test", null);
               //ParentWindow.SetValue(FormsWindow.CurrentTitleProperty,"Test");
                //  Title = "test";
                //hunInfo.Visibility = Visibility.Visible;
                //hunInfo.Height = 30;
                //hunInfo.Width = 30;
                //hunInfo.Background = new System.Windows.Media.SolidColorBrush(Colors.Black);
              //  ParentWindow?.SynchronizeAppBar();

                // ParentWindow?.SynchronizeToolbarCommands();

            }
        }

        private void Hambrger_OnClick(object sender, RoutedEventArgs e)
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

                Storyboard.SetTarget(sb,FlyoutView);
                Storyboard.SetTargetProperty(sb,new PropertyPath(Control.MarginProperty));

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
                sb.Completed  += (o, args) =>
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
            PageTitleLabel.Content = title;
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
    }
}

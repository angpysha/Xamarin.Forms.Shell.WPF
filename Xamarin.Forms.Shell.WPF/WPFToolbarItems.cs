using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Xamarin.Forms;
using BindingMode = System.Windows.Data.BindingMode;
using Brush = System.Windows.Media.Brush;
using Button = System.Windows.Controls.Button;
using Grid = System.Windows.Controls.Grid;
using GridLength = System.Windows.GridLength;
using GridUnitType = System.Windows.GridUnitType;
using Label = System.Windows.Controls.Label;
using RowDefinition = System.Windows.Controls.RowDefinition;
using Thickness = System.Windows.Thickness;

namespace ShellWpfApp.WPF.Shell
{
    public class WPFToolbarItems : StackPanel
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource),
            typeof(IEnumerable<object>),
            typeof(WPFToolbarItems),
            new PropertyMetadata(default, OnItemsSourcePropertyChanged));

        public static readonly DependencyProperty TitleItemBrushProperty = DependencyProperty.Register(nameof(TitleItemBrush),
            typeof(Brush),
            typeof(WPFToolbarItems),
            new FrameworkPropertyMetadata(new System.Windows.Media.SolidColorBrush(Colors.White)));


        public Brush TitleItemBrush
        {
            get => (Brush) GetValue(TitleItemBrushProperty);
            set => SetValue(TitleItemBrushProperty, value);
        }
        public IEnumerable<object> ItemsSource
        {
            get => (IEnumerable<object>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tabbar = d as WPFToolbarItems;


            var oldValue = e.OldValue as INotifyCollectionChanged;
            var newValue = e.NewValue as INotifyCollectionChanged;
            tabbar.OnItemsSourceChengedInternal(e.NewValue as IEnumerable<object>);


            if (oldValue != null)
            {
                oldValue.CollectionChanged -= tabbar.OnCollectionChanged;
            }

            if (newValue != null)
            {
                newValue.CollectionChanged += tabbar.OnCollectionChanged;

            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateTabbarCollection();
        }

        private void OnItemsSourceChengedInternal(IEnumerable<object> eNewValue)
        {

        }

        protected void UpdateTabbarCollection()
        {

            if (ItemsSource.Count() == 0)
            {
                Children.Clear();
            }
            else
            {
                var view = new Button()
                {
                    // Background = new SolidColorBrush(Colors.BlueViolet),
                    Background = new System.Windows.Media.SolidColorBrush(Colors.Transparent),
                    BorderThickness = new Thickness(0),
                    Width = 60,
                    Height = 80,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Stretch
                };

                var stk = new StackPanel()
                {
                    Width = 40,
                    Height = 40
                };
                var image = new System.Windows.Controls.Image()
                {
                    Width = 20,
                    Height = 20
                };
                stk.Children.Add(image);
                var title = ItemsSource.LastOrDefault() as ToolbarItem;
                
                var label = new Label()
                {
                    Content = title.Text,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeight.FromOpenTypeWeight(400)
                };
                var binding = new System.Windows.Data.Binding()
                {
                    Source = this,
                    Path = new PropertyPath(nameof(TitleItemBrush)),
                    Mode = BindingMode.TwoWay
                };
                label.SetBinding(Control.ForegroundProperty, binding);
                view.Click += (sender, args) =>
                {
                    (title as IMenuItemController)?.Activate();
                };
                stk.Children.Add(label);
                //var grid = new Grid();
                //grid.RowDefinitions.Add(new RowDefinition()
                //{
                //    Height = new GridLength(1,GridUnitType.Star)
                //});
                //grid.RowDefinitions.Add(new RowDefinition()
                //{
                //    Height = new GridLength(15,GridUnitType.Pixel)
                //});

                //label.VerticalAlignment = VerticalAlignment.Bottom;
                //grid.Children.Add(label);
                //Grid.SetRow(label,1);
                view.Content = stk;
                Children.Add(view);
            }

        }
    }
}

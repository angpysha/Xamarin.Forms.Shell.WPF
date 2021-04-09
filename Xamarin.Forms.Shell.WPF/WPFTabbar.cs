using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;
using Color = System.Windows.Media.Color;
using ColumnDefinition = System.Windows.Controls.ColumnDefinition;
using DataTemplate = System.Windows.DataTemplate;
using GridLength = System.Windows.GridLength;
using GridUnitType = System.Windows.GridUnitType;
using ImageSource = System.Windows.Media.ImageSource;
using Label = System.Windows.Controls.Label;
using WpfGrid = System.Windows.Controls.Grid;

namespace ShellWpfApp.WPF.Shell
{
    public class WPFTabbar : WpfGrid
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource),
            typeof(IEnumerable<object>),
            typeof(WPFTabbar),
            new PropertyMetadata(default, OnItemsSourcePropertyChanged));

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(WPFTabbar),
            new PropertyMetadata(default));

        public static readonly DependencyProperty ActiveColorProperty = DependencyProperty.Register(nameof(ActiveColor),
            typeof(System.Windows.Media.Brush),
            typeof(WPFTabbar),
            new PropertyMetadata(new System.Windows.Media.SolidColorBrush(Colors.White)));

        public static readonly DependencyProperty UnselectedColorProperty = DependencyProperty.Register(nameof(UnselectedColor),
            typeof(System.Windows.Media.Brush),
            typeof(WPFTabbar),
            new PropertyMetadata(new System.Windows.Media.SolidColorBrush(Colors.White)));

        public event EventHandler<BaseShellItem> OnItemClick = delegate { };

        public System.Windows.Media.Brush UnselectedColor
        {
            get => (System.Windows.Media.Brush)GetValue(UnselectedColorProperty);
            set => SetValue(UnselectedColorProperty, value);
        }

        public System.Windows.Media.Brush ActiveColor
        {
            get => (System.Windows.Media.Brush)GetValue(ActiveColorProperty);
            set => SetValue(ActiveColorProperty, value);
        }
        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        //    var tabbar = d as WPFTabbar;


        //    var oldValue = e.OldValue as INotifyCollectionChanged;
        //    var newValue = e.NewValue as INotifyCollectionChanged;
        //    tabbar.OnItemsSourceChengedInternal(e.NewValue as IEnumerable<object>);


        //    if (oldValue != null)
        //    {
        //        oldValue.CollectionChanged -= tabbar.OnCollectionChanged;
        //    }

        //    if (newValue != null)
        //    {
        //        newValue.CollectionChanged += tabbar.OnCollectionChanged;

        //    }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateTabbarCollection();
        }

        protected virtual void UpdateTabbarCollection()
        {
            //if (ItemsSource.Count() == 0)
            //{
            //    Children.Clear();
            //    ColumnDefinitions.Clear();
            //}
            //else
            //{
            //    ColumnDefinitions.Add(new ColumnDefinition()
            //    {
            //        Width = new GridLength(1, GridUnitType.Star)
            //    });

            //    if (ItemTemplate != null)
            //    {
            //        var view = ItemTemplate.LoadContent();
            //        //var renderer = Platform.GetOrCreateRenderer(view);

            //    }
            //    else
            //    {
            //        var view = new WpfGrid()
            //        {
            //            Height = 40,
            //            HorizontalAlignment = HorizontalAlignment.Stretch
            //        };
            //        var item = ((BaseShellItem)ItemsSource?.LastOrDefault());
            //        view.DataContext = item;
            //        //  var titleProperty = ItemsSource.LastOrDefault().GetType();
            //        var title = item?.Title;
            //        view.Children.Add(new Border()
            //        {
            //            Background = this.Background
            //        });
            //        view.Children.Add(new Label()
            //        {
            //            Content = title,
            //            HorizontalAlignment = HorizontalAlignment.Center,
            //            VerticalAlignment = VerticalAlignment.Bottom,
            //            FontSize = 13,
            //            Foreground = UnselectedColor
            //        });

            //        view.MouseDown += ViewOnMouseDown;
            //        WpfGrid.SetColumn(view, ColumnDefinitions.Count - 1);

            //        Children.Add(view);

            //    }
            //}
        }

        public void SetActiveTab(int index, int prevIndex = 0)
        {
            int i = 0;
            foreach (UIElement child in Children)
            {
                var grid = child as WpfGrid;
                var item = grid.Children.OfType<Label>().FirstOrDefault();
                if (item != null)
                {
                    item.FontWeight = i == index ?FontWeight.FromOpenTypeWeight(600): FontWeight.FromOpenTypeWeight(400);
                    item.Foreground = i == index ?ActiveColor : UnselectedColor;
                }

                i++;
            }
        }

        private void ViewOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var view = sender as WpfGrid;
            var binding = view.DataContext as BaseShellItem;

            OnItemClick(this, binding);

        }

        internal void OnItemsSourceChengedInternal(IEnumerable<object> items)
        {
            UpdateTabbarCollection();
        }

        public IEnumerable<object> ItemsSource
        {
            get => (IEnumerable<object>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);

        }

        public async void SetTabItems(IList<ShellSection> items)
        {

            foreach (var item in items)
            {
                ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });

                if (ItemTemplate != null)
                {
                    var view = ItemTemplate.LoadContent();
                    //var renderer = Platform.GetOrCreateRenderer(view);

                }
                else
                {
                    var view = new WpfGrid()
                    {
                        Height = 40,
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };
                    view.DataContext = item;
                    //  var titleProperty = ItemsSource.LastOrDefault().GetType();
                    var title = item?.Title;
                    var image = await item?.Icon.ToWindowsImageSourceAsync();
                    view.Children.Add(new Border()
                    {
                        Background = this.Background
                    });
                    view.Children.Add(new System.Windows.Controls.Image()
                    {
                        Source = image,
                        Height = 20,
                        Width = 20,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Center
                    });
                    view.Children.Add(new Label()
                    {
                        Content = title,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Bottom,
                        FontSize = 13,
                        Foreground = UnselectedColor,
                        FontWeight = FontWeight.FromOpenTypeWeight(400)
                    });

                    view.MouseDown += ViewOnMouseDown;
                    WpfGrid.SetColumn(view, ColumnDefinitions.Count - 1);

                    Children.Add(view);
                }
            }
        }
    }

    public class MenuItem
    {
        public string Title { get; set; }
        public ImageSource Icone { get; set; }
        public Color ActiveColor { get; set; }

        public Action<object> OnTap { get; set; }
    }
}

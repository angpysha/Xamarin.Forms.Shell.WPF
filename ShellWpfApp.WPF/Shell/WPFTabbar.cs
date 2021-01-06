using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfGrid = System.Windows.Controls.Grid;

namespace ShellWpfApp.WPF.Shell
{
    public class WPFTabbar : WpfGrid
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource),
            typeof(IEnumerable<object>),
            typeof(WPFTabbar),
            new PropertyMetadata(default,OnItemsSourcePropertyChanged));

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(WPFTabbar),
            new PropertyMetadata(default));

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }




        public IEnumerable<object> ItemsSource
        {
            get => (IEnumerable<object>) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate) GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);

        }
    }
}

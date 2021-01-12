using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Xamarin.Forms.ShellWPF
{
    /// <summary>
    /// Interaction logic for FlyoutItemView.xaml
    /// </summary>
    public partial class FlyoutItemView : UserControl
    {
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(FlyoutItemView),
            new PropertyMetadata(OnItemTemplateChanged));


        public DataTemplate ItemTemplate
        {
            get => (DataTemplate) GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var itemView = d as FlyoutItemView;
            var oldTemplate = e.OldValue as DataTemplate;
            var newTemplate = e.NewValue as DataTemplate;
            if (oldTemplate != newTemplate)
            {
                itemView.SetView();
            }
        }


        public FlyoutItemView()
        {
            InitializeComponent();
          //  SetView();
        }

        private void SetView()
        {
            if (ItemTemplate == null)
                return;
            
            var content = ItemTemplate.CreateContent() as View;
            SetView(content);
        }

        private View _xfView;
        public void SetView(View xfView)
        {
            _xfView = xfView;
            _xfView.BindingContext = DataContext;
            var renderer = Platform.WPF.Platform.GetOrCreateRenderer(xfView);
            var nativeView = renderer.GetNativeElement();
            Content = nativeView;
            nativeView.Loaded -= NativeViewOnLoaded;
            nativeView.Loaded += NativeViewOnLoaded;
        }

        private void NativeViewOnLoaded(object sender, RoutedEventArgs e)
        {

            var el = sender as FrameworkElement;
            _xfView.Layout(new Rectangle(0,0,el.ActualWidth,el.ActualHeight));
        }
    }
}

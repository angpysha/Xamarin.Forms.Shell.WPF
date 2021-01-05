using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;
using Page = System.Windows.Controls.Page;

namespace ShellWpfApp.WPF.Shell
{
    /// <summary>
    /// Interaction logic for ShellPageWrapper.xaml
    /// </summary>
    public partial class ShellPageWrapper : Page
    {
        public Xamarin.Forms.Page Page { get; set; }
        public ShellPageWrapper()
        {
            InitializeComponent();
            LoadPage();
        }

        public void LoadPage()
        {

            if (Page != null)
            {
                //CalendarDateChangedEventArgs render = Page.GetOrCreateRenderer();
                var renderer = Platform.GetOrCreateRenderer(Page).GetNativeElement();
                Root.Content = renderer;
                renderer.Loaded -= RendererOnLoaded;
                renderer.Loaded += RendererOnLoaded;

            }
        }

        private void RendererOnLoaded(object sender, RoutedEventArgs e)
        {

            var el = sender as FrameworkElement;
            Page.Layout(new Xamarin.Forms.Rectangle(0,0,el.ActualWidth,el.ActualHeight));

        }
    }
}

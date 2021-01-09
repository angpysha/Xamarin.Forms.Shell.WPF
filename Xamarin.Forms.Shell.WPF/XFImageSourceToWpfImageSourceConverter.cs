using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.WPF;

namespace ShellWpfApp.WPF.Shell
{
    public class XFImageSourceToWpfImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Xamarin.Forms.ImageSource imageSource)
            {
                IImageSourceHandler handler;
                if (imageSource != null && (handler = Registrar.Registered.GetHandlerForObject<IImageSourceHandler>(imageSource)) != null)
                {
                    ImageSource wpfImageSource;
                    try
                    {
                        wpfImageSource = handler.LoadImageAsync(imageSource).GetAwaiter().GetResult();
                        return wpfImageSource;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

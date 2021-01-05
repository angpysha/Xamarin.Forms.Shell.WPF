using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ShellAppWPF.Helpers;
using Xamarin.Forms.Platform.WPF;

namespace ShellWpfApp.WPF
{
    public static class Contants
    {
        public static Brush PrimaryColor
        {
            get
            {
                var xfColor = ResourcesHelper.PrimaryColor;
                var color = xfColor.ToBrush();
                return color;
            }
        }
    }
}

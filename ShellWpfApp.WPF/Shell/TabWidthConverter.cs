using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ShellWpfApp.WPF.Shell
{
    public class TabWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2)
            {
                var fst = values[0].ToString();
                var scd = values[1].ToString();
                if (double.TryParse(fst, out var fstDbl) && int.TryParse(scd, out var scdInt))
                {
                    var width = (fstDbl*0.8) / scdInt;
                    return width;
                }
                
            }
            return 50;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

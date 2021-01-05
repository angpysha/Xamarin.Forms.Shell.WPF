using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace ShellAppWPF.Helpers
{
    public static class ResourcesHelper
    {
        public static ResourceDictionary mergedDict;

        private static bool TrySetDictionary()
        {
            if (Application.Current.Resources.MergedDictionaries.Any())
            {
                mergedDict = Application.Current.Resources.MergedDictionaries.FirstOrDefault();
                return true;
            }

            return false;
        }

        public static Color PrimaryColor
        {
            get
            {
                if (TrySetDictionary())
                {
                    if (mergedDict.TryGetValue("Primary", out object color))
                    {
                        return (Color)color;
                    }
                }
                else if (Application.Current.Resources.ContainsKey("Primary"))
                {
                    return (Color) Application.Current.Resources["Primary"];
                }

                return default;
            }
        }
    }
}

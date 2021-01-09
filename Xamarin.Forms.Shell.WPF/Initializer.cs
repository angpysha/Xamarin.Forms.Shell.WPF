using System;
using System.Collections.Generic;
using System.Text;
using ShellWpfApp.WPF.Shell;

namespace Xamarin.Forms.ShellWPF
{
    public static class Xamarin
    {
        public static void Init()
        {
            Console.WriteLine($"{typeof(ShellRenderer).FullName} loaded");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ShellWpfApp.WPF.Shell;

namespace Xamarin.Forms.ShellWPF
{
    public static class Xamarin
    {
        public static void Init()
        {
            Console.WriteLine($"{typeof(ShellRenderer).FullName} loaded");
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(new System.Windows.ResourceDictionary
            {
                Source = new Uri(string.Format("/{0};component/ShellWPFResources.xaml", assemblyName), UriKind.Relative)
            });
        }
    }
}

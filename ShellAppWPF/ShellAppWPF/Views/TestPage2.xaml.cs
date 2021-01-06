using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShellAppWPF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage2 : ContentPage
    {
        public TestPage2()
        {
            InitializeComponent();
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {

            await Shell.Current.GoToAsync($"{nameof(TestPage3)}");
        }
    }
}
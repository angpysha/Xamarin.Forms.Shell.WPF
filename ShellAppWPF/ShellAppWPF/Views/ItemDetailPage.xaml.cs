using ShellAppWPF.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace ShellAppWPF.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
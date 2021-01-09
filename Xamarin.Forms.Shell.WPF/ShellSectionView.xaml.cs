using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Xaml.Behaviors.Core;
using Xamarin.Forms;

namespace ShellWpfApp.WPF.Shell
{
    /// <summary>
    /// Interaction logic for ShellSectionView.xaml
    /// </summary>
    public partial class ShellSectionView 
    {
        public ObservableCollection<ShellContent> ShellContents { get; set; }

        protected ShellSection ShellSection { get; set; }

        protected IShellSectionController ShellSectionController => ShellSection as IShellSectionController;

        public ICommand TopTabPressedCommand { get; set; }

        public ShellSectionView()
        {
            InitializeComponent();
            ShellContents = new ObservableCollection<ShellContent>();

            TopTabPressedCommand = new ActionCommand(OnTopTabPressed);
        }

        protected virtual void OnTopTabPressed(object obj)
        {
            
        }

        private void TopTabBar_OnItemClick(object sender, BaseShellItem e)
        {
            OnTopTabPressed(e);
        }
    }
}

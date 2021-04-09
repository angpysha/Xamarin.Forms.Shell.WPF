using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ShellWpfApp.Helpers;
using ShellWpfApp.WPF.Annotations;
using Xamarin.Forms;
using SelectionChangedEventArgs = System.Windows.Controls.SelectionChangedEventArgs;

namespace ShellWpfApp.WPF.Shell
{
    /// <summary>
    /// Interaction logic for ShellItemView.xaml
    /// </summary>
    public partial class ShellItemView :  INotifyPropertyChanged
    {
        internal ShellItem ShellItem { get; set; }
        IShellItemController ShellItemController => ShellItem;

        private ObservableCollectionEx<ShellSection> _shellSections;

        public ObservableCollectionEx<ShellSection> ShellSections
        {
            get => _shellSections;
            set
            {
                _shellSections = value;
                RaisePropertyChanged(nameof(ShellSections));
            }
        }

        public virtual float TabWidth { get; }

        public virtual int ItemsCount { get; }

        private ICommand tabClickCommand;

        public ICommand TabClickCommand
        {
            get
            {
                if (tabClickCommand == null)
                {
                    tabClickCommand = new ActionCommand(OnTabClickedAction);
                }

                return tabClickCommand;
            }
        }

        private void OnTabClickedAction(object obj)
        {
            if (obj is ShellSection tabSection)
            {
                ShellItemController.ProposeSection(tabSection);
            }
        }

        //public ActionCommand TabClieckCommand = new ActionCommand(OnTabCLicked);

        //protected virtual void OnTabCLicked(object value)
        //{

        //}

        public ShellItemView()
        {
            InitializeComponent();
            ShellSections = new ObservableCollectionEx<ShellSection>();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var iii = 0;
            //var item = e.AddedItems[0];
            //var section = item as ShellSection;
            //ShellItemController.ProposeSection(section);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void WpfTabbar_OnOnItemClick(object sender, BaseShellItem e)
        {
            
            OnTabClickedAction(e);
        }
    }
}

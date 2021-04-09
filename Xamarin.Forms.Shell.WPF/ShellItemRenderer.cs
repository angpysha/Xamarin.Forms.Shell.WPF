using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ShellWpfApp.Helpers;
using ShellWpfApp.WPF.Annotations;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.WPF;
using WpfGrid = System.Windows.Controls.Grid;
using WBrush = System.Windows.Media.Brush;

namespace ShellWpfApp.WPF.Shell
{
    public class ShellItemRenderer : ShellItemView,IAppearanceObserver, INotifyPropertyChanged
    {
        
        internal ShellRenderer ShellContext { get; set; }

        private ShellSection _shellSection;

        private Page DisplayedPage;
        IShellItemController ShellItemController => ShellItem as IShellItemController;

        private ShellSection ShellSection
        {
            get => _shellSection;
            set
            {
                if (_shellSection == value)
                    return;
                var oldValue = _shellSection;

                if (_shellSection != null)
                {
                    ((IShellSectionController) _shellSection).RemoveDisplayedPageObserver(this);
                }
                _shellSection = value;

                if (value != null)
                {
                    OnShellSectionChanged(oldValue, value);
                    ((IShellSectionController)value).AddDisplayedPageObserver(this,UpdateDisplayedPage);
                }
                UpdateBottomBar();
            }
        }

        private void OnShellSectionChanged(ShellSection oldValue, ShellSection value)
        {
            
            ShellSectionRenderer.NavigateToShellSection(value);
        }

        private void UpdateDisplayedPage(Page obj)
        {
            if (DisplayedPage != null)
            {
                DisplayedPage.PropertyChanged -= DisplayedPageOnPropertyChanged;
            }

            DisplayedPage = obj;

            if (DisplayedPage != null)
            {
                DisplayedPage.PropertyChanged += DisplayedPageOnPropertyChanged;
            }

            UpdateBottomBarVisibility();
            UpdatePageTitle();
            UpdateToolbar();
        }

        private void UpdateToolbar()
        {
            

        }

        private void UpdatePageTitle()
        {
            

        }

        private void DisplayedPageOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == Shell.TabBarIsVisibleProperty.PropertyName)
            //{
            //    UpdateBottomBarVisibility();
            //}
            //else if (e.PropertyName == Page.TitleProperty.PropertyName)
            //{
            //    UpdatePageTitle();
            //}
            //else if (e.PropertyName == Shell.NavBarIsVisibleProperty.PropertyName)
            //{
            //    UpdateNavBarVisibility();
            //}

        }

        public override float TabWidth => (float)(BottomBarView.ActualWidth / ShellItem.Items?.Count ?? 0);

        public override int ItemsCount => ShellItem.Items?.Count ?? 0;

      //  public ShellItem ShreItem { get; set; }
        IShellController ShellController => ShellContext?.Element;

        private ShellSectionRenderer ShellSectionRenderer { get; set; }

        public ShellItemRenderer()
        {
            //  Background = new System.Windows.Media.SolidColorBrush(Colors.Red);
          SectionControl.Content = ShellSectionRenderer = new ShellSectionRenderer();
          
        //  ShellSectionRenderer.Shell = ShellContext?.Element;
        }

        public void SetShellContext(ShellRenderer shellRenderer)
        {

            if (ShellContext != null)
            {
                ((IShellController) ShellContext.Element).RemoveAppearanceObserver(this);
            }
            ShellContext = shellRenderer;
            if (ShellContext != null)
            {
                ((IShellController)ShellContext.Element).AddAppearanceObserver(this,ShellContext.Element);
            }

        }


        public void OnAppearanceChanged(ShellAppearance appearance)
        {
            WBrush tabbarBackgroundColor = new System.Windows.Media.SolidColorBrush(Colors.White);
            WBrush tabbarForegroundColor = new System.Windows.Media.SolidColorBrush(Colors.Black);
            WBrush tabbarTitleColor = new System.Windows.Media.SolidColorBrush(Colors.Black);
            if (appearance != null)
            {
                var a = (IShellAppearanceElement) appearance;
                tabbarBackgroundColor = a.EffectiveTabBarBackgroundColor.ToBrush();
                tabbarForegroundColor = a.EffectiveTabBarForegroundColor.ToBrush();
                tabbarTitleColor = a.EffectiveTabBarTitleColor.ToBrush();
            }

            ShellContext.Control.TitleBarBackgroundColor = tabbarBackgroundColor;
            ShellContext.Control.TabBarForeground = tabbarForegroundColor;
            if (ShellSection is IAppearanceObserver iao)
            {
                iao.OnAppearanceChanged(appearance);
            }
        }

        public void NavigateToShellItem(ShellItem shellItem, bool animated = false)
        {
           // var 
           UnHookEvents(shellItem);

           if (shellItem?.CurrentItem?.CurrentItem == null)
               return;
           
           ShellItem = shellItem;
           ShellSection = shellItem.CurrentItem;
           HookEvents(ShellItem);
        }

        private void HookEvents(ShellItem shellItem)
        {
            if (ShellItem == null)
                return;
            shellItem.PropertyChanged += ShellItemOnPropertyChanged;
            ShellController.StructureChanged += ShellControllerOnStructureChanged;
            ShellItemController.ItemsCollectionChanged += ShellItemControllerOnItemsCollectionChanged;
            foreach (var child in ShellItemController.GetItems())
            {
                HookChildEvents(child);
            }
        }

        private void ShellItemControllerOnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            

        }

        private void HookChildEvents(ShellSection child)
        {
            
            ((IShellSectionController)child).NavigationRequested += OnNavigationRequested;
        }

        private void UnHookChildEvents(ShellSection child)
        {

            ((IShellSectionController)child).NavigationRequested -= OnNavigationRequested;
        }

        private void OnNavigationRequested(object sender, NavigationRequestedEventArgs e)
        {
            
           ShellSectionRenderer.NavigateToContent(e, (ShellSection) sender);
        }


        private void UnHookEvents(ShellItem shellItem)
        {
            if (ShellItem == null)
                return;
            foreach (var child in ((IShellItemController)shellItem).GetItems())
            {
                UnHookChildEvents(child);
            }
            shellItem.PropertyChanged -= ShellItemOnPropertyChanged;
            ShellController.StructureChanged -= ShellControllerOnStructureChanged;
            ShellItemController.ItemsCollectionChanged -= ShellItemControllerOnItemsCollectionChanged;

        }

        private void ShellControllerOnStructureChanged(object sender, EventArgs e)
        {
            UpdateBottomBarVisibility();
        }

        private void ShellItemOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ShellItem.CurrentItemProperty.PropertyName)
            {
                ShellSection = ShellItem.CurrentItem;
                //ShellSectionRenderer.NavigateToShellSection(ShellSection);
            }

            //if (e.PropertyName == Xamarin.Forms.Shell.TabBarIsVisibleProperty.PropertyName)
            //{
            //    UpdateBottomBarVisibility();
            //}
        }

        public void InitShellData()
        {

            ShellSectionRenderer.Shell = ShellContext?.Element;
            ShellSectionRenderer.ShellContext = ShellContext;
            ShellSectionRenderer.ShellItem = ShellItem;
        }

        public void UpdateData()
        {
            RaisePropertyChanged(nameof(ItemsCount));
        }

        private void UpdateBottomBarVisibility()
        {
            var visible = ((IShellItemController) ShellItem)?.ShowTabs ?? false;
            BottomBarView.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            if (visible)
            {
                //Grid.SetRowSpan(SectionGrid,1);
                WpfGrid.SetRowSpan(SectionGrid,1);
            }
            else
            {
                WpfGrid.SetRowSpan(SectionGrid,2);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateBottomBar()
        {
            if (ShellSections == null || ShellSections.Any() == false)
            {
                InitTabItems();
            }
            var tabBarColor = Xamarin.Forms.Shell.GetTabBarBackgroundColor(ShellItem);
            var activeColor = Xamarin.Forms.Shell.GetTabBarForegroundColor(ShellItem);
            var unselectedColor = Xamarin.Forms.Shell.GetTabBarUnselectedColor(ShellItem);
          //  WpfTabbar.Background = tabBarColor.ToBrush();
            WpfTabbar.UnselectedColor = unselectedColor.ToBrush();
            WpfTabbar.ActiveColor = activeColor.ToBrush();
            WpfTabbar.SetActiveTab(ShellSections.IndexOf(ShellSection));
        }

        private void InitTabItems()
        {
            var items = ShellItem.Items;
            ShellSections.AddRange(items);
            WpfTabbar.SetTabItems(items);
        }
    }
}

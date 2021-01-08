using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.WPF;
using WpfGrid = System.Windows.Controls.Grid;

namespace ShellWpfApp.WPF.Shell
{
    public class ShellSectionRenderer : ShellSectionView, IAppearanceObserver
    {


        public Xamarin.Forms.Shell Shell { get; set; }

        public ShellRenderer ShellContext { get; set; }

        private Page Page;

        public ShellItem ShellItem { get; set; }

        public List<Page> PagesNavigationStack;

        public ShellContent CurrentContent { get; set; }

        public void OnAppearanceChanged(ShellAppearance appearance)
        {

        }

        public async void NavigateToShellSection(ShellSection shellSection)
        {

            if (ShellSection != shellSection)
            {
                if (ShellSection != null)
                {
                    ShellSection.PropertyChanged -= ShellSectionOnPropertyChanged;
                    ShellSectionController.ItemsCollectionChanged -= ShellSectionControllerOnItemsCollectionChanged;
                    ShellSection = null;
                }

                ShellSection = shellSection;
                ShellSection.PropertyChanged += ShellSectionOnPropertyChanged;
                ShellSectionController.ItemsCollectionChanged += ShellSectionControllerOnItemsCollectionChanged;
            }

            UpdateTopTabs();

            var content = shellSection.CurrentItem;
            Page nextPage = (shellSection as IShellSectionController)
                .PresentedPage ?? ((IShellContentController)content)?.GetOrCreateContent();

            SectionFrame.Navigate(new ShellPageWrapper());
            await Task.Delay(100);
            var shellSectionCurrent = Shell.CurrentItem.CurrentItem;
            NavigateToContent(new NavigationRequestedEventArgs(nextPage, true), shellSectionCurrent);
        }

        //private void OnLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void ShellSectionControllerOnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {


        }

        private void ShellSectionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ShellSection.CurrentItemProperty.PropertyName)
            {
                NavigateToShellSection(ShellSection);
            }

        }

        //protected virtual NavigationTransitionInfo GetTransitionInfo(ShellNavigationSource navSource)
        //{
        //    switch (navSource)
        //    {
        //        case ShellNavigationSource.Push:
        //            return new SlideNavigationTransitionInfo(); // { Effect = SlideNavigationTransitionEffect.FromRight }; Requires SDK 17763
        //        case ShellNavigationSource.Pop:
        //        case ShellNavigationSource.PopToRoot:
        //            return new SlideNavigationTransitionInfo(); // { Effect = SlideNavigationTransitionEffect.FromLeft }; Requires SDK 17763
        //        case ShellNavigationSource.ShellSectionChanged:
        //            return null;
        //    }

        //    return null;
        //}
        public void UpdateTopTabs()
        {
            ShellContents.Clear();
            foreach (var shellContent in ShellSection.Items)
            {
                ShellContents.Add(shellContent);
            }
        }

        private void UpdateTopTabsVisisbility()
        {
            if (ShellSection.Items.Count > 1 && PagesNavigationStack.Count == 1)
            {
                TopTabContainer.Visibility = Visibility.Visible;
                WpfGrid.SetRow(SectionFrame, 1);
                WpfGrid.SetRowSpan(SectionFrame, 1);
            }
            else
            {
                TopTabContainer.Visibility = Visibility.Collapsed;
                WpfGrid.SetRow(SectionFrame, 0);
                WpfGrid.SetRowSpan(SectionFrame, 2);
            }
        }

        public async void NavigateToContent(NavigationRequestedEventArgs args, ShellSection shellSection)
        {
            Page nextPage = null;
            ShellContent shellContent = shellSection.CurrentItem;
            if (args.RequestType == NavigationRequestType.PopToRoot)
            {
                nextPage = (shellContent as IShellContentController).GetOrCreateContent();
            }
            else
            {
                nextPage = (shellSection as IShellSectionController)
                    .PresentedPage ?? ((IShellContentController)shellContent)?.GetOrCreateContent();
            }

            if (CurrentContent != null && Page != null)
            {
                Page.PropertyChanged -= OnPagePropertyChanged;
                ((IShellContentController)CurrentContent)?.RecyclePage(Page);
            }

            CurrentContent = shellContent;
            if (nextPage != null)
            {
                Page = nextPage;
                Page.PropertyChanged += OnPagePropertyChanged;
                switch (args.RequestType)
                {
                    case NavigationRequestType.Insert:
                        OnInsertRequested(args);
                        break;
                    case NavigationRequestType.Pop:
                        OnPopRequested(args);
                        break;
                    case NavigationRequestType.Unknown:
                        break;
                    case NavigationRequestType.Push:
                        OnPushRequested(args);
                        break;
                    case NavigationRequestType.PopToRoot:
                        //  OnPopToRootRequested(args);
                        break;
                    case NavigationRequestType.Remove:
                        //   OnRemoveRequested(args);
                        break;
                }

                await Task.Delay(100);
                var wrapper = (ShellPageWrapper)(SectionFrame.Content);
                if (wrapper.Page == null)
                {
                    wrapper.Page = Page;
                }

                wrapper.LoadPage();
                PagesNavigationStack = ShellSection.Stack.ToList();
                UpdateTopTabsVisisbility();
                OnPageContentChanged();
                UpdateTopTabsAppearence();
                UpdateTitle();
                UpdateToolbarItems();
            }
        }

        private void UpdateToolbarItems()
        {
            
            ShellContext.Control.ToolbarItems.Clear();
            if (Page.ToolbarItems != null && Page.ToolbarItems.Any())
            foreach (var item in Page.ToolbarItems)
            {
                    ShellContext.Control.ToolbarItems.Add(item);
            }
        }

        private void UpdateTitle()
        {
            var titleView = Xamarin.Forms.Shell.GetTitleView(Page);
            if (titleView != null)
            {
                var renderer = Platform.GetOrCreateRenderer(titleView);
                var nativeView = renderer.GetNativeElement();
                nativeView.Loaded += (sender, args) =>
                {
                    var fe = sender as FrameworkElement;
                    titleView.Layout(new Rectangle(0, 0, fe.ActualWidth, fe.ActualHeight));
                };
                ShellContext.Control.UpdateTitleContent(nativeView);

            }
            else
            {
                ShellContext.Control.UpdateTitle(Page.Title);
            }

        }

        private void UpdateTopTabsAppearence()
        {
            var background = Xamarin.Forms.Shell.GetBackgroundColor(ShellContext.Element.CurrentItem);
            var foreground = Xamarin.Forms.Shell.GetForegroundColor(ShellContext.Element.CurrentItem);
            var unselectedColor = Xamarin.Forms.Shell.GetTabBarUnselectedColor(ShellContext.Element.CurrentItem);
            TopTabBar.Background = background.ToBrush();
            TopTabBar.ActiveColor = foreground.ToBrush();
            TopTabBar.UnselectedColor = unselectedColor.ToBrush();
            var index = ShellContext.Element.CurrentItem.CurrentItem.Items.IndexOf(CurrentContent);
            TopTabBar.SetActiveTab(index);
        }
        private void OnPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Page.TitleProperty.PropertyName)
            {
                UpdateTitle();

            }
            else if (e.PropertyName == Xamarin.Forms.Shell.NavBarHasShadowProperty.PropertyName)
            {
                //ShellContext.Control.NavBarContainer.Visibility = Xamarin.Forms.Shell.GetTabBarIsVisible(Page)
                //    ? Visibility.Visible
                //    : Visibility.Collapsed;

            }
            else if (e.PropertyName == Xamarin.Forms.Shell.TabBarIsVisibleProperty.PropertyName)
            {

            }
            else if (e.PropertyName == Xamarin.Forms.Shell.TitleViewProperty.PropertyName)
            {
                UpdateTitle();
            }

        }

        private void OnInsertRequested(NavigationRequestedEventArgs args)
        {
            var pageIndex = ShellSection.Stack.ToList().IndexOf(args.Page);

            if (pageIndex == ((IEnumerable<object>) SectionFrame.BackStack).Count() - 1)
            {
                SectionFrame.Navigate(new ShellPageWrapper());
            }
            else
            {
              //  ((IEnumerable<object>)SectionFrame.BackStack).
            }
        }

       

        private async void OnPopRequested(NavigationRequestedEventArgs args)
        {
            SectionFrame.GoBack();
        }

        private void OnPageContentChanged()
        {

            if (PagesNavigationStack.Count > 1)
            {
                ShellContext.Control.BackContainer.Visibility = Visibility.Visible;
            }
            else
            {
                ShellContext.Control.BackContainer.Visibility = Visibility.Collapsed;

            }
        }

        private async void OnPushRequested(NavigationRequestedEventArgs args)
        {
            SectionFrame.Navigate(new ShellPageWrapper());
        }

        protected override void OnTopTabPressed(object obj)
        {
            var result = (Shell as IShellController)?.ProposeNavigation(ShellNavigationSource.ShellContentChanged, ShellItem,
                ShellSection, obj as ShellContent, null, true);
            if (result != null && result == true)
            {
                ShellSection.SetValueFromRenderer(ShellSection.CurrentItemProperty, obj as ShellContent);
            }
        }

        public void NavigateToContent(ShellContent shellContent, bool animated = false)
        {
            var content = ((IShellContentController)shellContent).GetOrCreateContent();
            if (content != null)
            {
                Page = content;
            }
            int ff = 0;
        }
    }
}

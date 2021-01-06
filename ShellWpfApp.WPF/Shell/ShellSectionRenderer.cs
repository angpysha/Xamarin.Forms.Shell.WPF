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
            //ShellSection = shellSection;
           
            UpdateTopTabs();

            var content = shellSection.CurrentItem;
            Page nextPage = (shellSection as IShellSectionController)
                .PresentedPage ?? ((IShellContentController)content)?.GetOrCreateContent();
           // SectionFrame.LoadCompleted += OnLoadCompleted;
           //TODO: Memory leak
           var sempahoreSlim = new SemaphoreSlim(0);
            //SectionFrame.LoadCompleted += (sender, args) =>
            //{

            //    //var shellSectionCurrent = Shell.CurrentItem.CurrentItem;
            //    //NavigateToContent(new NavigationRequestedEventArgs(nextPage, true), shellSectionCurrent);

            //  //  sempahoreSlim.Release();
            //    //semaphoreSlim.Release();
            //    //   int iii = 0;
            //};
            SectionFrame.Navigate(new ShellPageWrapper());
            //await sempahoreSlim.WaitAsync();
         
            await Task.Delay(500);
            var shellSectionCurrent = Shell.CurrentItem.CurrentItem;
            NavigateToContent(new NavigationRequestedEventArgs(nextPage, true), shellSectionCurrent);
            //   semaphoreSlim.Wait();
            //NavigateToContent(new NavigationRequestedEventArgs(nextPage, true), shellSection);
            //NavigateToContent(content);

            //void OnLoadCompleted(object sendr, System.Windows.Navigation.NavigationEventArgs eventArgs)
            //{
            //    var shellSectionCurrent = Shell.CurrentItem.CurrentItem;
            //    NavigateToContent(new NavigationRequestedEventArgs(nextPage, true), shellSectionCurrent);
            //    SectionFrame.LoadCompleted -= OnLoadCompleted;
            //}
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
                    //    OnInsertRequested(args);
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

                await Task.Delay(500);
                var wrapper = (ShellPageWrapper) (SectionFrame.Content);
                if (wrapper.Page == null)
                {
                    wrapper.Page = Page;
                }
                
                wrapper.LoadPage();
                ShellContext.Control.UpdateTitle(Page.Title);
                PagesNavigationStack = ShellSection.Stack.ToList();
                UpdateTopTabsVisisbility();
                OnPageContentChanged();
                //  UpdateSearchHandler(Shell.GetSearchHandler(Page));
                //var wrapper = (ShellPageWrapper)(Frame.Content);
                //if (wrapper.Page == null)
                //{
                //    wrapper.Page = Page;
                //}

                //wrapper.LoadPage();
                //FormsNavigationStack = ShellSection.Stack.ToList();
            }
        }

        private void OnPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            

        }

        private async void OnPopRequested(NavigationRequestedEventArgs args)
        {

            var semaphoreSlim = new SemaphoreSlim(0);
            SectionFrame.LoadCompleted += (sender, eventArgs) =>
            {
                semaphoreSlim.Release();
            };

            SectionFrame.GoBack();
            await semaphoreSlim.WaitAsync();
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
            var semaphoreSlim = new SemaphoreSlim(0);
            //SectionFrame.LoadCompleted += (sender, eventArgs) =>
            //{
            //    semaphoreSlim.Release();
            //};
            SectionFrame.Navigate(new ShellPageWrapper());
          //  await semaphoreSlim.WaitAsync();
        }

        protected override void OnTopTabPressed(object obj)
        {
            var result = (Shell as IShellController)?.ProposeNavigation(ShellNavigationSource.ShellContentChanged, ShellItem,
                ShellSection, obj as ShellContent, null, true);
            if (result!=null && result == true)
            {
                ShellSection.SetValueFromRenderer(ShellSection.CurrentItemProperty, obj as ShellContent);
            }
        }

        public void NavigateToContent(ShellContent shellContent, bool animated = false)
        {
            var content = ((IShellContentController) shellContent).GetOrCreateContent();
            if (content != null)
            {
                Page = content;
            }
            int ff = 0;
        }
    }
}

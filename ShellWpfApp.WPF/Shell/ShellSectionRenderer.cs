using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        private ShellSection ShellSection { get; set; }


        public void OnAppearanceChanged(ShellAppearance appearance)
        {
            
        }

        public async void NavigateToShellSection(ShellSection shellSection)
        {
            var semaphoreSlim = new SemaphoreSlim(0);
            ShellSection = shellSection;
            SectionFrame.LoadCompleted += (sender, args) =>
            {
                //semaphoreSlim.Release();
                //   int iii = 0;
            };
            var content = shellSection.CurrentItem;
            Page nextPage = (shellSection as IShellSectionController)
                .PresentedPage ?? ((IShellContentController)content)?.GetOrCreateContent();
            //SectionFrame.LoadCompleted += (sender, args) =>
            //{
 
            //        var shellSectionCurrent = Shell.CurrentItem.CurrentItem;
            //        NavigateToContent(new NavigationRequestedEventArgs(nextPage, true), shellSectionCurrent);


            //    //semaphoreSlim.Release();
            //    //   int iii = 0;
            //};
            SectionFrame.Navigate(new ShellPageWrapper());
            await Task.Delay(500);
            var shellSectionCurrent = Shell.CurrentItem.CurrentItem;
            NavigateToContent(new NavigationRequestedEventArgs(nextPage, true), shellSectionCurrent);
            //   semaphoreSlim.Wait();
            //NavigateToContent(new NavigationRequestedEventArgs(nextPage, true), shellSection);
            //NavigateToContent(content);
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

        private void NavigateToContent(NavigationRequestedEventArgs args, ShellSection shellSection)
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

            if (nextPage != null)
            {
                Page = nextPage;
              //  Page.PropertyChanged += OnPagePropertyChanged;
                switch (args.RequestType)
                {
                    case NavigationRequestType.Insert:
                    //    OnInsertRequested(args);
                        break;
                    case NavigationRequestType.Pop:
                      //  OnPopRequested(args);
                        break;
                    case NavigationRequestType.Unknown:
                        break;
                    case NavigationRequestType.Push:
                     //   OnPushRequested(args);
                        break;
                    case NavigationRequestType.PopToRoot:
                      //  OnPopToRootRequested(args);
                        break;
                    case NavigationRequestType.Remove:
                     //   OnRemoveRequested(args);
                        break;
                }
                
                var wrapper = (ShellPageWrapper) (SectionFrame.Content);
                if (wrapper.Page == null)
                {
                    wrapper.Page = Page;
                }

                wrapper.LoadPage();
                ShellContext.Control.UpdateTitle(Page.Title);
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

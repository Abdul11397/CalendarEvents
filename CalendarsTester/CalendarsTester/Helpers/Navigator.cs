using System;
using System.Threading.Tasks;
using CalendarsTester.Core.Helpers;
using CalendarsTester.Core.Services;
using Xamarin.Forms;

//Implementation of Generice Page Navigators
namespace CalendarsTester.Helpers
{
    public class Navigator : INavigator
    {
        
        //Class Fields
        private INavigation _navigation;
        private ViewProvider _viewProvider;
        private Page _sourcePage;


        //Constructor

        public Navigator(Page sourcePage)
        {
            _navigation = sourcePage.Navigation;
            _viewProvider = DependencyService.Get<ViewProvider>();
            _sourcePage = sourcePage;
        }

        //Navigators

        public async Task PushAsync<TViewModel>(TViewModel viewmodel)
            where TViewModel : ViewModelBase
        {
            var page = _viewProvider.GetView(viewmodel) as Page;

            if (page == null)
            {
                throw new ArgumentException("viewmodel does not correspond to a page that can be navigated to");
            }

            await _navigation.PushAsync(page);
        }

        public async Task PushAsync<TViewModel>(Action<TViewModel> customInit = null)
            where TViewModel : ViewModelBase, new()
        {
            var viewmodel = ViewModelProvider.GetViewModel<TViewModel>(customInit);


            if (viewmodel != null)
            {
                await PushAsync(viewmodel);
            }
        }

        public async Task PopAsync()
        {
            await _navigation.PopAsync();
        }

        public async Task PushModalAndWaitAsync<TViewModel>(TViewModel viewmodel)
            where TViewModel : ViewModelBase
        {
            var page = _viewProvider.GetView(viewmodel) as Page;

            if (page == null)
            {
                throw new ArgumentException("viewmodel does not correspond to a page that can be navigated to");
            }

            await PushModalAndWaitAsync(page);
        }

        public async Task<TViewModel> PushModalAndWaitAsync<TViewModel>(Action<TViewModel> customInit = null)
            where TViewModel : ViewModelBase, new()
        {
            var viewmodel = ViewModelProvider.GetViewModel<TViewModel>(customInit);

            if (viewmodel != null)
            {
                await PushModalAndWaitAsync(viewmodel);
            }

            return viewmodel;
        }

        public async Task PushModalAsync<TViewModel>(TViewModel viewmodel)
            where TViewModel : ViewModelBase
        {
            var page = _viewProvider.GetView(viewmodel) as Page;

            if (page == null)
            {
                throw new ArgumentException("viewmodel does not correspond to a page that can be navigated to");
            }

            await _navigation.PushModalAsync(page);
        }

        public async Task PushModalAsync<TViewModel>(Action<TViewModel> customInit = null)
            where TViewModel : ViewModelBase, new()
        {
            var viewmodel = ViewModelProvider.GetViewModel<TViewModel>(customInit);

            if (viewmodel != null)
            {
                await PushModalAsync(viewmodel);
            }
        }

        public async Task PopModalAsync()
        {
            await _navigation.PopModalAsync();
        }


        public async Task PushModalAndWaitAsync(Page page)
        {
            var tcs = new TaskCompletionSource<object>();

            // Using NavigationPages

            var navigationPage = new NavigationPage(page);

            EventHandler appearingHandler = null;
            EventHandler<ModalPoppedEventArgs> modalPoppedHandler = null;
            Action handler = () =>
            {
                tcs.SetResult(null);
                _sourcePage.Appearing -= appearingHandler;
                App.Current.ModalPopped -= modalPoppedHandler;
            };
            appearingHandler = (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("Got Appearing event");
                handler();
            };
            modalPoppedHandler = (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("Got ModalPopped event");

                if (e.Modal == navigationPage)
                {
                    handler();
                }
            };

            _sourcePage.Appearing += appearingHandler;

            App.Current.ModalPopped += modalPoppedHandler;

            await _navigation.PushModalAsync(navigationPage);

            await tcs.Task;
        }
    }
}
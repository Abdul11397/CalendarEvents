using CalendarsTester.Core.Services;
using CalendarsTester.Services;
using Xamarin.Forms;
using CalendarsTester.Helpers;
using CalendarsTester.Core.ViewModels;
using CalendarsTester.Pages;
using CalendarsTester.Core.Helpers;

namespace CalendarsTester
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<IReportingService, ReportingService>();
            DependencyService.Register<ViewProvider>();

            //viewRegistration();

            //var viewProvider = DependencyService.Get<ViewProvider>();

            //MainPage = new NavigationPage(viewProvider.GetView(ViewModelProvider.GetViewModel<CalendarsViewModel>()) as Page);
            MainPage = new NavigationPage(new Login());
        }

        private void viewRegistration()
        {
            var viewProvider = DependencyService.Get<ViewProvider>();

            viewProvider.Register<CalendarsViewModel, CalendarsPage>();
            viewProvider.Register<CalendarEditorViewModel, CalendarEditorPage>();
            viewProvider.Register<DateTimeRangeViewModel, DateTimeRangePage>();
            viewProvider.Register<EventsViewModel, EventsPage>();
            viewProvider.Register<EventEditorViewModel, EventEditorPage>();
            viewProvider.Register<ReminderEditorViewModel, ReminderEditorPage>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

    }
}
using System.Windows.Input;
using CalendarsTester.Core.Enums;
using Xamarin.Forms;

//Modal View Model Base Implementation
namespace CalendarsTester.Core.Helpers
{
    public class ModalViewModelBase : ViewModelBase
    {
        public ModalResult Result { get; set; }

        public ICommand CancelCommand { get { return new Command(Cancel); } }
        public ICommand DeleteCommand { get { return new Command(Delete); } }


        public ModalViewModelBase()
        {
            Result = ModalResult.Canceled;
        }


        protected virtual void Cancel()
        {
            Navigator.PopModalAsync();
        }

        protected virtual void Delete()
        {
            Result = ModalResult.Deleted;
            Navigator.PopModalAsync();
        }

        protected virtual void Done()
        {
            Result = ModalResult.Done;
            Navigator.PopModalAsync();
        }
    }
}
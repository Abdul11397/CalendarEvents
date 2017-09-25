using System.Windows.Input;
using Xamarin.Forms;
using CalendarsTester.Core.Helpers;
using Plugin.Calendars.Abstractions;
using CalendarsTester.Core.Extensions;
using CalendarsTester.Core.Services;

//Core implementation of the Calendar View Editor
namespace CalendarsTester.Core.ViewModels
{
    public class CalendarEditorViewModel : ModalViewModelBase
    {
        //attributes

        private string _calendarName;
        private string _calendarColor;
        private Command _doneCommand;

        private Calendar _calendar;

  

        //Properties for Add or Edit Calendar

        public string Title => _calendar == null ? "Add Calendar" : "Edit Calendar";

        public string CalendarName
        {
            get { return _calendarName; }
            set
            {
                if (_calendarName != value)
                {
                    _calendarName = value;
                    HasChanged();
                    _doneCommand.ChangeCanExecute();
                }
            }
        }

        // Set Calendar Color Property
        public string CalendarColor
        {
            get { return _calendarColor; }
            set
            {
                if (_calendarColor != value)
                {
                    _calendarColor = value;
                    HasChanged();
                }
            }
        }

        //Calendar Class implementation
        public Calendar Calendar
        {
            get { return _calendar; }
            set
            {
                if (_calendar != value)
                {
                    _calendar = value;

                    CalendarName = _calendar.Name;
                    CalendarColor = _calendar.Color;
                }
            }
        }

        public bool IsCreating => _calendar == null;

        public ICommand DoneCommand { get { return _doneCommand; } }
        public ICommand CopyIDCommand => new Command(CopyID, () => !string.IsNullOrEmpty(Calendar?.ExternalID));


        //Constructor

        public CalendarEditorViewModel()
        {
            _doneCommand = new Command(Done, () => !string.IsNullOrWhiteSpace(_calendarName)
                && (_calendar?.CanEditCalendar ?? true));
        }


        //methods

        protected override void Done()
        {
            if (_calendar == null)
            {
                _calendar = new Calendar();
            }

            _calendar.Name = CalendarName;

            if (!string.IsNullOrWhiteSpace(CalendarColor))
            {
                try
                {
                    var converter = new ColorTypeConverter();
                    var color = (Color)converter.ConvertFromInvariantString(CalendarColor);
                    _calendar.Color = color.ToHex();
                }
                catch { }
            }

            base.Done();
        }

        private void CopyID()
        {
            var clipboard = DependencyService.Get<IClipboardService>();
            if (clipboard != null && !string.IsNullOrEmpty(Calendar?.ExternalID))
            {
                clipboard.CopyToClipboard(Calendar.ExternalID);
            }
        }
    }
}
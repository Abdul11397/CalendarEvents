using System;
using Plugin.Calendars.Abstractions;
using CalendarsTester.Core.Helpers;
using CalendarsTester.Core.Enums;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;

//View Models
namespace CalendarsTester.Core.ViewModels
{
    public class ReminderEditorViewModel : ModalViewModelBase
    {
        //Methods

        private string _value = "15";
        private TimeUnits _units = TimeUnits.Minutes;
        private CalendarReminderMethod _method;

   

        //Properties : Reminders

        public string Title => "New Reminder";

        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    HasChanged();
                }
            }
        }

        public TimeUnits Units
        {
            get { return _units; }
            set
            {
                if (_units != value && Enum.IsDefined(typeof(TimeUnits), value))
                {
                    _units = value;
                    HasChanged();
                }
            }
        }

        public IList<string> UnitOptions { get; } = Enum.GetNames(typeof(TimeUnits));

        public CalendarReminderMethod Method
        {
            get { return _method; }
            set
            {
                if (_method != value && Enum.IsDefined(typeof(CalendarReminderMethod), value))
                {
                    _method = value;
                    HasChanged();
                }
            }
        }

        public IList<string> MethodOptions { get; } = Enum.GetNames(typeof(CalendarReminderMethod));

        public CalendarEventReminder Reminder { get; private set; }

        public ICommand DoneCommand => new Command(Done);


        protected override void Done()
        {
            Reminder = new CalendarEventReminder
            {
                TimeBefore = GetTimeSpan(_value, _units),
                Method = Method
            };
            
            base.Done();
        }

        private static TimeSpan GetTimeSpan(string strValue, TimeUnits units)
        {
            double value;

            if (!double.TryParse(strValue, out value))
            {
                return TimeSpan.Zero;
            }

            switch (units)
            {
                case TimeUnits.Seconds:
                    return TimeSpan.FromSeconds(value);
                case TimeUnits.Minutes:
                    return TimeSpan.FromMinutes(value);
                case TimeUnits.Hours:
                    return TimeSpan.FromHours(value);
                case TimeUnits.Days:
                    return TimeSpan.FromDays(value);
                default:
                    throw new ArgumentException("Unsupported TimeUnits value", nameof(units));
            }
        }
    }
}

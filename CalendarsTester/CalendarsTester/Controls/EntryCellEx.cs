using System;
using Xamarin.Forms;

//Controls for Android
namespace CalendarsTester.Controls
{
    /// 
    /// Adding support for initial focus.
    /// Currently use by iOS... 
    /// Android uses the default behavior
    /// 
    public class EntryCellEx : EntryCell
    {
        public void Focus()
        {
            if (FocusRequested != null)
            {
                FocusRequested(this, EventArgs.Empty);
            }
        }

        public event EventHandler FocusRequested;

        public bool RequestInitialFocus { get; set; }
    }
}
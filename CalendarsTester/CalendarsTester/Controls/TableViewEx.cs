using Xamarin.Forms;

//Implementation of the Table View Controls
namespace CalendarsTester.Controls
{
    public class TableViewEx : TableView
    {
        // Per-cell control over selectability would be preferable, but harder...
        //
        public static readonly BindableProperty AllowSelectionProperty =
            BindableProperty.Create(nameof(AllowSelection), typeof(bool), typeof(TableViewEx), true);

        public bool AllowSelection
        {
            get { return (bool)GetValue(AllowSelectionProperty); }
            set { SetValue(AllowSelectionProperty, value); }
        }
    }
}
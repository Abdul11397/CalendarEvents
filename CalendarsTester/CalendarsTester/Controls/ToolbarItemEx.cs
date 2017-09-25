using CalendarsTester.Enums;
using Xamarin.Forms;

//Implementation of the Toolbar controls
namespace CalendarsTester.Controls
{
    /// 
    /// Includes the support for the special toolbar item types specially for IOS
    ///
    /// 
    public class ToolbarItemEx : ToolbarItem
    {
        // Bindable property for the toolbar item type
        public static readonly BindableProperty ToolbarItemTypeProperty =
            BindableProperty.Create(nameof(ToolbarItemType), typeof(ToolbarItemType), typeof(ToolbarItemEx), ToolbarItemType.Standard);

        //Gets or sets the toolbar item type
        public ToolbarItemType ToolbarItemType
        {
            get { return (ToolbarItemType)GetValue(ToolbarItemTypeProperty); }
            set { SetValue(ToolbarItemTypeProperty, value); }
        }


        /// Public copy of the base class' internal method..
        public void Activate()
        {
            if (Command != null && Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
            OnClicked();
        }
    }
}
using System;
using Xamarin.Forms;

namespace CalendarsTester.Behaviors
{
    //This class sets the initial behavior to set the initial focus element for a page.

    public class InitialFocusBehavior : Behavior<Page>
    {
        // Bindable property for focus element
        public static readonly BindableProperty FocusElementProperty =
            BindableProperty.Create(nameof(FocusElement), typeof(VisualElement), typeof(InitialFocusBehavior), null);

        // Gets or sets initial focus element
        public VisualElement FocusElement
        {
            get { return (VisualElement)GetValue(FocusElementProperty); }
            set { SetValue(FocusElementProperty, value); }
        }

        // Bindable property for enable/disable
        public static readonly BindableProperty IsEnabledProperty =
            BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(InitialFocusBehavior), true);

        // Gets or sets whether to set initial focus to the focus element
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        protected override void OnAttachedTo(Page page)
        {
            page.Appearing += Page_Appearing;
            page.BindingContextChanged += Page_BindingContextChanged;

            BindingContext = page.BindingContext;

            base.OnAttachedTo(page);
        }

        private void Page_BindingContextChanged(object sender, EventArgs e)
        {
            BindingContext = (sender as BindableObject)?.BindingContext;
        }

        protected override void OnDetachingFrom(Page page)
        {
            page.Appearing -= Page_Appearing;
            page.BindingContextChanged -= Page_BindingContextChanged;

            base.OnDetachingFrom(page);
        }

        private void Page_Appearing(object sender, EventArgs e)
        {
            if (IsEnabled && FocusElement != null)
            {
                FocusElement.Focus();
            }
        }
    }
}
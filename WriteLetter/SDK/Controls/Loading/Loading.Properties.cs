using Windows.UI.Xaml;

namespace AppCore.SDK.Controls
{
    /// <summary>
    /// Loading control allows to show an loading animation with some xaml in it.
    /// </summary>
    public partial class Loading
    {
        /// <summary>
        /// Identifies the <see cref="IsLoading"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
            nameof(IsLoading), typeof(bool), typeof(Loading), new PropertyMetadata(default(bool), IsLoadingPropertyChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the control is in the loading state.
        /// </summary>
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        private static void IsLoadingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Loading;
            if (control._presenter == null)
            {
                control._presenter = control.GetTemplateChild("ContentGrid") as FrameworkElement;
            }

            control?.Update();
        }
    }
}

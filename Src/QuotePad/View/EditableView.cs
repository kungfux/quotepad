using System.Windows;
using System.Windows.Controls;

namespace QuotePad.View
{
    public class EditableView : UserControl
    {
        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
        }

        public static readonly DependencyProperty IsEditModeProperty =
            DependencyProperty.Register("IsEditMode", typeof(bool), typeof(EditableView), new UIPropertyMetadata(false));
    }
}

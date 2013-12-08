using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Blitz.Client.CRM.Client.Edit
{
    /// <summary>
    /// Taken from - http://stackoverflow.com/questions/5511/numeric-data-entry-in-wpf
    /// Usually the projects I work on have purchased Control Suites that have this functionality built in.
    /// </summary>
    public class TextBoxIntegerMaskBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.PreviewTextInput += AssociatedObject_PreviewTextInput;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewTextInput -= AssociatedObject_PreviewTextInput;
        }

        private void AssociatedObject_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidNumericChars(e.Text);
        }

        private bool AreAllValidNumericChars(string str)
        {
            return str.All(c => Char.IsNumber(c));
        }
    }
}
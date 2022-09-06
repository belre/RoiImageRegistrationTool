
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Input;


namespace ClipXmlReader.Behaviors
{
    public class DataGridRightClickAction : TriggerAction<DataGridCell>
    {
        private BindingExpression bindingExpression;


        protected override void OnAttached()
        {
            base.OnAttached();

            this.bindingExpression = this.AssociatedObject.GetBindingExpression(TextBox.TextProperty);
        }

        protected override void OnDetaching()
        {
            this.bindingExpression = null;
            base.OnDetaching();

        }

        protected override void Invoke(object parameter)
        {
            if (this.bindingExpression == null)
            {
                return;
            }

            this.bindingExpression.UpdateSource();
        }
    }
}

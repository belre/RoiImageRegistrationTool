using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ClipXmlReader.View
{
    public class UserContextMenu : System.Windows.Controls.ContextMenu
    {
        protected override void OnClosed(RoutedEventArgs e)
        {
            e.Handled = true;
        }

    }
}

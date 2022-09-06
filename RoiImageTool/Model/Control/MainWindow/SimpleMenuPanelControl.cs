using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.Control.MainWindow
{
    public class SimpleMenuPanelControl : MenuTreeControl
    {
        public SimpleMenuPanelControl()
            : base(new List<Interface.ITreeControl>())
        {

        }
    }
}

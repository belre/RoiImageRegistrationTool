using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.Control.MainWindow
{
    public class RibbonPanelControl : MenuTreeControl
    {

        public RibbonItemPanelControl Ribbon_RecipePanel
        {
            get;
            private set;
        }


        public RibbonItemPanelControl Ribbon_WorkspacePanel
        {
            get;
            private set;
        }


        public RibbonPanelControl()
            : base(new List<Interface.ITreeControl>())
        {
            Ribbon_RecipePanel = new RibbonItemPanelControl();
            Ribbon_WorkspacePanel = new RibbonItemPanelControl();

            _controls.Add(Ribbon_RecipePanel);
            _controls.Add(Ribbon_WorkspacePanel);


        }
    }
}

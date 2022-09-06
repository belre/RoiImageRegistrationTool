using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.Control.MainWindow
{
    public class MenuControl
    {
        private List<Interface.ITreeControl> _controls;

        public RibbonPanelControl RibbonPanel
        {
            get;
            private set;
        }

        public SimpleMenuPanelControl SimpleMenuPanel
        {
            get;
            private set;
        }

        public RibbonItemPanelControl Ribbon_RecipePanel
        {
            get
            {
                return RibbonPanel.Ribbon_RecipePanel;
            }
        }


        public RibbonItemPanelControl Ribbon_WorkspacePanel
        {
            get
            {
                return RibbonPanel.Ribbon_WorkspacePanel;
            }
        }


        public MenuControl()
        {
            RibbonPanel = new RibbonPanelControl();
            SimpleMenuPanel = new SimpleMenuPanelControl();
            _controls = new List<Interface.ITreeControl>() { RibbonPanel, SimpleMenuPanel };
        }

        public void Activate(Interface.ITreeControl target)
        {
            foreach( var obj in  _controls)
            {
                if( obj.Include(target))
                {
                    obj.Activate(target);
                }
                else
                {
                    obj.Disable(false);
                }
            }
        }

        public void ActivateAll()
        {
            foreach( var obj in _controls)
            {
                obj.Enable(true);
            }
        }





    }
}

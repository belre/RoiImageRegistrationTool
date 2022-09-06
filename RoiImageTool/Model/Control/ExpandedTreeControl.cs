using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.Control
{
    public class ExpandedTreeControl : PrimitiveTreeControl
    {
        protected object _keys_parsevisibility = new object();
        public System.Windows.Visibility PanelVisibility
        {
            get
            {
                return GetParameter<System.Windows.Visibility>(_keys_parsevisibility);
            }
            protected set
            {
                SetParameter(_keys_parsevisibility, value);
            }
        }

        public ExpandedTreeControl(List<Interface.ITreeControl> controls)
            : base(controls)
        {
            PanelVisibility = System.Windows.Visibility.Collapsed;
        }

        public override void Enable(bool isincludechild)
        {
            PanelVisibility = System.Windows.Visibility.Visible;

            base.Enable(isincludechild);
        }

        public override void Disable(bool isincludechild)
        {
            PanelVisibility = System.Windows.Visibility.Collapsed;

            base.Disable(isincludechild);
        }

        public override void Activate(Interface.ITreeControl target)
        {
            PanelVisibility = System.Windows.Visibility.Visible;
            
            if (this == target)
            {
                // 全階層をすべてDisableにしたあと、
                // 1階層下のみをEnableに設定する
                foreach( var obj in _controls)
                {
                    obj.Disable(true);
                    obj.Enable(false);
                }
            }
            else
            {
                foreach (var obj in _controls)
                {
                    obj.Activate(target);
                }
            }
        }


    }
}

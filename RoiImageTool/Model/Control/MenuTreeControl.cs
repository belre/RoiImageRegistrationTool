using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.Control
{
    public class MenuTreeControl : PrimitiveTreeControl
    {
        protected object _keys_parsevisibility = new object();
        public bool IsVisiblePanel
        {
            get
            {
                return GetParameter<bool>(_keys_parsevisibility);
            }
            protected set
            {
                SetParameter(_keys_parsevisibility, value);
            }
        }

        public MenuTreeControl(List<Interface.ITreeControl> controls)
            : base(controls)
        {
            IsVisiblePanel = false;
        }


        public override void Enable(bool isincludechild)
        {
            IsVisiblePanel = true;

            base.Enable(isincludechild);
        }

        public override void Disable(bool isincludechild)
        {
            IsVisiblePanel = false;

            base.Disable(isincludechild);
        }

        public override void Activate(Interface.ITreeControl target)
        {
            bool ismatch = (this == target);


            if (ismatch)
            {
                IsVisiblePanel = true;
            }
            else
            {
                foreach (var obj in _controls)
                {
                    if (obj.Include(target))
                    {
                        this.Enable(false);
                        obj.Activate(target);
                    }
                    else
                    {
                        obj.Disable(true);
                    }
                }
            }
        }




    }
}

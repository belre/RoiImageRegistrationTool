using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.Control
{
    public abstract class PrimitiveTreeControl : Interface.ITreeControl
    {

        protected Dictionary<object, object> _parameter;
        protected List<Interface.ITreeControl> _controls;


        public PrimitiveTreeControl()
            : this(new List<Interface.ITreeControl>())
        {

        }

        public PrimitiveTreeControl(List<Interface.ITreeControl> controls)
        {
            _controls = new List<Interface.ITreeControl>();
            foreach (var resource in controls)
            {
                _controls.Add(resource);
            }

            _parameter = new Dictionary<object, object>();
        }

        protected void SetParameter(object key, object value, bool isoverwrite=true)
        {
            if( !isoverwrite && _parameter.Keys.Contains(key))
            {
                return;
            }

            _parameter[key] = value;
        }

        protected T GetParameter<T>(object key)
        {
            if(_parameter.Keys.Contains(key))
            {
                return (T)_parameter[key];
            }
            else
            {
                return default(T);
            }
        }


        public virtual void Enable(bool isincludechild)
        {
            if (isincludechild)
            {
                foreach (var obj in _controls)
                {
                    obj.Enable(true);
                }
            }
        }

        public virtual void Disable(bool isincludechild)
        {
            if (isincludechild)
            {
                foreach (var obj in _controls)
                {
                    obj.Disable(true);
                }
            }
        }

        public virtual bool Include(Interface.ITreeControl target)
        {
            if (this == target)
            {
                return true;
            }

            bool isinclude = false;
            foreach (var obj in _controls)
            {
                if (obj.Include(target))
                {
                    isinclude = true;
                }
            }

            return isinclude;
        }


#if false
        public virtual ITreeControl Clone()
        {
            List<ITreeControl> temporary = new List<ITreeControl>();
            foreach (var obj in _controls)
            {
                temporary.Add(obj.Clone());
            }
            
            return ;
        }
#endif

        public abstract void Activate(Interface.ITreeControl target);
    }
}

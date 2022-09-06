using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.Base
{
    public class BaseModelClass : Interface.IErrorStatus
    {
        protected string _errormessage;

        public string CurrentErrorMessage
        {
            get
            {
                return _errormessage;
            }
        }

        public void ClearMessage()
        {
            _errormessage = null;
        }
    }
}

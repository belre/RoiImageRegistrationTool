using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ClipXmlReader.Model.Interface
{
    public interface ITableElement
    {
        T GetParameter<T>(object key);
        void SetParameter<T>(object key, T value);
    }
}

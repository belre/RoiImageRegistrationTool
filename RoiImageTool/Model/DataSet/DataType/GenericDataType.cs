using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.DataSet.DataType
{
    public class GenericDataType : IComparable<string>, IEquatable<string>
    {
        public virtual object Contents
        {
            get;
            protected set;
        }

        public virtual string DisplayedContents
        {
            get;
            protected set;
        }

        public virtual Type ValueType
        {
            get;
            protected set;
        }

        public GenericDataType(object contents, Type valuetype)
        {
            Contents = contents;
            DisplayedContents = contents == null ? "" : contents.ToString();
            ValueType = valuetype;
        }

        public GenericDataType(object contents, string displayedcontents, Type valuetype)
        {
            Contents = contents;
            DisplayedContents = displayedcontents;
            ValueType = valuetype;
        }

        public override string ToString()
        {
            return Contents.ToString();
        }

        public int CompareTo(string obj)
        {
            return Contents.ToString().CompareTo(obj);
        }

        public bool Equals(string obj)
        {
            return Contents.Equals(obj);
        }

        public virtual void InitializeContents(params object[] externalparam)
        {

        }
    }
}

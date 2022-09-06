using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.Control.MainWindow.RecipeHandler.Element
{
    public class RecipeItemElement : BaseElement
    {


        public override string ElementName
        {
            get
            {
                return "Measure";
            }
        }

        protected GenericContents _keys_name = new GenericContents("Name", typeof(string));
        public object Key_Name
        {
            get
            {
                return _keys_name;
            }
        }

        protected GenericContents _keys_id = new GenericContents("ID", typeof(EMeasBaseType));
        public object Key_ID
        {
            get
            {
                return _keys_id;
            }
        }

        public override GenericContents[] AttributeName
        {
            get
            {
                return new GenericContents[] { _keys_id };
            }
        }


        protected override List<GenericContents> ownedElement
        {
            get
            {
                return new List<GenericContents> { _keys_name };
            }
        }

        protected override bool IsBoundElementName
        {
            get
            {
                return false;
            }
        }

        public RecipeItemElement()
            : this(null)
        {
        }

        public RecipeItemElement(BaseGroup owner)
            : base(owner)
        {
            _params[Key_ID] = EMeasBaseType.NONE;
            _params[Key_Name] = string.Empty;

            Owner = owner;
        }

        public bool IsParsed
        {
            get;
            protected set;

        }

        public override void Parse(System.Xml.XmlReader reader, Stack<string> hierarchical)
        {
            IsParsed = false;

            string use_element = hierarchical.Pop();
            hierarchical.Push(use_element);

            foreach (var keyobj in ownedElement)
            {
                if (use_element.Equals(keyobj.Contents.ToString()))
                {
                    reader.Read();
                    if (reader.NodeType == System.Xml.XmlNodeType.Text)
                    {
                        SetParameterDynamically(keyobj, reader.Value, keyobj.ValueType);
                    }
                    reader.Read();
                    if (reader.NodeType == System.Xml.XmlNodeType.EndElement)
                    {
                        hierarchical.Pop();
                    }
                }
            }

            IsParsed = true;
        }

    }
}

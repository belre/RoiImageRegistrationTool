using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple
{
    /// <summary>
    /// ヘッダのオブジェクトを表します。
    /// </summary>
    public class HeaderTuple : Base.BaseTuple
    {
        protected DataType.GenericDataType _keys_update = new DataType.GenericDataType("Update", typeof(string));
        
        /// <summary>
        /// 更新日時に関するキーを表します。
        /// </summary>
        public object Key_Update
        {
            get
            {
                return _keys_update;
            }
        }

        protected DataType.GenericDataType _keys_format = new DataType.GenericDataType("Format", typeof(string));

        /// <summary>
        /// Formatに関するキーを表します。
        /// </summary>
        public object Key_Format
        {
            get
            {
                return _keys_format;
            }
        }


        protected DataType.GenericDataType _keys_number = new DataType.GenericDataType("Number", typeof(int));
        /// <summary>
        /// Numberに関するキーを表します。
        /// </summary>
        public object Key_Number
        {
            get
            {
                return _keys_number;
            }
        }

        /// <summary>
        /// 要素名を出力します。
        /// </summary>
        public override string ElementName
        {
            get
            {
                return "Head";
            }
        }

        /// <summary>
        /// 要素として保持しているキーの一覧を表します。
        /// </summary>
        protected override List<DataType.GenericDataType> ownedElement
        {
            get
            {
                return new List<DataType.GenericDataType> { _keys_update, _keys_format, _keys_number };
            }
        }

        public int Value_Number
        {
            get
            {
                return GetParameter<int>(_keys_number);
            }
            set
            {
                SetParameter<int>(_keys_number, value);
            }

        }

        public HeaderTuple()
            : this(null)
        {
        }

        public HeaderTuple(Base.BaseTreeGroup owner)
            : base(owner)
        {
            _params[Key_Update] = new DataType.GenericDataType(string.Empty, typeof(string)) ;
            _params[Key_Format] = new DataType.GenericDataType(string.Empty, typeof(string)) ;
            _params[Key_Number] = new DataType.GenericDataType(0, typeof(int)) ;

            Owner = owner;
        }


        /// <summary>
        /// XMLドキュメントを生成します。
        /// </summary>
        /// <param name="document"></param>
        /// <param name="parent"></param>
        public override void MakeXmlNode(XmlDocument document, XmlElement parent)
        {
            XmlElement root = document.CreateElement(ElementName);

            foreach (var element in ownedElement)
            {
                XmlElement item = document.CreateElement(element.ToString());

                if (element == Key_Update)
                {
                    item.InnerText = DateTime.Now.ToString();
                }
                else
                {
                    item.InnerText = _params[element].ToString();
                }

                root.AppendChild(item);
            }

            parent.AppendChild(root);
        }



    }
}

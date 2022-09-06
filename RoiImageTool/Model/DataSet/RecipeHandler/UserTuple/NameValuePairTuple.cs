using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple
{
    /// <summary>
    /// 名前と値が一対となっているオブジェクト変数を表します。
    /// </summary>
    public class NameValuePairTuple : Base.BaseTuple
    {
        protected DataType.GenericDataType _keys_name = new DataType.GenericDataType("Name", typeof(string));

        /// <summary>
        /// 名前のキーを表します。
        /// </summary>
        public object Key_Name
        {
            get
            {
                return _keys_name;
            }
        }


        protected DataType.GenericDataType _keys_displayed_name = new DataType.GenericDataType("DisplayedName", typeof(string));
        /// <summary>
        /// 画面に表示される名前のキーを表します。
        /// </summary>
        public object Key_DisplayedName
        {
            get
            {
                return _keys_displayed_name;
            }
        }



        protected object _keys_value = new object();
        /// <summary>
        /// 値を表します。
        /// </summary>
        public object Key_Value
        {
            get
            {
                return _keys_value;
            }
        }

        protected DataType.GenericDataType _keys_isreadonly = new DataType.GenericDataType("ReadOnly", typeof(bool));
        /// <summary>
        /// この値が、名前の変更を禁止していることを表すキーを表します。
        /// </summary>
        public object Key_NameChangeProhibited
        {
            get
            {
                return _keys_isreadonly;
            }
        }

        /// <summary>
        /// 名前を取得、設定します。
        /// </summary>
        public string Value_Name
        {
            get
            {
                return GetParameter<string>(Key_Name);
            }
            set
            {
                SetParameter<string>(Key_Name, value);

                if (Value_DisplayedName == null)
                {
                    SetParameter<string>(Key_DisplayedName, value);
                }
            }
        }

        /// <summary>
        /// 画面に表示される名前を取得、設定します。
        /// </summary>
        public string Value_DisplayedName
        {
            get
            {
                return GetParameter<string>(Key_DisplayedName);
            }
            set
            {
                SetParameter<string>(Key_DisplayedName, value);
            }
        }


        /// <summary>
        /// 値を取得、設定します。
        /// </summary>
        public object Value_Value
        {
            get
            {
                return GetParameter(Key_Value);
            }

            set
            {
                SetParameterDynamically(Key_Value, value.ToString(), Value_Value.GetType());
            }
        }

        /// <summary>
        /// 名前変更を禁止することを表す値を表します。
        /// </summary>
        public bool Value_NameChangeProhibited
        {
            get
            {
                return GetParameter<bool>(Key_NameChangeProhibited);
            }
            set
            {
                SetParameter<bool>(Key_NameChangeProhibited, value);
            }
        }


        public NameValuePairTuple(Base.BaseTreeGroup owner, bool isreadonly = false)
            : this(owner, "")
        {

        }

        public NameValuePairTuple(Base.BaseTreeGroup owner, object valueDefault, bool isreadonly=false)
            : base(owner)
        {
            _params[Key_Name] = new DataType.GenericDataType("Parameter", typeof(string));
            _params[Key_DisplayedName] = new DataType.GenericDataType("Parameter", typeof(string));
            _params[Key_Value] = new DataType.GenericDataType(valueDefault, valueDefault.GetType());
            _params[Key_NameChangeProhibited] = new DataType.GenericDataType((bool)isreadonly, typeof(bool));

            Owner = owner;
        }

        /// <summary>
        /// タグとの紐づけ要素がないので、チェックを行いません。
        /// </summary>
        protected override bool IsBoundElementName
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// このXMLの場合、読み取り時にオープンタグを検出した場合は例外を返します。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected override void ParseBeginElement(XmlReader reader, Stack<string> hierarchical)
        {
            throw new System.Xml.XmlException("Begin Element shall not be included in NameValuePairTuple.");
        }

        /// <summary>
        /// XML文書を読み取ります。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="elementname"></param>
        protected override void ParseText(XmlReader reader, string elementname)
        {
            Value_Name = elementname;
            Value_Value = reader.Value;
        }

        /// <summary>
        /// XMLドキュメントを生成します。
        /// </summary>
        /// <param name="document"></param>
        /// <param name="parent"></param>
        public override void MakeXmlNode(XmlDocument document, XmlElement parent)
        {
            {
                XmlElement item = document.CreateElement(_params[Key_Name].ToString());


                if (_params[Key_Value].Contents.GetType().IsEnum)
                {
                    int val = (int)_params[Key_Value].Contents;
                    item.InnerText = val.ToString();
                }
                else
                {
                    item.InnerText = _params[Key_Value].Contents.ToString();
                }

                parent.AppendChild(item);
            }
        }

    }
}

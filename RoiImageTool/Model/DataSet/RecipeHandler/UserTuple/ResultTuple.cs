using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple
{
    /// <summary>
    /// 結果を表す変数一覧を表します。
    /// </summary>
    public class ResultTuple : Base.BaseTuple
    {
        public override string ElementName
        {
            get
            {
                return "Result";
            }
        }

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

        protected DataType.GenericDataType _keys_unit = new DataType.GenericDataType("Unit", typeof(string));
        /// <summary>
        /// 単位のキーを表します。
        /// </summary>
        public object Key_Unit
        {
            get
            {
                return _keys_unit;
            }
        }


        protected DataType.GenericDataType _keys_validfig = new DataType.GenericDataType("Valid-fig", typeof(int));
        /// <summary>
        /// Valid-Figのキーを表します。
        /// </summary>
        public object Key_ValidFig
        {
            get
            {
                return _keys_validfig;
            }
        }



        protected DataType.GenericDataType _keys_lower = new DataType.GenericDataType("Std-lower", typeof(decimal));
        /// <summary>
        /// 下限値のキーを表します。
        /// </summary>
        public object Key_Lower
        {
            get
            {
                return _keys_lower;
            }
        }

        protected DataType.GenericDataType _keys_upper = new DataType.GenericDataType("Std-upper", typeof(decimal));
        /// <summary>
        /// 上限値のキーを表します。
        /// </summary>
        public object Key_Upper
        {
            get
            {
                return _keys_upper;
            }
        }

        protected DataType.GenericDataType _keys_useflag = new DataType.GenericDataType("UseFlag", typeof(int));
        /// <summary>
        /// フラグのキーを表します。
        /// </summary>
        public object Key_UseFlag
        {
            get
            {
                return _keys_useflag;
            }
        }


        protected DataType.GenericDataType _keys_isreadonly = new DataType.GenericDataType("ReadOnly", typeof(bool));
        /// <summary>
        /// 名前が変更されることを禁止しているかを表すキーを表します。
        /// </summary>
        public object Key_NameChangeProhibited
        {
            get
            {
                return _keys_isreadonly;
            }
        }

        /// <summary>
        /// アトリビュートの一覧を表します。
        /// </summary>
        public override DataType.GenericDataType[] AttributeName
        {
            get
            {
                return new DataType.GenericDataType[] { _keys_name };
            }
        }

        /// <summary>
        /// 保持している要素を表します。
        /// </summary>
        protected override List<DataType.GenericDataType> ownedElement
        {
            get
            {
                return new List<DataType.GenericDataType> { _keys_unit, _keys_validfig, _keys_lower, _keys_upper, _keys_useflag };
            }
        }



        public ResultTuple(Base.BaseTreeGroup owner, bool isreadonly)
            : base(owner)
        {
            _params[Key_Name] = new DataType.GenericDataType("Parameter", typeof(string));
            _params[Key_Unit] = new DataType.GenericDataType("mm", typeof(string));
            _params[Key_ValidFig] = new DataType.GenericDataType((int)-2, typeof(int));
            _params[Key_Lower] = new DataType.GenericDataType((decimal)0, typeof(decimal));
            _params[Key_Upper] = new DataType.GenericDataType((decimal)0, typeof(decimal));
            _params[Key_UseFlag] = new DataType.GenericDataType((int)0, typeof(int));
            _params[Key_NameChangeProhibited] = new DataType.GenericDataType((bool)isreadonly, typeof(bool));

            Owner = owner;
        }

        /// <summary>
        /// XMLドキュメントを生成します。
        /// </summary>
        /// <param name="document"></param>
        /// <param name="parent"></param>
        public override void MakeXmlNode(XmlDocument document, XmlElement parent)
        {
            var current = document.CreateElement(ElementName);

            foreach (var source in AttributeName)
            {
                XmlAttribute attr = document.CreateAttribute(source.ToString());
                if (_params[source].GetType().IsEnum)
                {
                    int val = (int)_params[source].Contents;
                    attr.Value = val.ToString();
                }
                else
                {
                    attr.Value = _params[source].ToString();
                }

                current.Attributes.Append(attr);
            }

            foreach (var element in ownedElement)
            {
                XmlElement item = document.CreateElement(element.ToString());

                if (_params[element].ToString().Equals(""))
                {
                    XmlWhitespace whitespace = document.CreateWhitespace("");
                    item.AppendChild(whitespace);
                }
                else
                {
                    item.InnerText = _params[element].ToString();
                }


                current.AppendChild(item);
            }

            parent.AppendChild(current);
        }
    }
}

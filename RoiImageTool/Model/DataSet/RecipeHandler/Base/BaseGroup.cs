using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Base
{
    /// <summary>
    /// レシピを管理するためのブランチクラスを表します。
    /// </summary>
    public class BaseGroup : BaseXmlNode
    {

        /// <summary>
        /// ブランチクラスの親クラスを表します。
        /// </summary>
        public virtual BaseTreeGroup Parent
        {
            get
            {
                return null;
            }
        }


        protected Relations.XmlRootSerialization _base_xml_template;

        /// <summary>
        /// XMLテンプレートを表します。
        /// </summary>
        public virtual Relations.XmlRootSerialization RelationsObject
        {
            get
            {
                if (Parent != null)
                {
                    return Parent.RelationsObject;
                }
                else
                {
                    return _base_xml_template;
                }
            }
            set
            {
                _base_xml_template = value;
            }
        }

        /// <summary>
        /// Attributeパラメータを、AttributeNameに基づいて初期化します。
        /// </summary>
        /// <param name="reader"></param>
        public virtual void InitAttributes(XmlReader reader)
        {
            Dictionary<object, string> attrlist = new Dictionary<object, string>();
            foreach (var attr in AttributeName)
            {
                if (!reader.GetAttribute(attr.ToString()).Equals(""))
                {
                    attrlist[attr] = reader.GetAttribute(attr.ToString());
                }
            }
            SetAttribute(attrlist);

            UpdateFromRelation();
        }



        /// <summary>
        /// 出力するときに必要なXmlDocumentを出力します。
        /// </summary>
        /// <param name="document">出力されるdocument</param>
        /// <param name="parent">一つ上位の要素</param>
        public override void MakeXmlNode(XmlDocument document, XmlElement parent)
        {

            if (ElementName != null)
            {
                var item = document.CreateElement(ElementName.ToString());
                item.IsEmpty = false;

                foreach (var source in AttributeName)
                {
                    XmlAttribute attr = document.CreateAttribute(source.ToString());
                    if (_params[source].Contents.GetType().IsEnum)
                    {
                        int val = (int)_params[source].Contents;
                        attr.Value = val.ToString();
                    }
                    else
                    {
                        attr.Value = _params[source].Contents.ToString();
                    }

                    item.Attributes.Append(attr);
                }

                DelegateGroup(document, parent);

                foreach( var source in ownedElement)
                {
                    XmlElement element = document.CreateElement(source.ToString());
                    element.IsEmpty = false;

                    if (_params[source].GetType().IsEnum)
                    {
                        int val = (int)_params[source].Contents;
                        element.InnerText = val.ToString();
                    }
                    else
                    {
                        element.InnerText = _params[source].ToString();
                    }

                    item.AppendChild(element);
                }
                
                
                DelegateSubGroup(document, item);

                parent.AppendChild(item);

            }
        }

        public virtual void UpdateFromRelation()
        {
            UpdateFromMeasureRelation();
            UpdateFromRegionRelation();
            UpdateFromFilterRelation();
        }

        public virtual void UpdateFromMeasureRelation()
        {

        }



        public virtual void UpdateFromRegionRelation()
        {

        }

        public virtual void UpdateFromFilterRelation()
        {

        }
    }
}

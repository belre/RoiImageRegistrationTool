using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Relations
{
    /// <summary>
    /// Filterの項目のシリアライズされたテンプレートを表します。
    /// </summary>
    public class FilterItemSerialization : RelationSerialization
    {
        /// <summary>
        /// FilterのIDを表します。
        /// </summary>
        [XmlAttribute]
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// Filterの名称を表すラベルを表します。
        /// </summary>
        [XmlElement]
        public string Label
        {
            get;
            set;
        }

        /// <summary>
        /// Filterのパラメータ制約を表します。
        /// </summary>
        [XmlElement("Filter-params")]
        public List<FilterParamsSerialization> FilterParamName
        {
            get;
            set;
        }

        public FilterItemSerialization()
        {
            FilterParamName = new List<FilterParamsSerialization>();
        }
    }
}

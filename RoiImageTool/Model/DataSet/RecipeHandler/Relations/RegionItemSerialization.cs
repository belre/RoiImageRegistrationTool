using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Relations
{

    /// <summary>
    /// Regionの項目のシリアライズされたテンプレートを表します。
    /// </summary>
    public class RegionItemSerialization : RelationSerialization
    {
        /// <summary>
        /// RegionのIDを表します。
        /// </summary>
        [XmlAttribute]
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// Regionのラベルを表します。
        /// </summary>
        [XmlElement]
        public string Label
        {
            get;
            set;
        }

        /// <summary>
        /// 指定されたパラメータがリファレンス参照かどうかを表します。
        /// </summary>
        [XmlElement]
        public bool IsReference
        {
            get;
            set;
        }

        /// <summary>
        /// Regionのパラメータ制約を表します。
        /// </summary>
        [XmlElement("Feature-params")]
        public List<RegionParamsSerialization> FeatureParamName
        {
            get;
            set;
        }

        public RegionItemSerialization()
        {
            FeatureParamName = new List<RegionParamsSerialization>();
        }
    }
}

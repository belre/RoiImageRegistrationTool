using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;


namespace ClipXmlReader.Model.DataSet.RecipeHandler.Relations
{
    /// <summary>
    /// Filterのパラメータのシリアライズされたテンプレートを表します。
    /// </summary>
    [Serializable]
    public class FilterParamsSerialization : RelationSerialization
    {
        /// <summary>
        /// パラメータ名を表します。
        /// </summary>
        [XmlElement]
        public string Label
        {
            get;
            set;
        }

        /// <summary>
        /// パラメータのデフォルト値を表します。
        /// ※型も同時指定されます。
        /// </summary>
        [XmlElement]
        public object Default
        {
            get;
            set;
        }
    }

}

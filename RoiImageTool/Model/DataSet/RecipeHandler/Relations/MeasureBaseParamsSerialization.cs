using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Relations
{
    [Serializable]
    public class MeasureBaseParamsSerialization
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

        [XmlElement]
        public string DisplayedLabel
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

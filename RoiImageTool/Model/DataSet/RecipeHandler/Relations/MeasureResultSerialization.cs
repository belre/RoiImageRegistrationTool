using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Relations
{
    [Serializable]
    public class MeasureResultSerialization : RelationSerialization
    {
        [XmlElement]
        public string ResultName
        {
            get;
            set;
        }

        [XmlElement]
        public int DefaultValidFig
        {
            get;
            set;
        }

        [XmlElement]
        public string DefaultUnit
        {
            get;
            set;
        }

        [XmlElement]
        public decimal DefaultLower
        {
            get;
            set;
        }

        [XmlElement]
        public decimal DefaultUpper
        {
            get;
            set;
        }

        public MeasureResultSerialization() : this("")
        {

        }

        /// <summary>
        /// 初期値設定
        /// </summary>
        public MeasureResultSerialization(string name)
        {
            ResultName = name;
            DefaultValidFig = 0;
            DefaultUnit = "";
            DefaultLower = 0;
            DefaultUpper = 0;
        }
    }
}

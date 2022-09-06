using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Xml.Serialization;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Relations
{
    /// <summary>
    /// Measuresのシリアライズされたテンプレートであることを表します。
    /// </summary>
    [Serializable]
    [XmlRoot("Measures")]
    public class MeasureRootSerialization : RelationSerialization
    {
        /// <summary>
        /// 計測項目を表します。
        /// シリアライズされて出力されます。
        /// </summary>
        [XmlElement]
        public List<MeasureItemSerialization> Measure
        {
            get;
            set;
        }


        /// <summary>
        /// ItemTypeをキーにDictionaryとしたものを出力します。
        /// </summary>
        [XmlIgnore]
        public Dictionary<int, MeasureItemSerialization> MeasureDict
        {
            get
            {
                if (!CheckObject()) return null;

                var dict = new Dictionary<int, MeasureItemSerialization>();
                foreach( var item in Measure)
                {
                    int id = item.ID;
                    dict[id] = item;
                }

                return dict;
            }
        }

        /// <summary>
        /// 
        /// ItemTypeをキーに、Labelを値としDictionaryとしたものを出力します。
        /// </summary>
        [XmlIgnore]
        public Dictionary<int, string> MeasureDictLabel
        {
            get
            {
                if (!CheckObject()) return null;

                var dict = MeasureDict;

                var newdict = new Dictionary<int, string>();
                foreach (var set in dict)
                {
                    newdict[set.Key] = set.Value.Label;
                }

                return newdict;
            }
        }

        protected override bool CheckObject()
        {
            ClearMessage();

            // ID重複チェック
            List<int> currentid_list = new List<int>();
            foreach( var item in Measure)
            {
                if( currentid_list.Contains(item.ID))
                {
                    _errormessage = ERROR_MULTIPLE_ID;                    
                    return false;
                }

                currentid_list.Add(item.ID);
            }

            return true;       
        }

        public MeasureRootSerialization()
        {
            Measure = new List<MeasureItemSerialization>();
        }

        public static readonly string ERROR_MULTIPLE_ID = "XML Template is invalid.";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Relations
{
    /// <summary>
    /// Regionsのシリアライズされたテンプレートであることを表します。
    /// </summary>
    [Serializable]
    [XmlRoot("Regions")]
    public class RegionRootSerialization : RelationSerialization
    {
        /// <summary>
        /// 領域を表します。
        /// シリアライズされて出力されます。
        /// </summary>
        [XmlElement]
        public List<RegionItemSerialization> Region
        {
            get;
            set;
        }

        /// <summary>
        /// 領域をItemTypeをキーにDictionaryとしたものを出力します。
        /// </summary>
        [XmlIgnore]
        public Dictionary<int, RegionItemSerialization> RegionDict
        {
            get
            {
                if (!CheckObject()) return null;

                var dict = new Dictionary<int, RegionItemSerialization>();
                foreach (var item in Region)
                {
                    int id = (int)item.ID;
                    dict[id] = item;
                }

                return dict;
            }
        }

        /// <summary>
        /// 領域をItemTupeをキーに、Labelを値としDictionaryとしたものを出力します。
        /// </summary>
        [XmlIgnore]
        public Dictionary<int, string> RegionDictLabel
        {
            get
            {
                if (!CheckObject()) return null;

                var dict = RegionDict;

                var newdict = new Dictionary<int, string>();
                foreach ( var set in dict)
                {
                    newdict[set.Key] = set.Value.Label;
                }

                return newdict;
            }
        }
        
        public RegionRootSerialization()
        {
            Region = new List<RegionItemSerialization>();
        }

        protected override bool CheckObject()
        {
            ClearMessage();

            // ID重複チェック
            List<int> currentid_list = new List<int>();
            foreach (var item in Region)
            {
                if (currentid_list.Contains(item.ID))
                {
                    _errormessage = ERROR_MULTIPLE_ID;
                    return false;
                }
                currentid_list.Add(item.ID);
            }

            return true;
        }

        public static readonly string ERROR_MULTIPLE_ID = "XML Template is invalid.";
    }
}

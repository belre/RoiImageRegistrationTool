using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Relations
{
    /// <summary>
    /// Filtersのシリアライズされたテンプレートであることを表します。
    /// </summary>
    [Serializable]
    [XmlRoot("Filters")]
    public class FilterRootSerialization : RelationSerialization
    {
        /// <summary>
        /// フィルタを表します。
        /// シリアライズされて出力されます。
        /// </summary>
        [XmlElement]
        public List<FilterItemSerialization> Filter
        {
            get;
            set;
        }

        /// <summary>
        /// フィルタをItemTypeをキーにDictionaryとしたものを出力します。
        /// </summary>
        [XmlIgnore]
        public Dictionary<int, FilterItemSerialization> FilterDict
        {
            get
            {
                if (!CheckObject()) return null;

                var dict = new Dictionary<int, FilterItemSerialization>();
                foreach (var item in Filter)
                {
                    int id = item.ID;
                    dict[id] = item;
                }

                return dict;
            }
        }

        /// <summary>
        /// フィルタをItemTupeをキーに、Labelを値としDictionaryとしたものを出力します。
        /// </summary>
        [XmlIgnore]
        public Dictionary<int, string> FilterDictLabel
        {
            get
            {
                if (!CheckObject()) return null;

                var dict = FilterDict;

                var newdict = new Dictionary<int, string>();
                foreach (var set in dict)
                {
                    newdict[set.Key] = set.Value.Label;
                }

                return newdict;
            }
        }

        public FilterRootSerialization()
        {
            Filter = new List<FilterItemSerialization>();
        }

        protected override bool CheckObject()
        {
            ClearMessage();

            // ID重複チェック
            List<int> currentid_list = new List<int>();
            foreach (var item in Filter)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;



namespace ClipXmlReader.Model.DataSet.RecipeHandler.Relations
{

    /// <summary>
    /// Machines情報を表すシリアライズされたテンプレートを表します。
    /// </summary>
    [Serializable]
    [XmlRoot("Machines")]
    public class MachineSettingSerialization : RelationSerialization
    {
        /// <summary>
        /// 機種を表します。
        /// シリアライズされて出力されます。
        /// </summary>
        [XmlElement]
        public List<MachineInfoSerialization> Machine
        {
            get;
            set;
        }

        public bool DevelopmentMode
        {
            get;
            set;
        }

        public MachineSettingSerialization()
        {
            Machine = new List<MachineInfoSerialization>();
        }

        /// <summary>
        /// IDをキーにDictionaryとしたものを出力します。
        /// </summary>
        [XmlIgnore]
        public Dictionary<int, MachineInfoSerialization> MeasureDict
        {
            get
            {
                if (!CheckObject()) return null;

                var dict = new Dictionary<int, MachineInfoSerialization>();
                foreach (var item in Machine)
                {
                    int id = item.ID;
                    dict[id] = item;
                }

                return dict;
            }
        }
    }
}

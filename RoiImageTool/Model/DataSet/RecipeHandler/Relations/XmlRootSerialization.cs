using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Relations
{

    /// <summary>
    /// テンプレートのルートオブジェクトを表すクラスです。
    /// </summary>
    [Serializable]
    [XmlRoot("XmlRoot")]
    public class XmlRootSerialization : RelationSerialization
    {
        /// <summary>
        /// テンプレートMeasuresのオブジェクトを表します。
        /// </summary>
        public MeasureRootSerialization Measures
        {
            get;
            set;
        }
        /// <summary>
        /// テンプレートRegionsのオブジェクトを表します。
        /// </summary>
        public RegionRootSerialization Regions
        {
            get;
            set;
        }
        /// <summary>
        /// テンプレートFiltersのオブジェクトを表します。
        /// </summary>
        public FilterRootSerialization Filters
        {
            get;
            set;
        }

        /// <summary>
        /// テンプレートMachinesのオブジェクトを表します。
        /// </summary>
        public MachineSettingSerialization Machines
        {
            get;
            set;
        }


        public XmlRootSerialization()
        {
            Measures = new MeasureRootSerialization();
            Regions = new RegionRootSerialization();
            Filters = new FilterRootSerialization();
            Machines = new MachineSettingSerialization();
        }

    }
}

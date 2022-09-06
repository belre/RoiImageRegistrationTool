using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Default
{


    public class RegionIDSerializationMaker
    {

        /// <summary>
        /// RegionIDTableを使用して、デフォルト値のRegionシリアライズオブジェクトを作成します。
        /// </summary>
        /// <returns>シリアライズオブジェクト</returns>
        public Relations.RegionRootSerialization MakeSerialization()
        {
            var listmap_featureparams = RegionIDTable.GetRegionFeatureParamsMap();
            var listmap_regionname = RegionIDTable.GetRoiTextMap();

            var serialization = new Relations.RegionRootSerialization();

            foreach (var id in listmap_featureparams)
            {
                if (!listmap_regionname.ContainsKey((RegionIDTable.EROIType)id.Key)) continue;
                if ((int)id.Key == (int)RegionIDTable.EROIType.NONE) continue;

                Relations.RegionItemSerialization regionitem = new Relations.RegionItemSerialization();
                regionitem.ID = (int)id.Key;
                regionitem.Label = listmap_regionname[(RegionIDTable.EROIType)id.Key];
                regionitem.IsReference = (id.Key == (int)RegionIDTable.EROIType.REFERENCE);

                regionitem.FeatureParamName = new List<Relations.RegionParamsSerialization>();
                foreach (var regionparam in listmap_featureparams[id.Key])
                {
                    var relation = new Relations.RegionParamsSerialization();
                    relation.Label = regionparam;
                    relation.Default = (decimal)0;

                    regionitem.FeatureParamName.Add(relation);
                }

                serialization.Region.Add(regionitem);
            }

            return serialization;

        }
    }
}

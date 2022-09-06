using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Default
{
    public class MeasureIDSerializationMaker
    {

        /// <summary>
        /// MeasureIDTableを使用して、デフォルト値のMeasureシリアライズオブジェクトを作成します。
        /// </summary>
        /// <returns>シリアライズオブジェクト</returns>
        public Relations.MeasureRootSerialization MakeSerialization()
        {

            var listmap_result = MeasureIDTable.GetResultListMap();
            var listmap_measureparams = MeasureIDTable.GetMeasureParamsListMap();
            var listmap_regiontype = MeasureIDTable.GetRegionIDListMap();
            var listmap_measurename = MeasureIDTable.GetMeasBaseTextMap();

            var serialization = new Relations.MeasureRootSerialization();

            foreach ( var id in listmap_result)
            {
                if (!listmap_measureparams.ContainsKey(id.Key)) continue;
                if (!listmap_regiontype.ContainsKey(id.Key)) continue;
                if (!listmap_measurename.ContainsKey(id.Key)) continue;
                if (id.Key == MeasureIDTable.EMeasBaseType.NONE) continue;

                Relations.MeasureItemSerialization measureitem = new Relations.MeasureItemSerialization();
                measureitem.ID = (int)id.Key;
                measureitem.Label = listmap_measurename[id.Key];
                measureitem.ResultName = listmap_result[id.Key];

                measureitem.MeasureParamName = new List<Relations.MeasureParamsSerialization>();
                foreach( var measureparam in listmap_measureparams[id.Key])
                {
                    var relation = new Relations.MeasureParamsSerialization();
                    relation.Label = measureparam;
                    relation.Default = (decimal)0;

                    measureitem.MeasureParamName.Add(relation);
                }

                measureitem.RegionID = new List<int>();
                foreach ( var regiontype in listmap_regiontype[id.Key])
                {
                    measureitem.RegionID.Add((int)regiontype);
                }

                serialization.Measure.Add(measureitem);
            }

            return serialization;
        }


    }
}

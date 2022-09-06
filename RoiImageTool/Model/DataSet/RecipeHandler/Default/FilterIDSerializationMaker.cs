using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Default
{
    public class FilterIDSerializationMaker
    {
        /// <summary>
        /// FilterIDTableを使用して、デフォルト値のFilterシリアライズオブジェクトを作成します。
        /// </summary>
        /// <returns>シリアライズオブジェクト</returns>
        public Relations.FilterRootSerialization MakeSerialization()
        {
            var listmap_filterparams = FilterIDTable.GetFilterParamsMap();
            var listmap_filtername = FilterIDTable.GetFilterTextMap();

            var serialization = new Relations.FilterRootSerialization();

            foreach (var id in listmap_filterparams)
            {
                if (!listmap_filtername.ContainsKey(id.Key)) continue;
                if (id.Key == FilterIDTable.EFilterType.NONE) continue;

                Relations.FilterItemSerialization filteritem = new Relations.FilterItemSerialization();
                filteritem.ID = (int)id.Key;
                filteritem.Label = listmap_filtername[id.Key];

                filteritem.FilterParamName = new List<Relations.FilterParamsSerialization>();
                foreach (var regionparam in listmap_filterparams[id.Key])
                {
                    var relation = new Relations.FilterParamsSerialization();
                    relation.Label = regionparam;
                    relation.Default = (decimal)0;

                    filteritem.FilterParamName.Add(relation);
                }

                serialization.Filter.Add(filteritem);
            }

            return serialization;

        }
    }
}

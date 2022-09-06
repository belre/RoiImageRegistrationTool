using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

using ClipXmlReader.Model.Interface;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Relations
{
    /// <summary>
    /// Measuresの項目のシリアライズされたテンプレートであることを表します。
    /// </summary>
    [Serializable]
    public class MeasureItemSerialization : RelationSerialization
    {
        /// <summary>
        /// MeasurenoのIDを表します。
        /// </summary>
        [XmlAttribute]
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// Measureのラベルを表します。
        /// </summary>
        [XmlElement]
        public string Label
        {
            get;
            set;
        }

        /// <summary>
        /// Measureの結果として表示される最低必要なラベルを表します。
        /// </summary>
        [XmlElement]
        public List<string> ResultName
        {
            get;
            set;
        }

        /// <summary>
        /// Measureを実行するうえで必要な、最低限のRegionの個数を表します。
        /// </summary>
        [XmlElement]
        public int MinimumRegionNumber
        {
            get;
            set;
        }


        /// <summary>
        /// Measureにおいて定義される、可能なRegionのIDを表します。
        /// </summary>
        [XmlElement]
        public List<int> RegionID
        {
            get;
            set;
        }

        /// <summary>
        /// レシピに登録する有効なIDを表します。
        /// </summary>
        [XmlElement]
        public int ValidID
        {
            get;
            set;
        }


        /// <summary>
        /// 自動座標計算で使用するモードを表します。
        /// </summary>
        [XmlElement]
        public ClipMeasure.AutoCoordinateCalculator.EModeCoordinateCalculator AutoCoordCalculationMode
        {
            get;
            set;
        }

        /// <summary>
        /// IDごとでのパラメータ制約(親に付随)を表します。
        /// </summary>
        [XmlElement("Measure-base-params")]
        public List<MeasureBaseParamsSerialization> MeasureBaseParamName
        {
            get;
            set;
        }

        /// <summary>
        /// Resultの詳細結果を出力します。
        /// </summary>
        [XmlElement("Result-default")]
        public List<MeasureResultSerialization> ResultDefaults
        {
            get;
            set;
        }

        /// <summary>
        /// とあるIDにおけるパラメータ制約を表します。
        /// </summary>
        [XmlElement("Measure-params")]
        public List<MeasureParamsSerialization> MeasureParamName
        {
            get;
            set;
        }


        /// <summary>
        /// Resultの詳細結果を、ResultDefaultsとResultNameから生成します。
        /// </summary>
        [XmlIgnore]
        public List<MeasureResultSerialization> ResultDefaultsAll
        {
            get
            {
                // ResultDefaultsをmapに変更
                Dictionary<string, MeasureResultSerialization> defaultmap = new Dictionary<string, MeasureResultSerialization>();
                foreach (var defaultitem in ResultDefaults)
                {
                    defaultmap[defaultitem.ResultName] = defaultitem;
                }

                List<MeasureResultSerialization> lists = new List<MeasureResultSerialization>();
                foreach (var name in ResultName)
                {
                    if (defaultmap.Keys.Contains(name))
                    {
                        lists.Add(defaultmap[name]);
                    }
                    else
                    {
                        lists.Add(new MeasureResultSerialization(name));
                    }
                }

                return lists;
            }
        }

        public MeasureItemSerialization()
        {
            MeasureParamName = new List<MeasureParamsSerialization>();
            ValidID = -1;
        }
    }
}

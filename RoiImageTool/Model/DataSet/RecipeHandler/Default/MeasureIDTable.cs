using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Default
{
    public class MeasureIDTable
    {
        /// <summary>
        /// 既定の計測項目を表します。
        /// </summary>
        public enum EMeasBaseType
        {
            NONE = 0,
            COORD_POINT = 1,
            DISTANCE,
            ANGLE,
            TANGENT,
            SINE,
            COSINE,
            PARALLEL,
            RADIUS,
            DIAMETER,
            CURVE,
            PITCH,
            DENSITY,
            CHROMA,
            COORD_TARGET,
            COORD3_TARGET,
            COORD4_TARGET,
            MEAS_SETTING,
            BAND_WIDTH = 100,
            CORNER_WIDTH,
            EDGE_STRAIGHTNESS,
            PARTICLE = 103,
            SCRATCH,
            UNIFORMITY,
            PATTERN_MATCH,
            MASTER_CAPTURE = 200,
            SCAN = 500
        }

        protected static Dictionary<EMeasBaseType, string> _measbase_textmap;

        /// <summary>
        /// 既定の計測項目のラベルを表します。
        /// </summary>
        /// <returns>既定の計測項目ラベルの一覧</returns>
        public static Dictionary<EMeasBaseType, string> GetMeasBaseTextMap()
        {
            if (_measbase_textmap == null)
            {
                _measbase_textmap = new Dictionary<EMeasBaseType, string>();

                _measbase_textmap[EMeasBaseType.COORD_POINT] = "座標点";
                _measbase_textmap[EMeasBaseType.DISTANCE] = "距離";
                _measbase_textmap[EMeasBaseType.ANGLE] = "角度";
                _measbase_textmap[EMeasBaseType.TANGENT] = "正接";
                _measbase_textmap[EMeasBaseType.SINE] = "正弦";
                _measbase_textmap[EMeasBaseType.COSINE] = "余弦";
                _measbase_textmap[EMeasBaseType.PARALLEL] = "平行";
                _measbase_textmap[EMeasBaseType.RADIUS] = "半径";
                _measbase_textmap[EMeasBaseType.DIAMETER] = "直径";
                _measbase_textmap[EMeasBaseType.CURVE] = "曲がり";
                _measbase_textmap[EMeasBaseType.PITCH] = "ピッチ";
                _measbase_textmap[EMeasBaseType.DENSITY] = "濃度";
                _measbase_textmap[EMeasBaseType.CHROMA] = "色相";
                _measbase_textmap[EMeasBaseType.COORD_TARGET] = "基準座標";
                _measbase_textmap[EMeasBaseType.COORD3_TARGET] = "基準座標3点";
                _measbase_textmap[EMeasBaseType.COORD4_TARGET] = "基準座標4点";
                _measbase_textmap[EMeasBaseType.MEAS_SETTING] = "測定設定";
                _measbase_textmap[EMeasBaseType.BAND_WIDTH] = "バンド幅";
                _measbase_textmap[EMeasBaseType.CORNER_WIDTH] = "コーナー幅";
                _measbase_textmap[EMeasBaseType.EDGE_STRAIGHTNESS] = "エッジ直線性";
                _measbase_textmap[EMeasBaseType.PARTICLE] = "粒子";
                _measbase_textmap[EMeasBaseType.SCRATCH] = "傷";
                _measbase_textmap[EMeasBaseType.UNIFORMITY] = "レベル均一性";
                _measbase_textmap[EMeasBaseType.PATTERN_MATCH] = "パターン一致率";
                _measbase_textmap[EMeasBaseType.MASTER_CAPTURE] = "マスター画像取得";
                _measbase_textmap[EMeasBaseType.SCAN] = "スキャン";
            }

            return _measbase_textmap;
        }

        /// <summary>
        /// 現在の計測タイプを表します。
        /// </summary>
        public EMeasBaseType ItemType
        {
            get;
            set;
        }

        public MeasureIDTable(EMeasBaseType itemtype)
        {
            ItemType = itemtype;
        }

        private static Dictionary<EMeasBaseType, List<string>> _resultlistmap;
        /// <summary>
        /// 計測項目に対する結果の一覧を表します。
        /// </summary>
        /// <returns></returns>
        public static Dictionary<EMeasBaseType, List<string>> GetResultListMap()
        { 
            if( _resultlistmap == null)
            {
                _resultlistmap = new Dictionary<EMeasBaseType, List<string>>();
                _resultlistmap[EMeasBaseType.NONE] = new List<string>() { };
                _resultlistmap[EMeasBaseType.COORD_POINT] = new List<string>() { "X", "Y" };
                _resultlistmap[EMeasBaseType.DISTANCE] = new List<string>() { "長さ" };
                _resultlistmap[EMeasBaseType.ANGLE] = new List<string>() { "角度" };
                _resultlistmap[EMeasBaseType.TANGENT] = new List<string>() { "長さ" };
                _resultlistmap[EMeasBaseType.SINE] = new List<string>() { "長さ" };
                _resultlistmap[EMeasBaseType.COSINE] = new List<string>() { "長さ" };
                _resultlistmap[EMeasBaseType.PARALLEL] = new List<string>() { "長さ" };
                _resultlistmap[EMeasBaseType.RADIUS] = new List<string>() { "中心X", "中心Y", "半径" };
                _resultlistmap[EMeasBaseType.DIAMETER] = new List<string>() { "中心X", "中心Y", "直径" };
                _resultlistmap[EMeasBaseType.CURVE] = new List<string>() { "曲り" };
                _resultlistmap[EMeasBaseType.PITCH] = new List<string>() { "平均", "差", "6σ" };
                _resultlistmap[EMeasBaseType.DENSITY] = new List<string>() { "平均", "差", "6σ" };
                _resultlistmap[EMeasBaseType.CHROMA] = new List<string>() { "L*", "A", "B", "⊿E" };
                _resultlistmap[EMeasBaseType.COORD_TARGET] = new List<string>() { "原点X", "原点Y", "角度" };
                _resultlistmap[EMeasBaseType.COORD3_TARGET] = new List<string>() { "原点X", "原点Y", "角度" };
                _resultlistmap[EMeasBaseType.COORD4_TARGET] = new List<string>() { "原点X", "原点Y", "角度" };
                _resultlistmap[EMeasBaseType.MEAS_SETTING] = new List<string>() { };
                _resultlistmap[EMeasBaseType.BAND_WIDTH] = new List<string>() { "幅" };
                _resultlistmap[EMeasBaseType.CORNER_WIDTH] = new List<string>() { "幅" };
                _resultlistmap[EMeasBaseType.EDGE_STRAIGHTNESS] = new List<string>() { "幅" };
                _resultlistmap[EMeasBaseType.PARTICLE] = new List<string>() { "個数", "最大", "最小", "平均" };
                _resultlistmap[EMeasBaseType.SCRATCH] = new List<string>() { "最大", "本数" };
                _resultlistmap[EMeasBaseType.UNIFORMITY] = new List<string>() { "レベル差" };
                _resultlistmap[EMeasBaseType.PATTERN_MATCH] = new List<string>() { "一致率" };
                _resultlistmap[EMeasBaseType.MASTER_CAPTURE] = new List<string>() { "画素数" };
                _resultlistmap[EMeasBaseType.SCAN] = new List<string>() { };
            }

            return _resultlistmap;
        }


        private static Dictionary<EMeasBaseType, List<string>> _measureparamslistmap;
        
        /// <summary>
        /// 計測項目に対するパラメータの一覧を表します。
        /// </summary>
        /// <returns></returns>
        public static Dictionary<EMeasBaseType, List<string>> GetMeasureParamsListMap()
        {
            if (_measureparamslistmap == null)
            {
                _measureparamslistmap = new Dictionary<EMeasBaseType, List<string>>();
                _measureparamslistmap[EMeasBaseType.NONE] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.COORD_POINT] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.DISTANCE] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.ANGLE] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.TANGENT] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.SINE] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.COSINE] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.PARALLEL] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.RADIUS] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.DIAMETER] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.CURVE] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.PITCH] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.DENSITY] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.CHROMA] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.COORD_TARGET] = new List<string>() { "CoordId" };
                _measureparamslistmap[EMeasBaseType.COORD3_TARGET] = new List<string>() { "CoordId", "Dir" };
                _measureparamslistmap[EMeasBaseType.COORD4_TARGET] = new List<string>() { "CoordId" };
                _measureparamslistmap[EMeasBaseType.MEAS_SETTING] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.BAND_WIDTH] = new List<string>() { "Mode", "Reference" };
                _measureparamslistmap[EMeasBaseType.CORNER_WIDTH] = new List<string>() { "Mode", "Reference" };
                _measureparamslistmap[EMeasBaseType.EDGE_STRAIGHTNESS] = new List<string>() { "Mode", "Reference" };
                _measureparamslistmap[EMeasBaseType.PARTICLE] = new List<string>() { "Mode", "Thresh" };
                _measureparamslistmap[EMeasBaseType.SCRATCH] = new List<string>() { "Mode", "Thresh" };
                _measureparamslistmap[EMeasBaseType.UNIFORMITY] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.PATTERN_MATCH] = new List<string>() { "Mode" };
                _measureparamslistmap[EMeasBaseType.MASTER_CAPTURE] = new List<string>() { };
                _measureparamslistmap[EMeasBaseType.SCAN] = new List<string>() { "Format", "Frame" };
            }

            return _measureparamslistmap;
        }

        /// <summary>
        /// 計測項目に対する許容されるパラメータの一覧を取得します。
        /// </summary>
        private static Dictionary<EMeasBaseType, List<RegionIDTable.EROIType>> _regionIDlistmap;
        public static Dictionary<EMeasBaseType, List<RegionIDTable.EROIType>> GetRegionIDListMap()
        {
            if( _regionIDlistmap == null )
            {
                _regionIDlistmap = new Dictionary<EMeasBaseType, List<RegionIDTable.EROIType>>();
                _regionIDlistmap[EMeasBaseType.NONE] = new List<RegionIDTable.EROIType>() { };
                _regionIDlistmap[EMeasBaseType.COORD_POINT] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.CROSSLINE, RegionIDTable.EROIType.CORNEREDGE };
                _regionIDlistmap[EMeasBaseType.DISTANCE] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.LINE, RegionIDTable.EROIType.CROSSLINE, RegionIDTable.EROIType.EDGE, RegionIDTable.EROIType.CORNEREDGE };
                _regionIDlistmap[EMeasBaseType.ANGLE] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.LINE, RegionIDTable.EROIType.CROSSLINE, RegionIDTable.EROIType.EDGE, RegionIDTable.EROIType.CORNEREDGE };
                _regionIDlistmap[EMeasBaseType.TANGENT] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.LINE, RegionIDTable.EROIType.CROSSLINE, RegionIDTable.EROIType.EDGE, RegionIDTable.EROIType.CORNEREDGE };
                _regionIDlistmap[EMeasBaseType.SINE] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.LINE, RegionIDTable.EROIType.CROSSLINE, RegionIDTable.EROIType.EDGE, RegionIDTable.EROIType.CORNEREDGE };
                _regionIDlistmap[EMeasBaseType.COSINE] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.LINE, RegionIDTable.EROIType.CROSSLINE, RegionIDTable.EROIType.EDGE, RegionIDTable.EROIType.CORNEREDGE };
                _regionIDlistmap[EMeasBaseType.PARALLEL] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.LINE, RegionIDTable.EROIType.CROSSLINE, RegionIDTable.EROIType.EDGE, RegionIDTable.EROIType.CORNEREDGE };
                _regionIDlistmap[EMeasBaseType.RADIUS] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.ARC };
                _regionIDlistmap[EMeasBaseType.DIAMETER] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.ARC };
                _regionIDlistmap[EMeasBaseType.CURVE] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.CURVE };
                _regionIDlistmap[EMeasBaseType.PITCH] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.PITCH };
                _regionIDlistmap[EMeasBaseType.DENSITY] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.DENSITY };
                _regionIDlistmap[EMeasBaseType.CHROMA] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.CHROMA };
                _regionIDlistmap[EMeasBaseType.COORD_TARGET] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.PING };
                _regionIDlistmap[EMeasBaseType.COORD3_TARGET] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.LINE, RegionIDTable.EROIType.CROSSLINE, RegionIDTable.EROIType.EDGE, RegionIDTable.EROIType.CORNEREDGE };
                _regionIDlistmap[EMeasBaseType.COORD4_TARGET] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.LINE, RegionIDTable.EROIType.CROSSLINE, RegionIDTable.EROIType.EDGE, RegionIDTable.EROIType.CORNEREDGE };
                _regionIDlistmap[EMeasBaseType.MEAS_SETTING] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.CHARACTER, RegionIDTable.EROIType.BARCODE };
                _regionIDlistmap[EMeasBaseType.BAND_WIDTH] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.BANDWIDTH };
                _regionIDlistmap[EMeasBaseType.CORNER_WIDTH] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.CORNERWIDTH };
                _regionIDlistmap[EMeasBaseType.EDGE_STRAIGHTNESS] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.STRAIGHTNESS };
                _regionIDlistmap[EMeasBaseType.PARTICLE] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.PARTICLE };
                _regionIDlistmap[EMeasBaseType.SCRATCH] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.PARTICLE };
                _regionIDlistmap[EMeasBaseType.UNIFORMITY] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.UNIFORMITY };
                _regionIDlistmap[EMeasBaseType.PATTERN_MATCH] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.PATTERN_MATCH };
                _regionIDlistmap[EMeasBaseType.MASTER_CAPTURE] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE, RegionIDTable.EROIType.MASTER_CAPTURE };
                _regionIDlistmap[EMeasBaseType.SCAN] = new List<RegionIDTable.EROIType>() { RegionIDTable.EROIType.REFERENCE };
            }

            return _regionIDlistmap;
        }




        /// <summary>
        /// 現在のItemTypeに対する結果を返します。
        /// </summary>
        public List<string> ResultsNameList
        {
            get
            {
                return GetResultListMap()[ItemType];
            }
        }

        /// <summary>
        /// 現在のItemTypeに対するパラメータを返します。
        /// </summary>
        public List<string> MeasureParamsList
        {
            get
            {
                return GetMeasureParamsListMap()[ItemType];
            }
        }

        /// <summary>
        /// 現在のItemTypeに対するRegion IDの候補を返します。
        /// </summary>
        public List<RegionIDTable.EROIType> RegionItemTypeList
        {
            get
            {
                return GetRegionIDListMap()[ItemType];
            }
        }

        
    }
}

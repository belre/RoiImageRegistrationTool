using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Default
{

    public class RegionIDTable
    {
        /// <summary>
        /// 既定のRegion IDを表します。
        /// </summary>
        public enum EROIType
        {
            NONE = -1,
            REFERENCE = 0,
            LINE = 1,
            CROSSLINE = 2,
            EDGE = 3,
            CORNEREDGE = 4,
            ARC = 5,
            CURVE = 6,
            PITCH = 7,
            DENSITY = 8,
            CHROMA = 9,
            PING = 10,
            CHARACTER = 11,
            BARCODE = 12,
            BANDWIDTH = 100,
            CORNERWIDTH = 101,
            STRAIGHTNESS = 102,
            PARTICLE = 103,
            UNIFORMITY = 104,
            PATTERN_MATCH = 105,
            MASTER_CAPTURE = 200
        }

        /// <summary>
        /// 既定のRegionに対するラベルを出力します。
        /// </summary>
        protected static Dictionary<EROIType, string> _roi_textmap;
        public static Dictionary<EROIType, string> GetRoiTextMap()
        {
            if (_roi_textmap == null)
            {
                _roi_textmap = new Dictionary<EROIType, string>();

                _roi_textmap[EROIType.REFERENCE] = "参照";
                _roi_textmap[EROIType.LINE] = "直線検出";
                _roi_textmap[EROIType.CROSSLINE] = "クロスライン検出";
                _roi_textmap[EROIType.EDGE] = "エッジ";
                _roi_textmap[EROIType.CORNEREDGE] = "コーナーエッジ";
                _roi_textmap[EROIType.ARC] = "円弧";
                _roi_textmap[EROIType.CURVE] = "曲り";
                _roi_textmap[EROIType.PITCH] = "ピッチ";
                _roi_textmap[EROIType.DENSITY] = "濃度";
                _roi_textmap[EROIType.CHROMA] = "色相";
                _roi_textmap[EROIType.PING] = "座標系";
                _roi_textmap[EROIType.CHARACTER] = "文字";
                _roi_textmap[EROIType.BARCODE] = "バーコード";
                _roi_textmap[EROIType.BANDWIDTH] = "バンド幅";
                _roi_textmap[EROIType.CORNERWIDTH] = "コーナー幅";
                _roi_textmap[EROIType.STRAIGHTNESS] = "直線性";
                _roi_textmap[EROIType.PARTICLE] = "粒子";
                _roi_textmap[EROIType.UNIFORMITY] = "レベル均一性";
                _roi_textmap[EROIType.PATTERN_MATCH] = "パターンマッチング";
                _roi_textmap[EROIType.MASTER_CAPTURE] = "マスターキャプチャー";
            }

            return _roi_textmap;
        }



        protected EROIType ItemType = EROIType.NONE;

        public RegionIDTable(EROIType itemtype)
        {
            ItemType = itemtype;
        }

        private static Dictionary<EROIType, List<string>> _regionparamlistmap;
        
        /// <summary>
        /// Region別に表示する領域を表すラベルを取得します。
        /// </summary>
        /// <returns></returns>
        public static Dictionary<EROIType, List<string>> GetRegionParamListMap()
        {
            if (_regionparamlistmap == null)
            {
                _regionparamlistmap = new Dictionary<EROIType, List<string>>();
                _regionparamlistmap[EROIType.REFERENCE] = new List<string>() { "Ref-meas", "Ref-rect" };
                _regionparamlistmap[EROIType.LINE] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.CROSSLINE] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.EDGE] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.CORNEREDGE] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.ARC] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.CURVE] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.PITCH] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.DENSITY] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.CHROMA] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.PING] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.CHARACTER] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.BARCODE] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.BANDWIDTH] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.CORNERWIDTH] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.STRAIGHTNESS] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.PARTICLE] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.UNIFORMITY] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.PATTERN_MATCH] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
                _regionparamlistmap[EROIType.MASTER_CAPTURE] = new List<string>() { "Coord.LT.x", "Coord.LT.y", "Coord.Width", "Coord.Height", "DetColor" };
            }

            return _regionparamlistmap;
        }

        private static Dictionary<EROIType, List<string>> _regionfeatureparamsmap;
        /// <summary>
        /// Regionにおけるパラメータのラベルを取得します。
        /// </summary>
        /// <returns></returns>
        public static Dictionary<EROIType, List<string>> GetRegionFeatureParamsMap()
        {
            if (_regionfeatureparamsmap == null)
            {
                _regionfeatureparamsmap = new Dictionary<EROIType, List<string>>();
                _regionfeatureparamsmap[EROIType.NONE] = new List<string>() { };
                _regionfeatureparamsmap[EROIType.REFERENCE] = new List<string>() { };
                _regionfeatureparamsmap[EROIType.LINE] = new List<string>() { "DetDir", "SearchDir", "Logic", "Thresh" };
                _regionfeatureparamsmap[EROIType.CROSSLINE] = new List<string>() { "SearchDirX", "SearchDirY", "Logic", "Thresh" };
                _regionfeatureparamsmap[EROIType.EDGE] = new List<string>() { "DetDir", "SearchDir", "Logic", "Thresh", "Filter" };
                _regionfeatureparamsmap[EROIType.CORNEREDGE] = new List<string>() { "DetDir", "SearchDir", "Logic", "Thresh", "Filter" };
                _regionfeatureparamsmap[EROIType.ARC] = new List<string>() { };
                _regionfeatureparamsmap[EROIType.CURVE] = new List<string>() { "DetDir", "SearchDir", "Logic", "Length", "Interval", "VirtualLine" };
                _regionfeatureparamsmap[EROIType.PITCH] = new List<string>() { "DetDir", "Logic", "Interval", "SearchRange", "Target" };
                _regionfeatureparamsmap[EROIType.DENSITY] = new List<string>() { };
                _regionfeatureparamsmap[EROIType.CHROMA] = new List<string>() { };
                _regionfeatureparamsmap[EROIType.PING] = new List<string>() { "FormType", "Logic", "Filter" };
                _regionfeatureparamsmap[EROIType.CHARACTER] = new List<string>() { };
                _regionfeatureparamsmap[EROIType.BARCODE] = new List<string>() { };
                _regionfeatureparamsmap[EROIType.BANDWIDTH] = new List<string>() { };
                _regionfeatureparamsmap[EROIType.CORNERWIDTH] = new List<string>() { };
                _regionfeatureparamsmap[EROIType.STRAIGHTNESS] = new List<string>() { };
                _regionfeatureparamsmap[EROIType.PARTICLE] = new List<string>() { "Logic", "Feature", "Thresh" };
                _regionfeatureparamsmap[EROIType.UNIFORMITY] = new List<string>() { };
                _regionfeatureparamsmap[EROIType.PATTERN_MATCH] = new List<string>() { };
                _regionfeatureparamsmap[EROIType.MASTER_CAPTURE] = new List<string>() { "Mode", "Logic", "Thresh", "Filter" };
            }

            return _regionfeatureparamsmap;
        }
       
        /// <summary>
        /// 領域の名称を表します。
        /// </summary>
        public List<string> ROINameList
        {
            get
            {
                return GetRegionParamListMap()[ItemType];
            }
        }
        
        /// <summary>
        /// 現在のパラメータを表します。
        /// </summary>
        public List<string> FeatureParamsList
        {
            get
            {
                return GetRegionFeatureParamsMap()[ItemType];
            }
        }


    }
}

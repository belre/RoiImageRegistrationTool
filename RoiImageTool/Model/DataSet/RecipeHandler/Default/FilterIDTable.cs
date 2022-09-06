using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Default
{
    /// <summary>
    /// FilterIDのデフォルト値を表すクラスです。
    /// </summary>
    public class FilterIDTable
    {
        /// <summary>
        /// 既定のフィルタタイプを表します。
        /// </summary>
        public enum EFilterType
        {
            NONE = 0,
            SOURCE = 1,
            BINARIZE = 2,
            EROSION = 3,
            DILATION = 4,
            EDGE_EXTRACTION = 5,
            IMAGE_DIFFERENCE = 6,
            MASTER_DIFFERENCE = 7,
            MASKING = 8,
            OPENING = 9,
            CLOSING = 10
        }

        /// <summary>
        /// 既定のフィルタタイプに対するラベルを表します。
        /// </summary>
        protected static Dictionary<EFilterType, string> _filter_textmap;
        public static Dictionary<EFilterType, string> GetFilterTextMap()
        {
            if (_filter_textmap == null)
            {
                _filter_textmap = new Dictionary<EFilterType, string>();

                _filter_textmap[EFilterType.SOURCE] = "ソース選択";
                _filter_textmap[EFilterType.BINARIZE] = "2値化";
                _filter_textmap[EFilterType.EROSION] = "エロージョン";
                _filter_textmap[EFilterType.DILATION] = "ダイレーション";
                _filter_textmap[EFilterType.EDGE_EXTRACTION] = "エッジ抽出";
                _filter_textmap[EFilterType.IMAGE_DIFFERENCE] = "差分画像";
                _filter_textmap[EFilterType.MASTER_DIFFERENCE] = "マスター差分画像";
                _filter_textmap[EFilterType.MASKING] = "マスク処理";
                _filter_textmap[EFilterType.OPENING] = "オープニング";
                _filter_textmap[EFilterType.CLOSING] = "クロージング";

            }

            return _filter_textmap;
        }

        protected EFilterType ItemType = EFilterType.NONE;

        public FilterIDTable(EFilterType itemtype)
        {
            ItemType = itemtype;
        }

        private static Dictionary<EFilterType, List<string>> _filterparamsmap;
        /// <summary>
        /// 既定のフィルタタイプに対するパラメータのラベルを表します。
        /// </summary>
        /// <returns></returns>
        public static Dictionary<EFilterType, List<string>> GetFilterParamsMap()
        {
            if (_filterparamsmap == null)
            {
                _filterparamsmap = new Dictionary<EFilterType, List<string>>();
                _filterparamsmap[EFilterType.NONE] = new List<string>() { };
                _filterparamsmap[EFilterType.SOURCE] = new List<string>() { "Source", "Color" };
                _filterparamsmap[EFilterType.BINARIZE] = new List<string>() { "Mode", "ThreshA", "ThreshB" };
                _filterparamsmap[EFilterType.EROSION] = new List<string>() { "Num" };
                _filterparamsmap[EFilterType.DILATION] = new List<string>() { "Num" };
                _filterparamsmap[EFilterType.EDGE_EXTRACTION] = new List<string>() { "Logic", "Width" };
                _filterparamsmap[EFilterType.IMAGE_DIFFERENCE] = new List<string>() { "Source1", "Source2", "Color1", "Color2" };
                _filterparamsmap[EFilterType.MASTER_DIFFERENCE] = new List<string>() { "Logic", "Thresh", "Filter", "Outside" };
                _filterparamsmap[EFilterType.MASKING] = new List<string>() { "Logic" };
                _filterparamsmap[EFilterType.OPENING] = new List<string>() { "Num" };
                _filterparamsmap[EFilterType.CLOSING] = new List<string>() { "Num" };
            }

            return _filterparamsmap;
        }

        /// <summary>
        /// ItemTypeが指定された時のフィルタパラメータのラベルを取得します。
        /// </summary>
        public List<string> FilterParamsList
        {
            get
            {
                return GetFilterParamsMap()[ItemType];
            }
        }

    }
}

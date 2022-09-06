using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using ClipMeasure.Wrapper.Managed;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Group
{
    /// <summary>
    /// 全部の領域を表すクラスです。
    /// </summary>
    public class RegionsGroup : Base.BaseTreeGroup
    {

        /// <summary>
        /// 領域の一覧を表します。
        /// </summary>
        public List<RegionGroup> RegionList
        {
            get;
            set;
        }

        /// <summary>
        /// 絶対座標系で表記されたRegionのラッパーを配列で渡します。
        /// </summary>
        public List<WpAutoCoordinateRegion> RelativeCoordinateRegionList
        {
            get
            {
                List<WpAutoCoordinateRegion> regionlist = new List<WpAutoCoordinateRegion>();
                foreach( var roi in RegionList)
                {
                    regionlist.Add(roi.RelativeCoordinateRegion);
                }

                return regionlist;
            }
        }

        /// <summary>
        /// 最低限登録可能な領域の個数を表します。
        /// </summary>
        public int MinimumRegionNumber
        {
            get
            {
                return RelationsObject.Measures.MeasureDict[ParentMeasType].MinimumRegionNumber;
            }
        }

        /// <summary>
        /// 参照を除く最初のItemTypeを取得します。
        /// </summary>
        public int FirstItemTypeWithoutReference
        {
            get
            {
                if (RelationsObject.Measures.MeasureDict[ParentMeasType].RegionID.Count == 1)
                {
                    return RelationsObject.Measures.MeasureDict[ParentMeasType].RegionID[0];
                }
                else if (RelationsObject.Measures.MeasureDict[ParentMeasType].RegionID.Count > 1)
                {
                    foreach (var id in RelationsObject.Measures.MeasureDict[ParentMeasType].RegionID)
                    {
                        if (id != 0)
                        {
                            return id;
                        }
                    }
                }

                return -1;
            }
        } 

        /// <summary>
        /// 親から与えられる計測項目IDを表します。
        /// </summary>
        public int ParentMeasType
        {
            get
            {
                return SpecifiedParent.ItemType;
            }
        }

        public override void UpdateFromMeasureRelation()
        {
            // 計測項目IDが変更された場合は、領域についてもテンプレートで変更する
            if (RelationsObject.Measures.MeasureDict.ContainsKey(ParentMeasType))
            {
                RegionList.Clear();

                int minnumber = MinimumRegionNumber;
                for (int i = 0; i < minnumber; i++)
                {
                    RegionGroup obj = new RegionGroup(this);
                    obj.SetParameter<int>(obj.Key_ItemType, FirstItemTypeWithoutReference);
                    obj.RelationsObject = RelationsObject;
                    RegionList.Add(obj);
                }
            }

            // 各領域についても親の参照を実行する
            foreach (var roi in RegionList)
            {
                roi.UpdateFromRelation();
            }
        }


        /// <summary>
        /// 座標変換に関するエラー状態を表します.
        /// </summary>
        public ECoordConversionErrorStatus RoiErrorStatus
        {
            get
            {
                foreach( var region in RegionList)
                {
                    if( region.RoiGroupObject.AbsoluteObjectErrorStatus == ECoordConversionErrorStatus.COORDID_REF_OUTOFRANGE)
                    {
                        return ECoordConversionErrorStatus.COORDID_REF_OUTOFRANGE;
                    }
                    else if( region.RoiGroupObject.AbsoluteObjectErrorStatus == ECoordConversionErrorStatus.COORDID_REF_SYNTAXERROR)
                    {
                        return ECoordConversionErrorStatus.COORDID_REF_SYNTAXERROR;
                    }
                    else if( region.RoiGroupObject.AbsoluteObjectErrorStatus == ECoordConversionErrorStatus.NOTRUN)
                    {
                        return ECoordConversionErrorStatus.NOTRUN;
                    }
                }

                return ECoordConversionErrorStatus.OK;
            }
        }


        public bool IsRefMeasValid
        {
            get
            {

                foreach (var region in RegionList)
                {
                    if( ! region.RoiGroupObject.IsNullRefMeasObject )
                    {
                        if( region.RoiGroupObject.RefMeas < 0)
                        {
                            return false;
                        }
                    }
                }

                return true;           
            }
        }

        public bool IsRefRectValid
        {
            get
            {

                foreach (var region in RegionList)
                {
                    if (!region.RoiGroupObject.IsNullRefMeasObject)
                    {
                        if (region.RoiGroupObject.RefRect < 0 ||
                            region.RoiGroupObject.RefRect >= region.RoiGroupObject.RefMeasObject.RegionsGroupObject.RegionList.Count)
                        {
                            return false;
                        }

                    }
                }

                return true;
            }

        }



        /// <summary>
        /// 特定された親オブジェクトを表します。
        /// </summary>
        public virtual MeasureGroup SpecifiedParent
        {
            get;
            protected set;
        }

        /// <summary>
        /// 汎用的な親オブジェクトを表します。
        /// </summary>
        public override Base.BaseTreeGroup Parent
        {
            get
            {
                return this.SpecifiedParent;
            }
        }

        public RegionsGroup(Group.MeasureGroup parent)
            : base()
        {
            _element_name = "Regions";
            SpecifiedParent = parent;

            RegionList = new List<RegionGroup>();
        }



        List<RegionGroup> _tmpregion = new List<RegionGroup>();
        /// <summary>
        /// オープンタグを検出したときの処理を表します。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected override void ParseBeginElement(XmlReader reader, Stack<string> hierarchical)
        {
            var item = new Group.RegionGroup(this);
            item.UpdateFromRelation();
            //item.ParentMeasType = ParentMeasType;

            if (reader.Name.Equals(item.ElementName))
            {
                item.InitAttributes(reader);
                item.RelationsObject = RelationsObject;
                item.ParseXmlRecipe(reader, hierarchical);
                _tmpregion.Add(item);            
            }
        }

        /// <summary>
        /// クローズタグを検出したときの処理を検出します。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected override void ParseEndElement(XmlReader reader, Stack<string> hierarchical)
        {
            // 直接RegionListを指定しない理由
            // ItemTypeが変更された場合、自動的に最低限の個数分の領域を確保する処理が実行されるため
            RegionList = _tmpregion;
        }

        /// <summary>
        /// XML Documentをサブクラスに移譲します。
        /// </summary>
        /// <param name="document"></param>
        /// <param name="current"></param>
        protected override void DelegateSubGroup(XmlDocument document, XmlElement current)
        {
            foreach (var itemgroup in RegionList)
            {
                itemgroup.MakeXmlNode(document, current);
            }
        }

        /// <summary>
        /// RegionsにRegionが追加された場合の命令を表します。
        /// </summary>
        /// <param name="targetgen"></param>
        /// <param name="isappendnext"></param>
        public override void InsertGroup(Base.BaseTreeGroup targetgen, bool isappendnext = false)
        {
            if (RegionList.Count == 0)
            {

                


            }
            else
            {
                var target = (RegionGroup)targetgen;

                int pos = RegionList.IndexOf(target);

                if (isappendnext)
                {
                    pos++;
                }

                RegionGroup region = new RegionGroup(this);
                region.ItemType = target.ItemType;
                region.RelationsObject = target.RelationsObject;

                if (pos == RegionList.Count)
                {
                    RegionList.Add(region);
                }
                else
                {
                    RegionList.Insert(pos, region);
                }

            }
        }


        /// <summary>
        /// 特定のRegionを削除する命令を表します。
        /// </summary>
        /// <param name="targetgen"></param>
        public override void DeleteGroup(Base.BaseTreeGroup targetgen)
        {
            if (RegionList.Count != 0)
            {
                RegionList.Remove((RegionGroup)targetgen);
            }
        }

    }
}

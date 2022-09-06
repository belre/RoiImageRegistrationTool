using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using ClipMeasure.Wrapper.Managed;

using System.Windows.Media;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Group
{
    /// <summary>
    /// 領域を表すクラスです。
    /// </summary>
    public class RegionGroup : Base.BaseTreeGroup
    {


        /// <summary>
        /// 祖先となる計測オブジェクトを表します。
        /// </summary>
        public MeasureGroup AncestorMeasureObject
        {
            get
            {
                return SpecifiedParent.SpecifiedParent;
            }
        }

        /// <summary>
        /// RegionListから見た時の現在の位置を表します。
        /// </summary>
        public int Index
        {
            get
            {
                if(SpecifiedParent == null)
                {
                    return -2;
                }

                return SpecifiedParent.RegionList.IndexOf(this);
            }
        }

        /// <summary>
        /// このオブジェクトが親に含まれていることを表します。
        /// </summary>
        public bool IsIncludesParent
        {
            get
            {
                if (SpecifiedParent == null)
                {
                    return false;
                }

                if (SpecifiedParent.RegionList.Exists(
                    (obj) => { return obj == this; }
                    ))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
      

        protected DataType.GenericDataType _keys_itemtype = new DataType.GenericDataType("ID", typeof(int));
        /// <summary>
        /// 領域IDを表すキーです。
        /// </summary>
        public object Key_ItemType
        {
            get
            {
                return _keys_itemtype;
            }
        }

        /// <summary>
        /// 現在格納されている領域IDを表します。
        /// setterを使用した場合、子オブジェクトにも反映されます。
        /// </summary>
        public int ItemType
        {
            get
            {
                return GetParameter<int>(_keys_itemtype);
            }
            set
            {
                SetParameter<int>(_keys_itemtype, value);

                if (RelationsObject != null)
                {
                    if (RoiGroupObject != null)
                    {
                        RoiGroupObject.UpdateFromRelation();
                    }

                    if (FeatureParamsGroupObject != null)
                    {
                        FeatureParamsGroupObject.UpdateFromRelation();
                    }
                }
            }
        }

        /// <summary>
        /// この領域が”参照”であることを表します。
        /// </summary>
        public bool IsReferenceRegion
        {
            get
            {
                return RelationsObject.Regions.RegionDict[ItemType].IsReference;
            }
        }


        /// <summary>
        /// 絶対座標系のラッパーを表します。
        /// </summary>
        public WpAutoCoordinateRegion RelativeCoordinateRegion
        {
            get
            {
                RegionGroup reference_item = new RegionGroup(this.SpecifiedParent);

                var obj = RoiGroupObject.GetRelativeCoordinateRegion(out reference_item);
                
                if( obj == null)
                {
                    return null;
                }

                if( reference_item == null)
                {
                    obj.Parameter = FeatureParamsGroupObject.FeatureParameterArray;
                }
                else
                {
                    obj.Parameter = reference_item.FeatureParamsGroupObject.FeatureParameterArray;
                }
                      
                
                return obj;
            }
        }

        /// <summary>
        /// アトリビュートを表すキーを示します。
        /// </summary>
        public override DataType.GenericDataType[] AttributeName
        {
            get
            {
                return new DataType.GenericDataType[] { _keys_itemtype };
            }
        }

        /// <summary>
        /// 自身の要素を表します
        /// (現在のXML書式の場合、入れないほうが良い)
        /// </summary>
        protected override List<DataType.GenericDataType> ownedElement
        {
            get
            {
                return new List<DataType.GenericDataType> {  };
            }
        }


        /// <summary>
        /// パラメータオブジェクトを表します。
        /// </summary>
        public FeatureParamsGroup FeatureParamsGroupObject
        {
            get;
            set;
        }

        /// <summary>
        /// ROI領域に関するオブジェクトを表します。
        /// </summary>
        public ROIGroup RoiGroupObject
        {
            get;
            set;
        }

        /// <summary>
        /// フィルタのオブジェクトを表します。
        /// </summary>
        public FiltersGroup FilterGroupObject
        {
            get;
            set;
        }

        /// <summary>
        /// ROI領域情報の一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> RoiParamList
        {
            get
            {
                return RoiGroupObject.RoiParamList;
            }
            set
            {
                RoiGroupObject.RoiParamList = value;
            }
        }

        /// <summary>
        /// パラメータ情報を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> FeatureParamsList
        {
            get
            {
                return FeatureParamsGroupObject.ParamsList;
            }
        }



        /// <summary>
        /// 計測項目として使用されるRegionで指定できるIDを表します。
        /// </summary>
        public List<int> ItemTypeCandidates
        {
            get
            {
                if (RelationsObject != null)
                {
                    var relationtable_int = RelationsObject.Measures.MeasureDict[ParentMeasType].RegionID;

                    List<int> relationtable = new List<int>();
                    foreach( var relation_int in relationtable_int)
                    {
                        relationtable.Add(relation_int);
                    }

                    return relationtable;
                }
                else
                {
                    var table = new Default.MeasureIDTable((Default.MeasureIDTable.EMeasBaseType)ParentMeasType);
                    var newlist = new List<int>();
                    foreach( var enobj in table.RegionItemTypeList)
                    {
                        newlist.Add((int)enobj);
                    }

                    return newlist;
                    //return table.RegionItemTypeList;
                }
            }
        }

        /// <summary>
        /// 親オブジェクトにおける計測項目IDを表します。
        /// </summary>
        public int ParentMeasType
        {
            get
            {
                return SpecifiedParent.ParentMeasType;
            }
        }

        /// <summary>
        /// 指定された親オブジェクトを表します。
        /// </summary>
        public virtual RegionsGroup SpecifiedParent
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

        /// <summary>
        /// アプリケーションに表示する表示色を表します。
        /// (現在は番号順に指定)
        /// </summary>
        public System.Windows.Media.Color AppBrushColor
        {
            get
            {
                switch (Index)
                {
                    case 0:
                        return Color.FromRgb(255, 0, 0);
                    case 1:
                        return Color.FromRgb(0, 255, 0);
                    case 2:
                        return Color.FromRgb(0, 0, 255);
                    case 3:
                        return Color.FromRgb(255, 255, 0);
                    case 4:
                        return Color.FromRgb(255, 0, 255);
                    default:
                        return Color.FromRgb(0, 255, 255);
                }
            }
        }

        public RegionGroup()
            : this(null)
        {
        }


        public RegionGroup(Group.RegionsGroup parent)
            : base()
        {
            _element_name = "Region";
            SpecifiedParent = parent;

            ItemType = 0;

            FeatureParamsGroupObject = new FeatureParamsGroup(this);
            FilterGroupObject = new FiltersGroup(this);
            RoiGroupObject = new ROIGroup(this);
        }




        /// <summary>
        /// オープンタグが検出された時の処理です。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected override void ParseBeginElement(XmlReader reader, Stack<string> hierarchical)
        {
            // Feature-params
            if(reader.Name.Equals(FeatureParamsGroupObject.ElementName))
            {
                FeatureParamsGroupObject.UpdateFromRelation();
                FeatureParamsGroupObject.ParseXmlRecipe(reader, hierarchical);
            }
            // Filters
            else if(reader.Name.Equals(FilterGroupObject.ElementName))
            {
                FilterGroupObject.UpdateFromRelation();
                FilterGroupObject.ParseXmlRecipe(reader, hierarchical);
            }
            // 特定されたタグ名がない場合
            else
            {
                RoiGroupObject.UpdateFromRelation();
                RoiGroupObject.ParseXmlRecipe(reader, hierarchical);
            }
        }

        /// <summary>
        /// XMLドキュメントを生成します。
        /// </summary>
        /// <param name="document"></param>
        /// <param name="current"></param>
        protected override void DelegateSubGroup(XmlDocument document, XmlElement current)
        { 
            /// ROIオブジェクトに移譲
            RoiGroupObject.MakeXmlNode(document, current);

            /// フィルタオブジェクトに移譲
            if (FilterGroupObject.FilterList.Count != 0)
            {
                FilterGroupObject.MakeXmlNode(document, current);
            }

            // パラメータオブジェクトに移譲
            if (FeatureParamsGroupObject.ParamsList.Count != 0)
            {
                FeatureParamsGroupObject.MakeXmlNode(document, current);
            }
        }

        /// <summary>
        /// 項目を追加する動作が行なわれた時の処理を表します。
        /// （Regionの子オブジェクトであるパラメータオブジェクトを追加します）
        /// </summary>
        /// <param name="targetgen"></param>
        /// <param name="isappendnext"></param>
        public override void InsertTuple(Base.BaseTuple targetgen, bool isappendnext = false)
        {
            UserTuple.NameValuePairTuple target = (UserTuple.NameValuePairTuple)targetgen;

            UserTuple.NameValuePairTuple newitem = new UserTuple.NameValuePairTuple(this);
            newitem.SetParameter<string>(newitem.Key_Name, target.GetParameter<string>(target.Key_Name));
            newitem.SetParameter<decimal>(newitem.Key_Value, target.GetParameter<decimal>(target.Key_Value));
            

            int index = FeatureParamsList.IndexOf(target);
            if (index == -1)
            {
                return;
            }

            if (isappendnext)
            {
                index++;
            }


            if (index == FeatureParamsList.Count)
            {
                FeatureParamsList.Add(newitem);
            }
            else
            {
                FeatureParamsList.Insert(index, newitem);
            }
        }

        /// <summary>
        /// 項目を削除したときの処理を表します
        /// (パラメータオブジェクトに処理を委譲して追加します)
        /// </summary>
        /// <param name="targetgen"></param>
        /// <param name="isappendnext"></param>
        public override void InsertGroup(Base.BaseTreeGroup targetgen, bool isappendnext = false)
        {
            if (targetgen.GetType() == FeatureParamsGroupObject.GetType())
            {
                var target = (FeatureParamsGroup)targetgen;

                var tuple = new UserTuple.NameValuePairTuple(FeatureParamsGroupObject);
                FeatureParamsGroupObject.InsertTuple(tuple);
               
            }
        }

        /// <summary>
        /// 特定の項目を削除します。
        /// </summary>
        /// <param name="targetgen"></param>
        public override void DeleteTuple(Base.BaseTuple targetgen)
        {
            FeatureParamsList.Remove((UserTuple.NameValuePairTuple)targetgen);
        }

        public override void UpdateFromMeasureRelation()
        {
            // それ以外の場合は
            // 値を確認し、変更があれば更新
            var current = GetParameter<int>(Key_ItemType);

            {
                if (RoiGroupObject != null)
                {
                    RoiGroupObject.UpdateFromRelation();
                }

                if (FeatureParamsGroupObject != null)
                {
                    FeatureParamsGroupObject.UpdateFromRelation();
                }


                if (FilterGroupObject != null)
                {
                    FilterGroupObject.UpdateFromRelation();
                }


            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using ClipMeasure.Wrapper.Managed;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Group
{
    public class ROIGroup : Base.BaseTreeGroup
    {
        /// <summary>
        /// DetColorを表す列挙型を表します。
        /// </summary>
        public enum EDetectionColor
        {
            NONE = -1,
            BLACK = 0,
            WHITE = 1,
            RED = 2,
            GREEN = 3,
            BLUE = 4,
            CYAN = 5,
            MAZENDA = 6,
            YELLOW = 7,
            RGB = 8
        }

        /// <summary>
        /// 列挙型に対応するラベルを表します。
        /// </summary>
        protected static Dictionary<EDetectionColor, string> _color_textmap;
        public static Dictionary<EDetectionColor, string> GetColorTextMap()
        {
            if (_color_textmap == null)
            {
                _color_textmap = new Dictionary<EDetectionColor, string>();

                _color_textmap[EDetectionColor.BLACK] = "黒";
                _color_textmap[EDetectionColor.WHITE] = "白";
                _color_textmap[EDetectionColor.RED] = "赤";
                _color_textmap[EDetectionColor.GREEN] = "緑";
                _color_textmap[EDetectionColor.BLUE] = "青";
                _color_textmap[EDetectionColor.CYAN] = "シアン";
                _color_textmap[EDetectionColor.MAZENDA] = "マゼンダ";
                _color_textmap[EDetectionColor.YELLOW] = "黄";
                _color_textmap[EDetectionColor.RGB] = "RGB";
            }

            return _color_textmap;
        }

        protected object _keys_reference_roilist = new object();
        /// <summary>
        /// 参照を表すパラメータリストを表します。
        /// </summary>
        public object Key_Reference_RoiParamList
        {
            get
            {
                return _keys_reference_roilist;
            }
        }

        protected object _keys_coordlt_roilist = new object();
        /// <summary>
        /// 座標情報を表すパラメータリストを表します。
        /// </summary>
        public object Key_CoordLT_RoiParamList
        {
            get
            {
                return _keys_coordlt_roilist;
            }
        }

       
        
        protected UserTuple.ReferenceTuple _reference_tuple;        // リファレンス情報を表すオブジェクト
        protected UserTuple.CoordLTTuple _coord_absolute_tuple;     // 絶対座標を表すオブジェクト
        protected UserTuple.CoordLTTuple _coord_relative_tuple;     // 相対座標を表すオブジェクト


        /// <summary>
        /// Referenceにおけるパラメータ一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> Reference_RoiParamList
        {
            get
            {
                return GetCoordinateObject(true, IsAbsolute);
            }
            set
            {
                _reference_tuple.ParamList = value;

            }
        }


        /// <summary>
        /// リファレンスを使用する場合に適用される参照先のオブジェクトを表します。
        /// </summary>
        public Group.MeasureGroup RefMeasObject
        {

            get
            {
                return _reference_tuple.RefMeasObject;
            }
        }

        /// <summary>
        /// 相対座標系表現のラッパーオブジェクトを取得します。
        /// </summary>
        /// <param name="reference">参照がある場合、ここに参照によるRegionのオブジェクトを結果として出力します。</param>
        /// <returns></returns>
        public WpAutoCoordinateRegion GetRelativeCoordinateRegion(out RegionGroup reference)
        {
            reference = null; 

            if (RelationRegionItem.IsReference)
            {
                var measobj = _reference_tuple.RefMeasObject; 
                if (measobj == null)
                {
                    return null;
                }
                int refindex = _reference_tuple.Value_RefRect;

                if (refindex >= measobj.RegionsGroupObject.RegionList.Count)
                {
                    return null;
                }

                var referobj = measobj.RegionsGroupObject.RegionList[refindex];
                while (referobj.RelationsObject.Regions.RegionDict[referobj.ItemType].IsReference)
                {
                    measobj = referobj.RoiGroupObject.RefMeasObject;
                    if(measobj == null)
                    {
                        return null;
                    }

                    referobj = measobj.RegionsGroupObject.RegionList[refindex];
                    refindex = referobj.Index;
                }

                WpAutoCoordinateRegion wpregion = referobj.RoiGroupObject._coord_absolute_tuple.CoordinateRegion;
                wpregion.ID = referobj.ItemType;
                wpregion.IsAbsolute = true;
                wpregion.SequenceIndex = measobj.Index;
                wpregion.CoordID = measobj.CoordinateGroupObject.CoordId;

                reference = referobj;

                return wpregion;
            }
            else
            {
                WpAutoCoordinateRegion wpregion = _coord_relative_tuple.CoordinateRegion;
                wpregion.ID = this.ParentRegionType;
                wpregion.IsAbsolute = false;
                wpregion.SequenceIndex = this.SpecifiedParent.AncestorMeasureObject.Index;
                wpregion.CoordID = this.SpecifiedParent.AncestorMeasureObject.CoordinateGroupObject.CoordId;

                reference = null;

                return wpregion;
            }
        }

        public WpAutoCoordinateRegion GetRelativeCoordinateRegion()
        {
            RegionGroup regiongroup = null;

            return GetRelativeCoordinateRegion(out regiongroup);

#if false
            if (RelationRegionItem.IsReference)
            {
                var measobj = _reference_tuple.RefMeasObject;
                if (measobj == null)
                {
                    return null;
                }
                int refindex = _reference_tuple.Value_RefRect;
                var referobj = measobj.RegionsGroupObject.RegionList[refindex];
                while (referobj.RelationsObject.Regions.RegionDict[referobj.ItemType].IsReference)
                {
                    measobj = referobj.RoiGroupObject.RefMeasObject;
                    if (measobj == null)
                    {
                        return null;
                    }

                    referobj = measobj.RegionsGroupObject.RegionList[refindex];
                    refindex = referobj.Index;
                }

                WpAutoCoordinateRegion wpregion = referobj.RoiGroupObject._coord_absolute_tuple.CoordinateRegion;
                wpregion.ID = referobj.ItemType;
                wpregion.IsAbsolute = true;
                wpregion.SequenceIndex = measobj.Index;

                return wpregion;
            }
            else
            {
                WpAutoCoordinateRegion wpregion = _coord_relative_tuple.CoordinateRegion;
                wpregion.ID = this.ParentRegionType;
                wpregion.IsAbsolute = false;
                wpregion.SequenceIndex = SpecifiedParent.AncestorMeasureObject.Index;

                return wpregion;
            }
#endif
        }



        /// <summary>
        /// 現在絶対座標系での出力になっているかを表します。
        /// </summary>
        public bool IsAbsolute
        {
            get
            {               
                return SpecifiedParent.SpecifiedParent.SpecifiedParent.SpecifiedParent.IsAbsoluteCoordinate;
            }
        }

        /// <summary>
        /// 座標系によるパラメータ一覧を表示または設定します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> CoordLT_RoiParamList
        {
            get
            {
                return GetCoordinateObject(false, IsAbsolute);
            }
            set
            {
                if(IsAbsolute)
                {
                    _coord_absolute_tuple.ParamList = value;
                    RefreshCoord(IsAbsolute);
                }
                else
                {
                    _coord_relative_tuple.ParamList = value;
                }
            }
        }
        
        /// <summary>
        /// 現在のオブジェクトのリストを取得します。
        /// </summary>
        /// <param name="isreference"></param>
        /// <param name="isabsolute"></param>
        /// <returns></returns>
        public List<UserTuple.NameValuePairTuple> GetCoordinateObject(bool isreference, bool isabsolute)
        {
            {
                if (isreference)
                {
                    return _reference_tuple.ParamList;
                }
                else
                {
                    if (isabsolute)
                    {
                        return _coord_absolute_tuple.ParamList;
                    }
                    else
                    {
                        return _coord_relative_tuple.ParamList;
                    }
                }
            }
        }

        public ECoordConversionErrorStatus AbsoluteObjectErrorStatus
        {
            get;
            protected set;
        }

        /// <summary>
        /// パラメータ一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> RoiParamList
        {
            get
            {
                if( RelationRegionItem != null)
                {
                    return GetCoordinateObject(RelationRegionItem.IsReference, IsAbsolute);
                }
                else
                {
                    return GetCoordinateObject(ParentRegionType == 0, IsAbsolute);
                }
            }
            set
            {
                if (RelationRegionItem != null)
                {
                    if (RelationRegionItem.IsReference)
                    {
                        Reference_RoiParamList = value;
                    }
                    else
                    {
                        CoordLT_RoiParamList = value;
                    }
                }
                else
                {
                    if (ParentRegionType == 0)
                    {
                        Reference_RoiParamList = value;
                    }
                    else
                    {
                        CoordLT_RoiParamList = value;
                    }
                }
            }
        }

        public Coordinate.TransformStatus SubOffset
        {
            get
            {
                if (RelationRegionItem.IsReference)
                {
                    return null;
                }
                else
                {
                    var coordid = SpecifiedParent.AncestorMeasureObject.CoordinateGroupObject.CoordId;
                    var suboffset = SpecifiedParent.AncestorMeasureObject.SpecifiedParent.UserTransformParameter[coordid];

                    return suboffset;
                }
            }
        }


        public UserTuple.CoordLTTuple AbsoluteCoordTuple
        {
            get
            {
                if( RelationRegionItem != null )
                {
                    if (RelationRegionItem.IsReference)
                    {
                        var item = GetRelativeCoordinateRegion();

                        if (item == null)
                        {
                            return null;
                        }

                        var coordid = item.CoordID;
                        var suboffset = SpecifiedParent.AncestorMeasureObject.SpecifiedParent.UserTransformParameter[coordid];

                        UserTuple.CoordLTTuple obj = new UserTuple.CoordLTTuple()
                        {
                            Value_Coord_LT_x = item.Coord_LT_X + suboffset.OffsetX,
                            Value_Coord_LT_y = item.Coord_LT_Y + suboffset.OffsetY,
                            Value_Coord_width = item.Coord_LT_Width,
                            Value_Coord_height = item.Coord_LT_Height,
                        };

                        return obj;
                    }
                    else
                    {
                        var coordid = SpecifiedParent.AncestorMeasureObject.CoordinateGroupObject.CoordId;
                        var suboffset = SpecifiedParent.AncestorMeasureObject.SpecifiedParent.UserTransformParameter[coordid];

                        UserTuple.CoordLTTuple obj = new UserTuple.CoordLTTuple()
                        {
                            Value_Coord_LT_x = _coord_absolute_tuple.Value_Coord_LT_x + suboffset.OffsetX,
                            Value_Coord_LT_y = _coord_absolute_tuple.Value_Coord_LT_y + suboffset.OffsetY,
                            Value_Coord_width = _coord_absolute_tuple.Value_Coord_width,
                            Value_Coord_height = _coord_absolute_tuple.Value_Coord_height,
                        };


                        return obj;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public UserTuple.CoordLTTuple CoordinateCoordTuple
        {
            get
            {
                if (RelationRegionItem != null)
                {
                    return _coord_relative_tuple;
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// このオブジェクトはCoord.LT.X～Coord.HeightおよびRef-meas, Ref-rectをRegionタグ直下にして読み取るので
        /// 読み取り時にタグ名のチェックをしません。
        /// </summary>
        protected override bool IsBoundElementName
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 親のRegion IDを表します。
        /// </summary>
        public int ParentRegionType
        {
            get
            {
                return SpecifiedParent.ItemType;
            }
        }


        public Relations.RegionItemSerialization RelationRegionItem
        {
            get
            {
                return SpecifiedParent.RelationsObject.Regions.RegionDict[SpecifiedParent.ItemType];
            }
        }

        protected bool IsIgnore = false;

        public virtual RegionGroup SpecifiedParent
        {
            get;
            protected set;
        }

        public override Base.BaseTreeGroup Parent
        {
            get
            {
                return this.SpecifiedParent;
            }
        }

        public ROIGroup(RegionGroup parent)
            : base()

        {
            SpecifiedParent = parent;

            _coord_absolute_tuple = new UserTuple.CoordLTTuple(this);
            _coord_relative_tuple = new UserTuple.CoordLTTuple(this);
            _reference_tuple = new UserTuple.ReferenceTuple(this, SpecifiedParent.AncestorMeasureObject);

            _iscurrentabsolute = IsAbsolute;

            AbsoluteObjectErrorStatus = ECoordConversionErrorStatus.NOTRUN;
        }

        public bool IsNullRefMeasObject
        {
            get
            {
                return _reference_tuple.RefMeasObject == null;
            }
        }

        public int RefMeas
        {
            get
            {
                return _reference_tuple.RefMeasObject.Index;
            }
        }

        public int RefRect
        {
            get
            {
                return _reference_tuple.Value_RefRect;
            }
        }


        protected override void ParseBeginElement(XmlReader reader, Stack<string> hierarchical)
        {
            // Sub階層に存在しないことを前提にしているので
            // Sub階層に存在するパラメータが含まれる場合は除外する

            IsIgnore = true;
        }

        protected override void ParseEndElement(XmlReader reader, Stack<string> hierarchical)
        {
            if (SpecifiedParent.IsReferenceRegion)
            {
                if (_reference_tuple.Value_RefMeas >= 0 && _reference_tuple.Value_RefMeas < SpecifiedParent.SpecifiedParent.SpecifiedParent.SpecifiedParent.RecipeItemGroup.Count)
                {
                    _reference_tuple.RefMeasObject = SpecifiedParent.SpecifiedParent.SpecifiedParent.SpecifiedParent.RecipeItemGroup[_reference_tuple.Value_RefMeas];
                }
            }


            IsIgnore = false;

            //RefreshCoord();
        }

        public void RefreshCoord()
        {
            RefreshCoord(IsAbsolute);
        }

        protected bool _iscurrentabsolute ;

        public void RefreshCoord(bool isabsolute)
        {
            var trvobj = ClipMeasure.AutoCoordinateCalculator.GetInstance();

            int coordid = SpecifiedParent.SpecifiedParent.SpecifiedParent.CoordinateGroupObject.CoordId;
            var trans_status = SpecifiedParent.AncestorMeasureObject.SpecifiedParent.UserTransformParameter[coordid];

            var coordinateobj = GetRelativeCoordinateRegion();
            var absolute = trvobj.GetAbsoluteCoordinate(coordinateobj, (uint)coordid, trans_status);
         
            //var absolute = trvobj.GetAbsoluteCoordinate(_coord_relative_tuple.CoordinateRegion, (uint)SpecifiedParent.SpecifiedParent.SpecifiedParent.CoordinateGroupObject.CoordId);
            if( absolute != null)
            { 
                _coord_absolute_tuple.Value_Coord_LT_x = absolute.Coord_LT_X;
                _coord_absolute_tuple.Value_Coord_LT_y = absolute.Coord_LT_Y;
                _coord_absolute_tuple.Value_Coord_width = absolute.Coord_LT_Width;
                _coord_absolute_tuple.Value_Coord_height = absolute.Coord_LT_Height;
                _coord_absolute_tuple.Value_DetColor = _coord_relative_tuple.Value_DetColor;

                AbsoluteObjectErrorStatus = absolute.ErrorStatus;
            }
         }


        protected override void ParseText(XmlReader reader, string elementname)
        {
            if (!IsIgnore)
            {
                Base.BaseTuple basetuple = null;
                if (SpecifiedParent.IsReferenceRegion)
                {
                    basetuple = _reference_tuple;
                }
                else
                {
                    basetuple = _coord_relative_tuple;
                }

                /// ----- ///
                /// 
                object keynew = null;
                foreach (var registered in RoiParamList)
                {
                    // RoiParamListに同名が存在する場合
                    if (registered.Value_Name.Equals(elementname))
                    {
                        keynew = basetuple.GetKey(elementname);

                    }
                }

                if (keynew != null)
                {
                    // 存在する場合は
                    // basetuple(ref or coord)に値を設定する
                    basetuple.SetParameterDynamically(keynew, reader.Value, basetuple.GetParameter(keynew).GetType());
                }
                else
                {
                    // 存在しない場合は、自由に設定できるパラメータに値を登録する
                    var item = new UserTuple.NameValuePairTuple(this);
                    item.SetParameter<string>(item.Key_Name, elementname);
                    item.SetParameter<string>(item.Key_Value, reader.Value);
                    RoiParamList.Add(item);
                }
            }
        }

        public override void MakeXmlNode(XmlDocument document, XmlElement parent)
        {
            var list = GetCoordinateObject(RelationRegionItem.IsReference, false);

            foreach (var obj in list)
            {
                {
                    obj.MakeXmlNode(document, parent);
                }
            }
        }

        public override void InsertTuple(Base.BaseTuple targetgen, bool isappendnext = false)
        {
            UserTuple.NameValuePairTuple target = (UserTuple.NameValuePairTuple)targetgen;

            UserTuple.NameValuePairTuple newitem = new UserTuple.NameValuePairTuple(this);
            newitem.SetParameter<string>(newitem.Key_Name, "Parameter");
            newitem.SetParameter<decimal>(newitem.Key_Value, 0);


            int index = RoiParamList.IndexOf(target);
            if (index == -1)
            {
                return;
            }

            if (isappendnext)
            {
                index++;
            }


            if (index == RoiParamList.Count)
            {
                RoiParamList.Add(newitem);
            }
            else
            {
                RoiParamList.Insert(index, newitem);
            }
        }

        public override void DeleteTuple(Base.BaseTuple targetgen)
        {
            RoiParamList.Remove((UserTuple.NameValuePairTuple)targetgen);
        }
    }
}

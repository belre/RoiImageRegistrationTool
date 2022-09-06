using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using ClipMeasure.Wrapper.Managed;


namespace ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple
{
    /// <summary>
    /// 座標を表す変数を表します。
    /// </summary>
    public class CoordLTTuple : Base.BaseTuple
    {

        protected DataType.GenericDataType _keys_lt_x = new DataType.GenericDataType("Coord.LT.x", "X", typeof(double));
        /// <summary>
        /// X座標を表します。
        /// </summary>
        public object Key_Coord_LT_X
        {
            get
            {
                return _keys_lt_x;
            }
        }

        protected DataType.GenericDataType _keys_lt_y = new DataType.GenericDataType("Coord.LT.y", "Y", typeof(double));
        /// <summary>
        /// Y座標を表します。
        /// </summary>
        public object Key_Coord_LT_Y
        {
            get
            {
                return _keys_lt_y;
            }
        }


        protected DataType.GenericDataType _keys_width = new DataType.GenericDataType("Coord.Width", "幅", typeof(double));
        /// <summary>
        /// 幅を表します。
        /// </summary>
        public object Key_Coord_Width
        {
            get
            {
                return _keys_width;
            }
        }

        protected DataType.GenericDataType _keys_height = new DataType.GenericDataType("Coord.Height", "高さ", typeof(double));
        /// <summary>
        /// 高さを表します。
        /// </summary>
        public object Key_Coord_Height
        {
            get
            {
                return _keys_height;
            }
        }

        protected DataType.GenericDataType _keys_detcolor = new DataType.GenericDataType("DetColor", "検出色", typeof(Group.ROIGroup.EDetectionColor));
        /// <summary>
        /// 検出色を表します。
        /// </summary>
        public object Key_DetColor
        {
            get
            {
                return _keys_detcolor;
            }
        }

        /// <summary>
        /// X座標の値を表します。
        /// </summary>
        public double Value_Coord_LT_x
        {
            get
            {
                return GetParameter<double>(Key_Coord_LT_X);
            }

            set
            {
                SetParameter<double>(Key_Coord_LT_X, value);
            }

        }

        /// <summary>
        /// Y座標の値を表します。
        /// </summary>
        public double Value_Coord_LT_y
        {
            get
            {
                return GetParameter<double>(Key_Coord_LT_Y);
            }

            set
            {
                SetParameter<double>(Key_Coord_LT_Y, value);
            }

        }

        /// <summary>
        /// 幅の値を表します。
        /// </summary>
        public double Value_Coord_width
        {
            get
            {
                return GetParameter<double>(Key_Coord_Width);
            }

            set
            {
                SetParameter<double>(Key_Coord_Width, value);
            }

        }

        /// <summary>
        /// 高さの値を表します。
        /// </summary>
        public double Value_Coord_height
        {
            get
            {
                return GetParameter<double>(Key_Coord_Height);
            }

            set
            {
                SetParameter<double>(Key_Coord_Height, value);
            }
        }

        /// <summary>
        /// 検出色の値を表します。
        /// </summary>
        public Group.ROIGroup.EDetectionColor Value_DetColor
        {
            get
            {
                return GetParameter<Group.ROIGroup.EDetectionColor>(Key_DetColor);
            }

            set
            {
                SetParameter<Group.ROIGroup.EDetectionColor>(Key_DetColor, value);
            }
        }

        /// <summary>
        /// 座標のラッパーを返します。
        /// </summary>
        public WpAutoCoordinateRegion CoordinateRegion
        {
            get
            {
                WpAutoCoordinateRegion wpregion = new WpAutoCoordinateRegion();
                wpregion.Coord_LT_X = GetParameter<double>(Key_Coord_LT_X);
                wpregion.Coord_LT_Y = GetParameter<double>(Key_Coord_LT_Y);
                wpregion.Coord_LT_Width = GetParameter<double>(Key_Coord_Width);
                wpregion.Coord_LT_Height = GetParameter<double>(Key_Coord_Height);
                wpregion.DetColor = (int)GetParameter<Group.ROIGroup.EDetectionColor>(Key_DetColor);

                return wpregion;
            }
        }

        /// <summary>
        /// 座標におけるパラメータ一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> ParamList
        {
            get
            {
                List<UserTuple.NameValuePairTuple> tuplelist = new List<NameValuePairTuple>();          

                foreach( var item in ownedElement)
                {
                    UserTuple.NameValuePairTuple vtuple = new NameValuePairTuple(Owner, true);          // 要変更 new しないように
                    vtuple.Value_Name = item.Contents.ToString();
                    vtuple.SetParameter(vtuple.Key_Value, _params[item].Contents);
                    vtuple.Value_DisplayedName = item.DisplayedContents;

                    tuplelist.Add(vtuple);
                }

                return tuplelist;
            }

            set
            {
                if(value == null)
                {
                    return;
                }

                foreach( var paramobj in value)
                {
                    foreach( var labelobj in ownedElement)
                    {
                        if( paramobj.Value_Name.Equals(labelobj.Contents.ToString()))
                        {
                            SetParameter(labelobj, paramobj.Value_Value);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// このオブジェクトが持つタグとして読み取れるキーを表します。
        /// </summary>
        protected override List<DataType.GenericDataType> ownedElement
        {
            get
            {
                return new List<DataType.GenericDataType> { _keys_lt_x, _keys_lt_y, _keys_width, _keys_height, _keys_detcolor };
            }
        }

        public CoordLTTuple()
            : this(null)
        {
        }

        public CoordLTTuple(Base.BaseTreeGroup owner)
            : base(owner)
        {
            _params[Key_Coord_LT_X] = new DataType.GenericDataType((double)0.0, "[mm]", typeof(double));
            _params[Key_Coord_LT_Y] = new DataType.GenericDataType((double)0.0, "[mm]", typeof(double));
            _params[Key_Coord_Width] = new DataType.GenericDataType((double)5.0, "[mm]", typeof(double));
            _params[Key_Coord_Height] = new DataType.GenericDataType((double)5.0, "[mm]", typeof(double));
            _params[Key_DetColor] = new DataType.GenericDataType(Group.ROIGroup.EDetectionColor.BLACK, typeof(Group.ROIGroup.EDetectionColor));


            Owner = owner;
        }


        /// <summary>
        /// XMLドキュメントを生成します。
        /// </summary>
        /// <param name="document"></param>
        /// <param name="parent"></param>
        public override void MakeXmlNode(XmlDocument document, XmlElement parent)
        {
            XmlElement root = document.CreateElement(ElementName);

            foreach (var element in ownedElement)
            {
                XmlElement item = document.CreateElement(element.ToString());

                if (element == Key_DetColor)
                {
                    item.InnerText = ((int)_params[element].Contents).ToString();
                }
                else
                {
                    item.InnerText = _params[element].ToString();
                }

                root.AppendChild(item);
            }

            parent.AppendChild(root);
        }


        public override void ParseXmlRecipe(XmlReader reader, Stack<string> hierarchical)
        {
            base.ParseXmlRecipe(reader, hierarchical);
        }

    }
}

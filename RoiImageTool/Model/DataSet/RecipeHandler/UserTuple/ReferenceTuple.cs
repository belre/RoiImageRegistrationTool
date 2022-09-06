using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple
{
    /// <summary>
    /// 参照を表す変数を表します。
    /// </summary>
    public class ReferenceTuple : Base.BaseTuple
    {
        protected DataType.GenericDataType _keys_ref_meas = new DataType.GenericDataType("Ref-meas", typeof(double));
        /// <summary>
        /// Ref-measのキーを表します。
        /// </summary>
        public object Key_RefMeas
        {
            get
            {
                return _keys_ref_meas;
            }
        }

        protected DataType.GenericDataType _keys_ref_rect = new DataType.GenericDataType("Ref-rect", typeof(double));
        /// <summary>
        /// Ref-rectのキーを表します。
        /// </summary>
        public object Key_RefRect
        {
            get
            {
                return _keys_ref_rect;
            }
        }

        /// <summary>
        /// Ref-measの値を表します。
        /// </summary>
        public int Value_RefMeas
        {
            get
            {
                return GetParameter<int>(Key_RefMeas);
            }

            set
            {
                SetParameter<int>(Key_RefMeas, value);
            }
        }

        /// <summary>
        /// Ref-measにおいて、参照オブジェクトの状態も含めた値を表します。
        /// ※データパースを除いてこの処理を使用して、Ref-measを読み取ります。
        /// </summary>
        public int Value_RefMeasRelated
        {
            get
            {
                if( RefMeasObject == null)
                {
                    return GetParameter<int>(Key_RefMeas);
                }

                // 更新が発生した場合、
                // SetParameterで値を変更する
                int refmeas_tmp = GetParameter<int>(Key_RefMeas);
                if( refmeas_tmp != RefMeasObject.Index)
                {
                    SetParameter<int>(Key_RefMeas, RefMeasObject.Index);
                }

                return GetParameter<int>(Key_RefMeas);
            }

            set
            {
                SetParameter<int>(Key_RefMeas, value);
            }
        }

        /// <summary>
        /// Ref-rectの値を取得します。
        /// </summary>
        public int Value_RefRect
        {
            get
            {

                return GetParameter<int>(Key_RefRect);
            }

            set
            {
                SetParameter<int>(Key_RefRect, value);
            }
        }

        /// <summary>
        /// Ref-meas, Ref-rectを表すキーの一覧を表します。
        /// </summary>
        protected override List<DataType.GenericDataType> ownedElement
        {
            get
            {
                return new List<DataType.GenericDataType> { _keys_ref_meas, _keys_ref_rect };
            }
        }

        /// <summary>
        /// パラメータの一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> ParamList
        {
            get
            {
                List<UserTuple.NameValuePairTuple> tuplelist = new List<NameValuePairTuple>();


                // Ref-Measのみ、参照を含めて渡すように処理記載
                {
                    UserTuple.NameValuePairTuple vtuple = new NameValuePairTuple(Owner, true);          // 要変更
                    vtuple.Value_Name = _keys_ref_meas.ToString();
                    vtuple.SetParameter(vtuple.Key_Value, Value_RefMeasRelated);
                    vtuple.Value_DisplayedName = "参照Index";
                    tuplelist.Add(vtuple);
                }

                {
                    UserTuple.NameValuePairTuple vtuple = new NameValuePairTuple(Owner, true);          // 要変更
                    vtuple.Value_Name = _keys_ref_rect.ToString();
                    vtuple.SetParameter(vtuple.Key_Value, _params[_keys_ref_rect].Contents);
                    vtuple.Value_DisplayedName = "参照領域No";
                    tuplelist.Add(vtuple);
                }

                return tuplelist;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                foreach (var paramobj in value)
                {
                    foreach (var labelobj in ownedElement)
                    {
                        if (paramobj.Value_Name.Equals(labelobj.Contents.ToString()))
                        {
                            SetParameter(labelobj, paramobj.Value_Value);
                            
                            // Ref-measの場合はオブジェクトを更新する
                            if( labelobj == Key_RefMeas)
                            {
                                int index = (int)paramobj.Value_Value;

                                if (index >= 0 && index <= _current_object.Index)
                                {
                                    _ref_meas_object = _current_object.SpecifiedParent.RecipeItemGroup[(int)paramobj.Value_Value];
                                }                             
                            }
                        }
                    }

                }
            }
        }


        protected Group.MeasureGroup _current_object;
        protected Group.MeasureGroup _ref_meas_object;
        
        /// <summary>
        /// 現在格納されている参照を表すオブジェクトを表します。
        /// </summary>
        public Group.MeasureGroup RefMeasObject
        {
            get
            {
                return _ref_meas_object;
            }
            set
            {
                _ref_meas_object = value;
            }
        }



        public ReferenceTuple()
            : this(null, null)
        {
        }

        public ReferenceTuple(Base.BaseTreeGroup owner, Group.MeasureGroup current_object)
            : base(owner)
        {
            _params[Key_RefMeas] = new DataType.GenericDataType((int)0.0, typeof(int));
            _params[Key_RefRect] = new DataType.GenericDataType((int)0.0, typeof(int));

            Owner = owner;
            _current_object = current_object;
        }

        public override void ParseXmlRecipe(XmlReader reader, Stack<string> hierarchical)
        {
            base.ParseXmlRecipe(reader, hierarchical);
        }

        /// <summary>
        /// XMLドキュメントを生成します。
        /// </summary>
        /// <param name="document"></param>
        /// <param name="parent"></param>
        public override void MakeXmlNode(XmlDocument document, XmlElement parent)
        {
            XmlElement root = document.CreateElement(ElementName);


            // Ref-Measのみ、参照を含めて渡すように処理記載
            {
                var element = _keys_ref_meas;
                XmlElement item = document.CreateElement(element.ToString());
                item.InnerText = Value_RefMeasRelated.ToString();  //_params[element].ToString();
                root.AppendChild(item);
            }

            {
                var element = _keys_ref_rect;
                XmlElement item = document.CreateElement(element.ToString());
                item.InnerText = _params[element].ToString();
                root.AppendChild(item);
            }


            parent.AppendChild(root);
        }

    }
}

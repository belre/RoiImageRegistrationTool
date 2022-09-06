using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Group
{
    public class CoordinateGroup : Base.BaseTreeGroup
    {
        public List<Relations.MeasureBaseParamsSerialization> MeasureBaseParamNameList
        {
            get
            {
                return RelationsObject.Measures.MeasureDict[SpecifiedParent.ItemType].MeasureBaseParamName;
            }
        }


        /// <summary>
        /// 制約がついているオブジェクトのキーを表します。
        /// </summary>
        protected object _keys_constraints_object = new object();
        public object Key_ConstraintsObject
        {
            get
            {
                return _keys_constraints_object;
            }
        }

        /// <summary>
        /// 制約がついていないオブジェクトのキーを表します。
        /// </summary>
        protected object _keys_object = new object();
        public object Key_Object
        {
            get
            {
                return _keys_object;
            }
        }

        /// <summary>
        /// 制約付きパラメータの一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> ConstraintsCoordinateList
        {
            get
            {
                return GetParameter<List<UserTuple.NameValuePairTuple>>(Key_ConstraintsObject);
            }
            set
            {
                SetParameter<List<UserTuple.NameValuePairTuple>>(Key_ConstraintsObject, value);
            }
        }

        /// <summary>
        /// 制約なしパラメータの一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> FreeCoordinateList
        {
            get
            {
                return GetParameter<List<UserTuple.NameValuePairTuple>>(Key_Object);
            }
            set
            {
                SetParameter<List<UserTuple.NameValuePairTuple>>(Key_Object, value);
            }
        }

        /// <summary>
        /// 座標系を表すパラメータを表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> CoordinateList
        {
            get
            {
                var params_free = GetParameter<List<UserTuple.NameValuePairTuple>>(Key_Object);
                var params_constraints = GetParameter<List<UserTuple.NameValuePairTuple>>(Key_ConstraintsObject);

                var params_all = new List<UserTuple.NameValuePairTuple>();
                foreach (var obj in params_constraints)
                {
                    params_all.Add(obj);
                }
                foreach (var obj in params_free)
                {
                    params_all.Add(obj);
                }

                return params_all;
            }
        }

        /// <summary>
        /// 参照座標IDを表します。
        /// </summary>
        public Int32 CoordId
        {
            get
            {
                foreach( var coorditem in ConstraintsCoordinateList)
                {
                    if( coorditem.Value_Name.ToString().Equals("Coord"))
                    {
                        int coord = -1;
                        if (int.TryParse(coorditem.Value_Value.ToString(), out coord ))
                        {
                            return coord;
                        }
                    }
                }

                return -1;
            }
        }

        public override Base.BaseTreeGroup Parent
        {
            get
            {
                return SpecifiedParent;
            }
        }


        /// <summary>
        /// 指定された親オブジェクトを表します。
        /// </summary>
        public virtual MeasureGroup SpecifiedParent
        {
            get;
            protected set;
        }

        public CoordinateGroup(Group.MeasureGroup parent)
            : base()
        {
            _element_name = "Coord";

            ConstraintsCoordinateList = new List<UserTuple.NameValuePairTuple>();
            ConstraintsCoordinateList.Add(new UserTuple.NameValuePairTuple(this) { Value_Name = "Coord", Value_Value = 1, Value_DisplayedName = "入力座標ID", Value_NameChangeProhibited = true });


            FreeCoordinateList = new List<UserTuple.NameValuePairTuple>();

            SpecifiedParent = parent;

            if (MeasureBaseParamNameList != null)
            {
                foreach (var item in MeasureBaseParamNameList)
                {
                    var obj = new UserTuple.NameValuePairTuple(this) { Value_Name = item.Label, Value_Value = item.Default, Value_NameChangeProhibited = true };
                    obj.Value_DisplayedName = item.DisplayedLabel;
                    FreeCoordinateList.Add(obj);
                }
            }
        }

        /// <summary>
        /// データ読み取り処理を表します。
        /// ※基本的にはCoordタグしかないので
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        public override void ParseXmlRecipe(XmlReader reader, Stack<string> hierarchical)
        {
            var item = new UserTuple.NameValuePairTuple(this, 0, true);
            item.ParseXmlRecipe(reader, hierarchical);
            
            // Coordタグの場合は制約付きに登録
            if( reader.Name.Equals(_element_name))
            {
                ConstraintsCoordinateList.Clear();

                item.Value_Name = _element_name;
                item.Value_DisplayedName = "入力座標ID";
                
                ConstraintsCoordinateList.Add(item);
            }
            // それ以外の場合はFreeに登録
            else
            {
                item.Value_NameChangeProhibited = false;
                foreach (var freeitem in FreeCoordinateList)
                {
                    if (freeitem.Value_Name.Equals(item.Value_Name))
                    {
                        freeitem.Value_Name = item.Value_Name;
                        freeitem.Value_Value = item.Value_Value;
                    }
                }

            }
        }

        public override void MakeXmlNode(XmlDocument document, XmlElement parent)
        {
            foreach (var obj in CoordinateList)
            {
                obj.MakeXmlNode(document, parent);
            }
        }

        public override void InsertTuple(Base.BaseTuple targetgen, bool isappendnext = false)
        {
            UserTuple.NameValuePairTuple newitem = new UserTuple.NameValuePairTuple(this);
            newitem.SetParameter<string>(newitem.Key_Name, "Parameter");
            newitem.SetParameter<decimal>(newitem.Key_Value, 0);

            if (FreeCoordinateList.Count == 0)
            {
                FreeCoordinateList.Add(newitem);
            }
            else
            {
                UserTuple.NameValuePairTuple target = (UserTuple.NameValuePairTuple)targetgen;

                int index = FreeCoordinateList.IndexOf(target);
                if (index == -1)
                {
                    return;
                }

                if (isappendnext)
                {
                    index++;
                }


                if (index == FreeCoordinateList.Count)
                {
                    FreeCoordinateList.Add(newitem);
                }
                else
                {
                    FreeCoordinateList.Insert(index, newitem);
                }
            }
        }

        public override void DeleteTuple(Base.BaseTuple targetgen)
        {
            FreeCoordinateList.Remove((UserTuple.NameValuePairTuple)targetgen);
        }

    }
}

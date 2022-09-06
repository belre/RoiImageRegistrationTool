using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Xml;
using ClipMeasure.Wrapper.Managed;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Group
{
    public class MeasureParamsGroup : Base.BaseTreeGroup
    {


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
        /// 制約がついているオブジェクトの値を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> ConstraintsMeasureParamsList
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
        /// 制約がついていないオブジェクトの値を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> FreeMeasureParamsList
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
        /// Measureパラメータのテンプレートオブジェクトを表します。
        /// </summary>
        public List<Relations.MeasureParamsSerialization> MeasureParamsRelation
        {
            get
            {
                return SpecifiedParent.RelationsObject.Measures.MeasureDict[SpecifiedParent.ItemType].MeasureParamName;
            }
        }

        /// <summary>
        /// 現在登録されているパラメータで、演算モジュールに与えるためのラッパーを返却します。
        /// </summary>
        public List<WpParameter> MeasureParameterArray
        {
            get
            {
                if( MeasureParamsRelation == null)
                {
                    return null;
                }

                List<WpParameter> list_obj = new List<WpParameter>();

                foreach (var relation in MeasureParamsRelation)
                {
                    foreach (var param in  MeasureParamsList)
                    {
                        if (param.Value_Name.Equals(relation.Label))
                        {
                            WpParameter paramvalue = new WpParameter()
                            {
                                Name = relation.Label, 
                                Value = param.Value_Value
                            };
                            list_obj.Add(paramvalue);
                        }
                    }
                }

                return list_obj;
            }
        }

        protected DataType.GenericDataType ConstraintsObject
        {
            get
            {
                return GetOriginalParameter(Key_ConstraintsObject);
            }

            set
            {
                SetOriginalParameter(Key_ConstraintsObject, value);
            }
        }



        /// <summary>
        /// 親から取得した計測項目のIDを表します。
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
            var _constraints_object = GetOriginalParameter(Key_ConstraintsObject);

            if (_constraints_object != null)
            {
                _constraints_object.InitializeContents();
            }
        }



        /// <summary>
        /// 計測項目の一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> MeasureParamsList
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



        public Group.MeasureGroup SpecifiedParent
        {
            get;
            protected set;
        }

        public override Base.BaseTreeGroup Parent
        {
            get
            {
                return SpecifiedParent;
            }
        }


        public MeasureParamsGroup(Group.MeasureGroup parent)
            : base()
        {
            _element_name = "Measure-params";
            SpecifiedParent = parent;
            //ParentMeasType = meastype;

            var constraintobject = new DataType.BaseConstraints<List<UserTuple.NameValuePairTuple>>(new object[] { });
            constraintobject.InitConstraints += GiveDefaultValue;
            constraintobject.InitializeContents();
            SetOriginalParameter(Key_ConstraintsObject, constraintobject);

            FreeMeasureParamsList = new List<UserTuple.NameValuePairTuple>();
        }

        /// <summary>
        /// Constraintsオブジェクトに相当するパラメータを生成します。
        /// </summary>
        /// <param name="param">入力値</param>
        /// <param name="extparam"></param>
        /// <returns></returns>
        protected List<UserTuple.NameValuePairTuple> GiveDefaultValue(object[] param, object[] extparam)
        {

            List<Relations.MeasureParamsSerialization> lists = new List<Relations.MeasureParamsSerialization>();
            
            // テンプレート読み取り
            if (MeasureParamsRelation != null)
            {
                lists = MeasureParamsRelation;
            }
            else
            {
                var obj = new Default.MeasureIDTable((Default.MeasureIDTable.EMeasBaseType)ParentMeasType);

                foreach( var str in obj.MeasureParamsList )
                {
                    Relations.MeasureParamsSerialization relation = new Relations.MeasureParamsSerialization();
                    relation.Label = str;
                    relation.Default = string.Empty;
                    lists.Add(relation);
                }
            }

            // テンプレートの値を使用して初期値を読み取り
            List<UserTuple.NameValuePairTuple> tuplelist = new List<UserTuple.NameValuePairTuple>();
            foreach (var relation in lists)
            {
                var item = new UserTuple.NameValuePairTuple(this, true);
                item.SetParameter<string>(item.Key_Name, relation.Label);
                item.SetParameter(item.Key_Value, relation.Default);
                SetDisplayedLabel(item, relation.Label);

                tuplelist.Add(item);
            }

            return tuplelist;
        }

        protected void SetDisplayedLabel(UserTuple.NameValuePairTuple tuple, string name)
        {
            foreach (var relation in MeasureParamsRelation)
            {
                if (relation.Label.Equals(name))
                {
                    if (relation.DisplayedLabel != null)
                    {
                        tuple.SetParameter(tuple.Key_DisplayedName, relation.DisplayedLabel);
                    }
                    else
                    {
                        tuple.SetParameter(tuple.Key_DisplayedName, relation.Label);
                    }


                    return;
                }
            }
        }


        /// <summary>
        /// オープンタグを検出したときの処理を表します。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected override void ParseBeginElement(XmlReader reader, Stack<string> hierarchical)
        {
            // 値を読み取り
            var item = new UserTuple.NameValuePairTuple(this);
            item.ParseXmlRecipe(reader, hierarchical);


            /// ----
            /// 制約付きパラメータに値が存在しないかどうかを確認する
            var contents = ConstraintsMeasureParamsList;
            foreach (var resultobj in contents)
            {
                var currentname = item.GetParameter<string>(item.Key_Name);

                // もし同名のオブジェクトが検出された場合は、
                // 制約付きパラメータに値を登録して終了
                if (resultobj.GetParameter<string>(resultobj.Key_Name).Equals(currentname))
                {
                    resultobj.SetParameter<string>(resultobj.Key_Name, item.GetParameter<string>(item.Key_Name));
                    resultobj.SetParameterDynamically(resultobj.Key_Value, item.GetParameter(item.Key_Value).ToString(), resultobj.GetParameter(resultobj.Key_Value).GetType());
                    resultobj.SetParameter<bool>(resultobj.Key_NameChangeProhibited, true);
                    SetDisplayedLabel(resultobj, item.Value_Name);
                    return;
                }
            }

            // もし同名のオブジェクトが存在しない場合は、
            // 制約なしパラメータに項目を追加して終了
            FreeMeasureParamsList.Add(item);
            /// ----
        }

        /// <summary>
        /// XML Document生成
        /// </summary>
        /// <param name="document"></param>
        /// <param name="current"></param>
        protected override void DelegateSubGroup(XmlDocument document, XmlElement current)
        {
            foreach (var itemgroup in MeasureParamsList)
            {
                // MeasureParamsListの各項目に処理を委譲
                itemgroup.MakeXmlNode(document, current);
            }
        }

        /// <summary>
        /// 項目を追加するような命令が与えられた時の処理を記述します。
        /// </summary>
        /// <param name="targetgen">対象のパラメータの前後に追加</param>
        /// <param name="isappendnext"></param>
        public override void InsertTuple(Base.BaseTuple targetgen, bool isappendnext = false)
        {
            // 空の場合
            if (FreeMeasureParamsList.Count == 0)
            {
                UserTuple.NameValuePairTuple newitem = new UserTuple.NameValuePairTuple(this);
                newitem.SetParameter<string>(newitem.Key_Name, "Parameter");
                newitem.SetParameter<string>(newitem.Key_Value, "");
                FreeMeasureParamsList.Add(newitem);
            }
            // 存在する場合
            else
            {
                UserTuple.NameValuePairTuple target = (UserTuple.NameValuePairTuple)targetgen;

                UserTuple.NameValuePairTuple newitem = new UserTuple.NameValuePairTuple(this);
                newitem.SetParameter<string>(newitem.Key_Name, target.GetParameter<string>(target.Key_Name));
                newitem.SetParameter(newitem.Key_Value, target.GetParameter(target.Key_Value));
                SetDisplayedLabel(newitem, target.Value_Name);


                int index = FreeMeasureParamsList.IndexOf(target);
                if (index == -1)
                {
                    return;
                }

                if (isappendnext)
                {
                    index++;
                }


                if (index == FreeMeasureParamsList.Count)
                {
                    FreeMeasureParamsList.Add(newitem);
                }
                else
                {
                    FreeMeasureParamsList.Insert(index, newitem);
                }
            }
        }
        
        /// <summary>
        /// 項目を削除するように命令された場合の処理を表します。
        /// </summary>
        /// <param name="targetgen">該当データを削除</param>
        public override void DeleteTuple(Base.BaseTuple targetgen)
        {
            FreeMeasureParamsList.Remove((UserTuple.NameValuePairTuple)targetgen);
        }
    }
}

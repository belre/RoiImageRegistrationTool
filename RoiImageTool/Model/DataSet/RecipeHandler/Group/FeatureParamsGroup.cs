using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using ClipMeasure.Wrapper.Managed;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Group
{
    public class FeatureParamsGroup : Base.BaseTreeGroup
    {
        protected object _keys_constraints_object = new object();
        /// <summary>
        /// 制約付きオブジェクトのキーを表します。
        /// </summary>
        public object Key_ConstraintsObject
        {
            get
            {
                return _keys_constraints_object;
            }
        }

        protected object _keys_object = new object();
        /// <summary>
        /// 制約なしオブジェクトのキーを表します。
        /// </summary>
        public object Key_Object
        {
            get
            {
                return _keys_object;
            }
        }

        /// <summary>
        /// 制約付きオブジェクトの一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> ConstraintsParamsList
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
        /// 制約なしオブジェクトの一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> FreeParamsList
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
        /// パラメータの一覧を表します。
        /// setterなし
        /// </summary>
        public List<UserTuple.NameValuePairTuple> ParamsList
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


        //protected DataType.BaseConstraints<List<UserTuple.NameValuePairTuple>> _constraints_object;         // `制約オブジェクト

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


        protected int _parentitemtype;

        /// <summary>
        /// 親で定義されるregion IDを表します。
        /// setterが実行されると、パラメータが初期化されます。
        /// </summary>
        public int ParentRegionType
        {
            get
            {
                return SpecifiedParent.ItemType;
            }
         }

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

        /// <summary>
        /// Feature-paramsのテンプレートのデフォルト値を表します。
        /// </summary>
        public List<Relations.RegionParamsSerialization> RelationFeatureParams
        {
            get
            {
                return SpecifiedParent.RelationsObject.Regions.RegionDict[SpecifiedParent.ItemType].FeatureParamName;
            }
        }

        public FeatureParamsGroup(Group.RegionGroup parent)
            : base()
        {
            _element_name = "Feature-params";
            SpecifiedParent = parent;

            FreeParamsList = new List<UserTuple.NameValuePairTuple>();

            var _constraints_object = new DataType.BaseConstraints<List<UserTuple.NameValuePairTuple>>(new object[] { });
            _constraints_object.InitConstraints += GiveDefaultValue;
            _constraints_object.InitializeContents();

            SetOriginalParameter(Key_ConstraintsObject, _constraints_object);
        }

        public override void UpdateFromRegionRelation()
        {

            var _constraints_object = new DataType.BaseConstraints<List<UserTuple.NameValuePairTuple>>(new object[] { });
            _constraints_object.InitConstraints += GiveDefaultValue;
            _constraints_object.InitializeContents();
            SetOriginalParameter(Key_ConstraintsObject, _constraints_object);
        }


        /// <summary>
        /// Feature-paramsのラッパーオブジェクトを表します。
        /// </summary>
        public List<WpParameter> FeatureParameterArray
        {
            get
            {
                if( RelationFeatureParams == null)
                {
                    return null;
                }

                List<WpParameter> list_obj = new List<WpParameter>();

                foreach( var relation in RelationFeatureParams)
                {
                    foreach ( var param in ParamsList)
                    {
                        if(param.Value_Name.Equals(relation.Label))
                        {
                            WpParameter parametervalue = new WpParameter()
                            {
                                Name = relation.Label,
                                Value = param.Value_Value
                            };
                            list_obj.Add(parametervalue);
                        }
                    }
                }

                return list_obj;
            }
        }


        protected void SetDisplayedLabel(UserTuple.NameValuePairTuple tuple, string name)
        {
            foreach (var relation in RelationFeatureParams)
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
        /// 制約付きオブジェクトの内容を出力します。
        /// </summary>
        /// <param name="param"></param>
        /// <param name="extparam"></param>
        /// <returns></returns>
        protected List<UserTuple.NameValuePairTuple> GiveDefaultValue(object[] param, object[] extparam)
        {
            List<Relations.RegionParamsSerialization> lists = null;
            if( RelationFeatureParams != null)
            {
                lists = RelationFeatureParams;
            }
            else
            {
                var obj = new Default.RegionIDTable((Default.RegionIDTable.EROIType)ParentRegionType);
                lists = new List<Relations.RegionParamsSerialization>();
                foreach( var item in obj.FeatureParamsList)
                {
                    var obj2 = new Relations.RegionParamsSerialization();
                    obj2.Label = item;
                    obj2.Default = string.Empty;
                    lists.Add(obj2);
                }
            }

            List<UserTuple.NameValuePairTuple> tuplelist = new List<UserTuple.NameValuePairTuple>();
            foreach (var str in lists)
            {
                var item = new UserTuple.NameValuePairTuple(this, true);
                item.SetParameter<string>(item.Key_Name, str.Label);
                item.SetParameter(item.Key_Value, str.Default);
                SetDisplayedLabel(item, str.Label);

                tuplelist.Add(item);
            }

            return tuplelist;
        }

        /// <summary>
        /// オープンタグを検出したときの処理を表します。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected override void ParseBeginElement(XmlReader reader, Stack<string> hierarchical)
        {
            var item = new UserTuple.NameValuePairTuple(this);
            item.ParseXmlRecipe(reader, hierarchical);

            /// ----
            var contents = ConstraintsParamsList;
            foreach (var resultobj in contents)
            {
                var currentname = item.GetParameter<string>(item.Key_Name);

                // 既存項目に含まれる場合は
                // 既存項目の変数の内容を変更する
                if (resultobj.GetParameter<string>(resultobj.Key_Name).Equals(currentname))
                {
                    resultobj.Value_Name = item.Value_Name;
                    resultobj.Value_Value = item.Value_Value;
                    SetDisplayedLabel(resultobj, item.Value_Name);
                    resultobj.Value_NameChangeProhibited = true;
                    return;
                }
            }

            // 既存項目に含まれない場合は、
            // 制約なしオブジェクトに追加する
            SetDisplayedLabel(item, item.Value_Name);
            FreeParamsList.Add(item);
            /// ----
        }


        /// <summary>
        /// XMLドキュメントを生成します。
        /// </summary>
        /// <param name="document"></param>
        /// <param name="current"></param>
        protected override void DelegateSubGroup(XmlDocument document, XmlElement current)
        {
            if (ParamsList.Count == 0)
            {
                return;
            }


            foreach (var itemgroup in ParamsList)
            {
                itemgroup.MakeXmlNode(document, current);
            }
        }

        /// <summary>
        /// パラメータを追加する命令が行なわれた時の処理を表します。
        /// </summary>
        /// <param name="targetgen"></param>
        /// <param name="isappendnext"></param>
        public override void InsertTuple(Base.BaseTuple targetgen, bool isappendnext = false)
        {
            UserTuple.NameValuePairTuple newitem = new UserTuple.NameValuePairTuple(this);
            newitem.SetParameter<string>(newitem.Key_Name, "Parameter");
            newitem.SetParameter<string>(newitem.Key_Value, "0");
            if (FreeParamsList.Count == 0)
            {
                FreeParamsList.Add(newitem);
            }
            else
            {
                UserTuple.NameValuePairTuple target = (UserTuple.NameValuePairTuple)targetgen;

                int index = FreeParamsList.IndexOf(target);
                if (index == -1)
                {
                    return;
                }

                if (isappendnext)
                {
                    index++;
                }

                if (index == ParamsList.Count)
                {
                    FreeParamsList.Add(newitem);
                }
                else
                {
                    FreeParamsList.Insert(index, newitem);
                }
            }
        }

        /// <summary>
        /// パラメータが削除される命令が行なわれた時の処理を表します。
        /// </summary>
        /// <param name="targetgen"></param>
        public override void DeleteTuple(Base.BaseTuple targetgen)
        {
            FreeParamsList.Remove((UserTuple.NameValuePairTuple)targetgen);
        }

#if false
        public override void UpdateFromRegionRelation()
        {
            ConstraintsObject.InitializeContents();
        }
#endif
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Group
{
    public class FilterParamsGroup : Base.BaseTreeGroup
    {


        #region [Parameter]

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
        public object Key_FreeObject
        {
            get
            {
                return _keys_object;
            }
        }

        #endregion

        /// <summary>
        /// 制約付きフィルタパラメータの一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> ConstraintsFilterParamsList
        {
            get
            {
                return GetParameter<List<UserTuple.NameValuePairTuple>>(Key_ConstraintsObject);
            }
            protected set
            {
                SetParameter<List<UserTuple.NameValuePairTuple>>(Key_ConstraintsObject, value);
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
        /// 制約なしフィルタパラメータの一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> FreeFilterParamsList
        {
            get
            {
                return GetParameter<List<UserTuple.NameValuePairTuple>>(Key_FreeObject);
            }
            protected set
            {
                SetParameter<List<UserTuple.NameValuePairTuple>>(Key_FreeObject, value);
            }
        }


        /// <summary>
        /// フィルタのテンプレート情報を表します。
        /// </summary>
        public List<Relations.FilterParamsSerialization> FilterParamsRelation
        {
            get
            {
                return SpecifiedParent.RelationsObject.Filters.FilterDict[SpecifiedParent.FilterType].FilterParamName;
            }
        }

        /// <summary>
        /// フィルタパラメータの一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> FilterParamsList
        {
            get
            {
                var params_free = GetParameter<List<UserTuple.NameValuePairTuple>>(Key_FreeObject);
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



        protected int _parent_filter_type = 0;
        /// <summary>
        /// 親のフィルタIDを表します。
        /// </summary>
        public int ParentFilterType
        {
            get
            {
                return SpecifiedParent.FilterType;
                //return _parent_filter_type;
            }
        }



        public Group.FilterGroup SpecifiedParent
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

        public FilterParamsGroup(Group.FilterGroup parent)
            : base()
        {
            SpecifiedParent = parent;

            _element_name = "Filter-params";

            FreeFilterParamsList = new List<UserTuple.NameValuePairTuple>();

            var _constraints_object = new DataType.BaseConstraints<List<UserTuple.NameValuePairTuple>>(new object[] {});
            _constraints_object.InitConstraints += GiveDefaultValue;
            _constraints_object.InitializeContents();
            SetOriginalParameter(Key_ConstraintsObject, _constraints_object);
        }

        /// <summary>
        /// 制約付きオブジェクトに値を設定します。
        /// </summary>
        /// <param name="param"></param>
        /// <param name="extparam"></param>
        /// <returns></returns>
        protected List<UserTuple.NameValuePairTuple> GiveDefaultValue(object[] param, object[] extparam)
        {

            List<Relations.FilterParamsSerialization> lists = null;
            if (FilterParamsRelation != null)
            {
                lists = FilterParamsRelation;
            }
            else
            {
                var obj = new Default.FilterIDTable((Default.FilterIDTable.EFilterType)ParentFilterType);

                lists = new List<Relations.FilterParamsSerialization>();
                foreach( var str in obj.FilterParamsList)
                {
                    Relations.FilterParamsSerialization item2 = new Relations.FilterParamsSerialization();
                    item2.Label = str;
                    item2.Default = string.Empty;
                }
            }


            List<UserTuple.NameValuePairTuple> tuplelist = new List<UserTuple.NameValuePairTuple>();
            foreach (var str in lists)
            {
                var item = new UserTuple.NameValuePairTuple(this, true);
                item.Value_Name = str.Label;
                item.Value_Value = str.Default;
                item.Value_NameChangeProhibited = true;

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
            var contents = ConstraintsFilterParamsList;
            foreach (var resultobj in contents)
            {
                var currentname = item.GetParameter<string>(item.Key_Name);

                if (resultobj.GetParameter<string>(resultobj.Key_Name).Equals(currentname))
                {
                    resultobj.SetParameter<string>(resultobj.Key_Name, item.GetParameter<string>(item.Key_Name));
                    resultobj.SetParameter(resultobj.Key_Value, item.GetParameter(item.Key_Value));
                    resultobj.SetParameter<bool>(resultobj.Key_NameChangeProhibited, true);
                    return;
                }
            }

            ConstraintsFilterParamsList.Add(item);
            /// ----


        }

        /// <summary>
        /// XMLドキュメントを生成します。
        /// </summary>
        /// <param name="document"></param>
        /// <param name="current"></param>
        protected override void DelegateSubGroup(XmlDocument document, XmlElement current)
        {

            foreach (var itemgroup in FilterParamsList)
            {
                itemgroup.MakeXmlNode(document, current);
            }
        }

        /// <summary>
        /// 新規パラメータが追加された時の処理を表します。
        /// </summary>
        /// <param name="targetgen"></param>
        /// <param name="isappendnext"></param>
        public override void InsertTuple(Base.BaseTuple targetgen, bool isappendnext = false)
        {
            UserTuple.NameValuePairTuple newitem = new UserTuple.NameValuePairTuple(this);
            newitem.SetParameter<string>(newitem.Key_Name, "Parameter");
            newitem.SetParameter<string>(newitem.Key_Value, "");
            if (FreeFilterParamsList.Count == 0)
            {
   
                FreeFilterParamsList.Add(newitem);
            }
            else
            {
                UserTuple.NameValuePairTuple target = (UserTuple.NameValuePairTuple)targetgen;

                int index = FreeFilterParamsList.IndexOf(target);
                if (index == -1)
                {
                    return;
                }

                if (isappendnext)
                {
                    index++;
                }

                if (index == FreeFilterParamsList.Count)
                {
                    FreeFilterParamsList.Add(newitem);
                }
                else
                {
                    FreeFilterParamsList.Insert(index, newitem);
                }
            }
        }

        /// <summary>
        /// パラメータ削除の処理を表します。
        /// </summary>
        /// <param name="targetgen"></param>
        public override void DeleteTuple(Base.BaseTuple targetgen)
        {
            FreeFilterParamsList.Remove((UserTuple.NameValuePairTuple)targetgen);
        }

        public override void UpdateFromFilterRelation()
        {
            ConstraintsObject.InitializeContents();
        }
    }
}

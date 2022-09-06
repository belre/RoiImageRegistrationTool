using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;


namespace ClipXmlReader.Model.DataSet.RecipeHandler.Group
{
    public class ResultsGroup : Base.BaseTreeGroup
    {

        protected object _keys_constraints_object = new object();
        /// <summary>
        /// 制約がついているオブジェクトのキーを表します。
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
        /// 制約がついていないオブジェクトのキーを表します。
        /// </summary>
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
        public List<UserTuple.ResultTuple> ConstraintsResultObject
        {
            get
            {
                return GetParameter<List<UserTuple.ResultTuple>>(Key_ConstraintsObject);
            }
            set
            {
                SetParameter<List<UserTuple.ResultTuple>>(Key_ConstraintsObject, value);
            }
        }

        /// <summary>
        /// 制約がついていないオブジェクトの値を表します。
        /// </summary>
        public List<UserTuple.ResultTuple> FreeResultObject
        {
            get
            {
                return GetParameter<List<UserTuple.ResultTuple>>(Key_Object);
            }
            set
            {
                SetParameter<List<UserTuple.ResultTuple>>(Key_Object, value);
            }
        }

        protected int _parent_meas_type;

        /// <summary>
        /// MeasureObjectから与えられる計測IDです。
        /// ※循環参照を防ぐため外部設定が必要
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
            var _constraints_object = (DataType.BaseConstraints<List<UserTuple.ResultTuple>>)GetOriginalParameter(Key_ConstraintsObject);

            if (_constraints_object != null)
            {
                _constraints_object.InitializeContents();
            }
        }

        /// <summary>
        /// 結果を表すオブジェクトの一覧です。
        /// </summary>
        public List<UserTuple.ResultTuple> ResultsObject
        {
            get
            {
                var params_free = GetParameter<List<UserTuple.ResultTuple>>(Key_Object);
                var params_constraints = GetParameter<List<UserTuple.ResultTuple>>(Key_ConstraintsObject);

                var params_all = new List<UserTuple.ResultTuple>();
                foreach( var obj in params_constraints)
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
        /// 結果として最低限与えられるパラメータの一覧です。
        /// </summary>
        //public List<string> ResultCandidates
        public List<Relations.MeasureResultSerialization> ResultCandidates
        {
            get
            {
                return SpecifiedParent.RelationsObject.Measures.MeasureDict[SpecifiedParent.ItemType].ResultDefaultsAll;
                //return SpecifiedParent.RelationsObject.Measures.MeasureDict[SpecifiedParent.ItemType].ResultName;
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

        public ResultsGroup(MeasureGroup parent)
            : base()
        {
            _element_name = "Results";
            SpecifiedParent = parent;

            var constraintobject = new DataType.BaseConstraints<List<UserTuple.ResultTuple>>(new object[] { });
            constraintobject.InitConstraints += GiveDefaultValue;
            constraintobject.InitializeContents();
            SetOriginalParameter(Key_ConstraintsObject, constraintobject);

            List<UserTuple.ResultTuple> freelist = new List<UserTuple.ResultTuple>();
            SetParameter<List<UserTuple.ResultTuple>>(Key_Object, freelist);
        }

        /// <summary>
        /// 制約付きパラメータに初期値を与えます。
        /// </summary>
        /// <param name="param"></param>
        /// <param name="extparam"></param>
        /// <returns></returns>
        protected List<UserTuple.ResultTuple> GiveDefaultValue(object[] param, object[] extparam)
        {

            List<Relations.MeasureResultSerialization> lists = null;
            if( ResultCandidates != null)
            {
                lists = ResultCandidates;
            }
            else
            {
                var obj = new Default.MeasureIDTable((Default.MeasureIDTable.EMeasBaseType)ParentMeasType);
                lists = new List<Relations.MeasureResultSerialization>();
                foreach (var name in obj.ResultsNameList)
                {
                    lists.Add(new Relations.MeasureResultSerialization(name));
                }
            }

            List<UserTuple.ResultTuple> tuplelist = new List<UserTuple.ResultTuple>();
            foreach ( var relation in lists)
            {
                var item = new UserTuple.ResultTuple(this, true);
                item.SetParameter<string>(item.Key_Name, relation.ResultName);
                item.SetParameter<int>(item.Key_ValidFig, relation.DefaultValidFig);
                item.SetParameter<string>(item.Key_Unit, relation.DefaultUnit);
                item.SetParameter<decimal>(item.Key_Lower, relation.DefaultLower);
                item.SetParameter<decimal>(item.Key_Upper, relation.DefaultUpper);

                tuplelist.Add(item);
            }

            return tuplelist;
        }

        /// <summary>
        /// オープンタグが検出されたときに実行する処理を表します。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected override void ParseBeginElement(XmlReader reader, Stack<string> hierarchical)
        {
            var item = new UserTuple.ResultTuple(this, false);

            if (reader.Name.Equals(item.ElementName))
            {
                // Attributeの取得と設定
                Dictionary<object, string> attr = new Dictionary<object, string>();
                foreach (var source in item.AttributeName)
                {
                    if (!reader.GetAttribute(source.Contents.ToString()).Equals(""))
                    {
                        attr[source] = reader.GetAttribute(source.Contents.ToString());
                    }
                }
                item.SetAttribute(attr);
                
                // 項目の読み取り
                item.ParseXmlRecipe(reader, hierarchical);

                /// ----
                var contents = ConstraintsResultObject;
                foreach (var resultobj in contents )
                {
                    var currentname = item.GetParameter<string>(item.Key_Name);

                    // 初期値と対応する名称が存在する場合
                    if( resultobj.GetParameter<string>(resultobj.Key_Name).Equals(currentname))
                    {
                        resultobj.SetParameter<string>(resultobj.Key_Unit, item.GetParameter<string>(item.Key_Unit));
                        resultobj.SetParameter<int>(resultobj.Key_ValidFig, item.GetParameter<int>(item.Key_ValidFig));
                        resultobj.SetParameter<int>(resultobj.Key_UseFlag, item.GetParameter<int>(item.Key_UseFlag));
                        resultobj.SetParameter<decimal>(resultobj.Key_Lower, item.GetParameter<decimal>(item.Key_Lower));
                        resultobj.SetParameter<decimal>(resultobj.Key_Upper, item.GetParameter<decimal>(item.Key_Upper));
                        resultobj.SetParameter<bool>(resultobj.Key_NameChangeProhibited, true);
                        return;
                    }
                }

                // 存在しない場合は制約なし条件に登録
                 FreeResultObject.Add(item);
                /// ----
            }
        }

        /// <summary>
        /// サブクラスに移譲してドキュメントを作成します。
        /// </summary>
        /// <param name="document"></param>
        /// <param name="current"></param>
        protected override void DelegateSubGroup(XmlDocument document, XmlElement current)
        {
            foreach (var itemgroup in ResultsObject)
            {
                itemgroup.MakeXmlNode(document, current);
            }
        }

        /// <summary>
        /// Resultsに項目が追加された時の処理を表します。
        /// </summary>
        /// <param name="targetgen"></param>
        /// <param name="isappendnext"></param>
        public override void InsertTuple(Base.BaseTuple targetgen, bool isappendnext = false)
        {
            UserTuple.ResultTuple newitem = new UserTuple.ResultTuple(this, false);
            newitem.SetParameter<string>(newitem.Key_Name, "Parameter");
            newitem.SetParameter<decimal>(newitem.Key_Lower, (decimal)0);
            newitem.SetParameter<decimal>(newitem.Key_Upper, (decimal)0);
            newitem.SetParameter<int>(newitem.Key_ValidFig, (int)0);
            newitem.SetParameter<int>(newitem.Key_UseFlag, 0);
            newitem.SetParameter<string>(newitem.Key_Unit, "");

            if (FreeResultObject.Count == 0)
            {

                FreeResultObject.Add(newitem);
            }
            else
            {
                UserTuple.ResultTuple target = (UserTuple.ResultTuple)targetgen;
                int index = FreeResultObject.IndexOf(target);

                if (index == -1)
                {
                    return;
                }

                if (isappendnext)
                {
                    index++;
                }

                if (index == FreeResultObject.Count)
                {
                    FreeResultObject.Add(newitem);
                }
                else
                {
                    FreeResultObject.Insert(index, newitem);
                }
            }
        }

        
        /// <summary>
        /// targetgenを項目から削除します。
        /// </summary>
        /// <param name="targetgen"></param>
        public override void DeleteTuple(Base.BaseTuple targetgen)
        {
            var target = (UserTuple.ResultTuple)targetgen;

            if( FreeResultObject.Contains(target))
            {
                FreeResultObject.Remove(target);
            } 
        }


    }
}

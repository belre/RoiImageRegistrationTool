using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using ClipMeasure.Wrapper.Managed;



namespace ClipXmlReader.Model.DataSet.RecipeHandler.Group
{
    public class MeasureGroup : Base.BaseTreeGroup
    {

        /// <summary>
        /// 現在のオブジェクトの添え字を表します。
        /// </summary>
        public int Index
        {
            get
            {
                if (SpecifiedParent == null)
                {
                    return -2;
                }

                return SpecifiedParent.RecipeItemGroup.IndexOf(this);            
            }
        }


        /// <summary>
        /// 現在のオブジェクトが、親クラスの一部であるかどうかを表します。
        /// </summary>
        public bool IsIncludesParent
        {
            get
            {
                if( SpecifiedParent == null)
                {
                    return false;
                }

                if (SpecifiedParent.RecipeItemGroup.Exists(
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


        public Error.RecipeErrorHandle.ERecipeError ErrorStatus
        {
            get
            {
                var flag = Error.RecipeErrorHandle.ERecipeError.OK;

                if( !RegionsGroupObject.IsRefMeasValid)
                {
                    flag = flag | Error.RecipeErrorHandle.ERecipeError.REFMEAS_REMOVE;
                }
                else if (! RegionsGroupObject.IsRefRectValid)
                {
                    flag = flag | Error.RecipeErrorHandle.ERecipeError.REFRECT_REMOVE;
                }


                if ( CoordinateErrorStatus == ECoordConversionErrorStatus.OK)
                {
                    return flag | Error.RecipeErrorHandle.ERecipeError.OK;
                }
                else if( CoordinateErrorStatus == ECoordConversionErrorStatus.COORDID_REF_SYNTAXERROR )
                {
                    return flag | Error.RecipeErrorHandle.ERecipeError.COORDID_REF_SYNTAXERROR;
                }
                else if( CoordinateErrorStatus == ECoordConversionErrorStatus.COORDID_REF_OUTOFRANGE)
                {
                    return flag | Error.RecipeErrorHandle.ERecipeError.COORDID_REF_OUTOFRANGE;
                }
                else if( CoordinateErrorStatus == ECoordConversionErrorStatus.NOTRUN)
                {
                    return flag | Error.RecipeErrorHandle.ERecipeError.NOTRUN;
                }


                return flag | Error.RecipeErrorHandle.ERecipeError.SYSTEM_ERROR;
            }
        }

        public ECoordConversionErrorStatus CoordinateErrorStatus
        {
            get
            {
                return RegionsGroupObject.RoiErrorStatus;
            }
        }

        /// <summary>
        /// Calculatorオブジェクトを使用して、
        /// 自動座標演算を行います
        /// </summary>
        /// <param name="calculator"></param>
        public void SetCoordinate()
        {
            if( !SpecifiedParent.UserTransformParameter.Keys.Contains(this.CoordinateGroupObject.CoordId) )
            {
                SpecifiedParent.UserTransformParameter[this.CoordinateGroupObject.CoordId] = new Coordinate.TransformStatus();
            }

            // シングルトンオブジェクトを取得
            var calculator = ClipMeasure.AutoCoordinateCalculator.GetInstance();

            // 絶対座標系取得
            var regionlist = this.RegionsGroupObject.RelativeCoordinateRegionList;

            // 座標変換実行
            calculator.SetCoordinate(this.ItemType, this.Index, RelationsObject, regionlist, this.MeasureParamsGroupObject.MeasureParameterArray, (uint)this.CoordinateGroupObject.CoordId);
        
            // 座標系更新
            foreach ( var region in RegionsGroupObject.RegionList)
            {
                region.RoiGroupObject.RefreshCoord();
            }
        }

        public void SetTransformParameterX(double x)
        {
            if (!SpecifiedParent.UserTransformParameter.Keys.Contains(this.CoordinateGroupObject.CoordId))
            {
                SpecifiedParent.UserTransformParameter[this.CoordinateGroupObject.CoordId] = new Coordinate.TransformStatus();
            }

            SpecifiedParent.UserTransformParameter[this.CoordinateGroupObject.CoordId].OffsetX = x;
        }

        public void SetTransformParameterY(double y)
        {
            if (!SpecifiedParent.UserTransformParameter.Keys.Contains(this.CoordinateGroupObject.CoordId))
            {
                SpecifiedParent.UserTransformParameter[this.CoordinateGroupObject.CoordId] = new Coordinate.TransformStatus();
            }

            SpecifiedParent.UserTransformParameter[this.CoordinateGroupObject.CoordId].OffsetY = y;
        }

        protected DataType.GenericDataType _keys_name = new DataType.GenericDataType("Name", typeof(string));
        /// <summary>
        /// 計測名称を表すキーです。
        /// </summary>
        public object Key_Name
        {
            get
            {
                return _keys_name;
            }
        }

        protected DataType.GenericDataType _keys_itemtype = new DataType.GenericDataType("ID", typeof(int));
        
        /// <summary>
        /// 計測項目を表すキーです。
        /// </summary>
        public object Key_ItemType
        {
            get
            {
                return _keys_itemtype;
            }
        }

        /// <summary>
        /// 現在の計測の種類を表します。
        /// setterを使用すると、子オブジェクトの状態もすべて変更されます。
        /// </summary>
        public int ItemType
        {
            get
            {
                return GetParameter<int>(Key_ItemType);
            }
            set
            {
                // 値が登録されていない場合は
                // 値を登録して終了
                if( !IsIncludeParameter(Key_ItemType))
                {
                    SetParameter<int>(Key_ItemType, value);
                    return;
                }


                // それ以外の場合は
                // 値を確認し、変更があれば更新
                var current = GetParameter<int>(Key_ItemType);

                if (current != value)
                {
                    SetParameter<int>(Key_ItemType, value);
                    UpdateFromRelation();                   
                }
            }
        }

        public override void UpdateFromMeasureRelation()
        {
            // それ以外の場合は
            // 値を確認し、変更があれば更新
            var current = GetParameter<int>(Key_ItemType);

            {
                if (ResultsGroupObject != null)
                {
                    ResultsGroupObject.UpdateFromRelation();
                }

                if (MeasureParamsGroupObject != null)
                {
                    MeasureParamsGroupObject.UpdateFromRelation();
                }


                if (RegionsGroupObject != null)
                {
                    RegionsGroupObject.UpdateFromRelation();
                }

                CoordinateGroupObject = new CoordinateGroup(this);
            }
        }


        /// <summary>
        /// Attributeに該当するキーです。
        /// </summary>
        public override DataType.GenericDataType[] AttributeName
        {
            get
            {
                return new DataType.GenericDataType[] { _keys_itemtype };
            }
        }

        /// <summary>
        /// 自分で保持しているオブジェクトのキーです。
        /// </summary>
        protected override List<DataType.GenericDataType> ownedElement
        {
            get
            {
                return new List<DataType.GenericDataType> { _keys_name };
            }
        }


        /// <summary>
        /// Cooordinateオブジェクトを表します。
        /// </summary>
        public CoordinateGroup CoordinateGroupObject
        {
            get;
            set;
        }

        /// <summary>
        /// MeasureParamsオブジェクトを表します。
        /// </summary>
        public MeasureParamsGroup MeasureParamsGroupObject
        {
            get;
            set;
        }

        /// <summary>
        /// Resultsオブジェクトを表します。
        /// </summary>
        public ResultsGroup ResultsGroupObject
        {
            get;
            set;
        }


        /// <summary>
        /// Regionsオブジェクトを表します。
        /// </summary>
        public RegionsGroup RegionsGroupObject
        {
            get;
            set;
        }

        /// <summary>
        /// Coordinateオブジェクトの変数一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> CoordinateList
        {
            get
            {
                return CoordinateGroupObject.CoordinateList;
            }
        }

        /// <summary>
        /// MeasureParamsオブジェクトの変数一覧を表します。
        /// </summary>
        public List<UserTuple.NameValuePairTuple> MeasureParamsList
        {
            get
            {
                return MeasureParamsGroupObject.MeasureParamsList;
            }
        }

        /// <summary>
        /// Resultsオブジェクトの変数一覧を表します。
        /// </summary>
        public List<UserTuple.ResultTuple> ResultsList
        {
            get
            {
                return ResultsGroupObject.ResultsObject;
            }
        }


        /// <summary>
        /// 実体化された親クラスを表します。
        /// </summary>
        public virtual MeasuresGroup SpecifiedParent
        {
            get;
            protected set;
        }

        /// <summary>
        /// 汎用的な親クラスを表します。
        /// </summary>
        public override Base.BaseTreeGroup Parent
        {
            get
            {
                return this.SpecifiedParent;
            }
        }

        public MeasureGroup(MeasuresGroup parent)
            : base()
        {
            _element_name = "Measure";

            ItemType = 0;

            SpecifiedParent = parent;
            CoordinateGroupObject = new CoordinateGroup(this);

            RegionsGroupObject = new RegionsGroup(this);

            MeasureParamsGroupObject = new MeasureParamsGroup(this);

            ResultsGroupObject = new ResultsGroup(this);
        }


        /// <summary>
        /// オープンタグを見つけた時の処理を記述します。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected override void ParseBeginElement(XmlReader reader, Stack<string> hierarchical)
        {           
            Relations.MeasureItemSerialization relation = null;
            
            // 自分自身のパラメータがオープンタグの場合は、
            // Textで読まれるので、処理をスキップする
            foreach ( var tag in ownedElement)
            { 
                if( reader.Name.Equals(tag.Contents.ToString()))
                {
                    return;
                }
            }


            // テンプレートを使用して現在のItemTypeの情報を読み取る
            if(RelationsObject != null)
            {
                if (RelationsObject.Measures.MeasureDict.ContainsKey(ItemType))
                { 
                    relation = RelationsObject.Measures.MeasureDict[ItemType];
                }                
            }

            // Measure-paramsタグを検出
            if (reader.Name.Equals(MeasureParamsGroupObject.ElementName))
            {
                MeasureParamsGroupObject.UpdateFromRelation();
                MeasureParamsGroupObject.ParseXmlRecipe(reader, hierarchical);
            }

            // Resultsタグを検出
            else if (reader.Name.Equals(ResultsGroupObject.ElementName))
            {
                ResultsGroupObject.UpdateFromRelation();
                ResultsGroupObject.ParseXmlRecipe(reader, hierarchical);
            }

            // Regionタグを検出
            else if (reader.Name.Equals(RegionsGroupObject.ElementName))
            {
                RegionsGroupObject.UpdateFromRelation();
                RegionsGroupObject.ParseXmlRecipe(reader, hierarchical);
            }
            // すべてに該当しない場合は、Coordinateオブジェクトに追加
            else
            {
                CoordinateGroupObject.ParseXmlRecipe(reader, hierarchical);
            }
        }

        /// <summary>
        /// 現在の階層のXMLの値を集計します。
        /// </summary>
        /// <param name="document"></param>
        /// <param name="current"></param>
        protected override void DelegateSubGroup(XmlDocument document, XmlElement current)
        {
            CoordinateGroupObject.MakeXmlNode(document, current);
            ResultsGroupObject.MakeXmlNode(document, current);
            RegionsGroupObject.MakeXmlNode(document, current);

            if (MeasureParamsGroupObject.MeasureParamsList.Count != 0)
            {
                MeasureParamsGroupObject.MakeXmlNode(document, current);
            }
        }
    }
}

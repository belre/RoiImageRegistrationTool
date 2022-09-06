using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Group
{
    /// <summary>
    /// フィルタを表します。
    /// </summary>
    public class FilterGroup : Base.BaseTreeGroup
    {
        protected DataType.GenericDataType _keys_itemtype = new DataType.GenericDataType("ID", typeof(int));
        /// <summary>
        /// フィルタIDを表します。
        /// </summary>
        public object Key_ItemType
        {
            get
            {
                return _keys_itemtype;
            }
        }

        /// <summary>
        /// XMLアトリビュートを表します。
        /// </summary>
        public override DataType.GenericDataType[] AttributeName
        {
            get
            {
                return new DataType.GenericDataType[] { _keys_itemtype };
            }
        }

        /// <summary>
        /// タグ内部に含まれる要素を表すキーを表します。
        /// </summary>
        protected override List<DataType.GenericDataType> ownedElement
        {
            get
            {
                return new List<DataType.GenericDataType> {  };
            }
        }

        /// <summary>
        /// フィルタパラメータを表します。
        /// </summary>
        public FilterParamsGroup FilterParamsGroupObject
        {
            get;
            set;
        }

        /// <summary>
        /// フィルタの種類を表します。
        /// </summary>
        public int FilterType
        {
            get
            {
                return GetParameter<int>(_keys_itemtype);
            }
            set
            {
                SetParameter<int>(_keys_itemtype, value);

                if (FilterParamsGroupObject != null)
                {
                    FilterParamsGroupObject.UpdateFromRelation();
                }
            }
        }

        /// <summary>
        /// 指定された親オブジェクトを表します。
        /// </summary>
        public virtual FiltersGroup SpecifiedParent
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

        public FilterGroup(Group.FiltersGroup parent)
            : base()
        {
            SpecifiedParent = parent;
            _element_name = "Filter";
            FilterType = 1;
            FilterParamsGroupObject = new FilterParamsGroup(this);
        }

        /// <summary>
        /// オープンタグを検出したときの処理を表します。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected override void ParseBeginElement(XmlReader reader, Stack<string> hierarchical)
        {
            if (reader.Name.Equals(FilterParamsGroupObject.ElementName))
            {
                FilterParamsGroupObject.ParseXmlRecipe(reader, hierarchical);
            }
        }


        /// <summary>
        /// XMLドキュメントを生成します。
        /// </summary>
        /// <param name="document"></param>
        /// <param name="current"></param>
        protected override void DelegateSubGroup(XmlDocument document, XmlElement current)
        {

            FilterParamsGroupObject.MakeXmlNode(document, current);
        }

        /// <summary>
        /// 新規フィルタを追加された時の処理を表します。
        /// </summary>
        /// <param name="targetgen"></param>
        /// <param name="isappendnext"></param>
        public override void InsertGroup(Base.BaseTreeGroup targetgen, bool isappendnext = false)
        {
            var obj_int = (FilterParamsGroup)targetgen;
            //obj_int.FilterParamsRelation = RelationsObject.Filters.FilterDict[FilterType].FilterParamName;
            //obj_int.ParentFilterType = FilterType;
            FilterType = 1;
            FilterParamsGroupObject = obj_int;
        }


        public override void UpdateFromFilterRelation()
        {
            FilterParamsGroupObject.UpdateFromRelation();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Group
{
    /// <summary>
    /// Filterの一覧を表すオブジェクトです。
    /// </summary>
    public class FiltersGroup : Base.BaseTreeGroup
    {
        /// <summary>
        /// フィルタの一覧を表します。
        /// </summary>
        public List<FilterGroup> FilterList
        {
            get;
            set;
        }

        /// <summary>
        /// 指定された親オブジェクトを表します。
        /// </summary>
        public virtual RegionGroup SpecifiedParent
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

        public FiltersGroup(Group.RegionGroup parent)
            : base()
        {
            SpecifiedParent = parent;
            _element_name = "Filters";

            FilterList = new List<FilterGroup>();
        }

        /// <summary>
        /// オープンタグを検出したときの処理を表します。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected override void ParseBeginElement(XmlReader reader, Stack<string> hierarchical)
        {
            var item = new Group.FilterGroup(this);
            if (reader.Name.Equals(item.ElementName))
            {
                item.InitAttributes(reader);
                //item.RelationsObject = RelationsObject;
                item.ParseXmlRecipe(reader, hierarchical);
                FilterList.Add(item);            
            }
        }

        /// <summary>
        /// XMLドキュメントを生成します。
        /// </summary>
        /// <param name="document"></param>
        /// <param name="current"></param>
        protected override void DelegateSubGroup(XmlDocument document, XmlElement current)
        {
            if (FilterList.Count == 0)
            {
                return;
            }

            foreach (var itemgroup in FilterList)
            {
                itemgroup.MakeXmlNode(document, current);
            }
        }

        /// <summary>
        /// 新規フィルタ追加時の処理を表します。
        /// </summary>
        /// <param name="targetgen"></param>
        /// <param name="isappendnext"></param>
        public override void InsertGroup(Base.BaseTreeGroup targetgen, bool isappendnext = false)
        {
            var obj = new FilterGroup(this);
            var obj_int = new FilterParamsGroup(obj);

            if( FilterList.Count == 0 )
            {

                obj.InsertGroup(obj_int);
                FilterList.Add(obj);
            }

            else
            {
                var target = (FilterGroup)targetgen;

                int pos = FilterList.IndexOf(target);
                
                if( isappendnext)
                {
                    pos++;
                }

                obj.InsertGroup(obj_int);


                if (pos == FilterList.Count)
                {
                    FilterList.Add(obj);
                }
                else
                {
                    FilterList.Insert(pos, obj);
                }
            }
        }

        /// <summary>
        /// 既存フィルタ削除時の処理を表します。
        /// </summary>
        /// <param name="targetgen"></param>
        public override void DeleteGroup(Base.BaseTreeGroup targetgen)
        {
            FilterList.Remove((FilterGroup)targetgen);

        }

    }
}

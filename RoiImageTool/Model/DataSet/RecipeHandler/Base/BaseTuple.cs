using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Base
{
    /// <summary>
    /// レシピを管理するためのリーフクラスを表します。
    /// </summary>
    public class BaseTuple : BaseXmlNode
    {

        public virtual BaseTreeGroup Owner
        {
            get;
            protected set;
        }


        /// <summary>
        /// labelに適合したこの要素のキーを取得します。
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public object GetKey(string label)
        {
            foreach (var item in ownedElement)
            {
                if (item.ToString().Equals(label))
                {
                    return item;
                }
            }
            return null;
        }

        public BaseTuple(BaseTreeGroup owner)
        {
            Owner = owner;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Base
{
    /// <summary>
    /// レシピを管理するためのブランチクラスにデータの操作を加えたクラスです。
    /// </summary>
    public class BaseTreeGroup : BaseGroup
    {







        /// <summary>
        /// ブランチクラスを初期化します。
        /// </summary>
        /// <param name="parent"></param>
        public BaseTreeGroup()
        {

        }


        /// <summary>
        /// ブランチクラスを追加します。
        /// </summary>
        /// <param name="targetgen"></param>
        /// <param name="isappendnext"></param>
        public virtual void InsertGroup(BaseTreeGroup targetgen, bool isappendnext = false)
        {
            
        }

        /// <summary>
        /// ノードクラスを追加します。
        /// </summary>
        /// <param name="targetgen"></param>
        /// <param name="isappendnext"></param>
        public virtual void InsertTuple(BaseTuple targetgen, bool isappendnext = false)
        {

        }

        /// <summary>
        /// ブランチクラスを削除します。
        /// </summary>
        /// <param name="targetgen"></param>
        public virtual void DeleteGroup(BaseTreeGroup targetgen)
        {

        }

        /// <summary>
        /// ノードクラスを削除します。
        /// </summary>
        /// <param name="targetgen"></param>
        public virtual void DeleteTuple(BaseTuple targetgen)
        {

        }





    }
}

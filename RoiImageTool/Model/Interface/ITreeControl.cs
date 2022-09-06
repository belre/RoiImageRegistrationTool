
using System;
using System.Collections.Generic;

namespace ClipXmlReader.Model.Interface
{
    public interface ITreeControl
    {
        /// <summary>
        /// 現在のオブジェクトのコントロールを使用可能にします。
        /// </summary>
        /// <param name="isincludechild"></param>
        void Enable(bool isincludechild);

        /// <summary>
        /// 現在のオブジェクトのコントロールを使用不可能にします。
        /// </summary>
        /// <param name="isincludechild"></param>
        void Disable(bool isincludechild);


        /// <summary>
        /// targetが自分自身か、子階層に存在するかを返します。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        bool Include(ITreeControl target);

        /// <summary>
        /// 自分自身または子階層を表すtargetを使用可能にします。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="func_list">nullの場合、何もしない。第一引数で指定されたキーを、返り値の値に変更する。ただし、第二引数はthis == targetの場合を表す。</param>
        void Activate(ITreeControl target);
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Error
{
    public class RecipeErrorHandle
    {
        /// <summary>
        /// 排他的な設定を維持
        /// </summary>
        public enum ERecipeError
        {
            OK = 0,
            NOTRUN = 0x01 << 1,
            COORDID_REF_OUTOFRANGE = 0x01 << 2,
            COORDID_REF_SYNTAXERROR = 0x01 << 3,
            REFMEAS_REMOVE = 0x01 << 4,
            REFRECT_REMOVE = 0x01 << 5,
            
            SYSTEM_ERROR = 0x01 << 31
        }

        public static string GetErrorMessage(ERecipeError error)
        {
            StringWriter ss_expr = new StringWriter();
            bool iserror = false;

            

            if (error.HasFlag(ERecipeError.COORDID_REF_OUTOFRANGE))
            {
                ss_expr.Write("Coordの範囲の指定に誤りがあります.");
                iserror = true;
            }
            else if (error.HasFlag(ERecipeError.COORDID_REF_SYNTAXERROR))
            {
                ss_expr.Write("指定されたCoordは参照できません.");
                iserror = true;
            }

            if (error.HasFlag(ERecipeError.REFMEAS_REMOVE))
            {
                ss_expr.Write("参照設定のRef-Measが正しく設定されていません.");
                iserror = true;
            }
            else if (error.HasFlag(ERecipeError.REFRECT_REMOVE))
            {
                ss_expr.Write("参照設定のRef-Rectが正しく設定されていません.");
                iserror = true;
            }
            

            ss_expr.Close();

            if (iserror)
            {
                return ss_expr.ToString();
            }
            else
            {
                return "OK";            
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace ClipXmlReader.Model.IO.Recipes
{
    public class RecipeFileHandler
    {
        /// <summary>
        /// ファイルパスを表します。
        /// </summary>
        public string DirFile
        {
            get;
            set;
        }

        public string LabelDirFile
        {
            get
            {
                if(RecipeContents == null)
                {
                    return DirFile + "(Broken)";
                }
                else
                {
                    return DirFile;
                }
            }
        }

        /// <summary>
        /// レシピを表します。
        /// </summary>
        public DataSet.RecipeHandler.Group.RecipeEntityGroup RecipeContents
        {
            get;
            set;
        }

        protected DataSet.RecipeHandler.Relations.XmlRootSerialization _xml_template;
        /// <summary>
        /// XMLテンプレートオブジェクトを表します。
        /// </summary>
        public DataSet.RecipeHandler.Relations.XmlRootSerialization XmlTemplate
        {
            get
            {
                return _xml_template;
            }
            set
            {
                _xml_template = value;
            }
        }

        protected Exception _parse_exception;


        public RecipeFileHandler()
        {


        }

        public Exception ParserException
        {
            get;
            protected set;
        }

        /// <summary>
        /// XMLをパースします。
        /// </summary>
        /// <param name="abspath"></param>
        public void ParseXML(string abspath)
        {
            RecipeContents = new DataSet.RecipeHandler.Group.RecipeEntityGroup(_xml_template);
            RecipeContents.RelationsObject = XmlTemplate;

            try
            { 
                using (var reader = XmlReader.Create(abspath))
                {               
                    RecipeContents.ParseXmlRecipe(reader);
                }

                ParserException = null;
            }
            catch( Exception err)
            {
                RecipeContents = null;
                ParserException = err;
            }
        }


    }
}

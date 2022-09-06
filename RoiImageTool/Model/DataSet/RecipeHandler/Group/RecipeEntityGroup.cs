using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;


namespace ClipXmlReader.Model.DataSet.RecipeHandler.Group
{
    public class RecipeEntityGroup : Base.BaseTreeGroup
    {
        public int RecipeNumber
        {
            get
            {
                return RecipeObject.RecipeItemGroup.Count;
            }
            
        }


        /// <summary>
        /// レシピの名称を表します。
        /// </summary>
        public string RecipeName
        {
            get;
            set;
        }

        private UserTuple.HeaderTuple _model_header;
        /// <summary>
        /// レシピのヘッダ部を表します。
        /// </summary>
        public UserTuple.HeaderTuple Header
        {
            get
            {
                return _model_header;
            }
            set
            {
                _model_header = value;
            }
        }


        private Group.MeasuresGroup _recipe_object;
        /// <summary>
        /// レシピの計測項目を表します。
        /// </summary>
        public Group.MeasuresGroup RecipeObject
        {
            get
            {
                return _recipe_object;
            }
            protected set
            {
                _recipe_object = value;
            }
        }

        public RecipeEntityGroup(Relations.XmlRootSerialization template)
            : base()
        {

            _base_xml_template = template;

            _model_header = new RecipeHandler.UserTuple.HeaderTuple();
            _recipe_object = new RecipeHandler.Group.MeasuresGroup(this);

        }

        protected override bool IsBoundElementName
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// XMLテキストを読み取ります。
        /// </summary>
        /// <param name="reader">ストリーム</param>
        public void ParseXmlRecipe(XmlReader reader)
        {
            Stack<string> hierarchical_stack = new Stack<string>();

            bool IsParsing = true;
            while(IsParsing && reader.Read())
            {
                if( reader.NodeType == XmlNodeType.Element)
                {
                    hierarchical_stack.Push(reader.Name);

                    ParseXmlRecipe(reader, hierarchical_stack);
                    IsParsing = false;
                }
            }
                
            if(IsParsing || hierarchical_stack.Count != 0)
            {
                throw new Exception("XML Parser Stack Failed.");
            } 
        }



        /// <summary>
        /// XMLパース処理前に行われる処理を表します。
        /// </summary>
        /// <param name="reader"></param>
        protected override void InitParseProcedures(XmlReader reader)
        {
            {
                RecipeName = reader.GetAttribute("Name");
            }
        }

        /// <summary>
        /// XMLレシピで、オープンタグを検出したときに実行する処理を表します。
        /// 何か処理をする場合は、オーバーライドします。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected override void ParseBeginElement(XmlReader reader, Stack<string> hierarchical)
        {
            var header = new UserTuple.HeaderTuple();
            var measures = new Group.MeasuresGroup(this);

            if (header.ElementName.Equals(reader.Name))
            {
                Header = header;
                Header.ParseXmlRecipe(reader, hierarchical);
            }
            else if (measures.ElementName.Equals(reader.Name))
            {
                // recipe process
                RecipeObject = measures;
                measures.ParseXmlRecipe(reader, hierarchical);
            }
        }





        /// <summary>
        /// 内容をXMLExportします。
        /// </summary>
        /// <param name="document"></param>
        public void ExportXmlRecipe(XmlDocument document)
        {
            var current_element = document.CreateElement("Setting");
            current_element.SetAttribute("Name", RecipeName);

            Header.Value_Number = this.RecipeNumber;
            Header.MakeXmlNode(document, current_element);
            RecipeObject.MakeXmlNode(document, current_element);

            document.AppendChild(current_element);

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Xml;


namespace ClipXmlReader.Model.IO.Recipes
{
    /// <summary>
    /// データの管理を行うクラスです。
    /// </summary>
    public class WorkspaceManager
    {

        /// <summary>
        /// 読み込まれているデータの一覧を表します。
        /// </summary>
        public Dictionary<string, ClipXmlReader.Model.IO.Recipes.RecipeFileHandler> DataSet
        {
            get
            {
                if (Handler == null) return null;

                var handler = new Dictionary<string, ClipXmlReader.Model.IO.Recipes.RecipeFileHandler>();

                foreach (var item in Handler)
                {
                    handler[item.DirFile] = item;
                }

                return handler;
#if false
                if (Handler == null) return null;

                var dict = new Dictionary<string, ClipXmlReader.Model.DataSet.RecipeHandler.Group.RecipeEntityGroup>();

                foreach (var item in Handler)
                {
                    dict[item.DirFile] = item.RecipeContents;
                }

                return dict;
#endif
            }
        }

        /// <summary>
        /// 現在のワークスペースフォルダを表します。
        /// </summary>
        public string WorkspaceDirectory
        {
            get;
            protected set;
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
        /// <summary>
        /// ワークスペース内で読み込むファイルを指定します。
        /// </summary>
        public string[] DirFiles
        {
            get
            {
                if(_handler == null)
                {
                    return null;
                }

                string[] tmp = new string[_handler.Length];

                for (int i = 0; i < _handler.Length; i++)
                {
                    tmp[i] = _handler[i].DirFile;
                }

                return tmp;
            }
        }

        protected RecipeFileHandler[] _handler;
        public RecipeFileHandler[] Handler
        {
            get
            {
                return _handler;
            }
        }

        public WorkspaceManager()
        {
            WorkspaceDirectory = "";
            //DataSet = new Dictionary<string, Model.DataSet.RecipeHandler.Group.RecipeEntityGroup>();
        }

        /// <summary>
        /// ワークスペースを切り替えます。
        /// </summary>
        /// <param name="root">ワークスペースのルートディレクトリ</param>
        public void SwitchWorkspace(string root)
        {
            WorkspaceDirectory = root;

            // ワークスペース一覧
            var source = System.IO.Directory.GetFiles(WorkspaceDirectory, "*.xml");
            var handlerlist = new List<RecipeFileHandler>();
            foreach (var obj in source)
            {
                var handler = new RecipeFileHandler() { DirFile = System.IO.Path.GetFileName(obj), XmlTemplate = _xml_template };
                string abspath = WorkspaceDirectory + @"\" + handler.DirFile;
                handler.ParseXML(abspath);

                handlerlist.Add(handler);
            }
            _handler = handlerlist.ToArray();
        }


        public void UpdateWorkspace()
        {
            // ワークスペース一覧
            var source = System.IO.Directory.GetFiles(WorkspaceDirectory, "*.xml");
            var addhandler = new List<RecipeFileHandler>();
            foreach (var obj in source)
            {
                if( !_handler.Any((RecipeFileHandler handler) => { return handler.DirFile == System.IO.Path.GetFileName(obj); }) ) {
                    var handler = new RecipeFileHandler() { DirFile = System.IO.Path.GetFileName(obj), XmlTemplate = _xml_template };
                    string abspath = WorkspaceDirectory + @"\" + handler.DirFile;
                    handler.ParseXML(abspath);

                    addhandler.Add(handler);
                }
            }

            List<RecipeFileHandler> newhandler = new List<RecipeFileHandler>();
            newhandler.AddRange(addhandler);
            newhandler.AddRange(_handler);

            _handler = newhandler.ToArray();
       }



        /// <summary>
        /// ファイルを上書き保存します。
        /// </summary>
        /// <param name="filepath"></param>
        public void SaveNewFile(string filepath)
        {
            var backup_path = WorkspaceDirectory + @"\" + filepath + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".bak";
            System.IO.File.Copy(WorkspaceDirectory + @"\" + filepath, backup_path, true);

            SaveAsNewFile(filepath, WorkspaceDirectory + @"\" + filepath);
        }

        /// <summary>
        /// ファイルを新規に保存します。
        /// </summary>
        /// <param name="currentdir"></param>
        /// <param name="filepath"></param>
        public void SaveAsNewFile(string currentdir, string filepath)
        {
            var currentdataset = DataSet[currentdir].RecipeContents;

            XmlWriterSettings setting = new XmlWriterSettings() 
                { Indent=true, IndentChars="\t", NewLineChars="\n", Encoding=Encoding.GetEncoding("Shift_JIS") };


            if( System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }


            using ( XmlWriter xmlwriter = XmlWriter.Create(filepath, setting))
            {
                XmlDocument document = new XmlDocument();

                document.CreateXmlDeclaration("1.0", "Shift_JIS", null);
                currentdataset.ExportXmlRecipe(document);
                
                document.Save(xmlwriter);

            }
        }

        /// <summary>
        /// srcpathを基にして、destpathの各レシピデータを更新します。
        /// </summary>
        /// <param name="srcpath"></param>
        /// <param name="destpath"></param>
        public void CopyWorkingData(string srcpath, string destpath)
        {
            if ( srcpath == null || destpath == null || !DataSet.Keys.Contains(srcpath) || !DataSet.Keys.Contains(destpath))
            {
                return;
            }

            // オフセット情報のコピー
            //DataSet[destpath].RecipeContents.RecipeObject.GetOffsetFromObject(DataSet[srcpath].RecipeContents.RecipeObject);
        }



    }
}

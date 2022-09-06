using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.IO.ReferenceImage
{
    public class ReferenceImageHandler
    {
        /// <summary>
        /// 現在のルートディレクトリを指定します。
        /// </summary>
        public string RootDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// 現在のルートディレクトリからのファイルの一覧を取得します。
        /// </summary>
        public string[] DirFiles
        {
            get;
            protected set;
        }

        /// <summary>
        /// 現在指定されている画像ファイルを表します。
        /// </summary>
        public string CurrentFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// 指定されたfilepathに基づいて、画像ファイルとファイル一覧を更新します。
        /// </summary>
        /// <param name="filepath"></param>
        public void SearchImageFiles(string filepath)
        {
            var fullpath = filepath;
            var root = System.IO.Path.GetDirectoryName(fullpath);
            var filename = System.IO.Path.GetFileName(fullpath);

            // ファイル検索
            var files = System.IO.Directory.GetFiles(root, "*.bmp", System.IO.SearchOption.TopDirectoryOnly);
            var lists = new List<string>();
            foreach (var file in files)
            {
                lists.Add(System.IO.Path.GetFileName(file));
            }

            RootDirectory = root;
            CurrentFilePath = filename;
            DirFiles = lists.ToArray();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.IO.ReferenceImage
{
    public class ReferenceWorkImageManager
    {
        private IO.ReferenceImage.ReferenceImageHandler _handler_referenceimage;
        private Dictionary<object, Control.ImageViewer.WorkImageView> _workimage_view_dict;
        private Dictionary<object, Control.ImageViewer.WorkImageStorage> _storage_dict;

        public string[] DirFiles
        {
            get
            {
                return _handler_referenceimage.DirFiles;
            }
        }

        public string CurrentFilePath
        {
            get
            {
                return _handler_referenceimage.CurrentFilePath;
            }
            set
            {
                _handler_referenceimage.CurrentFilePath = value;
            }
        }

        public string CurrentFilePathAbsolute
        {
            get
            {
                return _handler_referenceimage.RootDirectory + @"\" + CurrentFilePath;
            }
        }
        
        public object RecentStorageKey
        {
            get;
            protected set;
        }

        public object RecentViewKey
        {
            get;
            protected set;
        }

        public ReferenceWorkImageManager()
        {
            _handler_referenceimage = new IO.ReferenceImage.ReferenceImageHandler();
            _workimage_view_dict = new Dictionary<object, Control.ImageViewer.WorkImageView>();
            _storage_dict = new Dictionary<object, Control.ImageViewer.WorkImageStorage>();
        }

        public void GetDirFiles(string filepath)
        {
            _handler_referenceimage.SearchImageFiles(filepath);
        }

        public void MakeSharedView(Control.ImageViewer.WorkImageView anotherview)
        {
            RecentViewKey = new object();
            _workimage_view_dict[RecentViewKey] = new Control.ImageViewer.WorkImageView(anotherview.StorageObject);
        }

        public void MakeView(object storagekey)
        {
            RecentViewKey = new object();
            _workimage_view_dict[RecentViewKey] = new Control.ImageViewer.WorkImageView(_storage_dict[storagekey]);
        }

        public void MakeView()
        {
            MakeStorage();
            MakeView(RecentStorageKey);
        }

        public void MakeStorage()
        {
            RecentStorageKey = new object();
            _storage_dict[RecentStorageKey] = new Control.ImageViewer.WorkImageStorage();
        }

        public void CloseView(object viewkey)
        {
            _workimage_view_dict.Remove(viewkey);
        }

        public Control.ImageViewer.WorkImageView GetView(object viewkey)
        {
            return _workimage_view_dict[viewkey];
        }

        public void Release()
        {
            _workimage_view_dict.Clear();
            _storage_dict.Clear();
        }
    }
}

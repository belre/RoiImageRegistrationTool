using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;

using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace ClipXmlReader.ViewModel.Xml.Base
{
    public class BaseTreeSource : BaseWithRightClickMenuSource
    {
        public enum ERegionTreeUIType
        {
            None,
            TextBox,
            ComboBox
        }

        protected bool _isExpand = true;
        protected string _text = "";
        protected object _value;
        protected BaseTreeSource _parent = null;
        protected ObservableCollection<BaseTreeSource> _children = null;
        protected ObservableCollection<Xml.TreeView.TreeParameterSource> _sources;
        

        public void UpdateProperty()
        {
            OnPropertyChanged("Value");
        }


        protected virtual bool IsNode
        {
            get
            {
                return true;
            }
        }

        
        public virtual bool IsExpanded
        {
            get
            {
                return _isExpand;
            }

            set
            {
                _isExpand = value;
                OnPropertyChanged("IsExpanded");                
            }
        }

        public virtual bool IsRedo
        {
            get
            {
                return false;
            }
        }

        public virtual string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged("Text");       
            }
        }

        public virtual string Description
        {
            get
            {
                return "";
            }
            set
            {

            }
        }

        public virtual bool IsVisibleTextBoxArrangeKey
        {
            get
            {
                return false;
            }
            set
            {
                OnPropertyChanged("IsVisibleTextBoxArrangeKey");
            }
        }

        public virtual bool IsVisibleTextBoxNormalKey
        {
            get
            {
                return !IsVisibleTextBoxArrangeKey;
            }

            set
            {
                OnPropertyChanged("IsVisibleTextBoxNormalKey");
            }
        }

        

        public virtual object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }


        public virtual ObservableCollection<Xml.TreeView.TreeParameterSource> Sources
        {
            get
            {
                return _sources;
            }
            set
            {
                _sources = value;
                OnPropertyChanged("Sources");
            }
        }

        public virtual ERegionTreeUIType UIType
        {
            get;
            protected set;
        }

        public virtual System.Type ValueType
        {
            get;
            protected set;
        }

        public virtual BaseTreeSource Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                OnPropertyChanged("Parent");
            }
        }

        public virtual ObservableCollection<BaseTreeSource> Children
        {
            get
            {
                return _children;
            }
            set
            {
                _children = value;
                OnPropertyChanged("Children");
            }
        }

        public virtual bool IsVisibleTextBoxValue
        {
            get
            {
                return UIType == ERegionTreeUIType.TextBox ;
            }
            set
            {

            }
        }

        public virtual bool IsVisibleComboBoxValue
        {
            get
            {
                return UIType == ERegionTreeUIType.ComboBox ;
            }
            set
            {

            }
        }

        public virtual bool IsVisibleEqualsMark
        {
            get
            {
                return IsNode;
            }
        }




        public virtual bool IsVisibleCircleMark
        {
            get
            {
                return false;
            }
        }

        public virtual System.Windows.Media.Brush MarkColor
        {
            get
            {
                return System.Windows.Media.Brushes.Transparent;
            }
        }

        public BaseTreeSource()
        {

        }

        public void InitNode()
        {
            if (Children == null)
            {
                Children = new ObservableCollection<BaseTreeSource>();
            }


            Children.Clear();
        }

        public void AddNode(BaseTreeSource child)
        {
            if(Children == null)
            {
                Children = new ObservableCollection<BaseTreeSource>();
            }

            child.Parent = this;
            Children.Add(child);
        }

        public void InsertNode(int index, BaseTreeSource child)
        {
            if (Children == null)
            {
                Children = new ObservableCollection<BaseTreeSource>();
            }

            child.Parent = this;
            Children.Insert(index, child);
        }
        
        public void RemoveNode(int index)
        {
            Children.RemoveAt(index);
        }

    }
}

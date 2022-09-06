using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;

using System.Windows.Input;

namespace ClipXmlReader.ViewModel.Xml.RightClickMenu
{
    public class RightClickMenuSource : INotifyPropertyChanged
    {
        private string _headername;
        private bool _ischecked;
        private bool _isseparator;


        public string HeaderName
        {
            get
            {
                return _headername;
            }
            set
            {
                _headername = value;
                OnPropertyChanged("HeaderName");
            }
        }

        private ICommand _rightclickevent;
        public ICommand RightClickEvent
        {
            get
            {
                return _rightclickevent;
            }
            set
            {
                _rightclickevent = value;
                OnPropertyChanged("RightEvent");
            }
        }

        private object _rightclickparam;
        public object RightClickParam
        {
            get
            {
                return _rightclickparam;
            }
            set
            {
                _rightclickparam = value;
                OnPropertyChanged("RightClickParam");
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _ischecked;
            }
            set
            {
                _ischecked = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        public bool IsSeparator
        {
            get
            {
                return _isseparator;
            }
            set
            {
                _isseparator = value;
                OnPropertyChanged("IsSeparator");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null) return;

            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}

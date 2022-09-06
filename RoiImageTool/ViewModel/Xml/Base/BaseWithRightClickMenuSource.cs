using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;

using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace ClipXmlReader.ViewModel.Xml.Base
{
    public class BaseWithRightClickMenuSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null) return;

            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        protected ObservableCollection<RightClickMenu.RightClickMenuSource> _binding_rightclickmenu;
        public virtual ObservableCollection<RightClickMenu.RightClickMenuSource> BindRightClickMenu
        {
            get
            {
                return _binding_rightclickmenu;
            }
            protected set
            {
                _binding_rightclickmenu = value;
            }
        }

        public bool IsEnabledRightClickEvent
        {
            get
            {
                return CommandHandlingRightClick != null;
            }
        }

        private ICommand _command_handle_rightclick;
        public ICommand CommandHandlingRightClick
        {
            get
            {
                return _command_handle_rightclick ?? (this._command_handle_rightclick = new DelegateCommand<object>(HandleRightClick));
            }
        }

        protected virtual void HandleRightClick(object param)
        {
            MessageBox.Show("Handle right click");
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;


namespace ClipXmlReader.ViewModel.Xml.TreeView
{
    public class TreeParameterSource : INotifyPropertyChanged
    {
        protected string _parameter_label;
        protected object _binding_parameter_value;

        public string ParameterLabel
        {
            get
            {
                return _parameter_label;
            }
            set
            {
                _parameter_label = value;
                OnPropertyChanged("ParameterLabel");
            }
        }

        public object BindingParameterValue
        {
            get
            {
                return _binding_parameter_value;
            }
            set
            {
                _binding_parameter_value = value;
                OnPropertyChanged("BindingParameterValue");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null) return;

            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

    }
}

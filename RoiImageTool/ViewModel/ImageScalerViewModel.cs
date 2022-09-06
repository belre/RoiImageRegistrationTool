
using System.Windows;

using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using System.ComponentModel;


using System.Runtime.Serialization;

namespace ClipXmlReader.ViewModel
{

    public class ScalerSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null) return;

            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private string _scale_val;
        public string ScaleVal
        {
            get
            {
                return _scale_val;
            }
            set
            {
                _scale_val = value;
                OnPropertyChanged("ScaleVal");
            }
        }

        public bool IsMajorTicks
        {
            get;
            set;
        }

        public bool IsMinorTicks
        {
            get
            {
                return !IsMajorTicks;
            }
        }
    }

    public class ImageScalerViewModel : BindableBase
    {
        public ImageScalerViewModel()
        {
           
            UpdateScale(0, 1000.0, 10, 0);
        }

        private ObservableCollection<ScalerSource> _scales;
        public ObservableCollection<ScalerSource> Scales
        {
            get
            {
                return _scales;
            }
            set
            {
                _scales = value;
                RaisePropertyChanged("Scales");
            }
        }

        private int _length;
        public int Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
                RaisePropertyChanged("Length");                   
            }
        }

        public int Margin
        {
            get
            {
                return UserMargin;
            }
        }

        private int _usermargin;
        private int UserMargin
        {
            get
            {
                return _usermargin;
            }
            set
            {
                _usermargin = value;
                RaisePropertyChanged("Margin");
            }
        }


        private int _scrollmargin;
        public int ScrollMargin
        {
            get
            {
                return _scrollmargin;
            }
            set
            {
                _scrollmargin = value;
                RaisePropertyChanged("Margin");
            }
        }

        

        private int _stepnumber;
        public int StepNumber
        {
            get
            {
                return _stepnumber;
            }
            protected set
            {
                _stepnumber = value;
                RaisePropertyChanged("StepNumber");
            }
        }


        private double _scalestep;
        public double ScaleStep
        {
            get
            {
                return _scalestep;
            }
            protected set
            {
                _scalestep = value;
                RaisePropertyChanged("ScaleStep");
            }
        }

        private double _scalelower;
        public double ScaleLower
        {
            get
            {
                return _scalelower;
            }
            protected set
            {
                _scalelower = value;
                RaisePropertyChanged("ScaleLower");
            }
        }

        public void UpdateScale(double scalelower, double scalestep, int stepnumber, int margin)
        {
            Scales = new ObservableCollection<ScalerSource>();

            for (int i = 0; i < stepnumber; i++)
            {
                double scaleval =  scalestep * i;
                Scales.Add(new ScalerSource() { ScaleVal = scaleval.ToString("F1"), IsMajorTicks = true });
            }

            ScaleLower = scalelower;
            ScaleStep = scalestep;
            StepNumber = stepnumber;
            UserMargin = margin;
        }
    }
}

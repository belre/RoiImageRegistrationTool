using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;

using System.ComponentModel;
using System.Collections.ObjectModel;


using System.Windows.Input;
using Prism;
using Prism.Mvvm;
using Prism.Commands;

namespace ClipXmlReader.ViewModel.ImageViewer
{
    public class ImageViewSource : INotifyPropertyChanged
    {
        private Model.IO.Recipes.WorkspaceManager _model_workspace_manager;
        
        public ObservableCollection<ImageViewer.IViewableItem> ViewableItems
        {
            get;
            protected set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null) return;

            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }


        protected ImageViewerGuiViewModel _parentvm;
        public ImageViewSource(ImageViewerGuiViewModel parentvm, Model.IO.Recipes.WorkspaceManager manager)
        {
            _parentvm = parentvm;
            _model_workspace_manager = manager;

            ViewableItems = new ObservableCollection<ImageViewer.IViewableItem>();
#if false
            {
                new ImageViewer.ImageViewRectangleSource(this) 
                { 
                    ViewPoint = new System.Windows.Point(ScalerStep / (ResampleRate * StepOptX), (ScalerStep / (ResampleRate * StepOptY))),
                    Width = ScalerStep / (ResampleRate * StepOptX),
                    Height = ScalerStep / (ResampleRate * StepOptY),
                    Brush = new System.Windows.Media.SolidColorBrush(Color.FromArgb(128, 0, 0, 255)),
                    Text = "Hello",
                    StrokeThickness = 0,
                    IsVisible = true
                },
                new ImageViewer.ImageViewRectangleSource(this)
                {
                    ViewPoint = new System.Windows.Point(12641 / 4, 9837 / 4),
                    Width = 243 / 4,
                    Height = 237 / 4,
                    Brush = new System.Windows.Media.SolidColorBrush(Color.FromArgb(128, 0, 255, 0)),
                    Text = "Hello",                 
                    StrokeThickness = 0,
                    IsVisible = true
                }
            };
#endif
        }

        private double _scaler_step = 10.0;
        public double ScalerStep
        {
            get
            {
                return _scaler_step;
            }
            set
            {
                _scaler_step = value;
            }
        }


        private double _resample_rate = 4.0;
        public double ResampleRate
        {
            get
            {
                return _resample_rate;
            }

            set
            {
                _resample_rate = value;
                // RaisePropertyChangedは使用しない
                // DrawImage関連の処理が同時に動くことが前提となる.
            }
        }

        private double _resize_rate = 1.0;
        public double ResizeRate
        {
            get
            {
                return _resize_rate;
            }
            set
            {
                _resize_rate = value;
                OnPropertyChanged("ResizeRate");
                OnPropertyChanged("ResizeScale");
                OnPropertyChanged("ImageWidth");
                OnPropertyChanged("ImageHeight");
            }
        }

        public double ResizeScale
        {
            get
            {
                return 1 / _resize_rate;
            }
            set
            {
                _resize_rate = 1 / _resize_rate;
                OnPropertyChanged("ResizeRate");
                OnPropertyChanged("ResizeScale");
                OnPropertyChanged("ImageWidth");
                OnPropertyChanged("ImageHeight");
            }
        }

        public double StepOptX
        {
            get
            {
                if (_model_workspace_manager == null)
                {
                    return Double.NaN;
                }
                return _model_workspace_manager.XmlTemplate.Machines.Machine[0].Resolution.X;
                //return 0.02;
            }
        }

        public double StepOptY
        {
            get
            {
                if (_model_workspace_manager == null)
                {
                    return Double.NaN;
                }

                return _model_workspace_manager.XmlTemplate.Machines.Machine[0].Resolution.Y;
                //return 0.0198;
            }
        }

        public void ResetPopupDisplay()
        {
            foreach (var item in ViewableItems)
            {
                item.IsViewPopup = false;
            }
        }

        public object KeyViewObject
        {
            get;
            set;
        }

        public System.Windows.Point MousePosition
        {
            get;
            protected set;
        }

        private ICommand _mousemove_command;
        public ICommand MouseMoveCommand
        {
            get
            {
                return _mousemove_command ?? (_mousemove_command = new DelegateCommand<object>(MouseMove));
            }
        }

        public void MouseMove(object param)
        {
#if true
            // マウス座標取得
            var element = (System.Windows.IInputElement)param;
            var mousepos = Mouse.GetPosition(element);
            mousepos.X *= _parentvm.ResizeRate;
            mousepos.Y *= _parentvm.ResizeRate;
            MousePosition = mousepos;

            foreach( var item in ViewableItems)
            {
                if( item.IsMovable)
                {
                    item.MoveViewPoint(MousePosition);
                }
            }
#endif
        }


        private BitmapSource _displayedimage;
        public BitmapSource DisplayedImage
        {
            get
            {
                return _displayedimage;
            }
            set
            {
                _displayedimage = value;
                OnPropertyChanged("DisplayedImage");
                OnPropertyChanged("ImageWidth");
                OnPropertyChanged("ImageHeight");
            }
        }

        public double ImageWidth
        {
            get
            {
                if (_displayedimage == null)
                {
                    return 0;
                }
                else
                {
                    return _displayedimage.Width ;
                }
            }
        }

        public double ImageHeight
        {
            get
            {
                if (_displayedimage == null)
                {
                    return 0;
                }
                else
                {
                    return _displayedimage.Height ;
                }
            }
        }


        public CroppedBitmap Crop(System.Windows.Rect rect)
        {
            CroppedBitmap crop = new CroppedBitmap(_displayedimage, new System.Windows.Int32Rect() { X = (int)rect.X, Y = (int)rect.Y, Width = (int)rect.Width, Height = (int)rect.Height });
            return crop;
        }

        public void Invalidate()
        {
            foreach (var area in ViewableItems)
            {
                area.Invalidate();
            }
        }

        public void SetCoordinatesFromViewableItem(IViewableItem item)
        {
            _parentvm.SetCoordinatesFromViewableItem(item);
        }
    }
}

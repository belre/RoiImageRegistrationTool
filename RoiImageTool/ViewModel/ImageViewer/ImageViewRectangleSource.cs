using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Shapes;
using System.Windows.Media;

using System.ComponentModel;
using System.Collections.ObjectModel;


using System.Windows.Input;
using Prism;
using Prism.Mvvm;
using Prism.Commands;

namespace ClipXmlReader.ViewModel.ImageViewer
{
    public class ImageViewRectangleSource : INotifyPropertyChanged, IViewableItem
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null) return;

            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public ImageViewSource _parent_imageview;

        public ImageViewRectangleSource(ImageViewSource parent_imageview, Model.DataSet.RecipeHandler.Group.RegionGroup regionobj)
        {
            _parent_imageview = parent_imageview;
            SourceRegion = regionobj;
        }


        // 各設定値
        protected double _width;
        public double Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                Invalidate();
            }
        }

        protected double _height;
        public double Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                Invalidate();
            }
        }

        protected double _areaangle;
        public double AreaAngle
        {
            get
            {
                return _areaangle;
            }
            set
            {
                _areaangle = value;
                Invalidate();
            }
        }

        protected Brush _brush;
        public Brush Brush
        {
            get
            {
                return _brush;
            }
            set
            {
                _brush = value;
                Invalidate();
            }
        }

        protected string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                Invalidate();
            }
        }

        public void Invalidate()
        {
            OnPropertyChanged("ViewPoint");
            OnPropertyChanged("Shape");
            OnPropertyChanged("TextLabel");

            OnPropertyChanged("StrokeThickness");
            OnPropertyChanged("StrokeShapeOffset");
        }

        protected bool _isvisible;
        public bool IsVisible
        {
            get
            {
                return _isvisible;
            }
            set
            {
                _isvisible = value;
                Invalidate();
            }
        }

        protected System.Windows.Point _viewpoint;
        public System.Windows.Point ViewPoint
        {
            get
            {
                return _viewpoint;
            }
            set
            {
                _viewpoint = value;
                Invalidate();
            }
        }

        public System.Windows.Controls.TextBlock TextLabel
        {
            get
            {
                System.Windows.Controls.TextBlock textblock = new System.Windows.Controls.TextBlock();
                textblock.Text = Text;

                return textblock;
            }
        }

        public Shape Shape
        {
            get
            {
                var rect = new Rectangle();
                rect.Width = Width;
                rect.Height = Height;

                if (_parent_imageview.DisplayedImage != null)
                {

#if false
                    rect.Fill = new ImageBrush()
                    {                       
                        ImageSource = BinaryImage
                    };
#else
                    rect.Fill = Brush;

#if false
                    ImageSource picture = MarkedAreaImage;
                    rect.Fill = new ImageBrush() {
                        ImageSource = picture
                    };
#endif


#endif

                }

                return rect;
            }
        }

        public Shape StrokeShape
        {
            get
            {
                var rect = new Rectangle();
                rect.Width = Width + 2 * StrokeThickness;
                rect.Height = Height + 2 * StrokeThickness;

                rect.Fill = Brushes.Transparent;
                rect.StrokeThickness = StrokeThickness;
                rect.Stroke = Brushes.Red;

                return rect;

            }
        }

        private int _stroke_thickness;
        public int StrokeThickness
        {
            get
            {
                return _stroke_thickness;
            }

            set
            {
                _stroke_thickness = value;
                Invalidate();
            }
        }

        public System.Drawing.Point StrokeShapeOffset
        {
            get
            {
                return new System.Drawing.Point(-_stroke_thickness, -_stroke_thickness);
            }
        }

        public System.Windows.Media.Imaging.CroppedBitmap CroppedImage
        {
            get
            {
                return _parent_imageview.Crop(new System.Windows.Rect(ViewPoint.X, ViewPoint.Y, Width, Height));
            }
        }



        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 2値化練習
        /// </summary>
        public System.Windows.Media.Imaging.BitmapSource BinaryImage
        {
            get
            {
                int bitCount = System.Drawing.Bitmap.GetPixelFormatSize(System.Drawing.Imaging.PixelFormat.Format32bppArgb) / 8;

                // ピクセルデータをコピー
                int width = (int)CroppedImage.Width;
                int height = (int)CroppedImage.Height;
                int stride = width * bitCount;
                byte[] datas = new byte[stride * height];
                CroppedImage.CopyPixels(datas, stride, 0);

                // Bitmap へピクセルデータ書き出し
                System.Drawing.Bitmap dest = new System.Drawing.Bitmap(
                    (int)CroppedImage.Width,
                    (int)CroppedImage.Height,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                System.Drawing.Imaging.BitmapData destdata =
                    dest.LockBits(new System.Drawing.Rectangle(0, 0, (int)CroppedImage.Width, (int)CroppedImage.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                IntPtr pointer = destdata.Scan0;
                byte[] destdata_bits = new byte[datas.Length];

                for(int i = 0 ; i < dest.Width * dest.Height ; i ++ )
                {
                    int currentpixel = (datas[bitCount * i + 0] + datas[bitCount * i + 1] + datas[bitCount * i + 2]) / 3;

                    if (currentpixel < 192)
                    {
                        destdata_bits[bitCount * i + 0] = destdata_bits[bitCount * i + 1] = destdata_bits[bitCount * i + 2] = 0;
                    }
                    else
                    {
                        destdata_bits[bitCount * i + 0] = destdata_bits[bitCount * i + 1] = destdata_bits[bitCount * i + 2] = 255;                        
                    }
                    destdata_bits[bitCount * i + 3] = 255;
                }

                System.Runtime.InteropServices.Marshal.Copy(destdata_bits, 0, pointer, dest.Width * dest.Height * 4);

                dest.UnlockBits(destdata);


                IntPtr source = dest.GetHbitmap();
                var imagesource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(source,
                                  IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(source);


                return imagesource;
            }
        }

        public System.Windows.Media.Imaging.BitmapSource MarkedAreaImage
        {
            get
            {
                int bitCount = System.Drawing.Bitmap.GetPixelFormatSize(System.Drawing.Imaging.PixelFormat.Format32bppArgb) / 8;

                // ピクセルデータをコピー
                int width = (int)CroppedImage.Width;
                int height = (int)CroppedImage.Height;
                int stride = width * bitCount;
                byte[] datas = new byte[stride * height];
                CroppedImage.CopyPixels(datas, stride, 0);

                // Bitmap へピクセルデータ書き出し
                System.Drawing.Bitmap dest = new System.Drawing.Bitmap(
                    (int)CroppedImage.Width,
                    (int)CroppedImage.Height,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dest);

                g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(128, 255, 0, 0)), 0, 0, width, height);
                g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 255, 0, 0), 3), 0, 0, width, height);
                g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 255, 0, 0), 3), 0, height, width, 0);

                IntPtr source = dest.GetHbitmap();
                var imagesource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(source,
                                  IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(source);


                return imagesource;
            }
        }

        public System.Windows.VerticalAlignment VerticalAlignment
        {
            get
            {
                return System.Windows.VerticalAlignment.Bottom;
            }
        }

        public System.Windows.HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return System.Windows.HorizontalAlignment.Right;
            }
        }

        public System.Drawing.PointF CenterPoint
        {
            get
            {
                return new System.Drawing.PointF((float)(Shape.Width / 2), (float)(Shape.Height / 2));
            }
        }

        public double Angle
        {
            get
            {
                return AreaAngle;
            }
        }


        private ICommand _rightclick_command;
        public ICommand RightClickCommand
        {
            get
            {
                return _rightclick_command ?? (_rightclick_command = new DelegateCommand<object>(ShowPopup));
            }
        }

        public void ShowPopup(object param)
        {
            IsViewPopup = true;
            Invalidate();
        }

        private bool _ispopup = false;
        public bool IsViewPopup
        {
            get
            {
                return _ispopup;
            }
            set
            {
                _ispopup = value;
                OnPropertyChanged("Viewpopup");
            }
        }

        private ICommand _mousepush_command;
        public ICommand MousePushCommand
        {
            get
            {
                return _mousepush_command ?? (_mousepush_command = new DelegateCommand<object>(MousePush));
            }
        }

        private ICommand _mousemove_command;
        public ICommand MouseMoveCommand
        {
            get
            {
                return _mousemove_command ?? (_mousemove_command = new DelegateCommand<object>(MouseMove));
            }
        }

        private ICommand _mouserelease_command;
        public ICommand MouseReleaseCommand
        {
            get
            {
                return _mouserelease_command ?? (_mouserelease_command = new DelegateCommand<object>(MouseRelease));
            }
        }

        bool _isleftpush = false; 
        System.Windows.Point _init_point;
        System.Windows.Point _init_image_point;
        public void MousePush(object param)
        {
            // マウス座標取得
            var element = (System.Windows.IInputElement)param;
            _init_point = _viewpoint;
            _init_image_point = _parent_imageview.MousePosition;
            _isleftpush = true;
            Console.WriteLine("MousePush:" + _init_point.X + "," + _init_point.Y);
        }

        public void MouseMove(object param)
        {
  
        }

        public bool IsMovable
        {
            get
            {
                return _isleftpush;
            }
            set
            {
                _isleftpush = value;
                OnPropertyChanged("IsMovable");
            }
        }

        public void MoveViewPoint(System.Windows.Point point)
        {
            _viewpoint.X = (point.X - _init_image_point.X) + _init_point.X;
            _viewpoint.Y = (point.Y - _init_image_point.Y) + _init_point.Y;

            Invalidate();
        }

        public void MouseRelease(object param)
        {

            // マウス座標取得
            //var element = (System.Windows.IInputElement)param;
            //var position = Mouse.GetPosition(element);
            _isleftpush = false;
            //Console.WriteLine("MouseRelease:" + position.X + "," + position.Y);

            _parent_imageview.SetCoordinatesFromViewableItem(this);

        }

        public Model.DataSet.RecipeHandler.Group.RegionGroup SourceRegion
        {
            get;
            protected set;
        }

    }
}

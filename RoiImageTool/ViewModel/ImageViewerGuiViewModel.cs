
using System.Windows;

using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Windows.Forms;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;

using System.Threading;

using System.ComponentModel;
using System.Collections.ObjectModel;


namespace ClipXmlReader.ViewModel
{
    public class ImageViewerGuiViewModel: BindableBase
    {
        private Model.IO.ReferenceImage.ReferenceWorkImageManager _model_workimage;
        private Model.IO.Recipes.WorkspaceManager _model_workspace_manager;
        private ViewModel.MainWindowViewModel _mastervm;

        public ImageViewerGuiViewModel()
            : this(null, null)
        {

        }

        public ImageViewerGuiViewModel(ViewModel.MainWindowViewModel mastervm, Model.IO.Recipes.WorkspaceManager manager)
        {
            ViewModelHorizontalImageScaler = new ImageScalerViewModel();
            ViewModelVerticalImageScaler = new ImageScalerViewModel();

            _model_workimage = new Model.IO.ReferenceImage.ReferenceWorkImageManager();
            _model_workspace_manager = manager;

            _model_workimage.MakeView();
            ImageObject = new ImageViewer.ImageViewSource(this, _model_workspace_manager)
            {
                KeyViewObject = _model_workimage.RecentViewKey
            };

            ResampleRate = 4.0;
            ResizeRate = 1.0;

            _mastervm = mastervm;

        }



        public string[] ImageFilePathList
        {
            get
            {
                if (_model_workimage == null)
                {
                    return null;
                }
                else
                {
                    return _model_workimage.DirFiles;
                }
            }
        }

        public string CurrentImageFilePath
        {
            get
            {
                if (_model_workimage == null)
                {
                    return null;
                }
                else
                {
                    return _model_workimage.CurrentFilePath;
                }
            }
            set
            {
                _model_workimage.CurrentFilePath = value;
                DrawImage();
                RaisePropertyChanged("CurrentImageFilePath");
            }
        }

        public double ResampleRate
        {
            get
            {
                return ImageObject.ResampleRate;
            }

            set
            {
                ImageObject.ResampleRate = value;
                // RaisePropertyChangedは使用しない
                // DrawImage関連の処理が同時に動くことが前提となる.
            }
        }



        int _scrollmargin_width;
        public int ScrollMarginWidth
        {
            get
            {
                return _scrollmargin_width;
            }
            set
            {
                _scrollmargin_width = value;
                RaisePropertyChanged("ScrollMarginWidth");
            }
        }

        public double ResizeRate
        {
            get
            {
                return ImageObject.ResizeRate;
            }
            set
            {
                ImageObject.ResizeRate = value;
                UpdateAllOrigin();
            }
        }

        public double ResizeScale
        {
            get
            {
                return ImageObject.ResizeScale;
            }
            set
            {
                ImageObject.ResizeScale = value;
                UpdateAllOrigin();
            }
        }


        public string ZoomValue
        {
            get
            {
                return (ResizeRate * ResampleRate).ToString();
            }

            set
            {
                double resize = 0.0;
                if ( double.TryParse(value, out resize))
                {
                    if (resize > 0)
                    {
                        ResizeRate = resize / ResampleRate;
                    }
                }

                UpdateScale();

                RaisePropertyChanged("ResizeRate");
                RaisePropertyChanged("ZoomValue");
            }
        }


        private ImageScalerViewModel _viewmodel_horizontalimagescaler;
        public ImageScalerViewModel ViewModelHorizontalImageScaler
        {
            get
            {
                return _viewmodel_horizontalimagescaler;
            }
            set
            {
                _viewmodel_horizontalimagescaler = value;
                RaisePropertyChanged("ViewModelHorizontalImageScaler");
            }
        }

        private ImageScalerViewModel _viewmodel_verticalimagescaler;
        public ImageScalerViewModel ViewModelVerticalImageScaler
        {
            get
            {
                return _viewmodel_verticalimagescaler;
            }
            set
            {
                _viewmodel_verticalimagescaler = value;
                RaisePropertyChanged("ViewModelHorizontalImageScaler");
            }
        }
        
        private ICommand _command_referimage;
        public ICommand CommandReferImage
        {
            get
            {

                return _command_referimage ?? (_command_referimage = new DelegateCommand<object>(SearchImageFiles));
            }
        }

        private ICommand _command_resizeimage;
        public ICommand CommandResizeImage
        {
            get
            {
                return _command_resizeimage ?? (_command_resizeimage = new DelegateCommand<object>(ResizeImage));
            }
        }

        private ICommand _command_setorigin;
        public ICommand CommandSetOrigin
        {
            get
            {
                return _command_setorigin ?? (_command_setorigin = new DelegateCommand<object>(SetOriginFromImage));
            }
        }

        private ICommand _command_setsecondorigin;
        public ICommand CommandSetSecondOrigin
        {
            get
            {
                return _command_setsecondorigin ?? (_command_setsecondorigin = new DelegateCommand<object>(SetSecondOriginFromImage));
            }
        }

        private ICommand _command_trimimage;
        public ICommand CommandTrimImage
        {
            get
            {
                return _command_trimimage ?? (_command_trimimage = new DelegateCommand<object>(TrimRoiImage));
            }
        }

        private ICommand _command_vieworigin;
        public ICommand CommandViewOrigin
        {
            get
            {
                return _command_vieworigin ?? (_command_vieworigin = new DelegateCommand<object>(ViewOrigin));
            }
        }

        private ICommand _command_mousemoveoriented;
        public ICommand MouseMoveOrientedCommand
        {
            get
            {
                return _command_mousemoveoriented ?? (_command_mousemoveoriented = new DelegateCommand<object>(MoveOriented, CheckMovingOriented));
            }
        }

        private ICommand _command_disablemousemoveoriented;
        public ICommand DisableMouseMoveOrientedCommand
        {
            get
            {
                return _command_disablemousemoveoriented ?? (_command_disablemousemoveoriented = new DelegateCommand<object>(DisableMovingOriented, CheckMovingOriented));
            }
        }

        private ImageViewer.ImageViewSource _image_object;       
        public ImageViewer.ImageViewSource ImageObject
        {
            get
            {
                return _image_object;
            }
            set
            {
                _image_object = value;
                RaisePropertyChanged("ImageFileListObject");
            }
        }

        public bool IsViewImageControl
        {
            get
            {
                if (ImageObject.DisplayedImage == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        public void SearchImageFiles(object param)
        {
            _model_workimage.GetDirFiles(param.ToString());

            DrawImage();

            RaisePropertyChanged("ImageFilePathList");
            RaisePropertyChanged("CurrentImageFilePath");
        }

        public void ResizeImage(object param)
        {
            string paramstring = param.ToString();

            if( paramstring.Equals("Up"))
            {
                if (ResizeRate >= 2)
                {
                    ResizeRate /= 2;
                    ImageObject.ResetPopupDisplay();

                    UpdateScale();
                }

            }
            else if(paramstring.Equals("Down"))
            {
                if( ResizeRate <= 8)
                {
                    ResizeRate *= 2;
                    ImageObject.ResetPopupDisplay();

                    UpdateScale();
                }
            }

            RaisePropertyChanged("ResizeRate");
            RaisePropertyChanged("ZoomValue");
        }

        public void ViewOrigin(object param)
        {

        }

        private double _pooled_orient_x;
        private double _pooled_orient_y;
        private double _pooled_secondorient_x;
        private double _pooled_secondorient_y;
        public void SetOriginFromImage(object param)
        {
            IsViewedOriented = true;
            IsEnableOriented = true;
            _pooled_orient_x = OrientActualX;
            _pooled_orient_y = OrientActualY;

        }

        public void SetSecondOriginFromImage(object param)
        {
            IsViewedSecondOriented = true;
            IsEnableSecondOriented = true;
            _pooled_secondorient_x = SecondOrientOffsetActualX;
            _pooled_secondorient_y = SecondOrientOffsetActualY;
        }


        public void TrimRoiImage(object param)
        {
            if( ImageObject == null || ImageObject.ViewableItems == null ) {
                return;
            }

            int index = 0;
            try
            {
                foreach (var viewableitem in ImageObject.ViewableItems)
                {
                    CroppedBitmap image = viewableitem.CroppedImage;

                    string path = param.ToString() + "\\" + _mastervm.CurrentRecipeName + "_" + index++ + ".bmp";

                    System.IO.FileStream mStream = new System.IO.FileStream(path, System.IO.FileMode.Create);
                    BitmapEncoder jEncoder = new BmpBitmapEncoder();
                    jEncoder.Frames.Add(BitmapFrame.Create(image));
                    jEncoder.Save(mStream);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error for saving bitmap.");
            }
        }

        private bool _isviewed_oriented = false;
        public bool IsViewedOriented
        {
            get
            {
                return _isviewed_oriented;
            }
            set
            {
                _isviewed_oriented = value;
                RaisePropertyChanged("IsViewedOriented");
            }
        }
        
        private bool _isviewed_secondoriented = false;
        public bool IsViewedSecondOriented
        {
            get
            {
                return _isviewed_secondoriented;
            }
            set
            {
                _isviewed_secondoriented = value;
                RaisePropertyChanged("IsViewedSecondOriented");
            }
        }

        private bool _isenable_oriented = false;
        public bool IsEnableOriented
        {
            get
            {
                return _isenable_oriented;
            }
            set
            {
                _isenable_oriented = value;
                RaisePropertyChanged("IsEnableOriented");
                RaisePropertyChanged("ScrollbarControl");
            }
        }

        private bool _isenable_secondoriented = false;
        public bool IsEnableSecondOriented
        {
            get
            {
                return _isenable_secondoriented;
            }
            set
            {
                _isenable_secondoriented = value;
                RaisePropertyChanged("IsEnableSecondOriented");
                RaisePropertyChanged("ScrollbarControl");
            }
        }

        public string OrientTextX
        {
            get
            {
                return string.Format("{0:f1}", OrientActualX);
            }
            set
            {
                double val = 0.0;
                if( double.TryParse(value, out val)) {
                    OrientActualX = val ;                  
                    UpdateAllOrigin();
                    ReflectCurrentRecipe();
                }
            }
        }

        public string OrientTextY
        {
            get
            {
                return string.Format("{0:f1}", OrientActualY);
            }
            set
            {
                double val = 0.0;
                if (double.TryParse(value, out val))
                {
                    OrientActualY = val ;
                    UpdateAllOrigin();
                    ReflectCurrentRecipe();
                }

            }
        }


        private double OrientActualX
        {
            get
            {
                return OrientX * ResampleRate * ImageObject.StepOptX;
            }

            set
            {
                OrientX = value / (ResampleRate * ImageObject.StepOptX );
            }
        }

        private double OrientActualY
        {
            get
            {
                return OrientY * ResampleRate * ImageObject.StepOptY;
            }

            set
            {
                OrientY = value / (ResampleRate * ImageObject.StepOptY);
            }
        }


        private double _orient_x;
        public double OrientX
        {
            get
            {
                if (_mastervm != null && _mastervm.CurrentDataSetList != null)
                {
                    return _mastervm.CurrentDataSetList.AllOffsetX / (ResampleRate * ImageObject.StepOptX);
                }

                return _orient_x ;
                //return OrientActualX;
            }
            set
            {
                //_orient_x = value * ResizeRate;
                if (_mastervm != null && _mastervm.CurrentDataSetList != null)
                {
                    _mastervm.CurrentDataSetList.AllOffsetX = value * (ResampleRate * ImageObject.StepOptX);
                }

                //OrientActualX = value * ResizeRate;
                RaisePropertyChanged("OrientX");
                RaisePropertyChanged("OrientRoundX");
                RaisePropertyChanged("OrientTextX");
            }
        }


        private double _orient_y;
        public double OrientY
        {
            get
            {
                if (_mastervm != null && _mastervm.CurrentDataSetList != null)
                {
                    return _mastervm.CurrentDataSetList.AllOffsetY / (ResampleRate * ImageObject.StepOptY);
                }

                return _orient_y ; 
                //return OrientActualY;
            }
            set
            {
                //_orient_y = value * ResizeRate;

                if (_mastervm != null && _mastervm.CurrentDataSetList != null)
                {
                    _mastervm.CurrentDataSetList.AllOffsetY = value * (ResampleRate * ImageObject.StepOptY);
                }

                //OrientActualY = value * ResizeRate;
                RaisePropertyChanged("OrientY");
                RaisePropertyChanged("OrientRoundY");
                RaisePropertyChanged("OrientTextY");
            }
        }

        public double SecondOrientTextPosY
        {
            get
            {
                var formattedText = new FormattedText("Y=", System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface("Meiryo UI"), OrientFontSize, new SolidColorBrush());

                int margin = 20;

                return SecondOrientMouseOffsetY - 2 * formattedText.Height - margin;
            }
        }

        private double _secondorient_base_x;
        public double SecondOrientBaseX
        {
            get
            {
                return _secondorient_base_x ;
            }
            set
            {
                _secondorient_base_x = value * ResizeRate;
                RaisePropertyChanged("SecondOrientActualX");
                RaisePropertyChanged("SecondOrientBaseX");
                RaisePropertyChanged("SecondOrientMouseOffsetX");
                RaisePropertyChanged("SecondOrientTextX");
                RaisePropertyChanged("SecondOrientRectX");
            }
        }


        public string SecondOrientActualX
        {
            get
            {
                return string.Format("{0:F1}", SecondOrientBaseActualX + SecondOrientOffsetActualX);
            }
        }

        public string SecondOrientActualY
        {
            get
            {
                return string.Format("{0:F1}", SecondOrientBaseActualY + SecondOrientOffsetActualY);
            }
        }

        public double SecondOrientBaseActualX
        {
            get
            {
                return _secondorient_base_x * (ResampleRate * ImageObject.StepOptX);
            }
            set
            {
                _secondorient_base_x = value / (ResampleRate * ImageObject.StepOptX);
                RaisePropertyChanged("SecondOrientBaseActualX");
                RaisePropertyChanged("SecondOrientBaseX");
                RaisePropertyChanged("SecondOrientMouseOffsetX");
                RaisePropertyChanged("SecondOrientTextX");
                RaisePropertyChanged("SecondOrientRectX");
            }
        }

        private double _secondorient_base_y;
        public double SecondOrientBaseY
        {
            get
            {
                return _secondorient_base_y ;
            }
            set
            {
                _secondorient_base_y = value * ResizeRate ;
                RaisePropertyChanged("SecondOrientBaseActualY");
                RaisePropertyChanged("SecondOrientBaseY");
                RaisePropertyChanged("SecondOrientMouseOffsetY");
                RaisePropertyChanged("SecondOrientRectY");
                RaisePropertyChanged("SecondOrientTextY");
                RaisePropertyChanged("SecondOrientTextPosY");
            }
        }


        public double SecondOrientBaseActualY
        {
            get
            {
                return _secondorient_base_y * (ResampleRate * ImageObject.StepOptY);
            }
            set
            {
                _secondorient_base_y = value / (ResampleRate * ImageObject.StepOptY);
                RaisePropertyChanged("SecondOrientBaseY");
                RaisePropertyChanged("SecondOrientMouseOffsetY");
                RaisePropertyChanged("SecondOrientRectY");
                RaisePropertyChanged("SecondOrientTextY");
                RaisePropertyChanged("SecondOrientTextPosY");
            }
        }


        public double SecondOrientOffsetActualX
        {
            get
            {
                return (_secondorient_mouseoffset_x - _secondorient_base_x) * (ResampleRate * ImageObject.StepOptX);
            }
            set
            {
                _secondorient_mouseoffset_x = (value) / (ResampleRate * ImageObject.StepOptX) + _secondorient_base_x;

                if (_mastervm.CurrentRecipeObject != null)
                {
                    var obj = _mastervm.CurrentRecipeObject.ModelObject;
                    obj.SetTransformParameterX(SecondOrientOffsetActualX);
                }

                RaisePropertyChanged("SecondOrientActualX");
                RaisePropertyChanged("SecondOrientBaseX");
                RaisePropertyChanged("SecondOrientMouseOffsetX");
                RaisePropertyChanged("SecondOrientTextX");
                RaisePropertyChanged("SecondOrientRectX");
                RaisePropertyChanged("SecondOrientMouseOffsetX");
            }
        }

        private double _secondorient_mouseoffset_x;
        public double SecondOrientMouseOffsetX
        {
            get
            {
                return _secondorient_mouseoffset_x;
            }
            set
            {
                _secondorient_mouseoffset_x = value * ResizeRate;

                if (_mastervm.CurrentRecipeObject != null)
                {
                    var obj = _mastervm.CurrentRecipeObject.ModelObject;
                    obj.SetTransformParameterX(SecondOrientOffsetActualX);
                }

                RaisePropertyChanged("SecondOrientActualX");
                RaisePropertyChanged("SecondOrientBaseX");
                RaisePropertyChanged("SecondOrientMouseOffsetX");
                RaisePropertyChanged("SecondOrientTextX");
                RaisePropertyChanged("SecondOrientRectX");
                RaisePropertyChanged("SecondOrientMouseOffsetX");
            }
        }

        public double SecondOrientOffsetActualY
        {
            get
            {
                return (_secondorient_mouseoffset_y - _secondorient_base_y) * (ResampleRate * ImageObject.StepOptY);
            }
            set
            {
                _secondorient_mouseoffset_y = (value) / (ResampleRate * ImageObject.StepOptY) + _secondorient_base_y;

                if (_mastervm.CurrentRecipeObject != null)
                {
                    var obj = _mastervm.CurrentRecipeObject.ModelObject;
                    obj.SetTransformParameterY(SecondOrientOffsetActualY);
                }

                RaisePropertyChanged("SecondOrientActualY");
                RaisePropertyChanged("SecondOrientBaseY");
                RaisePropertyChanged("SecondOrientMouseOffsetY");
                RaisePropertyChanged("SecondOrientRectY");
                RaisePropertyChanged("SecondOrientTextY");
                RaisePropertyChanged("SecondOrientTextPosY");
                RaisePropertyChanged("SecondOrientMouseOffsetY");
            }
        }

        private double _secondorient_mouseoffset_y;
        public double SecondOrientMouseOffsetY
        {
            get
            {
                return _secondorient_mouseoffset_y;
            }
            set
            {
                _secondorient_mouseoffset_y = value * ResizeRate;

                if (_mastervm.CurrentRecipeObject != null)
                {
                    var obj = _mastervm.CurrentRecipeObject.ModelObject;
                    obj.SetTransformParameterY(SecondOrientOffsetActualY);
                }

                RaisePropertyChanged("SecondOrientActualY");
                RaisePropertyChanged("SecondOrientBaseY");
                RaisePropertyChanged("SecondOrientMouseOffsetY");
                RaisePropertyChanged("SecondOrientRectY");
                RaisePropertyChanged("SecondOrientTextY");
                RaisePropertyChanged("SecondOrientTextPosY");
                RaisePropertyChanged("SecondOrientMouseOffsetY");
            }
        }
        
        public double OrientFontSize
        {
            get
            {
                return 24 * ResizeRate;
            }
        }

        public double OrientRoundRadius
        {
            get
            {
                return 20 * ResizeRate;
            }
        }

        public double OrientRoundX
        {
            get
            {
                return OrientX - OrientRoundRadius / (2 );
            }
        }

        public double OrientRoundY
        {
            get
            {
                return OrientY - OrientRoundRadius / (2 );
            }
        }

        public double SecondOrientRectX
        {
            get
            {
                return SecondOrientMouseOffsetX - OrientRoundRadius / 2;
            }
        }

        public double SecondOrientRectY
        {
            get
            {
                return SecondOrientMouseOffsetY - OrientRoundRadius / 2;
            }
        }

        public string SecondOrientTextX
        {
            get
            {
                return string.Format("{0:f1}", SecondOrientOffsetActualX);
            }
            set
            {
                double val = 0.0;
                if (double.TryParse(value, out val))
                {
                    SecondOrientOffsetActualX = val;
                    UpdateAllOrigin();
                    ReflectCurrentRecipe();
                }
            }
        }

        public string SecondOrientTextY
        {
            get
            {
                return string.Format("{0:f1}", SecondOrientOffsetActualY);
            }
            set
            {
                double val = 0.0;
                if (double.TryParse(value, out val))
                {
                    SecondOrientOffsetActualY = val;
                    UpdateAllOrigin();
                    ReflectCurrentRecipe();
                }
            }
        }

        public bool CheckMovingOriented(object param)
        {
            return IsEnableOriented || IsEnableSecondOriented;
        }

        

        public void DisableMovingOriented(object param)
        {
            bool isreset = false;
            if( param != null)
            {
                if (param.ToString().Equals("Reset"))
                {
                    isreset = true;
                }
            }


            if (IsEnableOriented && isreset)
            {
                OrientActualX = _pooled_orient_x;
                OrientActualY = _pooled_orient_y;

            }
            if (IsEnableSecondOriented && isreset)
            {
                SecondOrientOffsetActualX = _pooled_secondorient_x ;
                SecondOrientOffsetActualY = _pooled_secondorient_y;
            }

            IsEnableOriented = false;
            IsEnableSecondOriented = false;





            UpdateAllOrigin();
            ReflectCurrentRecipe();


#if false
            // 確定処理
            // Singleton取得
            var obj = Model.DataSet.RecipeHandler.ClipMeasure.AutoCoordinateCalculator.GetInstance();
            obj.SetPrintingOffset((float)OrientActualX, (float)OrientActualY);

            if (_mastervm.CurrentRecipeObject != null)
            {
                _mastervm.CurrentDataSetList.RunCoordinateCalculator();
                ReflectCurrentRecipe();
            }
#endif
        }



        public void MoveOriented(object param)
        {
            double[] position = (double[])param;

            if( IsEnableOriented)
            {
                OrientX = position[0] * ResizeRate;
                OrientY = position[1] * ResizeRate;
            }

            if(IsEnableSecondOriented)
            {
                SecondOrientMouseOffsetX = position[0];
                SecondOrientMouseOffsetY = position[1];
            }
        }

        /// <summary>
        /// ※現在は固定値で設定しているが、GUIを変更する場合にはこの値を変更する必要がある
        /// </summary>
        public static readonly int INTERNAL_SCALER_MARGIN = 20;

        private void UpdateScale()
        {
            if (ImageObject.DisplayedImage == null)
            {
                return;
            }

            ViewModelVerticalImageScaler.ScrollMargin = INTERNAL_SCALER_MARGIN;
            ViewModelHorizontalImageScaler.ScrollMargin = INTERNAL_SCALER_MARGIN;

            double mm_step_display = (25.4 / 96);       // 1pixelあたりのmmのサイズ(フルHD)
            double pixel_step = 10.0;

            int divwidth = (int)(ResampleRate * ImageObject.DisplayedImage.Width / (pixel_step / ImageObject.StepOptX)) ;
            int divheight = (int)(ResampleRate * ImageObject.DisplayedImage.Height / (pixel_step / ImageObject.StepOptY)) ;

            double imgpix_x_permm = (pixel_step / (ResampleRate * ImageObject.StepOptX));
            double imgpix_y_permm = (pixel_step / (ResampleRate * ImageObject.StepOptY));

            
            int marginx = (int)(ImageObject.DisplayedImage.Width % imgpix_x_permm + 0.5);
            int marginy = (int)(ImageObject.DisplayedImage.Height % imgpix_y_permm + 0.5);

            ViewModelHorizontalImageScaler.Length = (int)(ImageObject.DisplayedImage.Width);

            ViewModelHorizontalImageScaler.UpdateScale(0,
                            ResizeRate * pixel_step,
                            divwidth, marginx);

            ViewModelVerticalImageScaler.UpdateScale(0,
                            ResizeRate * pixel_step,
                            divheight, marginy);
        }

        public void DrawImage()
        {
            var view = _model_workimage.GetView(ImageObject.KeyViewObject);
            view.StorageObject.LoadImage(_model_workimage.CurrentFilePathAbsolute);
            view.ResampleViewImage(ResampleRate);
            ImageObject.DisplayedImage = view.HoldingViewImageSource;          

            UpdateScale();

            ImageObject.Invalidate();

            RaisePropertyChanged("IsViewImageControl");
        }

        public void SetCoordinatesFromViewableItem(ImageViewer.IViewableItem item)
        {
           Model.DataSet.RecipeHandler.Group.MeasureGroup currentobj = _mastervm.CurrentRecipeObject.ModelObject ;
           foreach (var obj in currentobj.RegionsGroupObject.RegionList)
           {
               if (obj == item.SourceRegion )
               {
                   if (!obj.IsReferenceRegion && obj.RoiGroupObject.SubOffset != null)
                   {
                       obj.RoiGroupObject.CoordinateCoordTuple.Value_Coord_LT_x += item.ViewPoint.X * (ResampleRate * ImageObject.StepOptX) - obj.RoiGroupObject.AbsoluteCoordTuple.Value_Coord_LT_x;
                       obj.RoiGroupObject.CoordinateCoordTuple.Value_Coord_LT_y += item.ViewPoint.Y * (ResampleRate * ImageObject.StepOptY) - obj.RoiGroupObject.AbsoluteCoordTuple.Value_Coord_LT_y;
                       _mastervm.UpdateProperty();
                   }
               }
           }

        }


        public void ReflectCurrentRecipe()
        {
            if (_mastervm == null || _mastervm.CurrentRecipeObject == null)
            {
                return;
            }

            // 確定処理
            // Singleton取得
            var singleobj = Model.DataSet.RecipeHandler.ClipMeasure.AutoCoordinateCalculator.GetInstance();
            //singleobj.SetPrintingOffset((float)OrientActualX, (float)OrientActualY);

            if (_mastervm.CurrentRecipeObject != null)
            {
                //OrientActualX = _mastervm.CurrentDataSetList.AllOffsetX ;
                //OrientActualY = _mastervm.CurrentDataSetList.AllOffsetY ;
                //_mastervm.CurrentDataSetList.AllOffsetX = OrientActualX;
                //_mastervm.CurrentDataSetList.AllOffsetY = OrientActualY;>	ClipXmlEditor.exe!ClipXmlReader.ViewModel.ImageViewerGuiViewModel.ReflectCurrentRecipe() Line 1030	C#

                _mastervm.CurrentDataSetList.RunCoordinateCalculator();
            }

            ImageObject.ViewableItems.Clear();
            Model.DataSet.RecipeHandler.Group.MeasureGroup currentobj = _mastervm.CurrentRecipeObject.ModelObject ;
            int idx = 1;
            foreach (var obj in currentobj.RegionsGroupObject.RegionList)
            {
                var absolute = obj.RoiGroupObject.AbsoluteCoordTuple;

                if (absolute != null)
                {
                    Func<Color, Brush> lambda = (x) =>
                    {
                        return new SolidColorBrush(Color.FromArgb(128, x.R, x.G, x.B));
                    };

                    ImageObject.ViewableItems.Add(
                        new ImageViewer.ImageViewRectangleSource(ImageObject, obj)
                        {
                            ViewPoint = new System.Windows.Point((absolute.Value_Coord_LT_x / (ResampleRate * ImageObject.StepOptX)), ((absolute.Value_Coord_LT_y / (ResampleRate * ImageObject.StepOptY)))),
                            Width = (absolute.Value_Coord_width / (ResampleRate * ImageObject.StepOptX)),
                            Height = (absolute.Value_Coord_height / (ResampleRate * ImageObject.StepOptY)),
                            Brush = lambda(obj.AppBrushColor),
                            Text = "Area" + idx++,
                            IsVisible = true
                        }
                    );

                }
            }



            var coordinfo = singleobj.GetCoordinateInfo((uint)currentobj.Index, (uint)currentobj.CoordinateGroupObject.CoordId);
            SecondOrientBaseActualX = coordinfo.CenterX;
            SecondOrientBaseActualY = coordinfo.CenterY;

            SecondOrientOffsetActualX = currentobj.SpecifiedParent.UserTransformParameter[currentobj.CoordinateGroupObject.CoordId].OffsetX;
            SecondOrientOffsetActualY = currentobj.SpecifiedParent.UserTransformParameter[currentobj.CoordinateGroupObject.CoordId].OffsetY;
        }


        public void UpdateAllOrigin()
        {
            RaisePropertyChanged("ResizeRate");
            RaisePropertyChanged("ResizeScale");
            RaisePropertyChanged("OrientX");
            RaisePropertyChanged("OrientY");
            RaisePropertyChanged("OrientRoundX");
            RaisePropertyChanged("OrientRoundY");
            RaisePropertyChanged("SecondOrientRectX");
            RaisePropertyChanged("SecondOrientRectY");
            RaisePropertyChanged("SecondOrientTextPosY");
            RaisePropertyChanged("OrientFontSize");
            RaisePropertyChanged("OrientRoundRadius");
            RaisePropertyChanged("OrientTextX");
            RaisePropertyChanged("OrientTextY");
            RaisePropertyChanged("SecondOrientTextX");
            RaisePropertyChanged("SecondOrientTextY");
            RaisePropertyChanged("ZoomValue");
        }


    }
}

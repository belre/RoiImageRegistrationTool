using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Shapes;
using System.Drawing;

using System.ComponentModel;
using System.Collections.ObjectModel;

using System.Windows.Input;

namespace ClipXmlReader.ViewModel.ImageViewer
{
    public interface IViewableItem
    {
        Model.DataSet.RecipeHandler.Group.RegionGroup SourceRegion
        {
            get;
        }

        System.Windows.Point ViewPoint
        {
            get;
        }

        System.Windows.Media.Imaging.CroppedBitmap CroppedImage
        {
            get;
        }

        bool IsVisible
        {
            get;
        }

        System.Windows.Shapes.Shape Shape
        {
            get;
        }

        System.Windows.Shapes.Shape StrokeShape
        {
            get;
        }

        Point StrokeShapeOffset
        {
            get;
        }

        System.Windows.Controls.TextBlock TextLabel
        {
            get;
        }

        System.Drawing.PointF CenterPoint
        {
            get;
        }

        System.Windows.VerticalAlignment VerticalAlignment
        {
            get;
        }

        System.Windows.HorizontalAlignment HorizontalAlignment
        {
            get;
        }

        double Angle
        {
            get;
        }

        ICommand RightClickCommand
        {
            get;
        }

        ICommand MousePushCommand
        {
            get;
        }

        ICommand MouseMoveCommand
        {
            get;
        }

        ICommand MouseReleaseCommand
        {
            get;
        }


        bool IsViewPopup
        {
            get;
            set;
        }

        bool IsMovable
        {
            get;
            set;
        }

        void MoveViewPoint(System.Windows.Point imagepoint);


        void Invalidate();
    }
}

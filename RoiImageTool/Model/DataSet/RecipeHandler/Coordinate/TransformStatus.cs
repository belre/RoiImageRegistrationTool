using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Coordinate
{
    public class TransformStatus
    {
        private double _offset_x;
        public double OffsetX
        {
            get
            {
                return _offset_x;
            }
            set
            {
                _offset_x = value;
            }
        }

        private double _offset_y;
        public double OffsetY
        {
            get
            {
                return _offset_y;
            }
            set
            {
                _offset_y = value;
            }
        }

        private double _declination_angle;
        public double DeclinationAngle
        {
            get
            {
                return _declination_angle;
            }
            set
            {
                _declination_angle = value;
            }
        }

    }
}

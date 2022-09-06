using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.DataSet.DataType
{
    

    public class BaseConstraints<T> : GenericDataType
    {

        public object[] Parameter
        {
            get;
            set;
        }

        protected object[] ExternalParameter
        {
            get;
            set;
        }

        
        public delegate T CalculateInConstraints(object[] param, object[] extparam);
        public CalculateInConstraints InitConstraints
        {
            get;
            set;
        }


        protected T _dependency_value;
        public T DependentValue
        {
            get
            {
                return _dependency_value;
            }
            protected set
            {
                _dependency_value = value;
            }
        }

        public override object Contents
        {
            get
            {
                return DependentValue;
            }
            protected set
            {
            }
        }

        public override Type ValueType
        {
            get
            {
                return DependentValue.GetType();
            }
            protected set
            {
            }
        }


        public BaseConstraints (  params object[] param)
            : this(null, param)
        {

        }

        public BaseConstraints(CalculateInConstraints constraints, params object[] param)
            : base(null, null)
        {
            Parameter = param;
            InitConstraints += constraints;
        }

        public override void InitializeContents(params object[] externalparam)
        {
            if (InitConstraints != null)
            {
                DependentValue = InitConstraints(Parameter, externalparam);
                ExternalParameter = externalparam;
            }
        }


        public object TestInitParameter(params object[] externalparam)
        {
            object dependent = null;
            if (InitConstraints != null)
            {
                dependent = InitConstraints(Parameter, externalparam);
            }

            return dependent;
        }
    }
}

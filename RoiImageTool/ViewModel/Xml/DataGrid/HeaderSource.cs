using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;


namespace ClipXmlReader.ViewModel.Xml.DataGrid
{
    public class HeaderSource : Base.BaseDataGridSource
    {
        public ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple.HeaderTuple ModelObject
        {
            get;
            set;
        }

        public string RecipeName
        {
            get
            {
                return ModelObject.GetParameter<string>(ModelObject.Key_Format);
            }
            set
            {
                ModelObject.SetParameter<string>(ModelObject.Key_Format, value);
                OnPropertyChanged("RecipeName");
            }
        }

        public string UpdateDate
        {
            get
            {
                return ModelObject.GetParameter<string>(ModelObject.Key_Update);
            }
            set
            {
                ModelObject.SetParameter<string>(ModelObject.Key_Update, value);
                OnPropertyChanged("UpdateDate");
            }
        }

        public HeaderSource(ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple.HeaderTuple _model_object)
        {
            ModelObject = _model_object;
        }
    }
}

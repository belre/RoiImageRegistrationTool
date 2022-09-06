using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;


using ClipXmlReader.Model.DataSet.RecipeHandler.Group;

namespace ClipXmlReader.ViewModel.Xml.DataGrid
{
    public class ItemTypeSource : INotifyPropertyChanged
    {

        protected int _internal_itemtype;
        public int ItemType
        {
            get
            {
                if (ModelObject == null)
                {
                    return _internal_itemtype;
                }
                else
                {
                    return ModelObject.ItemType;
                    //return ModelObject.GetParameter<MeasureGroup.EMeasBaseType>(ModelObject.Key_ItemType);
                }
            }
            set
            {
                // ここに直接処理が入る前に、
                // VMとしてエラー通知する処理が必要



                if (ModelObject == null)
                {
                    _internal_itemtype = value;
                }
                else
                {

                    // UpdatePropertyの更新回数によって処理遅延が発生しているので
                    // 更新回数を減らすために条件を付加する
                    if (ModelObject.ItemType != value)
                    {
                        ModelObject.ItemType = value;
                        ParentVM.UpdateProperty();
                    }
                    //ParentVM.UpdateProperty();
                    //ModelObject.SetParameter<MeasureGroup.EMeasBaseType>(ModelObject.Key_ItemType, value);
                }


                OnPropertyChanged("ItemType");
            }
        }

        protected Dictionary<int, string> MeasLabelMap
        {
            get
            {
                if (RelationsObject != null)
                {
                    return RelationsObject.Measures.MeasureDictLabel;
                }
                else
                {
                    var dict = Model.DataSet.RecipeHandler.Default.MeasureIDTable.GetMeasBaseTextMap();
                    Dictionary<int, string> newdict = new Dictionary<int, string>();

                    foreach (var obj in dict)
                    {
                        newdict[(int)obj.Key] = obj.Value;
                    }

                    return newdict;
                }
            }
        }


        public string ItemLabel
        {
            get
            {
                Dictionary<int, string> mapobj = MeasLabelMap;

                if( mapobj.Keys.Contains(ItemType))
                {
                    return mapobj[ItemType];
                }
                else
                {
                    return "** ERROR **";
                }
            }
        }

        protected MainWindowViewModel ParentVM
        {
            get;
            set;
        }


        protected ClipXmlReader.Model.DataSet.RecipeHandler.Group.MeasureGroup ModelObject
        {
            get;
            set;
        }


        protected ClipXmlReader.Model.DataSet.RecipeHandler.Relations.XmlRootSerialization RelationsObject
        {
            get
            {
                if( ModelObject == null)
                {
                    return _xml_template;
                }
                else
                {
                    return ModelObject.RelationsObject;
                }

            }
        }


        protected ClipXmlReader.Model.DataSet.RecipeHandler.Relations.XmlRootSerialization _xml_template;



#if false
        public ItemTypeSource()
            : this(null, null)
        {
            //ModelObject = model_object;
        }
#endif

        public ItemTypeSource(ClipXmlReader.Model.DataSet.RecipeHandler.Relations.XmlRootSerialization xml_template)
        {
            _xml_template = xml_template;

        }


        public ItemTypeSource(MainWindowViewModel mastervm, ClipXmlReader.Model.DataSet.RecipeHandler.Group.MeasureGroup _model_object)
        {
            ModelObject = _model_object;
            ParentVM = mastervm;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null) return;

            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}

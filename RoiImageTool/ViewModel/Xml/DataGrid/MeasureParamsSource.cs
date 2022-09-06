using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

using System.Collections.ObjectModel;

using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;


namespace ClipXmlReader.ViewModel.Xml.DataGrid
{
    public class MeasureParamsSource : Base.BaseDataGridSource
    {
        public string Name
        {
            get
            {
                return ModelObject.GetParameter<string>(ModelObject.Key_DisplayedName);
            }
#if false
            set
            {
                if(!ModelObject.GetParameter<bool>(ModelObject.Key_NameChangeProhibited))
                {
                    ModelObject.SetParameter<string>(ModelObject.Key_Name, value);                
                }

                OnPropertyChanged("Name");
            }
#endif
        }

        public string Value
        {
            get
            {
                return ModelObject.GetParameter(ModelObject.Key_Value).ToString();
            }
            set
            {
                ModelObject.Value_Value = value;

                OnPropertyChanged("Value");
                ParentVM.UpdateAll();
                ParentVM.UpdateImage();
            }
        }


        protected MainWindowViewModel ParentVM
        {
            get;
            set;
        }

        public ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple.NameValuePairTuple ModelObject
        {
            get;
            protected set;
        }

        public MeasureParamsSource(MainWindowViewModel mastervm, ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple.NameValuePairTuple model_object)
        {
            ParentVM = mastervm;
            ModelObject = model_object;

            BindRightClickMenu = new System.Collections.ObjectModel.ObservableCollection<RightClickMenu.RightClickMenuSource>();
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "上にパラメータを追加", RightClickEvent = this.CommandInsertPrevItem, IsEnabled = Environments.OperationStatus.IsDevelopmentMode });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "下にパラメータを追加", RightClickEvent = this.CommandInsertNextItem, IsEnabled = Environments.OperationStatus.IsDevelopmentMode });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { IsSeparator = true });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "パラメータ削除", RightClickEvent = this.CommandDeleteItem, IsEnabled = Environments.OperationStatus.IsDevelopmentMode ? !ModelObject.GetParameter<bool>(ModelObject.Key_NameChangeProhibited) : false });           
        }

        private ICommand _command_insert_prev_item;
        public ICommand CommandInsertPrevItem
        {
            get
            {
                return _command_insert_prev_item ?? (this._command_insert_prev_item = new DelegateCommand<object>(InsertPrevItem));
            }
        }


        private ICommand _command_insert_next_item;
        public ICommand CommandInsertNextItem
        {
            get
            {
                return _command_insert_next_item ?? (this._command_insert_next_item = new DelegateCommand<object>(InsertNextItem));
            }
        }

        private ICommand _command_delete_item;
        public ICommand CommandDeleteItem
        {
            get
            {
                return _command_delete_item ?? (this._command_delete_item = new DelegateCommand<object>(DeleteItem));
            }
        }


        public void InsertPrevItem(object param)
        {
            Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTuple, bool> func = ((measures, target) =>
            {
                measures.InsertTuple(target);
                return true;
            });


            ParentVM.RunDataGridCommand(this.ModelObject, func);

        }

        public void InsertNextItem(object param)
        {
            Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTuple, bool> func = ((measures, target) =>
            {
                measures.InsertTuple(target, true);
                return true;
            });

            ParentVM.RunDataGridCommand(this.ModelObject, func);
        }


        public void DeleteItem(object param)
        {
            Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTuple, bool> func = ((measures, target) =>
            {
                measures.DeleteTuple(target);
                return true;
            });

            ParentVM.RunDataGridCommand(this.ModelObject, func);
        }
    }
}

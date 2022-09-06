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
    public class CoordinateParamSource : Base.BaseDataGridSource
    {
        public string Name
        {
            get
            {
                return ModelObject.Value_DisplayedName;
            }
            /*
            set
            {
                if ( !ModelObject.Value_NameChangeProhibited)
                {
                    ModelObject.Value_Name = value;
                }
                OnPropertyChanged("Name");
            }
             * */
        }

        public string Value
        {
            get
            {
                return ModelObject.Value_Value.ToString();
            }
            set
            {
                ModelObject.Value_Value = value;
                OnPropertyChanged("Value");
                ParentVM.UpdateAll();
                ParentVM.UpdateImage();
            }
        }



        public ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple.NameValuePairTuple ModelObject
        {
            get;
            set;
        }

        protected MainWindowViewModel ParentVM
        {
            get;
            set;
        }

        public CoordinateParamSource(MainWindowViewModel mastervm, ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple.NameValuePairTuple _model_object)
        {
            ParentVM = mastervm;
            ModelObject = _model_object;

            /*
            BindRightClickMenu = new System.Collections.ObjectModel.ObservableCollection<RightClickMenu.RightClickMenuSource>();
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "上にパラメータを追加", RightClickEvent = this.CommandInsertPrevItem, IsEnabled = true });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "下にパラメータを追加", RightClickEvent = this.CommandInsertNextItem, IsEnabled = true });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { IsSeparator = true });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "パラメータ削除", RightClickEvent = this.CommandDeleteItem, IsEnabled = true ? !ModelObject.GetParameter<bool>(ModelObject.Key_NameChangeProhibited) : false });           
            */
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

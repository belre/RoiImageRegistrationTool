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
    public class ResultSource : Base.BaseDataGridSource
    {
        public string Name
        {
            get
            {
                return ModelObject.GetParameter<string>(ModelObject.Key_Name);
            }
            set
            {
                if (!ModelObject.GetParameter<bool>(ModelObject.Key_NameChangeProhibited))
                {
                    ModelObject.SetParameter<string>(ModelObject.Key_Name, value);
                }

                OnPropertyChanged("Name");
            }
        }

        public string Unit
        {
            get
            {
                return ModelObject.GetParameter<string>(ModelObject.Key_Unit);
            }
            set
            {
                ModelObject.SetParameter<string>(ModelObject.Key_Unit, value);

                OnPropertyChanged("Unit");
            }
        }

        public int ValidFig
        {
            get
            {
                return ModelObject.GetParameter<int>(ModelObject.Key_ValidFig);
            }
            set
            {
                ModelObject.SetParameter<int>(ModelObject.Key_ValidFig, value);
            }
        }


        public decimal UpperBounds
        {
            get
            {
                return ModelObject.GetParameter<decimal>(ModelObject.Key_Upper);
            }
            set
            {
                ModelObject.SetParameter<decimal>(ModelObject.Key_Upper, value);
            }
        }

        public decimal LowerBounds
        {
            get
            {
                return ModelObject.GetParameter<decimal>(ModelObject.Key_Lower);
            }
            set
            {
                ModelObject.SetParameter<decimal>(ModelObject.Key_Lower, value);
            }
        }



        public int UseFlag
        {
            get
            {
                int flag = ModelObject.GetParameter<int>(ModelObject.Key_UseFlag);
                return flag ;
            }
            set
            {
                ModelObject.SetParameter<int>(ModelObject.Key_UseFlag, value);
            }
        }

        public ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple.ResultTuple ModelObject
        {
            get;
            set;
        }



        protected MainWindowViewModel ParentVM
        {
            get;
            set;
        }

        public ResultSource(MainWindowViewModel mastervm, ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple.ResultTuple model_object)
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

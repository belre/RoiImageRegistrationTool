using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

using System.Collections.ObjectModel;

using System.Windows;

using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace ClipXmlReader.ViewModel.Xml.DataGrid
{
    public class RecipeItemSource : Base.BaseDataGridSource
    {
        public int Index
        {
            get
            {
                return ModelObject.Index;
            }
        }

        protected ObservableCollection<Xml.DataGrid.ItemTypeSource> _binding_recipeitem_item;
        public ObservableCollection<Xml.DataGrid.ItemTypeSource> BindMeasItemList
        {
            get
            {
                if (_binding_recipeitem_item != null)
                {
                    return _binding_recipeitem_item;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                _binding_recipeitem_item = value;
                OnPropertyChanged("BindMeasItemList");
            }
        }

        protected ItemTypeSource _item_object;
        public ItemTypeSource ItemObject
        {
            get
            {

                return _item_object;
            }
            set
            {
                _item_object = value;
            }
        }

        public string Name
        {
            get
            {
                return ModelObject.GetParameter<string>(ModelObject.Key_Name);
            }
            set
            {
                ModelObject.SetParameter<string>(ModelObject.Key_Name, value);               
            }
        }

        public override void UpdateProperty()
        {
            OnPropertyChanged("Name");
            OnPropertyChanged("WarningColorInList");
        }

        public string ErrorMessage
        {
            get
            {
                return Model.DataSet.RecipeHandler.Error.RecipeErrorHandle.GetErrorMessage(ModelObject.ErrorStatus);
            }
        }

        public bool IsNeedsErrorHandling
        {
            get
            {
                if(ModelObject.ErrorStatus == Model.DataSet.RecipeHandler.Error.RecipeErrorHandle.ERecipeError.OK ||
                    ModelObject.ErrorStatus == Model.DataSet.RecipeHandler.Error.RecipeErrorHandle.ERecipeError.NOTRUN)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public Visibility ErrorVisibility
        {
            get
            {
                if( IsNeedsErrorHandling)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public System.Windows.Media.Brush WarningColorInList
        {
            get
            {               
                if(  ModelObject.ErrorStatus == Model.DataSet.RecipeHandler.Error.RecipeErrorHandle.ERecipeError.NOTRUN)
                {
                    return System.Windows.Media.Brushes.LightGray;
                }
                else if ( IsNeedsErrorHandling )
                {
                    return System.Windows.Media.Brushes.Red;
                }
                else
                {
                    return System.Windows.Media.Brushes.LightGreen;
                }
            }
        }


        protected MainWindowViewModel ParentVM
        {
            get;
            set;
        }

        public ClipXmlReader.Model.DataSet.RecipeHandler.Group.MeasureGroup ModelObject
        {
            get;
            protected set;
        }

        public RecipeItemSource(MainWindowViewModel mastervm, ClipXmlReader.Model.DataSet.RecipeHandler.Group.MeasureGroup _model_object)
        {
            ParentVM = mastervm;
            ModelObject = _model_object;

            ItemObject = new ItemTypeSource(ParentVM, _model_object);
            ItemObject.ItemType = _model_object.ItemType;
            //ItemObject.ItemType = _model_object.GetParameter<int>(_model_object.Key_ItemType);
            Name = _model_object.GetParameter<string>(_model_object.Key_Name);

            BindRightClickMenu = new System.Collections.ObjectModel.ObservableCollection<RightClickMenu.RightClickMenuSource>();
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "上に項目を追加", RightClickEvent = this.CommandInsertPrevItem, IsEnabled = true });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "下に項目を追加", RightClickEvent = this.CommandInsertNextItem, IsEnabled = true });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { IsSeparator = true });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "項目削除", RightClickEvent = this.CommandDeleteItem, IsEnabled = true });           

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


        protected void InsertPrevItem(object param)
        {
            Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTreeGroup, bool> func = ( (measures, target) =>
                {
                    measures.InsertGroup(target);
                    return true;
                });


            ParentVM.RunDataGridCommand(this.ModelObject, func);

        }

        protected void InsertNextItem(object param)
        {
            Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTreeGroup, bool> func = ((measures, target) =>
            {
                measures.InsertGroup(target, true);
                return true;
            });

            ParentVM.RunDataGridCommand(this.ModelObject, func);
        }


        protected void DeleteItem(object param)
        {
            Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTreeGroup, bool> func = ((measures, target) =>
            {
                measures.DeleteGroup(target);
                return true;
            });

            ParentVM.RunDataGridCommand(this.ModelObject, func);
        }

    }
}

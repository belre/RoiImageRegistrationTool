using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;

using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;



namespace ClipXmlReader.ViewModel.Xml.TreeView
{
    public class FilterTreeSource : Base.BaseTreeSource
    {
        public override string Text
        {
            get
            {
                var map_filtertext = FilterMap ;
                return map_filtertext[ModelObject.FilterType];
            }
            set { }
        }

        protected override bool IsNode
        {
            get
            {
                return false;
            }
        }


        protected MainWindowViewModel ParentVM
        {
            get;
            set;
        }

        protected Model.DataSet.RecipeHandler.Group.FilterGroup ModelObject
        {
            get;
            set;
        }

        public override object Value
        {
            get
            {
                return ModelObject.GetParameter<int>(ModelObject.Key_ItemType);
            }
            set
            {
                var item = (int)value;

                ModelObject.FilterType = item;

                OnPropertyChanged("Text");
                OnPropertyChanged("Value");

                ParentVM.UpdateAll();
                ParentVM.UpdateImage();
            }
        }

        protected bool _status_arranging = true;
        public override bool IsVisibleTextBoxNormalKey
        {
            get
            {
                return _status_arranging;
            }

            set
            {
                _status_arranging = value;
                OnPropertyChanged("IsVisibleTextBoxNormalKey");
                OnPropertyChanged("IsVisibleComboBoxValue");
            }
        }

        public override bool IsVisibleComboBoxValue
        {
            get
            {
                return !IsVisibleTextBoxNormalKey;
            }

            set
            {
                OnPropertyChanged("IsVisibleTextBoxNormalKey");
                OnPropertyChanged("IsVisibleComboBoxValue");
            }
        }



        public override Base.BaseTreeSource.ERegionTreeUIType UIType
        {
            get
            {
                return ERegionTreeUIType.ComboBox;
            }
        }

        protected Dictionary<int, string> FilterMap
        {
            get
            {
                if (ModelObject.RelationsObject != null)
                {
                    return ModelObject.RelationsObject.Filters.FilterDictLabel;
                }
                else
                {
                    var dict = Model.DataSet.RecipeHandler.Default.FilterIDTable.GetFilterTextMap();
                    Dictionary<int, string> newdict = new Dictionary<int, string>();

                    foreach (var obj in dict)
                    {
                        newdict[(int)obj.Key] = obj.Value;
                    }

                    return newdict;
                }
            }
        }

        protected ObservableCollection<TreeParameterSource> ListFilter
        {
            get
            {
                var list = new System.Collections.ObjectModel.ObservableCollection<TreeParameterSource>();
                foreach (var coloritem in FilterMap)
                {
                    list.Add(new TreeParameterSource() { ParameterLabel = coloritem.Value, BindingParameterValue = coloritem.Key });
                }

                return list;
            }
        }

        public override ObservableCollection<TreeParameterSource> Sources
        {
            get
            {
                var type = ModelObject.FilterType.GetType(); //ModelObject.GetParameter<object>(ModelObject.Key_ItemType).GetType();
                if (type == typeof(int))
                {
                    return ListFilter;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _sources = value;
                OnPropertyChanged("Sources");
            }
        }

        public FilterTreeSource(MainWindowViewModel mastervm, Model.DataSet.RecipeHandler.Group.FilterGroup model_object)
        {
            ParentVM = mastervm;
            ModelObject = model_object;

            InitViewModel();

            BindRightClickMenu = new System.Collections.ObjectModel.ObservableCollection<RightClickMenu.RightClickMenuSource>();
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "上にフィルタを追加", RightClickEvent = this.CommandInsertPrevParams, IsEnabled = true });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "下にフィルタを追加", RightClickEvent = this.CommandInsertNextParams, IsEnabled = true });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { IsSeparator = true });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "フィルタタイプ変更", RightClickEvent = this.CommandChangeParameterName, IsEnabled = true });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "フィルタ削除", RightClickEvent = this.CommandDeleteParams, IsEnabled = true });           
        }

        protected void InitViewModel()
        {

            {
                InitNode();
                foreach (var tuple in ModelObject.FilterParamsGroupObject.FilterParamsList)
                {
                    AddNode(new TreeView.FilterParamsNodeSource(ParentVM, tuple));
                }
            }
        }

        private ICommand _command_insert_prev_filter;
        public ICommand CommandInsertPrevParams
        {
            get
            {
                return _command_insert_prev_filter ?? (this._command_insert_prev_filter = new DelegateCommand<object>(InsertPrevFilter));
            }
        }


        private ICommand _command_insert_next_filter;
        public ICommand CommandInsertNextParams
        {
            get
            {
                return _command_insert_next_filter ?? (this._command_insert_next_filter = new DelegateCommand<object>(InsertNextFilter));
            }
        }

        private ICommand _command_change_filter;
        public ICommand CommandChangeParameterName
        {
            get
            {
                return _command_change_filter ?? (this._command_change_filter = new DelegateCommand<object>(ChangeFilter));
            }
        }

        private ICommand _command_delete_filters;
        public ICommand CommandDeleteParams
        {
            get
            {
                return _command_delete_filters ?? (this._command_delete_filters = new DelegateCommand<object>(DeleteFilter));
            }
        }

        protected void InsertPrevFilter(object param)
        {
            Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTreeGroup, bool> func = ((measures, target) =>
            {
                measures.InsertGroup(target);
                return true;
            });


            ParentVM.RunDataGridCommand(this.ModelObject, func);
        }

        protected void InsertNextFilter(object param)
        {
            Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTreeGroup, bool> func = ((measures, target) =>
            {
                measures.InsertGroup(target, true);
                return true;
            });

            ParentVM.RunDataGridCommand(this.ModelObject, func);
        }

        protected void ChangeFilter(object param)
        {

            IsVisibleTextBoxValue = true;
            IsVisibleTextBoxNormalKey = false;

            //ParentVM.UpdateProperty();
        }

        protected void DeleteFilter(object param)
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

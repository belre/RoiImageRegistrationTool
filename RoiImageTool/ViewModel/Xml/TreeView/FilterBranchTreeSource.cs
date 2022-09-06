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
    public class FilterBranchTreeSource : Base.BaseTreeSource
    {
        public override string Text
        {
            get
            {
                return "フィルタ";
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


        public override ObservableCollection<RightClickMenu.RightClickMenuSource> BindRightClickMenu
        {
            get
            {
                bool ishasfilter = true;
                if( ModelObject == null || ModelObject.FilterList.Count == 0)
                {
                    ishasfilter = false;
                }

                
                var _rightclickmenu = new System.Collections.ObjectModel.ObservableCollection<RightClickMenu.RightClickMenuSource>();
                _rightclickmenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "フィルタ新規追加", RightClickEvent= CommandCreateNewFilter, IsEnabled = !ishasfilter} );

                return _rightclickmenu;
            }
        }

        protected MainWindowViewModel ParentVM
        {
            get;
            set;
        }

        protected Model.DataSet.RecipeHandler.Group.FiltersGroup ModelObject
        {
            get;
            set;
        }




        public FilterBranchTreeSource(MainWindowViewModel mastervm, Model.DataSet.RecipeHandler.Group.FiltersGroup model_object)
        {
            ParentVM = mastervm;
            ModelObject = model_object;

            Children = new System.Collections.ObjectModel.ObservableCollection<Base.BaseTreeSource>();
            foreach (var tuple in ModelObject.FilterList)
            {
                Children.Add(new TreeView.FilterTreeSource(ParentVM, tuple));
            }
        }

        private ICommand _command_create_newfilter;
        public ICommand CommandCreateNewFilter
        {
            get
            {
                return _command_create_newfilter ?? (this._command_create_newfilter = new DelegateCommand<object>(CreateNewFilter));
            }
        }

        protected void CreateNewFilter(object param)
        {
            Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTreeGroup, bool> func = ((measures, target) =>
            {
                target.InsertGroup(target);
                return true;
            });

            ParentVM.RunDataGridCommand(this.ModelObject, func);
        }

    }
}

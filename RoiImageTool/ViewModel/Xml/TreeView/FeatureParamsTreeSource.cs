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
    public class FeatureParamsTreeSource : Base.BaseTreeSource
    {

        public override string Text
        {
            get
            {
                return "パラメータ";
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


        public override ObservableCollection<RightClickMenu.RightClickMenuSource> BindRightClickMenu
        {
            get
            {
                bool ishasfilter = true;
                if (ModelObject == null || ModelObject.ParamsList.Count == 0)
                {
                    ishasfilter = false;
                }


                var _rightclickmenu = new System.Collections.ObjectModel.ObservableCollection<RightClickMenu.RightClickMenuSource>();
                _rightclickmenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "パラメータ新規追加", RightClickEvent = CommandCreateNewParams, IsEnabled = Environments.OperationStatus.IsDevelopmentMode ? !ishasfilter : false });

                return _rightclickmenu;
            }
        }


        protected Model.DataSet.RecipeHandler.Group.FeatureParamsGroup ModelObject
        {
            get;
            set;
        }

        public FeatureParamsTreeSource(MainWindowViewModel mastervm, Model.DataSet.RecipeHandler.Group.FeatureParamsGroup model_object)
        {
            ParentVM = mastervm;
            ModelObject = model_object;
            InitModelObject();

        }

        
        protected void InitModelObject()
        {
            {
                InitNode();
                foreach (var tuple in ModelObject.ParamsList)
                {
                    AddNode(new TreeView.FeatureParamsNodeSource(ParentVM, tuple));
                }
            }
        }

        private ICommand _command_create_newparams;
        public ICommand CommandCreateNewParams
        {
            get
            {
                return _command_create_newparams ?? (this._command_create_newparams = new DelegateCommand<object>(CreateNewParams));
            }
        }


        protected void CreateNewParams(object param)
        {
            Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTreeGroup, bool> func = ((measures, target) =>
            {
                measures.InsertGroup(target);
                return true;
            });


            ParentVM.RunDataGridCommand(this.ModelObject, func);
        }
    }
}

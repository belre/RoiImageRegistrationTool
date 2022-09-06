
using System.Windows;

using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using System.ComponentModel;


using System.Runtime.Serialization;


namespace ClipXmlReader.ViewModel
{
    public enum EMenuPanelStatus
    {
        MENUPANEL_RIBBON,
        MENUPANEL_SIMPLEMENU
    }


    public enum ERibbonItemStatus
    {
        RIBBONITEM_RECIPE,
        RIBBONITEM_WORKSPACE
    }

    // https://qiita.com/hotelmoskva_/items/13ecc724bdad00078c16
    public class MainWindowViewModel : BindableBase
    {
        private Model.Control.MainWindow.MenuControl _model_menucontrol;
        private Model.IO.Recipes.WorkspaceManager _model_workspace;
        private Model.IO.Recipes.RecipeTemplateManager _model_recipetemplate;


        private ICommand _commandtest_usercontrol;
        public ICommand CommandTestUserControl
        {
            get
            {
                return _commandtest_usercontrol ?? (_commandtest_usercontrol = new DelegateCommand<object>(TestCommand));
            }
        }

        public int index = 0;

        public void TestCommand(object param)
        {
            //ContextImageViewer.TestText = "Wow!!!! : " + (index++);
        }

#if false
        private DataContext.ImageViewerGuiParameter _context_imageviewer;
        public DataContext.ImageViewerGuiParameter ContextImageViewer
        {
            get
            {
                return _context_imageviewer;
            }

            set
            {
                _context_imageviewer = value;
                RaisePropertyChanged("ContextImageViewer");
            }
        }
#endif

        private ImageViewerGuiViewModel _viewmodel_imageviewergui;
        public ImageViewerGuiViewModel ViewModelImageViewerGui
        {
            get
            {
                return _viewmodel_imageviewergui;
            }
            set
            {
                _viewmodel_imageviewergui = value;
                RaisePropertyChanged("ViewModelImageViewerGui");
            }
        }



        #region [Constructor]
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            _model_menucontrol = new Model.Control.MainWindow.MenuControl();

            if ((bool)(System.ComponentModel.DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue))
            {
                _model_menucontrol.ActivateAll();
            }
            else
            {
                _model_menucontrol.Activate(_model_menucontrol.Ribbon_RecipePanel);
            }

            BindMeasItemList = new ObservableCollection<Xml.DataGrid.ItemTypeSource>();
         
            /// FILE Load
            /// 
            _model_recipetemplate = new Model.IO.Recipes.RecipeTemplateManager();
            _model_recipetemplate.LoadTemplate();

            _model_workspace = new Model.IO.Recipes.WorkspaceManager();
            _model_workspace.XmlTemplate = _model_recipetemplate.XmlTemplate;


            foreach (var mapobj in _model_recipetemplate.XmlTemplate.Measures.MeasureDict)
            {
                BindMeasItemList.Add(new Xml.DataGrid.ItemTypeSource(_model_recipetemplate.XmlTemplate)
                {
                    ItemType = (int)mapobj.Key
                });
            }


            // タイトルの取得
            _title_foruse = _model_recipetemplate.XmlTemplate.Machines.MeasureDict[int.Parse(Properties.Resources.MachineID)].Name;

            ViewModelImageViewerGui = new ImageViewerGuiViewModel(this, _model_workspace);
            //ContextImageViewer = new DataContext.ImageViewerGuiParameter();
        }
        #endregion

        #region [Init]
        protected string _title_foruse = "";
        public string DialogTitle
        {
            get
            {
                return "Clip XML Editor (" + _title_foruse + ")";
            }
        }
        #endregion

        #region [ (GUI) Ribbon, Menu]

        #region [Property]
        public bool IsVisibleUserRibbonMenu
        {
            get
            {
                return _model_menucontrol.RibbonPanel.IsVisiblePanel;
            }
        }
        
        public bool IsVisibleSimpleMenu
        {
            get
            {
                return _model_menucontrol.SimpleMenuPanel.IsVisiblePanel;
            }
        }


        public bool IsVisibleWorkspace
        {
            get
            {
                return _model_menucontrol.Ribbon_WorkspacePanel.IsVisiblePanel;
            }
        }

        #endregion

        #region [Command]

        private ICommand _command_switchmenupanel;
        public ICommand CommandSwitchMenuPanel
        {
            get
            {
                return this._command_switchmenupanel ?? (this._command_switchmenupanel = new DelegateCommand<object>(SwitchMenuPanel));
            }
        }


        private ICommand _command_selectribbonitem;
        public ICommand CommandSelectRibbonItem
        {
            get
            {
                return this._command_selectribbonitem ?? (this._command_selectribbonitem = new DelegateCommand<object>(SelectRibbonItem));
            }
        }

        #endregion

        #region [Notify]

        #endregion

        #region [Command Method]
        private void SwitchMenuPanel(object param)
        {
            var status = (EMenuPanelStatus)param;

            if (status == EMenuPanelStatus.MENUPANEL_SIMPLEMENU)
            {
                _model_menucontrol.Activate(_model_menucontrol.SimpleMenuPanel);
            }
            else if (status == EMenuPanelStatus.MENUPANEL_RIBBON)
            {
                _model_menucontrol.Activate(_model_menucontrol.RibbonPanel);
            }

            RaisePropertyChanged("IsVisibleUserRibbonMenu");
            RaisePropertyChanged("IsVisibleSimpleMenu");
            RaisePropertyChanged("IsVisibleWorkspace");
        }

        private void SelectRibbonItem(object param)
        {
            var status = (ERibbonItemStatus)param;

            if (status == ERibbonItemStatus.RIBBONITEM_RECIPE)
            {
                _model_menucontrol.Activate(_model_menucontrol.Ribbon_RecipePanel);
            }
            else if (status == ERibbonItemStatus.RIBBONITEM_WORKSPACE)
            {
                _model_menucontrol.Activate(_model_menucontrol.Ribbon_WorkspacePanel);
            }


            RaisePropertyChanged("IsVisibleWorkspace");
        }

        #endregion

        #endregion

        #region [ (GUI) Data IO]
        #region [Property]

        public string CurrentWorkspace
        {
            get
            {
                return _model_workspace.WorkspaceDirectory;
            }
        }

        public string[] CurrentDirFiles
        {
            get
            {
               return _model_workspace.DirFiles;
            }
        }

        private string _currentdir;
        public string CurrentDir
        {
            get
            {
                return _currentdir;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                _currentdir = value;

                IsAbsoluteChecking = false;


                ClearList();

                RaisePropertyChanged("CurrentDir");
                RaisePropertyChanged("ErrorText");
                RaisePropertyChanged("IsVisibleErrorText");
                RaisePropertyChanged("IsVisibleAppErrorText");
                RaisePropertyChanged("IsVisibleRecipeItems");
                RaisePropertyChanged("IsVisibleRegions");
                RaisePropertyChanged("Header");
                RaisePropertyChanged("RecipeName");
                RaisePropertyChanged("BindRecipeItem");               
                RaisePropertyChanged("BindRegionTreeRoot");

                UpdateCoordinate(null);
                _viewmodel_imageviewergui.UpdateAllOrigin();
            }
        }


        #endregion

        #region [ Command ]

        private ICommand _command_switchworkspace;
        public ICommand CommandSwitchWorkspace
        {
            get
            {
                return this._command_switchworkspace ?? (this._command_switchworkspace = new DelegateCommand<object>(SwitchWorkspace));
            }
        }

        private ICommand _command_selectotherrecipe;
        public ICommand CommandSelectOtherRecipe
        {
            get
            {
                return this._command_selectotherrecipe ?? (this._command_selectotherrecipe = new DelegateCommand<object>(SelectOtherRecipe));
            }
        }

        private ICommand _command_savecurrenttable;
        public ICommand CommandSaveCurrentTable
        {
            get
            {
                return this._command_savecurrenttable ?? (this._command_savecurrenttable = new DelegateCommand<object>(SaveCurrentTable));
            }
        }

        private ICommand _command_saveascurrenttable;
        public ICommand CommandSaveAsCurrentTable
        {
            get
            {
                return this._command_saveascurrenttable ?? (this._command_saveascurrenttable = new DelegateCommand<object>(SaveAsCurrentTable));
            }
        }

        private ICommand _command_createtable;
        public ICommand CommandCreateTable
        {
            get
            {
                return this._command_createtable ?? (this._command_createtable = new DelegateCommand<object>(CreateTable));
            }
        }

        private ICommand _command_switchtablepanel;
        public ICommand CommandSwitchTablePanel
        {
            get
            {
                return this._command_switchtablepanel ?? (this._command_switchtablepanel = new DelegateCommand<object>(SwitchTablePanel));
            }
        }

        private ICommand _command_loadoriginlists;
        public ICommand CommandLoadOriginLists
        {
            get
            {
                return this._command_loadoriginlists ?? (this._command_loadoriginlists = new DelegateCommand<object>(LoadOriginLists));
            }
        }

        private ICommand _command_saveoriginlists;
        public ICommand CommandSaveOriginLists
        {
            get
            {
                return this._command_saveoriginlists ?? (this._command_saveoriginlists = new DelegateCommand<object>(SaveOriginLists));
            }
        }


        #endregion

        #region [ Command Method]

        public void SwitchWorkspace(object param)
        {
            SetFileInCurrentDirFiles(param.ToString(), true);

            IsAbsoluteChecking = false;

            RaisePropertyChanged("CurrentWorkspace");
            RaisePropertyChanged("CurrentDir");
            RaisePropertyChanged("CurrentDirFiles");

            RaisePropertyChanged("Header");
            RaisePropertyChanged("RecipeName");
            RaisePropertyChanged("BindRecipeItem");

            UpdateCoordinate(null);
        }

        public void ClearList()
        {
            if (BindCoordinateList != null && BindParameterObject != null && BindResultsObject != null && BindRegionTreeRoot != null)
            {
                BindRegionTreeRoot.Clear();
            }
            
            CurrentRecipeObject = null;

        }

        public void SelectOtherRecipe(object param)
        {
            RaisePropertyChanged("CurrentDir");
            RaisePropertyChanged("Header");
            RaisePropertyChanged("RecipeName");
            RaisePropertyChanged("BindRecipeItem");
        }


        public void SaveCurrentTable(object param)
        {
            string filepath = param.ToString();

            _model_workspace.SaveNewFile(filepath);
        }

        public void SaveAsCurrentTable(object param)
        {
            string filepath = param.ToString();
            _model_workspace.SaveAsNewFile(CurrentDir, filepath);

            SetFileInCurrentDirFiles(param.ToString(), false);
        }


        public void CreateTable(object param)
        {
            if( param.GetType() == BindCoordinateList.GetType())
            {
                var tarobj = CurrentRecipeObject.ModelObject.CoordinateGroupObject;
                tarobj.CoordinateList.Add(new Model.DataSet.RecipeHandler.UserTuple.NameValuePairTuple(tarobj));

                RaisePropertyChanged("BindCoordinateList");
                RaisePropertyChanged("IsEmptyBindCoordinate");
            }
            else if (param.GetType() == BindParameterObject.GetType())
            {
                var tarobj = CurrentRecipeObject.ModelObject.MeasureParamsGroupObject;
                tarobj.FreeMeasureParamsList.Add(new Model.DataSet.RecipeHandler.UserTuple.NameValuePairTuple(tarobj));

                RaisePropertyChanged("BindParameterObject");
                RaisePropertyChanged("IsEmptyBindParameter");
            }

            else if (param.GetType() == BindResultsObject.GetType())
            {
                var tarobj = CurrentRecipeObject.ModelObject.ResultsGroupObject;

                tarobj.FreeResultObject.Add(new Model.DataSet.RecipeHandler.UserTuple.ResultTuple(tarobj, false));

                RaisePropertyChanged("BindResultsObject");
                RaisePropertyChanged("IsEmptyBindResults");
            }
        }

        public void SwitchTablePanel(object param)
        {
            if( param.Equals("RecipeItems"))
            {
                _isvisibleuser_recipeitems = !_isvisibleuser_recipeitems;
                RaisePropertyChanged("IsVisibleRecipeItems");
                RaisePropertyChanged("IsHiddenUserRecipeItems");
                RaisePropertyChanged("CurrentRecipeIndex");
                RaisePropertyChanged("CurrentRecipeItem");
                RaisePropertyChanged("CurrentRecipeName");

            }
            else if( param.Equals("Regions"))
            {
                _isvisibleuser_regions = !_isvisibleuser_regions;
                RaisePropertyChanged("IsVisibleRegions");
                RaisePropertyChanged("IsHiddenUserRegions");
            }
        }

        public void LoadOriginLists(object param)
        {
            if(CurrentDataSetList == null)
            {
                return;
            }

            string filepath = param.ToString();
            CurrentDataSetList.LoadTransformParameter(filepath);

            _viewmodel_imageviewergui.UpdateAllOrigin();
            _viewmodel_imageviewergui.ReflectCurrentRecipe();
        }

        public void SaveOriginLists(object param)
        {
            if(CurrentDataSetList == null)
            {
                return;
            }

            string filepath = param.ToString();
            CurrentDataSetList.SaveTransformParameter(filepath);
        }


        private void SetFileInCurrentDirFiles(string filepath, bool isswitchworkspace)
        {
            if (isswitchworkspace)
            {
                _model_workspace.SwitchWorkspace(System.IO.Path.GetDirectoryName(filepath));
            }
            else
            {
                _model_workspace.UpdateWorkspace();
            }

            string curfilename = System.IO.Path.GetFileName(filepath.ToString());

            if (CurrentDirFiles.Any((string text) => { return curfilename.Equals(text); }))
            {
                string nextdir = CurrentDirFiles.First((string str) => { return curfilename.Equals(str); });

                // データの一部を転写する
                _model_workspace.CopyWorkingData(CurrentDir, nextdir);

                CurrentDir = nextdir;
            }

            RaisePropertyChanged("CurrentWorkspace");
            RaisePropertyChanged("CurrentDir");
            RaisePropertyChanged("CurrentDirFiles");

            RaisePropertyChanged("Header");
            RaisePropertyChanged("RecipeName");
            RaisePropertyChanged("BindRecipeItem");
        }

        #endregion

        #endregion

        #region [ (GUI) Recipe XML Table]


        #region [Property]
  
        public string ErrorText
        {
            get
            {
                if (CurrentDir == null)
                {
                    return null;
                }
                if (_model_workspace.DataSet[CurrentDir] == null) return null;
                if (_model_workspace.DataSet[CurrentDir].ParserException == null) return null;

                return _model_workspace.DataSet[CurrentDir].ParserException.Message;
            }
        }

        public bool IsVisibleErrorText
        {
            get
            {
                if (CurrentDir == null) return false;
                if (_model_workspace.DataSet[CurrentDir] == null) return false;
                
                if( _model_workspace.DataSet[CurrentDir].ParserException == null)
                {
                    if (CurrentRecipeObject == null ||
                        CurrentRecipeObject.ModelObject.ErrorStatus == Model.DataSet.RecipeHandler.Error.RecipeErrorHandle.ERecipeError.OK ||
                        CurrentRecipeObject.ModelObject.ErrorStatus == Model.DataSet.RecipeHandler.Error.RecipeErrorHandle.ERecipeError.NOTRUN)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool IsVisibleAppErrorText
        {
            get
            {
                if (CurrentDir == null) return false;
                if (_model_workspace.DataSet[CurrentDir] == null) return false;

                if (_model_workspace.DataSet[CurrentDir].ParserException == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public string ErrorRecipe
        {
            get
            {
                if (CurrentDir == null)
                {
                    return "";
                }
                if (_model_workspace.DataSet[CurrentDir] == null) return "";
                if (CurrentRecipeObject == null) return "";

                return CurrentRecipeObject.ErrorMessage;
            }

        }

        public bool IsVisibleErrorOfRecipes
        {
            get
            {
                if (!IsVisibleErrorText)
                {
                    return false;
                }
                if (CurrentRecipeObject == null) return false;

                return true;
            }
        }

        private bool _isvisibleuser_recipeitems = true;
        private bool _isvisibleuser_regions = true;
        public bool IsVisibleRecipeItems
        {
            get
            {
                if (CurrentDir == null) return false;
                if (_model_workspace.DataSet[CurrentDir] == null) return false;

                if (_model_workspace.DataSet[CurrentDir].ParserException == null)
                {
                    return _isvisibleuser_recipeitems;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsHiddenUserRecipeItems
        {
            get
            {
                return !_isvisibleuser_recipeitems;
            }
        }

        public bool IsVisibleRegions
        {
            get
            {
                if (CurrentDir == null) return false;
                if (_model_workspace.DataSet[CurrentDir] == null) return false;

                if (_model_workspace.DataSet[CurrentDir].ParserException == null)
                {
                    return _isvisibleuser_regions;
                }
                else
                {
                    return false;
                }
            }
        }


        public bool IsHiddenUserRegions
        {
            get
            {
                return !_isvisibleuser_regions;
            }
        }


        #endregion

        #region [Property (DataSet)]


        public Model.DataSet.RecipeHandler.Group.MeasuresGroup CurrentDataSetList
        {
            get
            {
                if(CurrentDir == null)
                {
                    return null;
                }
                if (_model_workspace.DataSet[CurrentDir] == null) return null;
                if (_model_workspace.DataSet[CurrentDir].RecipeContents == null) return null;

                return _model_workspace.DataSet[CurrentDir].RecipeContents.RecipeObject;
            }
        }

        protected List<Model.DataSet.RecipeHandler.Group.MeasureGroup> CurrentDataSet
        {
            get
            {
                return CurrentDataSetList.RecipeItemGroup;
            }
        }

        public Xml.DataGrid.HeaderSource Header
        {
            get
            {
                if (CurrentDirFiles == null) return null;
                if (CurrentDir == null) return null;
                if (_model_workspace.DataSet[CurrentDir] == null) return null;
                if (_model_workspace.DataSet[CurrentDir].RecipeContents == null) return null;
                return new Xml.DataGrid.HeaderSource(_model_workspace.DataSet[CurrentDir].RecipeContents.Header);
            }


            set
            {
                if (CurrentDirFiles != null)
                {
                    _model_workspace.DataSet[CurrentDir].RecipeContents.Header = value.ModelObject;
                    RaisePropertyChanged("Header");
                }
            }
        }


        public ObservableCollection<Xml.DataGrid.RecipeItemSource> BindRecipeItem
        {
            get
            {
                if (CurrentDirFiles == null) return null;
                if (_model_workspace.DataSet[CurrentDir] == null) return null;
                if (_model_workspace.DataSet[CurrentDir].RecipeContents == null) return null;
                var targetlist = new ObservableCollection<Xml.DataGrid.RecipeItemSource>();
                foreach (var obj in CurrentDataSet)
                {
                    var source = new Xml.DataGrid.RecipeItemSource(this, obj);
                    targetlist.Add(source);
                    source.UpdateProperty();
                }

                return targetlist;
            }

            set
            {
                if (CurrentDirFiles == null) return;

                CurrentDataSet.Clear();
                foreach( var obj in value)
                {
                    CurrentDataSet.Add(obj.ModelObject);
                }

                UpdateAll();
                UpdateImage();
            }
        }


        private Xml.DataGrid.RecipeItemSource _current_recipe;
        public Xml.DataGrid.RecipeItemSource CurrentRecipeObject
        {
            get
            {
                return _current_recipe;
            }
            set
            {
                _current_recipe = value;

                _viewmodel_imageviewergui.ReflectCurrentRecipe();

                RaisePropertyChanged("ErrorRecipe");
                RaisePropertyChanged("IsVisibleErrorOfRecipes");
            }
        }

        public string CurrentRecipeIndex
        {
            get
            {
                if (_current_recipe == null) return "";
                return "[" + _current_recipe.Index.ToString() + "]";
            }
        }

        public string CurrentRecipeItem
        {
            get
            {

                if (_current_recipe == null) return "";
                return "<" + _current_recipe.ItemObject.ItemLabel + ">";
            }
        }

        public string CurrentRecipeName
        {
            get
            {

                if (_current_recipe == null) return "";
                return _current_recipe.Name;
            }
        }



        public ObservableCollection<Xml.DataGrid.CoordinateParamSource> BindCoordinateList
        {
            get
            {
                if( CurrentRecipeObject == null)
                {
                    return null;
                }

                if (_model_workspace.DataSet[CurrentDir] == null) return null;
                if (_model_workspace.DataSet[CurrentDir].RecipeContents == null) return null;
                var target = CurrentRecipeObject.ModelObject;
                ObservableCollection<Xml.DataGrid.CoordinateParamSource> list = new ObservableCollection<Xml.DataGrid.CoordinateParamSource>();

                if (target != null)
                {
                    foreach (var obj in target.CoordinateList)
                    {
                        list.Add(new Xml.DataGrid.CoordinateParamSource(this, obj));
                    }
                }

                return list;
            }
        }

        public bool IsEmptyBindCoordinate
        {
            get
            {
                if( BindRecipeItem == null || BindRecipeItem.Count == 0 )
                {
                    return false;
                }

                if (BindCoordinateList != null && BindCoordinateList.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public ObservableCollection<Xml.DataGrid.MeasureParamsSource> BindParameterObject
        {
            get
            {
                if (CurrentRecipeObject == null)
                {
                    return null;
                }
                if (_model_workspace.DataSet[CurrentDir] == null) return null;
                if (_model_workspace.DataSet[CurrentDir].RecipeContents == null) return null;
                var target = CurrentRecipeObject.ModelObject;
                ObservableCollection<Xml.DataGrid.MeasureParamsSource> list = new ObservableCollection<Xml.DataGrid.MeasureParamsSource>();

                if (target != null)
                {
                    foreach (var obj in target.MeasureParamsList)
                    {
                        list.Add(new Xml.DataGrid.MeasureParamsSource(this, obj));
                     }
                }

                return list;
            }
        }

        public bool IsEmptyBindParameter
        {
            get
            {
                return false;
#if false
                if (BindRecipeItem == null || BindRecipeItem.Count == 0)
                {
                    return false;
                }

                if (BindParameterObject != null && BindRecipeItem.Count != 0 && BindParameterObject.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
#endif

            }
        }


        public ObservableCollection<Xml.DataGrid.ResultSource> BindResultsObject
        {
            get
            {
                if (CurrentRecipeObject == null)
                {
                    return null;
                }
                if (_model_workspace.DataSet[CurrentDir] == null) return null;
                if (_model_workspace.DataSet[CurrentDir].RecipeContents == null) return null;
                ObservableCollection<Xml.DataGrid.ResultSource> _binding_results_object = new ObservableCollection<Xml.DataGrid.ResultSource>();


                var target = CurrentRecipeObject.ModelObject;
                if (target != null)
                {
                    foreach (var nobj in target.ResultsList)
                    {
                        _binding_results_object.Add(new Xml.DataGrid.ResultSource(this, nobj));
                    }
                }

                return _binding_results_object;
            }
        }

        public bool IsEmptyBindResults
        {
            get
            {
                if (BindResultsObject == null || BindRecipeItem.Count == 0)
                {
                    return false;
                }

                if (BindResultsObject.Count == 0 )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        private ObservableCollection<Xml.DataGrid.ItemTypeSource> _binding_recipeitem_item;
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
            }
        }


        private ObservableCollection<Xml.TreeView.RegionTreeSource> _binding_region_tree;
        public ObservableCollection<Xml.TreeView.RegionTreeSource> BindRegionTreeRoot
        {
            get
            {

                return _binding_region_tree;
            }

            set
            {
                _binding_region_tree = value;
            }
        }



        #endregion


        #region [Property (Viewing)]
        public string RecipeName
        {
            get
            {
                if (CurrentDirFiles == null) return "";
                if (CurrentDir == null) return "";
                if (_model_workspace.DataSet[CurrentDir] == null) return "";
                if (_model_workspace.DataSet[CurrentDir].RecipeContents == null) return null;
                return _model_workspace.DataSet[CurrentDir].RecipeContents.RecipeName;
            }
            set
            {
                if (CurrentDir == null) return;
                if (_model_workspace.DataSet[CurrentDir] == null) return;

                if (CurrentDirFiles != null)
                {
                    _model_workspace.DataSet[CurrentDir].RecipeContents.RecipeName = value;
                }

                RaisePropertyChanged("RecipeName");
            }
        }
        #endregion

        #region [Command]


        private ICommand _command_changerecipeitemrow;
        public ICommand CommandChangeRecipeItemRow
        {
            get
            {
                return this._command_changerecipeitemrow ?? (this._command_changerecipeitemrow = new DelegateCommand<object>(ChangeRecipeItemRow));
            }
        }

        #endregion

        #region [Notify]

        #endregion

        #region [Command Method]

        public void ChangeRecipeItemRow(object param)
        {
            if (param == null) return;

            CurrentRecipeObject = (Xml.DataGrid.RecipeItemSource)param;

            UpdateCoordinate();
        }

        public void UpdateProperty()
        {
            if(CurrentRecipeObject != null)
            {
                var target = CurrentRecipeObject.ModelObject;
                _binding_region_tree = new ObservableCollection<Xml.TreeView.RegionTreeSource>();

                if (target != null)
                {
                    foreach (var obj in target.RegionsGroupObject.RegionList)
                    {
                        _binding_region_tree.Add(new Xml.TreeView.RegionTreeSource(this, obj));
                    }
                }

                foreach (var obj in _binding_region_tree)
                {
                    //obj.RefreshDataSet();
                }
            }


            RaisePropertyChanged("BindCoordinateList");
            RaisePropertyChanged("BindParameterObject");
            RaisePropertyChanged("BindResultsObject");
            RaisePropertyChanged("BindRegionTreeRoot");

            RaisePropertyChanged("IsEmptyBindCoordinate");
            RaisePropertyChanged("IsEmptyBindParameter");
            RaisePropertyChanged("IsEmptyBindResults");
        }

        #endregion

        #region [Utility]


        public void RunDataGridCommand(Model.DataSet.RecipeHandler.Base.BaseTreeGroup target, Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTreeGroup, bool> func)
        {
            func(target.Parent, target);

            RaisePropertyChanged("BindRecipeItem");
            UpdateCoordinate();
        }

        public void RunDataGridCommand(Model.DataSet.RecipeHandler.Base.BaseTuple target, Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTuple, bool> func)
        {
            func(target.Owner, target);

            RaisePropertyChanged("BindRecipeItem");
            UpdateCoordinate();
        }

        public void UpdateAll()
        {
            RaisePropertyChanged("BindRecipeItem");
            UpdateCoordinate();
        }

        public void UpdateImage()
        {
            if( _viewmodel_imageviewergui != null && _viewmodel_imageviewergui.ImageObject != null)
            {
                _viewmodel_imageviewergui.UpdateAllOrigin();
                _viewmodel_imageviewergui.ReflectCurrentRecipe();
            }
        }

        #endregion

        #endregion

        #region [ (GUI) Auto Coordinate]

        private ICommand _command_autocoordinate;
        public ICommand CommandAutoCoordinate
        {
            get
            {
                return this._command_autocoordinate ?? (this._command_autocoordinate = new DelegateCommand<object>(RunAutoCoordinate));
            }
        }

        public void RunAutoCoordinate(object param)
        {
            CurrentDataSetList.RunCoordinateCalculator();
        }

        private ICommand _command_updatecoordinate;
        public ICommand CommandUpdateCoordinate
        {
            get
            {
                return this._command_updatecoordinate ?? (this._command_updatecoordinate = new DelegateCommand<object>(UpdateCoordinate));
            }

        }

        private ICommand _command_changeabsolute;
        public ICommand CommandChangeAbsolute
        {
            get
            {
                return this._command_changeabsolute ?? (this._command_changeabsolute = new DelegateCommand<object>(ChangeAbsoluteMode));
            }
        }

        protected void UpdateCoordinate(object param=null)
        {
            if (_model_workspace.DataSet[CurrentDir] == null) return;


            //var formtest = new Model.DataSet.RecipeHandler.GroupTree.TreeFormTest();
            //formtest.MakeTree(CurrentDataSetList);

            //CurrentDataSetList.RunCoordinateCalculator();
            UpdateProperty();
        }

        protected void ChangeAbsoluteMode(object param)
        {
            if (CurrentRecipeObject == null)
            {
                return;
            }

            var target = CurrentRecipeObject.ModelObject;
            _binding_region_tree = new ObservableCollection<Xml.TreeView.RegionTreeSource>();

            if (target != null)
            {
                foreach (var obj in target.RegionsGroupObject.RegionList)
                {
                    _binding_region_tree.Add(new Xml.TreeView.RegionTreeSource(this, obj));
                }
            }

            UpdateProperty();
        }

        public bool IsAbsoluteChecking
        {
            get
            {
                if (CurrentDataSetList == null)
                {
                    return false;
                }
                else
                {
                    return CurrentDataSetList.IsAbsoluteCoordinate;
                }
            }

            set
            {
                if (_model_workspace.DataSet == null) return;
                if (_model_workspace.DataSet[CurrentDir] == null) return;
                if (CurrentDataSetList == null) return;
                CurrentDataSetList.IsAbsoluteCoordinate = value;

                RaisePropertyChanged("IsAbsoluteChecking");
                UpdateProperty();

            }

        }

        #endregion

        #region [ (Hidden) Template]

        private ICommand _command_makexmltable;
        public ICommand CommandMakeXmlTable
        {
            get
            {
                return this._command_makexmltable ?? (this._command_makexmltable = new DelegateCommand<object>(MakeXmlTable));
            }
        }

        public void MakeXmlTable(object param)
        {
            _model_recipetemplate.SaveTemplate();
        }


        #endregion

    }
}

using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;

using System;

using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace ClipXmlReader.ViewModel.Xml.TreeView
{
    public class RegionTreeSource : Base.BaseTreeSource 
    {
        protected Model.DataSet.RecipeHandler.Group.RegionGroup ModelObject
        {
            get;
            set;
        }


        public int Index
        {
            get
            {
                return ModelObject.Index;
            }
        }


        protected RegionNodeSource _binding_region;
        public RegionNodeSource BindRegion
        {
            get
            {
                return _binding_region;
            }
            set
            {
                _binding_region = value;
            }

        }

        protected FeatureParamsTreeSource _binding_featureparams;
        public FeatureParamsTreeSource BindFeatureParams
        {
            get
            {
                return _binding_featureparams;
            }
            set
            {
                _binding_featureparams = value; 
            }
        }

        protected FilterBranchTreeSource _binding_filterbranch;
        public FilterBranchTreeSource BindFilterBranch
        {
            get
            {
                return _binding_filterbranch;
            }
            set
            {
                _binding_filterbranch = value;
            }
        }


        public override bool IsVisibleCircleMark
        {
            get
            {
                return true;
            }
        }


        public override System.Windows.Media.Brush MarkColor
        {
            get
            {
                return new System.Windows.Media.SolidColorBrush(ModelObject.AppBrushColor);
            }
        }

        public override string Text
        {
            get
            {
                return string.Format("{0} : 領域 <" + RegionMap[ModelObject.ItemType] + ">", Index);
            }

            set { }
        }

        public override object Value
        {
            get
            {
                return ModelObject.ItemType;
                //return ModelObject.GetParameter<int>(ModelObject.Key_ItemType);
            }
            set
            {
                var item = (int)value;
                ModelObject.ItemType = item;

                IsVisibleTextBoxNormalKey = true;

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



        public override ObservableCollection<TreeParameterSource> Sources
        {
            get
            {
                var type = ModelObject.GetParameter<object>(ModelObject.Key_ItemType).GetType();
                if (type == typeof(int))
                {
                    return ListRegion;
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

        protected override bool IsNode
        {
            get
            {
                return false;
            }
        }

        public void RefreshDataSet()
        {
            ModelObject.RoiGroupObject.RefreshCoord();
        }

        public override Base.BaseTreeSource.ERegionTreeUIType UIType
        {
            get
            {
                return ERegionTreeUIType.ComboBox;
            }
        }

        protected Dictionary<int, string> RegionMap
        {
            get
            {
                if (ModelObject.RelationsObject != null)
                {
                    return ModelObject.RelationsObject.Regions.RegionDictLabel;
                }
                else
                {
                    var dict = Model.DataSet.RecipeHandler.Default.RegionIDTable.GetRoiTextMap();
                    Dictionary<int, string> newdict = new Dictionary<int, string>();

                    foreach( var obj in dict)
                    {
                        newdict[(int)obj.Key] = obj.Value;
                    }

                    return newdict;
                }
            }
        }

        protected List<int> RegionItemTypeCandidates
        {
            get
            {
                return ModelObject.ItemTypeCandidates;
            }
        }

        protected ObservableCollection<TreeParameterSource> ListRegion
        {
            get
            {
                var list = new System.Collections.ObjectModel.ObservableCollection<TreeParameterSource>();
                foreach (var regionitem in RegionMap)
                {
                    if (RegionItemTypeCandidates.Contains(regionitem.Key))
                    {
                        list.Add(new TreeParameterSource() { ParameterLabel = regionitem.Value, BindingParameterValue = regionitem.Key });
                    }
                }

                return list;
            }
        }


        protected MainWindowViewModel ParentVM
        {
            get;
            set;
        }


        private ICommand _command_insert_prev_region;
        public ICommand CommandInsertPrevParams
        {
            get
            {
                return _command_insert_prev_region ?? (this._command_insert_prev_region = new DelegateCommand<object>(InsertPrevRegion));
            }
        }


        private ICommand _command_insert_next_region;
        public ICommand CommandInsertNextParams
        {
            get
            {
                return _command_insert_next_region ?? (this._command_insert_next_region = new DelegateCommand<object>(InsertNextRegion));
            }
        }

        private ICommand _command_change_regionid;
        public ICommand CommandChangeRegionID
        {
            get
            {
                return _command_change_regionid ?? (this._command_change_regionid = new DelegateCommand<object>(ChangeRegionID));
            }
        }

        private ICommand _command_delete_region;
        public ICommand CommandDeleteRegion
        {
            get
            {
                return _command_delete_region ?? (this._command_delete_region = new DelegateCommand<object>(DeleteRegion));
            }
        }

        public RegionTreeSource(MainWindowViewModel mastervm, Model.DataSet.RecipeHandler.Group.RegionGroup model_object)
            : base()
        {
            ParentVM = mastervm;
            ModelObject = model_object;

            //Children = new ObservableCollection<Base.BaseTreeSource>();

            InitViewModel();


            BindRightClickMenu = new System.Collections.ObjectModel.ObservableCollection<RightClickMenu.RightClickMenuSource>();
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "上に領域を追加", RightClickEvent = this.CommandInsertPrevParams, IsEnabled = Environments.OperationStatus.IsDevelopmentMode });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "下に領域を追加", RightClickEvent = this.CommandInsertNextParams, IsEnabled = Environments.OperationStatus.IsDevelopmentMode });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { IsSeparator = true });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "領域タイプ変更", RightClickEvent = this.CommandChangeRegionID, IsEnabled = true });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "領域削除", RightClickEvent = this.CommandDeleteRegion, IsEnabled = Environments.OperationStatus.IsDevelopmentMode ? ModelObject.SpecifiedParent.RegionList.Count > ModelObject.SpecifiedParent.MinimumRegionNumber : false });           
        }

        protected void InitViewModel()
        {
            if (ModelObject.RoiGroupObject.RoiParamList == null)
            {
                ModelObject.RoiGroupObject.RoiParamList = new List<Model.DataSet.RecipeHandler.UserTuple.NameValuePairTuple>();
            }

            {
                InitNode();
                foreach (var tuple in ModelObject.RoiGroupObject.RoiParamList)
                {
                    AddNode(new TreeView.RegionNodeSource(ParentVM, ModelObject.RoiGroupObject, tuple));
                }
            }


            BindFeatureParams = new FeatureParamsTreeSource(ParentVM, ModelObject.FeatureParamsGroupObject);
            AddNode(BindFeatureParams);

            BindFilterBranch = new FilterBranchTreeSource(ParentVM, ModelObject.FilterGroupObject);
            AddNode(BindFilterBranch);
        }


        protected void InsertPrevRegion(object param)
        {
            Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTreeGroup, bool> func = ((measures, target) =>
            {
                measures.InsertGroup(target);
                return true;
            });


            ParentVM.RunDataGridCommand(this.ModelObject, func);
        }

        protected void InsertNextRegion(object param)
        {
            Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTreeGroup, bool> func = ((measures, target) =>
            {
                measures.InsertGroup(target, true);
                return true;
            });

            ParentVM.RunDataGridCommand(this.ModelObject, func);
        }


        protected void ChangeRegionID(object param)
        {
            IsVisibleTextBoxNormalKey = false;
        }


        protected void DeleteRegion(object param)
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

using System;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace ClipXmlReader.ViewModel.Xml.TreeView
{
    public class RegionNodeSource : Base.BaseTreeSource 
    {

        protected Dictionary<ClipXmlReader.Model.DataSet.RecipeHandler.Group.ROIGroup.EDetectionColor, string> ColorMap
        {
            get
            {
                return Model.DataSet.RecipeHandler.Group.ROIGroup.GetColorTextMap();
            }
        }

        protected ObservableCollection<TreeParameterSource> ListDetectionColor
        {
            get
            {
                var list_detcolor = new System.Collections.ObjectModel.ObservableCollection<TreeParameterSource>();
                foreach (var coloritem in ColorMap)
                {
                    list_detcolor.Add(new TreeParameterSource() { ParameterLabel = coloritem.Value, BindingParameterValue = coloritem.Key });
                }

                return list_detcolor;
            }

        }

        public override string Text
        {
            get
            {
                return ModelObject.GetParameter<string>(ModelObject.Key_DisplayedName);
            }


            set
            {
                ModelObject.SetParameter<string>(ModelObject.Key_Name, value);
                IsVisibleTextBoxArrangeKey = false;
                OnPropertyChanged("Text");
            }
        }



        bool _arranging_status = false;
        public override bool IsVisibleTextBoxArrangeKey
        {
            get
            {
                return _arranging_status;
            }
            set
            {
                _arranging_status = value;
                OnPropertyChanged("IsVisibleTextBoxArrangeKey");
                OnPropertyChanged("IsVisibleTextBoxNormalKey");
            }
        }

        public override object Value
        {
            get
            {
                for (int i = 0; i < _basegroup.RoiParamList.Count; i++)
                {
                    if (_basegroup.RoiParamList[i].Value_Name.Equals(ModelObject.Value_Name))
                    {
                        if (_basegroup.RoiParamList[i].Value_Value.GetType() == typeof(double))
                        {
                            return Math.Round(((double)_basegroup.RoiParamList[i].Value_Value), 4);
                        }
                        else
                        { 
                            return _basegroup.RoiParamList[i].Value_Value;
                        }
                       
                    }
                }

                return null;


                //return ModelObject.GetParameter(ModelObject.Key_Value);
            }
            set
            {
                /**
                 * 本来はこのようなコーディングは望ましくない(親のListを@名前で書き換えるという考え方)
                 * 本来はオブジェクトでの比較が必要だが、同名の扱いが決まっていないため、保留
                 * 機会を見てリファクタリングする必要あり
                 * */


                var obj_tmp = _basegroup.RoiParamList;
                int parent_ind = -1;

                for (int i = 0; i < _basegroup.RoiParamList.Count; i++)
                {
                    if (_basegroup.RoiParamList[i].Value_Name.Equals(ModelObject.Value_Name))
                    {
                        parent_ind = i;
                    }
                }

                var type = ModelObject.Value_Value.GetType();
                if (type == typeof(Model.DataSet.RecipeHandler.Group.ROIGroup.EDetectionColor))
                {
                    foreach (var textcolor in ColorMap)
                    {
                        if (textcolor.Key.ToString().Equals(value.ToString()))
                        {
                            obj_tmp[parent_ind].SetParameterEnum(obj_tmp[parent_ind].Key_Value, textcolor.Key, type);
                        }
                    }
                }
                else
                {
                    obj_tmp[parent_ind].Value_Value = value;
                }

                _basegroup.RoiParamList = obj_tmp;

                OnPropertyChanged("Value");
                ParentVM.UpdateAll();
                ParentVM.UpdateImage();


#if false
                if (type == typeof(Model.DataSet.RecipeHandler.Group.ROIGroup.EDetectionColor))
                {
                    foreach( var textcolor in ColorMap)
                    {
                        if( textcolor.Key.ToString().Equals(value.ToString())  )
                        {
                            ModelObject.SetParameterEnum(ModelObject.Key_Value, textcolor.Key, type);
                            return;
                        }
                    }
                }
                else
                {

                    ModelObject.Value_Value = value;

                    var obj_tmp = _basegroup.RoiParamList;

                    for( int i = 0 ; i < _basegroup.RoiParamList.Count; i ++)
                    {
                        if (_basegroup.RoiParamList[i].Value_Name.Equals(ModelObject.Value_Name))
                        {
                            obj_tmp[i].Value_Value = value;                  
                        }
                    }

                    _basegroup.RoiParamList = obj_tmp;





#if false
                    decimal val = new decimal();

                    if (decimal.TryParse(value.ToString(), out val))
                    {
                        ModelObject.SetParameter<decimal>(ModelObject.Key_Value, val);
                    }
#endif
                }
#endif
            }
        }






        public override ObservableCollection<TreeParameterSource> Sources
        {
            get
            {
                var type = ModelObject.GetParameter<object>(ModelObject.Key_Value).GetType();
                if (type == typeof(Model.DataSet.RecipeHandler.Group.ROIGroup.EDetectionColor))
                {
                    return ListDetectionColor;
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
        


        public override Base.BaseTreeSource.ERegionTreeUIType UIType
        {
            get
            {
                var type = ModelObject.GetParameter<object>(ModelObject.Key_Value).GetType();

                if (type == typeof(Model.DataSet.RecipeHandler.Group.ROIGroup.EDetectionColor))
                {
                    return ERegionTreeUIType.ComboBox;

                }
                else
                {
                    return ERegionTreeUIType.TextBox;
                }
            }
        }


        private ICommand _command_insert_prev_params;
        public ICommand CommandInsertPrevParams
        {
            get
            {
                return _command_insert_prev_params ?? (this._command_insert_prev_params = new DelegateCommand<object>(InsertPrevItem));
            }
        }


        private ICommand _command_insert_next_params;
        public ICommand CommandInsertNextParams
        {
            get
            {
                return _command_insert_next_params ?? (this._command_insert_next_params = new DelegateCommand<object>(InsertNextItem));
            }
        }

        private ICommand _command_change_parametername;
        public ICommand CommandChangeParameterName
        {
            get
            {
                return _command_change_parametername ?? (this._command_change_parametername = new DelegateCommand<object>(ChangeParameterName));
            }
        }

        private ICommand _command_delete_params;
        public ICommand CommandDeleteParams
        {
            get
            {
                return _command_delete_params ?? (this._command_delete_params = new DelegateCommand<object>(DeleteParameter));
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

        protected ClipXmlReader.Model.DataSet.RecipeHandler.Group.ROIGroup _basegroup;

        public RegionNodeSource(MainWindowViewModel mastervm, ClipXmlReader.Model.DataSet.RecipeHandler.Group.ROIGroup basegroup, ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple.NameValuePairTuple model_object)
        {

            ParentVM = mastervm;
            ModelObject = model_object;
            _basegroup = basegroup;

            BindRightClickMenu = new System.Collections.ObjectModel.ObservableCollection<RightClickMenu.RightClickMenuSource>();
            
            // 追加機能については、今後検討の余地あり。
            // CoordLTTupleとReferenceTupleを新規に追加して整備するような構成が必要
            // また、それ以外にFreeParameterを持たせる設計にする必要がある
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "...", IsEnabled = false });
             
            //BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "上にパラメータを追加", RightClickEvent = this.CommandInsertPrevParams, IsEnabled = true });
            //BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "下にパラメータを追加", RightClickEvent = this.CommandInsertNextParams, IsEnabled = true });
            //BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { IsSeparator = true });
            //BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "パラメータ名変更", RightClickEvent = this.CommandChangeParameterName, IsEnabled = !ModelObject.GetParameter<bool>(ModelObject.Key_NameChangeProhibited) });
            //BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "パラメータ削除", RightClickEvent = this.CommandDeleteParams, IsEnabled = !ModelObject.GetParameter<bool>(ModelObject.Key_NameChangeProhibited) });           

        }




        protected void InsertPrevItem(object param)
        {
            Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTuple, bool> func = ((measures, target) =>
            {
                measures.InsertTuple(target);
                return true;
            });


            ParentVM.RunDataGridCommand(this.ModelObject, func);
        }

        protected void InsertNextItem(object param)
        {
            Func<Model.DataSet.RecipeHandler.Base.BaseTreeGroup, Model.DataSet.RecipeHandler.Base.BaseTuple, bool> func = ((measures, target) =>
            {
                measures.InsertTuple(target, true);
                return true;
            });

            ParentVM.RunDataGridCommand(this.ModelObject, func);
        }


        protected void ChangeParameterName(object param)
        {
            IsVisibleTextBoxArrangeKey = true;
        }

        protected void DeleteParameter(object param)
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

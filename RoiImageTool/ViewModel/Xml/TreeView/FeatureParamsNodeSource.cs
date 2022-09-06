using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

using System.Windows;

namespace ClipXmlReader.ViewModel.Xml.TreeView
{
    public class FeatureParamsNodeSource : Base.BaseTreeSource
    {
        public override string Text
        {
            get
            {
                string org = ModelObject.GetParameter<string>(ModelObject.Key_DisplayedName);
                string[] text = org.Split(new char[] { ';' });
                if (text.Length >= 2)
                {
                    return text[0];
                }
                else
                {
                    return org;
                }
            }


            set
            {
                ModelObject.SetParameter<string>(ModelObject.Key_DisplayedName, value);
                OnPropertyChanged("Text");

                IsVisibleTextBoxArrangeKey = false;
            }
        }

        public override string Description
        {
            get
            {
                string org = ModelObject.GetParameter<string>(ModelObject.Key_DisplayedName);
                string[] text = org.Split(new char[] {';'});
                if (text.Length >= 2)
                {
                    return text[1];
                }
                else
                {
                    return "";
                }
            }
            set
            {
                OnPropertyChanged("Description");
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
                return ModelObject.GetParameter(ModelObject.Key_Value);
            }
            set
            {
                decimal val = new decimal();


                if( decimal.TryParse( value.ToString(), out val))
                {
                    ModelObject.Value_Value = val;
                    //ModelObject.SetParameter<decimal>(ModelObject.Key_Value, val);
                    OnPropertyChanged("Value");
                }

                ParentVM.UpdateAll();
                ParentVM.UpdateImage();
            }
        }

        public override Base.BaseTreeSource.ERegionTreeUIType UIType
        {
            get
            {
                return Base.BaseTreeSource.ERegionTreeUIType.TextBox;
            }
        }

        protected MainWindowViewModel ParentVM
        {
            get;
            set;
        }

        protected ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple.NameValuePairTuple ModelObject
        {
            get;
            set;
        }

        public FeatureParamsNodeSource(MainWindowViewModel mastervm, ClipXmlReader.Model.DataSet.RecipeHandler.UserTuple.NameValuePairTuple model_object)
        {
            ParentVM = mastervm;
            ModelObject = model_object;

            BindRightClickMenu = new System.Collections.ObjectModel.ObservableCollection<RightClickMenu.RightClickMenuSource>();
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "上にパラメータを追加", RightClickEvent = this.CommandInsertPrevParams, IsEnabled = Environments.OperationStatus.IsDevelopmentMode });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "下にパラメータを追加", RightClickEvent = this.CommandInsertNextParams, IsEnabled = Environments.OperationStatus.IsDevelopmentMode });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { IsSeparator = true });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "パラメータ名変更", RightClickEvent = this.CommandChangeParameterName, IsEnabled = !ModelObject.GetParameter<bool>(ModelObject.Key_NameChangeProhibited) });
            BindRightClickMenu.Add(new RightClickMenu.RightClickMenuSource() { HeaderName = "パラメータ削除", RightClickEvent = this.CommandDeleteParams, IsEnabled = Environments.OperationStatus.IsDevelopmentMode ?  !ModelObject.GetParameter<bool>(ModelObject.Key_NameChangeProhibited) : false });           
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;

namespace ClipXmlReader
{
    namespace View
    {
        /// <summary>
        /// MainWindow.xaml の相互作用ロジック
        /// </summary>
        public partial class MainWindow : Window
        {

            public MainWindow()
            {
                InitializeComponent();

                Width = 1170;
                Height = 650;
            }

            private void ui_WorkspaceSWButton_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    var fileDialog = new OpenFileDialog();
                    fileDialog.Filter = "Recipe Files(.xml)|*.xml";
                    fileDialog.InitialDirectory = Properties.Settings.Default.XmlInitFilePath;


                    if (fileDialog.ShowDialog() == true)
                    {
                        Properties.Settings.Default.XmlInitFilePath = System.IO.Path.GetDirectoryName(fileDialog.FileName);

                        var viewmodel = (ViewModel.MainWindowViewModel)DataContext;

                        if (viewmodel.CommandSwitchWorkspace.CanExecute(null))
                        {
                            viewmodel.CommandSwitchWorkspace.Execute(fileDialog.FileName);
                            Properties.Settings.Default.XmlInitFilePath = System.IO.Path.GetDirectoryName(fileDialog.FileName);
                            Properties.Settings.Default.Save();
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Application Error");
                }

            }



            private void RecipeItemDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
            {
                try
                {
                    var viewmodel = (ViewModel.MainWindowViewModel)DataContext;
                    if (viewmodel.CommandChangeRecipeItemRow.CanExecute(null))
                    {
                        viewmodel.CommandChangeRecipeItemRow.Execute(((DataGrid)sender).SelectedItem);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Application Error");
                }

            }

            private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                try
                {
                    var viewmodel = (ViewModel.MainWindowViewModel)DataContext;
                    if (viewmodel.CommandSelectOtherRecipe.CanExecute(null))
                    {
                        viewmodel.CommandSelectOtherRecipe.Execute(RecipeItemDataGrid.SelectedItem);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Application Error");
                }

            }

            private void RecipeItemDataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
            {

            }

            private void CheckBox_CheckingTrigger(object sender, RoutedEventArgs e)
            {
                try
                {
                    var viewmodel = (ViewModel.MainWindowViewModel)DataContext;
                    if (viewmodel.CommandChangeAbsolute.CanExecute(null))
                    {
                        viewmodel.CommandChangeAbsolute.Execute(null);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Application Error");
                }

            }


            /// **
            // TextBox End Edit Helper Methods
            // URL : [Stackoverflow] https://stackoverflow.com/questions/888324/endedit-equivalent-in-wpf
            private void EndEdit(DependencyObject parent)
            {
                LocalValueEnumerator localValues = parent.GetLocalValueEnumerator();
                while (localValues.MoveNext())
                {
                    LocalValueEntry entry = localValues.Current;
                    if (BindingOperations.IsDataBound(parent, entry.Property))
                    {
                        BindingExpression binding = BindingOperations.GetBindingExpression(parent, entry.Property);
                        if (binding != null)
                        {
                            binding.UpdateSource();
                        }
                    }
                }

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    this.EndEdit(child);
                }
            }

            protected void EndEdit()
            {
                EndEdit(ui_recipename_textbox);
                if (_arranging_textbox != null)
                {
                    EndEdit(_arranging_textbox);

                    if (_arranging_textbox.DataContext.ToString() != "{DisconnectedItem}")
                    {
                        var obj = (ViewModel.Xml.Base.BaseTreeSource)_arranging_textbox.DataContext;
                        obj.Value = _arranging_textbox.Text;
                    }
                }

                RecipeItemDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                CoordinateDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                ParameterDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                ResultsDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            }
            /// **


            private void ui_button_saverecipexml_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    ui_button_saverecipexml.Focus();
                    EndEdit();

                    var viewmodel = (ViewModel.MainWindowViewModel)DataContext;
                    if (viewmodel.CommandSaveCurrentTable.CanExecute(null))
                    {
                        viewmodel.CommandSaveCurrentTable.Execute(viewmodel.CurrentDir);
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("Application Error");
                }
            }

            private void ui_SaveAsButton_Click(object sender, RoutedEventArgs e)
            {

                try
                {
                    var fileDialog = new SaveFileDialog();
                    fileDialog.Filter = "Recipe Files(.xml)|*.xml";

                    if (fileDialog.ShowDialog() == true)
                    {
                        EndEdit();

                        var viewmodel = (ViewModel.MainWindowViewModel)DataContext;

                        if (viewmodel.CommandSaveAsCurrentTable.CanExecute(null))
                        {
                            viewmodel.CommandSaveAsCurrentTable.Execute(fileDialog.FileName);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Application Error");
                }

            }

            private void ui_updatecoordinate_button_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    ui_updatecoordinate_button.Focus();

                    EndEdit(ui_recipename_textbox);


                    var viewmodel = (ViewModel.MainWindowViewModel)DataContext;
                    if (viewmodel.CommandUpdateCoordinate.CanExecute(null))
                    {
                        viewmodel.CommandUpdateCoordinate.Execute(null);
                    }

                    RecipeItemDataGrid.Items.Refresh();
                }
                catch (Exception err)
                {
                    MessageBox.Show("Application Error");
                }
            }

            private void RecipeItemDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
            {

            }


            protected TextBox _arranging_textbox;


            private void TextBox_GotFocus(object sender, RoutedEventArgs e)
            {

                if (sender.GetType() == typeof(TextBox))
                {
                    _arranging_textbox = (TextBox)sender;
                }

            }

            private void RecipeItemDataGrid_GotFocus(object sender, RoutedEventArgs e)
            {


            }

            private void LoadOriginLists_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    var fileDialog = new OpenFileDialog();
                    fileDialog.Filter = "Origin Lists(.csv)|*.csv";
                    fileDialog.InitialDirectory = Properties.Settings.Default.XmlInitFilePath;

                    if (fileDialog.ShowDialog() == true)
                    {
                        var viewmodel = (ViewModel.MainWindowViewModel)DataContext;
                        if (viewmodel.CommandLoadOriginLists.CanExecute(null))
                        {
                            viewmodel.CommandLoadOriginLists.Execute(fileDialog.FileName);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Application Error");
                }
            }


            private void SaveOriginLists_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    var fileDialog = new SaveFileDialog();
                    fileDialog.Filter = "Origin Lists(.csv)|*.csv";
                    fileDialog.InitialDirectory = Properties.Settings.Default.XmlInitFilePath;

                    if (fileDialog.ShowDialog() == true)
                    {
                        var viewmodel = (ViewModel.MainWindowViewModel)DataContext;
                        if (viewmodel.CommandSaveOriginLists.CanExecute(null))
                        {
                            viewmodel.CommandSaveOriginLists.Execute(fileDialog.FileName);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Application Error");
                }
            }

            private void ui_ImageViewerGui_Loaded(object sender, RoutedEventArgs e)
            {

            }





        }
    }
}

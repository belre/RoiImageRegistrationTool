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

using System.Text.RegularExpressions;



using Microsoft.Win32;

namespace ClipXmlReader.View
{
    /// <summary>
    /// ImageViewerGui.xaml の相互作用ロジック
    /// </summary>
    public partial class ImageViewerGui : UserControl
    {

#if false
        // Binding Property生成時に役立つので置いておく
        // ただしコードビハインドの処理となるので対応方法の検討は必要

        // 依存プロパティの定義
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("TestText", typeof(string), typeof(ImageViewerGui), new FrameworkPropertyMetadata("TestText3", new PropertyChangedCallback(OnTestTextChanged)));
        

        // 値の定義
        public string TestText
        {
            get 
            { 
                return (string)GetValue(TextProperty); 
            }
            set 
            { 
                SetValue(TextProperty, value); 
            }
        }

        // プロパティが変更されたとき呼ばれるコールバック関数の定義
        private static void OnTestTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            // オブジェクトを取得して処理する
            ImageViewerGui ctrl = obj as ImageViewerGui;
            if (ctrl != null)
            {
                ctrl.ui_TestText.Text = ctrl.TestText;
            }
        }
#endif


        public ImageViewerGui()
        {
            InitializeComponent();

            // Prevent Copy & Paste
            ui_OrientXTextBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, ui_OrientXTextBox_PreviewExecuted));
            ui_OrientYTextBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, ui_OrientXTextBox_PreviewExecuted));
            ui_zoomtextbox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, ui_OrientXTextBox_PreviewExecuted));
            ui_SecondOrientXTextBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, ui_OrientXTextBox_PreviewExecuted));
            ui_SecondOrientYTextBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, ui_OrientXTextBox_PreviewExecuted));

        }



        private void ImageVerticalScaler_MouseDown(object sender, MouseButtonEventArgs e)
        {

 
        }

        private void ui_ReferDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Image File|*.bmp";
                fileDialog.InitialDirectory = Properties.Settings.Default.BmpInitFilePath;
                    

                if (fileDialog.ShowDialog() == true)
                {
                    Properties.Settings.Default.BmpInitFilePath = System.IO.Path.GetDirectoryName(fileDialog.FileName);

                    var viewmodel = (ViewModel.ImageViewerGuiViewModel)DataContext;

                    if (viewmodel.CommandReferImage.CanExecute(null))
                    {
                        viewmodel.CommandReferImage.Execute(fileDialog.FileName);
                        Properties.Settings.Default.BmpInitFilePath = System.IO.Path.GetDirectoryName(fileDialog.FileName);
                        Properties.Settings.Default.Save();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Application Error");
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if( e.Delta > 0)
            {
                var viewmodel = (ViewModel.ImageViewerGuiViewModel)DataContext;
                if( viewmodel.CommandResizeImage.CanExecute(null))
                {
                    viewmodel.CommandResizeImage.Execute("Up");
                }
            }
            else if( e.Delta < 0)
            {
                var viewmodel = (ViewModel.ImageViewerGuiViewModel)DataContext;
                if (viewmodel.CommandResizeImage.CanExecute(null))
                {
                    viewmodel.CommandResizeImage.Execute("Down");
                }
            }

            e.Handled = true;
        }

        bool _ispressed_left;
        private void ScrollViewer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _ispressed_left = true;
            }
        }



        private void ScrollViewer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var viewmodel = (ViewModel.ImageViewerGuiViewModel)DataContext;

            if( e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Released )
            {

                //if (viewmodel.IsEnableOriented)
                {
                    viewmodel.DisableMouseMoveOrientedCommand.Execute(null);
                }
            }
            else if (e.ChangedButton == MouseButton.Right && e.ButtonState == MouseButtonState.Released)
            {
                viewmodel.DisableMouseMoveOrientedCommand.Execute("Reset");
            }
        }

        private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if( e.MiddleButton == MouseButtonState.Pressed )
            {

            }
            else
            {
                double[] position = new double[2] { ui_scrollviewer.HorizontalOffset + e.GetPosition(ui_scrollviewer).X, ui_scrollviewer.VerticalOffset + e.GetPosition(ui_scrollviewer).Y };

                var viewmodel = (ViewModel.ImageViewerGuiViewModel)DataContext;
                if (viewmodel.MouseMoveOrientedCommand.CanExecute(null))
                {
                    viewmodel.MouseMoveOrientedCommand.Execute(position);
                }
            }
        }

        private void mousemenuclick_event(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void areacanvas_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }

        private void viewerpopup_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void ui_OrientXTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex reg = new Regex(@"^[0-9\.\-]*$");

            double outval = 0.0;
            if (reg.IsMatch(e.Text))
            {

                
            }
            else
            {
                e.Handled = true;
            }
        }
        private void ui_OrientXTextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if( e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
        
        private void ui_ExplicitTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    TextBox obj = (TextBox)sender;
                    BindingExpression be_orienty = obj.GetBindingExpression(TextBox.TextProperty);
                    be_orienty.UpdateSource();
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        
        private void ui_scrollviewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange == 0)
            {
                e.Handled = false;
            }
        }

        private void ui_scrollviewer_horzguide_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if( e.HorizontalChange == 0 )
            {
                e.Handled = false;
            }
        }

        private void button_trim_Click(object sender, RoutedEventArgs e)
        {
            try
            {               
                var fileDialog = new SaveFileDialog();
                fileDialog.Filter = "Image File|*.bmp";
                fileDialog.InitialDirectory = Properties.Settings.Default.BmpInitFilePath;

                if (fileDialog.ShowDialog() == true)
                {
                    Properties.Settings.Default.BmpInitFilePath = System.IO.Path.GetDirectoryName(fileDialog.FileName);

                    var viewmodel = (ViewModel.ImageViewerGuiViewModel)DataContext;

                    if (viewmodel.CommandTrimImage.CanExecute(null))
                    {
                        viewmodel.CommandTrimImage.Execute( System.IO.Path.GetDirectoryName(fileDialog.FileName));
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Application Error");
            }

        }

    }
}

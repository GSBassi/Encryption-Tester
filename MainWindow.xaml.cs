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

namespace EncryptionTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateComboBoxes();
        }

        void PopulateComboBoxes()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                cmbAsym.DataContext = SharedMethods.GetASymmetricTypes();
                cmbSym.DataContext = SharedMethods.GetSymmetricTypes();
                //If cmboInput Is Nothing Then Exit Sub
                //If Not SelectedConversionType.HasValue Then Exit Sub
                //grdNormalInput.Visibility = Windows.Visibility.Collapsed
                //ctlGPS.Visibility = Windows.Visibility.Collapsed
                //'Me.cmboInput.Items.Clear()
                //Dim m_Convo As ConvertObject = ConversionLogic.ReturnConvoGroupType(SelectedConversionType)
                //If m_Convo IsNot Nothing Then
                //    grdNormalInput.Visibility = Windows.Visibility.Visible

                //    Dim sStringCol As New Specialized.StringCollection
                //    sStringCol = m_Convo.ConvertTypes
                //    cmboInput.DataContext = sStringCol
                //ElseIf SelectedConversionType = ConversionLogic.ConversionTypes.GPS Then
                //    ctlGPS.Visibility = Windows.Visibility.Visible
                //    ctlGPS.ClearValues()
                //    txtInput.Text = String.Empty

                //End If
                cmbAsym.SelectedIndex = 0;
                cmbSym.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                //ExceptionHandler.HandleException(ex)
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void btnConvertsym_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(symInput.Text))
                {
                    SharedMethods.HashType hshTyp = (SharedMethods.HashType)Enum.Parse(typeof(SharedMethods.HashType), cmbSym.SelectedValue.ToString());
                    symOutput.Text = SharedMethods.ComputeHash(symInput.Text, hshTyp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void btnConvertAsym_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((!string.IsNullOrEmpty(asymInput.Text)) && (!string.IsNullOrEmpty(txtPass.Text)))
                {
                    asymOutput.Text = SharedMethods.Encrypt(asymInput.Text, txtPass.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
        }
    }
}

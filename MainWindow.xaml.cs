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
                cmbASym.DataContext = SharedMethods.GetASymmetricTypes();
                cmbSym.DataContext = SharedMethods.GetSymmetricTypes();
                cmbASym.SelectedIndex = 0;
                cmbSym.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
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
                    SharedMethods.HashType hshTyp = (SharedMethods.HashType)Enum.Parse(typeof(SharedMethods.HashType), cmbASym.SelectedValue.ToString());
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
                    SharedMethods.SymType aTyp = (SharedMethods.SymType)Enum.Parse(typeof(SharedMethods.SymType), cmbSym.SelectedValue.ToString());
                    asymOutput.Text = SharedMethods.Encrypt(asymInput.Text, txtPass.Text, aTyp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
        }
    }
}

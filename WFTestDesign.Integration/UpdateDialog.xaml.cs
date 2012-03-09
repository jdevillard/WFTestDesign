using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.WFTestDesign_Integration
{
    /// <summary>
    /// Interaction logic for UpdateDialog.xaml
    /// </summary>
    public partial class UpdateDialog : Window
    {
        public string updatelink;

        public UpdateDialog()
        {
            InitializeComponent();
        }

        private void checkNeverRemind_Checked(object sender, RoutedEventArgs e)
        {
            this.btnLater.Content = "Close";
        }

        private void checkNeverRemind_Unchecked(object sender, RoutedEventArgs e)
        {
            this.btnLater.Content = "Later";
        }

        private void btnLater_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(updatelink);
            this.Close();
        }


    }
}

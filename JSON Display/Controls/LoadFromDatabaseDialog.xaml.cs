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
using System.Windows.Shapes;
using JSON_Display.Extensions;

namespace JSON_Display.Controls
{
    /// <summary>
    /// Interaction logic for LoadFromDatabaseDialog.xaml
    /// </summary>
    public partial class LoadFromDatabaseDialog : Window
    {
        public LoadFromDatabaseDialog()
        {
            InitializeComponent();
            tbConnectionString.Text = @"Data Source=.\Trajan.pods-local-db;Integrated Security=SSPI;app=JSONDisplay";
        }


        private async void ClickTestConnection(object sender, RoutedEventArgs e)
        {
            btnTest.IsEnabled =
            tbConnectionString.IsEnabled = false;
            var result = await tbConnectionString.Text.IsConnectionViable();

            tbTestConnection.Foreground = result ? Brushes.Green : Brushes.Red;
            btnTest.IsEnabled =
            tbConnectionString.IsEnabled = true;
        }
    }
}

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

namespace JSON_Display.Controls
{
    /// <summary>
    /// Interaction logic for SettingsDatabase.xaml
    /// </summary>
    public partial class SettingsDatabase : UserControl
    {

        #region public OperationSettingDatabase Database
        /// <summary></summary>
        public OperationSettingDatabase Database
        {
            get { return GetValue(DatabaseProperty) as OperationSettingDatabase; }
            set { SetValue(DatabaseProperty, value); }
        }

        /// <summary>
        /// Identifies the Database dependency property.
        /// </summary>
        public static readonly DependencyProperty DatabaseProperty =
            DependencyProperty.Register(
                "Database",
                typeof(OperationSettingDatabase),
                typeof(SettingsDatabase),
                new PropertyMetadata(null));
        #endregion public OperationSettingDatabase Database


        public SettingsDatabase()
        {
            InitializeComponent();
        }
    }
}

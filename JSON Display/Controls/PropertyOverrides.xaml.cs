using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JSON_Display.Models;
using JSON_Models;

namespace JSON_Display.Controls
{
    /// <summary>
    /// Interaction logic for PropertyOverrides.xaml
    /// </summary>
    public partial class PropertyOverrides : UserControl
    {

        #region public List<PropertyOverride> Properties
        /// <summary>The list of properties. </summary>
        public List<PropertyOverride> Properties
        {
            get { return GetValue(PropertiesProperty) as List<PropertyOverride>; }
            set { SetValue(PropertiesProperty, value); }
        }

        /// <summary>
        /// Identifies the Properties dependency property.
        /// </summary>
        public static readonly DependencyProperty PropertiesProperty =
            DependencyProperty.Register(
                "Properties",
                typeof(List<PropertyOverride>),
                typeof(PropertyOverrides),
                new PropertyMetadata(null));
        #endregion public List<PropertyOverride> Properties



        public PropertyOverrides()
        {
            InitializeComponent();
        }
    }
}

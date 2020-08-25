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
using JSON_Display.Operation;

namespace JSON_Display.Controls
{

    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {



        #region public CSharpSettings CSharp

        /// <summary>The settings for csharp processor</summary>
        public OperationSettings CSharp
        {
            get => GetValue(CSharpProperty) as OperationSettings;
            set => SetValue(CSharpProperty, value);
        }

        /// <summary>
        /// Identifies the CSharp dependency property.
        /// </summary>
        public static readonly DependencyProperty CSharpProperty =
            DependencyProperty.Register(
                "CSharp",
                typeof(OperationSettings),
                typeof(Settings),
                new PropertyMetadata(null));
        #endregion public CSharpSettings CSharp

        public Settings()
        {
            InitializeComponent();
        }
    }
}

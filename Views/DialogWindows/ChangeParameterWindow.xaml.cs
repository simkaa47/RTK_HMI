using DataAccess.Models;
using System.Windows;

namespace RTK_HMI.Views.DialogWindows
{
    /// <summary>
    /// Interaction logic for ChangeParameterWindow.xaml
    /// </summary>
    public partial class ChangeParameterWindow : Window
    {
        public Parameter Parameter { get; set; }
        public ChangeParameterWindow(Parameter parameter)
        {
            InitializeComponent();
            this.DataContext = parameter;
            Parameter = parameter;
        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

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
using System.Windows.Shapes;

namespace RTK_HMI.Views.DialogWindows
{
    /// <summary>
    /// Interaction logic for SaveToJsonWindow.xaml
    /// </summary>
    public partial class SaveToJsonWindow : Window
    {
        public string Name { get; set; } = String.Empty;
        public SaveToJsonWindow(string fileName)
        {
            InitializeComponent();
            if(string.IsNullOrEmpty(fileName))Path.Text = $"data_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}";
            else Path.Text = fileName;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Name = Path.Text;   
        }

       
    }
}

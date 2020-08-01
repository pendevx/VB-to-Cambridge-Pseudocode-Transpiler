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

namespace VB_to_Cambridge_Pseudocode_Transpiler
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
        }

        private void transpileButton_Click(object sender, RoutedEventArgs e)
        {
            string input = inputTextbox.Text;
            Parser parser = new Parser(input);
            outputTextbox.Text = parser.Parse();
        }
    }
}

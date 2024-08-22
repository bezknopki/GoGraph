using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GoGraph.ViewModel;

namespace GoGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Regex isNumbersOnly = new Regex("[^0-9]+");

        public MainWindow()
        {
            InitializeComponent();
            var dc = new GraphViewModel();
            dc.Grid = grid;
            DataContext = dc;
            
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) => e.Handled = isNumbersOnly.IsMatch(e.Text);
        
    }
}
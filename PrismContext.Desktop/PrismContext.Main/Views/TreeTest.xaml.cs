using System.Windows.Controls;
using PrismContext.Desktop.Main.ViewModels;

namespace PrismContext.Desktop.Main.Views
{
    /// <summary>
    /// Interaction logic for TreeTest.xaml
    /// </summary>
    public partial class TreeTest : UserControl
    {
        public TreeTest(TreeTestViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}

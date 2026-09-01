using System.Windows;
using ZMT.SteelCost.App.ViewModels;

namespace ZMT.SteelCost.App;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

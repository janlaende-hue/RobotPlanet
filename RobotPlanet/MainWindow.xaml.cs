using System.Windows;
using RobotPlanet.ViewModels;

namespace RobotPlanet;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}
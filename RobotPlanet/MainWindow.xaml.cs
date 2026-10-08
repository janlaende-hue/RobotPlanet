using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using RobotPlanet.Core.Models;
using RobotPlanet.Core.Interfaces;

namespace RobotPlanet;

public partial class MainWindow : Window
{
    private ObservableCollection<Robot> robots = new();
    private Robot? selectedRobot;
    public MainWindow()
    {
        InitializeComponent();

        RobotListBox.ItemsSource = robots;

        RobotTypeComboBox.SelectedIndex = 0;
    }

    private void AddRobot_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string name = NameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a name for the robot.");
                return;
            }
            string type = ((ComboBoxItem)RobotTypeComboBox.SelectedItem).Content.ToString()!;

            Robot robot = type switch
            {
                "CleanerBot" => new CleanerBot(name, 100),
                "ExplorerBot" => new ExplorerBot(name, 100),
                "RepairBot" => new RepairBot(name, 100),
                _ => new GuardBot(name, 100)
            };

            robots.Add(robot);

            LogListBox.Items.Add($"{robot.Name} added to Robo planet!");
            NameTextBox.Clear();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void RobotListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        selectedRobot = RobotListBox.SelectedItem as Robot;
        if (selectedRobot == null)
            return;
        BatteryBar.Value = selectedRobot.Battery;
    }

    private void PerformTask_Click(object sender, RoutedEventArgs e)
    {
        if (selectedRobot == null)
            return;

            LogListBox.Items.Add(selectedRobot.Work());

        BatteryBar.Value = selectedRobot.Battery;
    }
    private void CrazyAction_Click(object sender, RoutedEventArgs e)
    {
        if (selectedRobot == null)
            return;
        LogListBox.Items.Add(selectedRobot.CrazyAction());
        BatteryBar.Value = selectedRobot.Battery;
    }
    private void Scan_Click(object sender, RoutedEventArgs e)
    {
        if (selectedRobot == null)
        {
            LogListBox.Items.Add("Select a robot first!");
            return;
        }
        if (selectedRobot is IScan scanner)
        {
            LogListBox.Items.Add(scanner.Scan());
        }
        else
        {
            LogListBox.Items.Add($"{selectedRobot.Name} cannot scan.");
        }
    }
    private void Repair_Click(object sender, RoutedEventArgs e)
    {
        if (selectedRobot == null)
            {
            LogListBox.Items.Add("Select a robot first!");
            return;
        }
        if (selectedRobot is IRepair repairBot)
        {
            LogListBox.Items.Add(repairBot.Repair());
        }
        else
        {
            LogListBox.Items.Add($"{selectedRobot.Name} cannot repair.");
        }
    }

    private void Charge_Click(object sender, RoutedEventArgs e)
    {
        if (selectedRobot == null)
        {
            LogListBox.Items.Add("Select a robot first.");
            return;
        }
        if (selectedRobot is Ichargeable chargeableBot)
        {
            LogListBox.Items.Add(chargeableBot.Charge(20));

            BatteryBar.Value = selectedRobot.Battery;
        }
        else
        {
            LogListBox.Items.Add($"{selectedRobot.Name} cannot be charged.");
        }
    }

    private void RemoveRobot_Click(object sender, RoutedEventArgs e)
    {
        if (selectedRobot == null)
            return;

        LogListBox.Items.Add($"{selectedRobot.Name} removed from Robo planet!");

        robots.Remove(selectedRobot);

        selectedRobot = null;

        BatteryBar.Value = 0;
    }
}
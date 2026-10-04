using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Vehicles.Core;

namespace Vehicles.AvaloniaApp;

public partial class MainWindow : Window
{
    // Ühine Vehicle-kollektsioon
    private readonly ObservableCollection<Vehicle> _vehicles = new();
    private readonly ObservableCollection<string> _log = new();

    public MainWindow()
    {
        InitializeComponent();
        VehiclesListBox.ItemsSource = _vehicles;
        LogListBox.ItemsSource = _log;
    }

    private void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        string type = ((ComboBoxItem)TypeComboBox.SelectedItem!).Content!.ToString()!;

        try
        {
            Vehicle vehicle = type switch
            {
                "Car" => new Car(NameTextBox.Text ?? ""),
                "Boat" => new Boat(NameTextBox.Text ?? ""),
                _ => new AmphibiousCar(NameTextBox.Text ?? "")
            };

            _vehicles.Add(vehicle);
            _log.Add($"Lisatud: {vehicle.Describe()}");
            NameTextBox.Text = "";
        }
        catch (ArgumentException ex)
        {
            _log.Add("Viga: " + ex.Message);
        }
    }

    private void MoveButton_Click(object? sender, RoutedEventArgs e)
    {
        if (VehiclesListBox.SelectedItem is not Vehicle vehicle)
        {
            _log.Add("Viga: vali esmalt sõiduk.");
            return;
        }

        string text = (DistanceTextBox.Text ?? "").Replace(',', '.');
        if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double distance))
        {
            _log.Add("Viga: vahemaa peab olema number.");
            return;
        }

        _log.Add(vehicle.Move(distance));

        // Asendame elemendi samal kohal, et nimekiri uuendaks läbisõitu
        int index = _vehicles.IndexOf(vehicle);
        _vehicles[index] = vehicle;
        VehiclesListBox.SelectedIndex = index;
    }
}
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Vehicles.Core;

namespace Vehicles.AvaloniaApp;

public partial class MainWindow : Window
{
    // Kaks garaaži: üks autodele, teine paatidele.
    private readonly ObservableCollection<Vehicle> _cars = new();
    private readonly ObservableCollection<Vehicle> _boats = new();
    private readonly ObservableCollection<string> _log = new();

    public MainWindow()
    {
        InitializeComponent();
        CarsListBox.ItemsSource = _cars;
        BoatsListBox.ItemsSource = _boats;
        LogListBox.ItemsSource = _log;
    }

    // --- Lisamine ---

    private void AddCarButton_Click(object? sender, RoutedEventArgs e)
    {
        string type = ((ComboBoxItem)CarTypeComboBox.SelectedItem!).Content!.ToString()!;

        try
        {
            Vehicle car = type == "AmphibiousCar"
                ? new AmphibiousCar(CarNameTextBox.Text ?? "")
                : new Car(CarNameTextBox.Text ?? "");

            AddToGarage(_cars, "Autode garaaž", car);
            CarNameTextBox.Text = "";
        }
        catch (ArgumentException ex)
        {
            _log.Add("Viga: " + ex.Message);
        }
    }

    private void AddBoatButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            Vehicle boat = new Boat(BoatNameTextBox.Text ?? "");

            AddToGarage(_boats, "Paatide garaaž", boat);
            BoatNameTextBox.Text = "";
        }
        catch (ArgumentException ex)
        {
            _log.Add("Viga: " + ex.Message);
        }
    }

    // Lisab sõiduki garaaži. Enne lisamist annavad teised sama garaaži sõidukid signaali.
    private void AddToGarage(ObservableCollection<Vehicle> garage, string garageName, Vehicle vehicle)
    {
        _log.Add($"[{garageName}] Saabub {vehicle.Describe()}");

        foreach (Vehicle other in garage)
            _log.Add($"[{garageName}] {other.Signal()} (tervitab saabujat {vehicle.Name})");

        garage.Add(vehicle);
        _log.Add($"[{garageName}] Lisatud. Sõidukeid garaažis: {garage.Count}.");
    }

    // --- Eemaldamine ---

    private void RemoveCarButton_Click(object? sender, RoutedEventArgs e)
        => RemoveFromGarage(CarsListBox, _cars, "Autode garaaž");

    private void RemoveBoatButton_Click(object? sender, RoutedEventArgs e)
        => RemoveFromGarage(BoatsListBox, _boats, "Paatide garaaž");

    // Eemaldab valitud sõiduki. Pärast seda vilgutavad allesjäänud sõidukid tulesid.
    private void RemoveFromGarage(ListBox listBox, ObservableCollection<Vehicle> garage, string garageName)
    {
        if (listBox.SelectedItem is not Vehicle vehicle)
        {
            _log.Add($"Viga: vali esmalt sõiduk garaažist \"{garageName}\".");
            return;
        }

        garage.Remove(vehicle);
        _log.Add($"[{garageName}] Eemaldatud {vehicle.Describe()}");

        foreach (Vehicle remaining in garage)
            _log.Add($"[{garageName}] {remaining.FlashLights()} (saadab teele sõiduki {vehicle.Name})");

        _log.Add($"[{garageName}] Sõidukeid garaažis: {garage.Count}.");
    }

    // --- Liikumine ---

    private void MoveCarButton_Click(object? sender, RoutedEventArgs e)
        => MoveVehicle(CarsListBox, _cars, CarDistanceTextBox, "Autode garaaž");

    private void MoveBoatButton_Click(object? sender, RoutedEventArgs e)
        => MoveVehicle(BoatsListBox, _boats, BoatDistanceTextBox, "Paatide garaaž");

    private void MoveVehicle(ListBox listBox, ObservableCollection<Vehicle> garage,
                             TextBox distanceTextBox, string garageName)
    {
        if (listBox.SelectedItem is not Vehicle vehicle)
        {
            _log.Add($"Viga: vali esmalt sõiduk garaažist \"{garageName}\".");
            return;
        }

        string text = (distanceTextBox.Text ?? "").Replace(',', '.');
        if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double distance))
        {
            _log.Add("Viga: vahemaa peab olema number.");
            return;
        }

        _log.Add($"[{garageName}] {vehicle.Move(distance)}");

        // Asendame elemendi samal kohal, et nimekiri uuendaks läbisõitu
        int index = garage.IndexOf(vehicle);
        garage[index] = vehicle;
        listBox.SelectedIndex = index;
    }

    // --- Valimine ---

    private void VehicleListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListBox listBox || listBox.SelectedItem is not Vehicle vehicle)
            return;

        string garageName = listBox == CarsListBox ? "Autode garaaž" : "Paatide garaaž";
        _log.Add($"[{garageName}] Valitud: {vehicle.Describe()}");
    }
}

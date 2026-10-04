namespace Vehicles.Core;

public class Boat : Vehicle
{
    public Boat(string name) : base(name) { }

    public override string Move(double distance)
    {
        string result = base.Move(distance);
        return result.StartsWith("Viga") ? result : "[Paat vees] " + result;
    }

    public override string Describe() => $"Paat: {Name}";
}
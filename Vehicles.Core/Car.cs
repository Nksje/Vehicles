namespace Vehicles.Core;

public class Car : Vehicle
{
    public Car(string name) : base(name) { }

    public override string Move(double distance)
    {
        string result = base.Move(distance);
        return result.StartsWith("Viga") ? result : "[Auto maanteel] " + result;
    }

    public override string Describe() => $"Auto: {Name}";
}
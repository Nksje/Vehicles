namespace Vehicles.Core;

public class AmphibiousCar : Car
{
    public AmphibiousCar(string name) : base(name) { }

    public override string Move(double distance)
    {
        string result = base.Move(distance);
        return result.StartsWith("Viga") ? result : result + " (maal ja vees)";
    }

    public override string Describe() => $"Amfiibauto: {Name}";
}
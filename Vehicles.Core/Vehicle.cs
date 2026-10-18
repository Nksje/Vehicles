namespace Vehicles.Core;

public abstract class Vehicle : IMovable, IDescribable
{
    public string Name { get; }
    public double Mileage { get; private set; }

    protected Vehicle(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nimi ei tohi olla tühi.");
        Name = name.Trim();
        Mileage = 0;
    }

    public virtual string Move(double distance)
    {
        if (double.IsNaN(distance) || double.IsInfinity(distance) || distance <= 0)
            return $"Viga: {Name} ei saa liikuda, vigane vahemaa.";

        Mileage += distance;
        return $"{Name} sõitis {distance} km. Läbisõit: {Mileage} km.";
    }

    public abstract string Describe();

    // Signaal, mida sõiduk annab, kui garaaži tuleb uus sõiduk.
    public virtual string Signal() => $"{Name} annab signaali: Piiks!";

    // Tulede vilgutamine, kui mõni sõiduk garaažist lahkub.
    public virtual string FlashLights() => $"{Name} vilgutab tulesid.";

    public override string ToString() => $"{Describe()} | Läbisõit: {Mileage} km";
}

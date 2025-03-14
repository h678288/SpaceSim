using SpaceSim;

class Astronomy
{
    static void Main()
    {
        var solarSystem = new SolarSystem();

        Console.WriteLine("Enter the number of days since time 0:");
        if (!double.TryParse(Console.ReadLine(), out double time))
        {
            Console.WriteLine("Invalid input. Defaulting to time 0.");
            time = 0;
        }

        Console.WriteLine("Enter the name of a planet (or press Enter to default to the Sun):");
        var planetName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(planetName))
        {
            planetName = "Sun";
        }

        var selectedObject = solarSystem.GetSpaceObjectByName(planetName);
        if (selectedObject == null)
        {
            Console.WriteLine($"No space object found with the name {planetName}. Defaulting to the Sun.");
            selectedObject = solarSystem.GetSpaceObjectByName("Sun");
        }

        DisplayObjectDetails(selectedObject, time);

        if (selectedObject is not (Planet or Star)) return;
        var moons = solarSystem.GetMoonsOfPlanet(selectedObject.Name);
        foreach (var moon in moons)
        {
            DisplayObjectDetails(moon, time);
        }
    }

    private static void DisplayObjectDetails(SpaceObject? obj, double time)
    {
        var position = obj!.CalculatePosition(time);
        Console.WriteLine($"{obj.Name} Details:");
        Console.WriteLine($"  Position: X = {position.X:F2}, Y = {position.Y:F2}");
        Console.WriteLine($"  Orbit Radius: {obj.OrbitRadius} km");
        Console.WriteLine($"  Orbit Period: {obj.OrbitPeriod} days");
        Console.WriteLine($"  Object Radius: {obj.ObjectRadius} km");
        Console.WriteLine($"  Rotation Period: {obj.RotationPeriod}");
        Console.WriteLine($"  Color: {obj.Color}");
        Console.WriteLine();
    }
}


namespace SpaceSim;

public class SpaceObject
{
    public string Name { get; protected set; }
    public double OrbitRadius { get; set; }
    public double OrbitPeriod { get; set; }
    public double ObjectRadius { get; set; }
    public double RotationPeriod { get; set; }
    public string Color { get; set; }
    public string ParentPlanet { get; set; }

    public SpaceObject(string name)
    {
        Name = name;
    }

    public virtual void Draw()
    {
        Console.WriteLine(Name);
    }

    public (double X, double Y) CalculatePosition(double time)
    {
        if (OrbitPeriod == 0)
        {
            return (0, 0);
        }

        double angle = 2 * Math.PI * (time / OrbitPeriod);
        double x = OrbitRadius * Math.Cos(angle);
        double y = OrbitRadius * Math.Sin(angle);
        return (x, y);
    }
}

public class Star : SpaceObject
{
    public Star(string name) : base(name)
    {
    }

    public override void Draw()
    {
        Console.Write("Star: ");
        base.Draw();
    }
}

public class Planet : SpaceObject
{
    public Planet(string name) : base(name)
    {
    }

    public override void Draw()
    {
        Console.Write("Planet: ");
        base.Draw();
    }
}

public class Moon : SpaceObject
{
    public Moon(string name) : base(name)
    {
    }

    public override void Draw()
    {
        Console.Write("Moon: ");
        base.Draw();
    }
}

public class DwarfPlanet : SpaceObject
{
    public DwarfPlanet(string name) : base(name)
    {
    }

    public override void Draw()
    {
        Console.Write("Dwarf Planet: ");
        base.Draw();
    }
}

public class Asteroid : SpaceObject
{
    public Asteroid(string name) : base(name)
    {
    }

    public override void Draw()
    {
        Console.Write("Asteroid: ");
        base.Draw();
    }
}

public class Comet : SpaceObject
{
    public Comet(string name) : base(name)
    {
    }

    public override void Draw()
    {
        Console.Write("Comet: ");
        base.Draw();
    }
}

public class AsteroidBelt : SpaceObject
{
    public AsteroidBelt(string name) : base(name)
    {
    }

    public override void Draw()
    {
        Console.Write("Asteroid Belt: ");
        base.Draw();
    }
}

public class SolarSystem
{
    public List<SpaceObject?> SpaceObjects { get; private set; }

    public SolarSystem()
    {
        SpaceObjects = new List<SpaceObject?>
        {
            new Star("Sun") { ObjectRadius = 1392000, Color = "Yellow" },
            new Planet("Mercury") { OrbitRadius = 57910000, OrbitPeriod = 87.97, ObjectRadius = 2439.7, Color = "#9b9b9b" },
            new Planet("Venus") { OrbitRadius = 108200000, OrbitPeriod = 224.70, ObjectRadius = 6051.8, Color = "#e6e6e6"},
            new Planet("Earth") { OrbitRadius = 149600000, OrbitPeriod = 365.26, ObjectRadius = 6378, Color = "#2f6a69"},
            new Planet("Mars") { OrbitRadius = 227940000, OrbitPeriod = 686.98, ObjectRadius = 3389.5, Color = "#993d00" },
            new Planet("Jupiter") { OrbitRadius = 778330000, OrbitPeriod = 4332.71, ObjectRadius = 699110, Color = "#b07f35" },
            new Planet("Saturn") { OrbitRadius = 1429400000, OrbitPeriod = 10759.50, ObjectRadius = 582320, Color = "#b08f36"},
            new Planet("Uranus") { OrbitRadius = 2870990000, OrbitPeriod = 30685.00, ObjectRadius = 253620, Color = "#5580aa"},
            new Planet("Neptune") { OrbitRadius = 4504300000, OrbitPeriod = 60190.00, ObjectRadius = 246220, Color = "#366896"},
            new Planet("Pluto") { OrbitRadius = 5913520000, OrbitPeriod = 90550.00},
            
            new AsteroidBelt("Main Belt") { OrbitRadius = (double)(227940000 + 778330000) / 2 },
            
            // Earth moon
            new Moon("Moon") { OrbitRadius = 384, OrbitPeriod = 27.32, ParentPlanet = "Earth" },
            
            // Mars moons
            new Moon("Phobos") { OrbitRadius = 9, OrbitPeriod = 0.32, ParentPlanet = "Mars" },
            new Moon("Deimos") { OrbitRadius = 23, OrbitPeriod = 1.26, ParentPlanet = "Mars" },
            
            // Jupiter moons
            new Moon("Io") { OrbitRadius = 422, OrbitPeriod = 1.77, ParentPlanet = "Jupiter" },
            new Moon("Europa") { OrbitRadius = 671, OrbitPeriod = 3.55, ParentPlanet = "Jupiter" },
            new Moon("Ganymede") { OrbitRadius = 1070, OrbitPeriod = 7.15, ParentPlanet = "Jupiter" },
            new Moon("Callisto") { OrbitRadius = 1883, OrbitPeriod = 16.69, ParentPlanet = "Jupiter" },
            
            // Saturn moons
            new Moon("Mimas") { OrbitRadius = 185, OrbitPeriod = 0.94, ParentPlanet = "Saturn" },
            new Moon("Enceladus") { OrbitRadius = 238, OrbitPeriod = 1.37, ParentPlanet = "Saturn" },
            new Moon("Tethys") { OrbitRadius = 294, OrbitPeriod = 1.89, ParentPlanet = "Saturn" },
            new Moon("Dione") { OrbitRadius = 377, OrbitPeriod = 2.74, ParentPlanet = "Saturn" },
            new Moon("Rhea") { OrbitRadius = 527, OrbitPeriod = 4.52, ParentPlanet = "Saturn" },
            new Moon("Pandora") { OrbitRadius = 141, OrbitPeriod = 0.63, ParentPlanet = "Saturn" },
            new Moon("Titan") { OrbitRadius = 1222, OrbitPeriod = 15.95, ParentPlanet = "Saturn" },
            
            // Uranus moons
            new Moon("Miranda") { OrbitRadius = 130, OrbitPeriod = 1.41, ParentPlanet = "Uranus" },
            new Moon("Ariel") { OrbitRadius = 191, OrbitPeriod = 2.52, ParentPlanet = "Uranus" },
            new Moon("Umbriel") { OrbitRadius = 266, OrbitPeriod = 4.14, ParentPlanet = "Uranus" },
            new Moon("Titania") { OrbitRadius = 436, OrbitPeriod = 8.71, ParentPlanet = "Uranus" },
            new Moon("Oberon") { OrbitRadius = 583, OrbitPeriod = 13.46, ParentPlanet = "Uranus" },
            
            // Neptune moons
            new Moon("Triton") { OrbitRadius = 355, OrbitPeriod = -5.88, ParentPlanet = "Neptune" },
            new Moon("Nereid") { OrbitRadius = 550, OrbitPeriod = 11.38, ParentPlanet = "Neptune" },
            new Moon("Proteus") { OrbitRadius = 117, OrbitPeriod = 1.12, ParentPlanet = "Neptune" },
            new Moon("Larissa") { OrbitRadius = 97, OrbitPeriod = 0.31, ParentPlanet = "Neptune" },
            
            // Pluto moons
            new Moon("Charon") { OrbitRadius = 20, OrbitPeriod = 6.39, ParentPlanet = "Pluto" },
            new Moon("Hydra") { OrbitRadius = 65, OrbitPeriod = 9.66, ParentPlanet = "Pluto" },
            new Moon("Nix") { OrbitRadius = 48, OrbitPeriod = 24.93, ParentPlanet = "Pluto" },
            
        };
    }

    public SpaceObject? GetSpaceObjectByName(string? name)
    {
        return SpaceObjects.Find(obj => obj != null && obj.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public List<SpaceObject?> GetMoonsOfPlanet(string planetName)
    {
        return SpaceObjects.FindAll(obj =>
            obj is Moon && obj.ParentPlanet.Equals(planetName, StringComparison.OrdinalIgnoreCase));
    }
}
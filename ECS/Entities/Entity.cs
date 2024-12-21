namespace Blokus.ECS.Entities;

using Coord = (int X, int Y);

public abstract class Entity()
{
    public int EntityID { get; set; }
    public virtual string Name { get; set; } = "";
    public Coord Position { get; set; }
    public string Color { get; set; } = "orange";
    public int? PlayerID { get; set; }

    public virtual List<Coord>? Cells { get; set; }


    public override string ToString()
    {
        string res = $"#{EntityID}, Name: {Name}, At: {Position}, ";
        if (Color != null) res += $"Color: {Color}\n";
        if (PlayerID != null) res += $"PlayerID: {PlayerID}";

        return res;
    }
}
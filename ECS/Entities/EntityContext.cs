namespace Blokus.ECS.Entities;

using Blokus.DataStructures;

using Coord = (int X, int Y);

public class EntityContext()
{
    private int _nextEntityID = 0;
    public int NextEntityID
    {
        get { return _nextEntityID++; }
    }

    public SparseGrid<Entity> Grid { get; set; } = new((Entity)null);
    public Dict<int, Entity> Entities { get; set; } = new(null);

    private List<Entity> ToAdd { get; set; } = [];
    private List<Entity> ToRemove { get; set; } = [];

    public Random Rand { get; set; } = new();


    public Entity this[int entityID]
    {
        get => this.Entities[entityID];
    }


    public void ForEach(Action<Entity> f)
    {
        foreach (var entity in Entities.Values)
        {
            f(entity);
        }
        OnAfterUpdate();
    }

    public Piece? GetPlayerPiece(int playerID)
    {
        return (Piece?)Entities.Values
            .FirstOrDefault(e => e.Name == "Piece" && e.PlayerID == playerID);
    }

    public void SetPlayerPiece(int playerID, Piece piece)
    {
        Piece? CurPiece = (Piece?)Entities.Values
            .FirstOrDefault(e => e.Name == "Piece" && e.PlayerID == playerID);
        if (CurPiece != null) Remove(CurPiece);
        piece.Position = (-5, -5);
        Add(piece);
        OnAfterUpdate();
    }

    public List<Entity> GetNeighbors(Entity entity, bool includesDiagonal = false)
    {
        return Grid.GetNeighbors(entity.Position, includesDiagonal)
            .Select(Grid.At).Where(n => n != null).ToList()!;
    }

    public List<Coord> GetEmptyNeighbors(Entity entity, bool includesDiagonal = false)
    {
        return Grid.GetNeighbors(entity.Position, includesDiagonal)
            .Where(c => Grid.At(c) == null).ToList();
    }

    public void OnAfterUpdate()
    {
        foreach (var entity in ToAdd)
        {
            entity.EntityID = NextEntityID;
            Entities.Add(entity.EntityID, entity);
            if (entity.Name != "Piece") Grid.Set(entity.Position, entity);
        }
        foreach (var entity in ToRemove)
        {
            Entities.Remove(entity.EntityID);
            if (entity.Name != "Piece") Grid.Remove(entity.Position);
        }
        ToAdd = [];
        ToRemove = [];
    }


    public void Move(Entity entity, Coord To)
    {
        if (entity.Name != "Piece") Grid.Remove(entity.Position);
        entity.Position = To;
        if (entity.Name != "Piece") Grid.Set(entity.Position, entity);
    }
    public void Add(Entity entity)
    {
        ToAdd.Add(entity);
    }
    public void Remove(Entity entity)
    {
        ToRemove.Add(entity);
    }

}
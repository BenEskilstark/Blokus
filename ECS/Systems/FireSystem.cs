namespace Blokus.ECS.Systems;

using Blokus.ECS.Entities;

using Coord = (int X, int Y);

public class PieceSystem() : System()
{
    public override bool IsRelevant(Entity entity)
    {
        return entity.Name == "Piece";
    }

    public override void Update(EntityState entities)
    {
        entities.ForEach(entity =>
        {
            if (!IsRelevant(entity)) return;
        });
    }
}
namespace Blokus.ECS.Systems;

using Blokus.ECS.Entities;

public class FuelSystem() : System()
{
    public override bool IsRelevant(Entity entity)
    {
        // return entity.Fuel != null;
        return false;
    }

    public override void Update(EntityContext entities)
    {
        entities.ForEach(entity =>
        {
            if (!IsRelevant(entity)) return;
        });
    }
}
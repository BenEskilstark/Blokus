namespace Blokus.ECS.Systems;

using Blokus.ECS.Entities;

public abstract class System
{
    public abstract bool IsRelevant(Entity entity);
    public abstract void Update(EntityContext entities);
}
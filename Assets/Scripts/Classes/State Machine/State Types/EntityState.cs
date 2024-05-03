using UnityEngine;

public abstract class EntityState : State
{
    public EntityCore EntityCore => (EntityCore)Core;
    public Entity Entity => EntityCore.Entity;
}
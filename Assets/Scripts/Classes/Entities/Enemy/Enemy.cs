using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : Entity
    {
        public Entity Target;
        protected Transform _worldCenter;

        protected virtual void Start()
        {
            EnemiesManager.Instance.Enemies.Add(this);
            _worldCenter = EnemiesManager.Instance.WorldCenter;
            Target = Player.Instance;
        }

        public override void Die()
        {
            base.Die();
            EnemiesManager.Instance.Enemies.Remove(this);
        }
    }
}

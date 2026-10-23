using Byte.Collidable;
using Byte.Player;
using Byte.Projectile;
using Byte.Sprite.Enemy;

public static class ProjectileCollisionHandler
{
    public static void Register(CollisionDispatcher dispatcher)
    {
        dispatcher.Register(CollidableTypes.Projectile, CollidableTypes.Enemy, (a, b, side) =>
        {
            if (a is IProjectile proj) proj.Collided();
            if (b is AbstractEnemy enemy)
            {
                enemy.TakeDamage(); 
            }
        });

        dispatcher.Register(CollidableTypes.Projectile, CollidableTypes.Block, (a, b, side) =>
        {
            if (a is IProjectile proj) proj.Collided();
        });

        dispatcher.Register(CollidableTypes.EnemyProjectile, CollidableTypes.Player, (a, b, side) =>
        {
            if (a is IProjectile proj) proj.Collided();
            if (b is Link link)
            {
                link.TakeDamage();
            }
        });
    }
}
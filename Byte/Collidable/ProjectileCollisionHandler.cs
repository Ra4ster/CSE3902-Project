using Byte.Collidable;
using Byte.Player;
using Byte.Projectile;
using Byte.Rooms;
using Byte.Sprite.Enemy;

public static class ProjectileCollisionHandler
{
    public static void Register(CollisionDispatcher dispatcher)
    {
        dispatcher.Register(CollidableTypes.Projectile, CollidableTypes.Enemy, (a, b, side) =>
        {
            if (a is IProjectile proj)
            {
                if(proj is Boomerang)
                {
                    Program.soundManager.StopBoomerangSound(proj);
                }
                proj.Collided();

            }
            if (b is AbstractEnemy enemy)
            {
                enemy.TakeDamage();
            }
        });

        dispatcher.Register(CollidableTypes.Projectile, CollidableTypes.Block, (a, b, side) =>
        {
            if (a is IProjectile proj && b.Properties.BlocksPlayer) proj.Collided();
        });

        dispatcher.Register(CollidableTypes.Projectile, CollidableTypes.Door, (a, b, side) =>
        {
            if (a is Bomb bomb && b is Door door)
            {
                if (door.TryBlowOpen())
                {
                    Program.soundManager.PlaySound("Secret");
                }
            }
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
using Byte.Player;
using Byte.Sprite.Enemy;
using Microsoft.Xna.Framework;

namespace Byte.Collidable;

public static class LinkCollisionHandler
{
    public static void Register(CollisionDispatcher dispatcher)
    {
        dispatcher.Register(CollidableTypes.Player, CollidableTypes.Enemy, DamagePlayer);
        dispatcher.Register(CollidableTypes.Player, CollidableTypes.Block, ResolveBlock);
        dispatcher.Register(CollidableTypes.Player, CollidableTypes.Door,
            (player, door, side) => door.Properties.OnCollide?.Invoke(player));
        dispatcher.Register(CollidableTypes.Sword, CollidableTypes.Enemy,
            (sword, enemy, side) =>
            {
                if (sword is SwordHitbox hitbox && enemy is AbstractEnemy target)
                    hitbox.Hit(target);
            });
    }

    private static void DamagePlayer(ICollidable player, ICollidable enemy, CollisionSide side)
    {
        if (player is Link link && enemy.Properties.DamagesPlayer)
            link.TakeDamage();
    }

    private static void ResolveBlock(ICollidable player, ICollidable block, CollisionSide side)
    {
        if (player is not Link link || !block.Properties.BlocksPlayer)
            return;

        Rectangle overlap = Rectangle.Intersect(link.BoundingBox, block.BoundingBox);
        if (overlap.Width <= 0 || overlap.Height <= 0)
            return;

        Vector2 correction = side switch
        {
            CollisionSide.Right => new Vector2(-overlap.Width, 0),
            CollisionSide.Left => new Vector2(overlap.Width, 0),
            CollisionSide.Bottom => new Vector2(0, -overlap.Height),
            CollisionSide.Top => new Vector2(0, overlap.Height),
            _ => Vector2.Zero
        };
        link.CorrectPosition(link.Position + correction);
    }

    public static void ClampToRoom(Link link, Rectangle roomBounds)
    {
        Rectangle body = link.BoundingBox;
        Vector2 offset = link.Position - new Vector2(body.Left, body.Top);
        Vector2 min = new Vector2(roomBounds.Left, roomBounds.Top) + offset;
        Vector2 max = new Vector2(roomBounds.Right - body.Width, roomBounds.Bottom - body.Height) + offset;
        link.CorrectPosition(Vector2.Clamp(link.Position, min, Vector2.Max(min, max)));
    }
}

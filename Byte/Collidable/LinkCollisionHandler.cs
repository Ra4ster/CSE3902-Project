using Byte.Item;
using Byte.Player;
using Byte.Sprite.Enemy;
using Microsoft.Xna.Framework;

namespace Byte.Collidable;

public static class LinkCollisionHandler
{
    // Link's hitbox is exactly one tile, so door gaps need some help lining up.
    public const int CornerAssist = 24;

    public static void Register(CollisionDispatcher dispatcher)
    {
        dispatcher.Register(CollidableTypes.Player, CollidableTypes.Enemy, DamagePlayer);
        dispatcher.Register(CollidableTypes.Player, CollidableTypes.Block, ResolveBlock);
        dispatcher.Register(CollidableTypes.Player, CollidableTypes.Door,
            (player, door, side) => door.Properties.OnCollide?.Invoke(player));
        dispatcher.Register(CollidableTypes.Player, CollidableTypes.ItemPickup, PickUpItem);
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

    private static void PickUpItem(ICollidable player, ICollidable pickup, CollisionSide side)
    {
        if (player is Link && pickup is IItem item)
            item.Collect();
    }

    private static void ResolveBlock(ICollidable player, ICollidable block, CollisionSide side)
    {
        if (player is not Link link || !block.Properties.BlocksPlayer)
            return;

        Rectangle overlap = Rectangle.Intersect(link.BoundingBox, block.BoundingBox);
        if (overlap.Width <= 0 || overlap.Height <= 0)
            return;

        bool horizontalHit = side is CollisionSide.Left or CollisionSide.Right;
        Vector2 correction;
        if (horizontalHit && overlap.Height <= CornerAssist)
            correction = new Vector2(0, link.BoundingBox.Center.Y < block.BoundingBox.Center.Y ? -overlap.Height : overlap.Height);
        else if (!horizontalHit && overlap.Width <= CornerAssist)
            correction = new Vector2(link.BoundingBox.Center.X < block.BoundingBox.Center.X ? -overlap.Width : overlap.Width, 0);
        else
            correction = side switch
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

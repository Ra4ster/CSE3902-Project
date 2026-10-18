using Byte.Player;
using Byte.Sprite.Enemy;
using Microsoft.Xna.Framework;

namespace Byte.Collidable;

// A separate collider keeps sword contact from being treated as body contact.
public sealed class SwordHitbox : ICollidable
{
    private readonly HashSet<AbstractEnemy> hitEnemies = new();
    public Rectangle BoundingBox { get; private set; }
    public CollisionProperties Properties { get; } = new CollisionProperties
    {
        Type = CollidableTypes.Sword,
        DamagesEnemies = true
    };

    public void BeginAttack() => hitEnemies.Clear();

    public void Update(Link link)
    {
        BoundingBox = Rectangle.Empty;
        if (link.state is not PlayerAttackState || !link.IsAttacking ||
            link.currentSprite is not MovingAnimatedSprite sprite ||
            sprite.currentFrame is not (1 or 2))
            return;

        Rectangle body = link.BoundingBox;
        int reach = 10 * Link.SpriteScale;
        int width = 8 * Link.SpriteScale;
        if (link.Direction == CardinalDirections.North)
            BoundingBox = new Rectangle(body.Center.X - width / 2, body.Top - reach, width, reach);
        else if (link.Direction == CardinalDirections.South)
            BoundingBox = new Rectangle(body.Center.X - width / 2, body.Bottom, width, reach);
        else if (link.Direction == CardinalDirections.West)
            BoundingBox = new Rectangle(body.Left - reach, body.Center.Y - width / 2, reach, width);
        else
            BoundingBox = new Rectangle(body.Right, body.Center.Y - width / 2, reach, width);
    }

    public void Hit(AbstractEnemy enemy)
    {
        if (!BoundingBox.IsEmpty && BoundingBox.Intersects(enemy.BoundingBox) && hitEnemies.Add(enemy))
            enemy.TakeDamage();
    }
}

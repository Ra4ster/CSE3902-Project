using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Projectile;

// makes every projectile, only place the sheet coordinates should live
public class ProjectileFactory
{
    // projectile sprites are in the "Other Weapons/Items" row of the link sheet
    private static readonly Rectangle ARROW_UP = new Rectangle(1, 185, 8, 16);
    private static readonly Rectangle ARROW_RIGHT = new Rectangle(10, 185, 16, 16);
    private static readonly Rectangle[] BOOMERANG_FRAMES =
    {
        new Rectangle(64, 185, 8, 16),
        new Rectangle(73, 185, 8, 16),
        new Rectangle(82, 185, 8, 16)
    };
    private static readonly Rectangle BOMB = new Rectangle(129, 185, 8, 16);
    private static readonly Rectangle[] EXPLOSION_FRAMES =
    {
        new Rectangle(138, 185, 16, 16),
        new Rectangle(155, 185, 16, 16),
        new Rectangle(172, 185, 16, 16)
    };

    private const float SCALE = 6.0f;
    private const float BOOMERANG_FRAME_DURATION = 0.08f;
    private const float EXPLOSION_FRAME_DURATION = 0.1f;

    private readonly Texture2D sheet;

    public ProjectileFactory(Texture2D sheet)
    {
        this.sheet = sheet;
    }

    // direction is a unit vector in screen space (+Y is down)
    public IProjectile CreateArrow(Vector2 position, Vector2 direction)
    {
        bool isHorizontal = direction.X != 0;
        SpriteEffects flip = SpriteEffects.None;
        if (direction.X < 0)
        {
            flip = SpriteEffects.FlipHorizontally;
        }
        else if (direction.Y > 0)
        {
            flip = SpriteEffects.FlipVertically;
        }

        MovingAnimatedSprite sprite = CreateSprite(new[] { isHorizontal ? ARROW_RIGHT : ARROW_UP }, 1f, position);
        sprite.Effects = flip;
        return new Arrow(sprite, position, direction);
    }

    public IProjectile CreateBoomerang(Vector2 position, Vector2 direction)
    {
        MovingAnimatedSprite sprite = CreateSprite(BOOMERANG_FRAMES, BOOMERANG_FRAME_DURATION, position);
        return new Boomerang(sprite, position, direction);
    }

    public IProjectile CreateBomb(Vector2 position)
    {
        MovingAnimatedSprite bombSprite = CreateSprite(new[] { BOMB }, 1f, position);
        MovingAnimatedSprite explosionSprite = CreateSprite(EXPLOSION_FRAMES, EXPLOSION_FRAME_DURATION, position);
        explosionSprite.Loop = false;
        return new Bomb(bombSprite, explosionSprite, position);
    }

    // sprites are centered on the projectile's position
    private MovingAnimatedSprite CreateSprite(Rectangle[] frames, float frameDuration, Vector2 position)
    {
        MovingAnimatedSprite sprite = new MovingAnimatedSprite(sheet, position, Vector2.Zero, Color.White, frames, frameDuration, SCALE);
        sprite.MoveOrigin(new Vector2(frames[0].Width / 2f, frames[0].Height / 2f));
        return sprite;
    }
}

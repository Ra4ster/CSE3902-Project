using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Projectile;

// sits where it was placed until the fuse runs out, then plays the explosion once and disappears
public class Bomb : IProjectile
{
    private const float FUSE_SECONDS = 1.0f;

    private readonly MovingAnimatedSprite bombSprite;
    private readonly MovingAnimatedSprite explosionSprite;
    private float fuseTimer;
    private bool hasExploded;

    public bool IsExpired => hasExploded && explosionSprite.IsFinished;

    public Bomb(MovingAnimatedSprite bombSprite, MovingAnimatedSprite explosionSprite, Vector2 position)
    {
        this.bombSprite = bombSprite;
        this.explosionSprite = explosionSprite;
        bombSprite.SetPos(position);
        explosionSprite.SetPos(position);
    }

    public void Update(GameTime gameTime)
    {
        if (hasExploded)
        {
            explosionSprite.Update(gameTime);
            return;
        }

        fuseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        hasExploded = fuseTimer >= FUSE_SECONDS;
        bombSprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (hasExploded)
        {
            explosionSprite.Draw(spriteBatch);
        }
        else
        {
            bombSprite.Draw(spriteBatch);
        }
    }
}

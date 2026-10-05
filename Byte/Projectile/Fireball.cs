using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Projectile;

// aquamentus's attack, flies straight and disappears after a set distance or off screen
public class Fireball : IProjectile
{
    private const float SPEED = 400f;
    private const float MAX_DISTANCE = 1200f;

    private readonly MovingAnimatedSprite sprite;
    private readonly Vector2 velocity;
    private Vector2 position;
    private float distanceTraveled;

    public bool IsExpired { get; private set; }

    public Fireball(MovingAnimatedSprite sprite, Vector2 position, Vector2 direction)
    {
        this.sprite = sprite;
        this.position = position;
        velocity = Vector2.Normalize(direction) * SPEED;
    }

    public void Update(GameTime gameTime)
    {
        float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        position += velocity * seconds;
        distanceTraveled += SPEED * seconds;

        if (distanceTraveled >= MAX_DISTANCE || !GameConstants.WINDOW_SIZE.Contains(position))
        {
            IsExpired = true;
        }

        sprite.SetPos(position);
        sprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch) => sprite.Draw(spriteBatch);
}

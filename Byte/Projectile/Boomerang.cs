using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Projectile;

// flies out to its range, turns around, and disappears when it gets back to where it was thrown
public class Boomerang : IProjectile
{
    private const float SPEED = 600f;
    private const float RANGE = 450f;

    private readonly MovingAnimatedSprite sprite;
    private Vector2 velocity;
    private Vector2 position;
    private float distanceTraveled;
    private bool isReturning;

    public bool IsExpired { get; private set; }

    public Boomerang(MovingAnimatedSprite sprite, Vector2 position, Vector2 direction)
    {
        this.sprite = sprite;
        this.position = position;
        velocity = direction * SPEED;
    }

    public void Update(GameTime gameTime)
    {
        float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        position += velocity * seconds;
        distanceTraveled += SPEED * seconds;

        if (!isReturning && distanceTraveled >= RANGE)
        {
            isReturning = true;
            velocity = -velocity;
        }
        else if (isReturning && distanceTraveled >= 2 * RANGE)
        {
            IsExpired = true;
        }

        sprite.SetPos(position);
        sprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch) => sprite.Draw(spriteBatch);
}

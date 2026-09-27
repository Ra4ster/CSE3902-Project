using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Projectile;

// flies straight in one direction and disappears after a set distance or off screen
public class Arrow : IProjectile
{
    private const float SPEED = 800f;
    private const float MAX_DISTANCE = 700f;

    private readonly MovingAnimatedSprite sprite;
    private readonly Vector2 velocity;
    private Vector2 position;
    private float distanceTraveled;

    public bool IsExpired { get; private set; }

    public Arrow(MovingAnimatedSprite sprite, Vector2 position, Vector2 direction)
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

        if (distanceTraveled >= MAX_DISTANCE || !Game.WINDOW_SIZE.Contains(position))
        {
            IsExpired = true;
        }

        sprite.SetPos(position);
        sprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch) => sprite.Draw(spriteBatch);
}

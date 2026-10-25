using Byte.Collidable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy;

/// <summary>
/// An abstract enemy; an instance of a moving, animated sprite with a vengeance...
/// </summary>
public abstract class AbstractEnemy : ICollidable
{
    protected int health
    {
        get; set;
    } = 3;
    private float damageTimer;
    public int Health => health;
    public bool IsDefeated => health <= 0;
    public bool IsDamaged => damageTimer > 0;
    public Rectangle BoundingBox => new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    public CollisionProperties Properties { get; } = new CollisionProperties
    {
        Type = CollidableTypes.Enemy,
        DamagesPlayer = true
    };

    public void TakeDamage()
    {
        if (IsDefeated) return;
        health = Math.Max(0, health - 1);
        damageTimer = 0.3f;
    }

    public void UpdateDamage(GameTime gameTime)
        => damageTimer = Math.Max(0, damageTimer - (float)gameTime.ElapsedGameTime.TotalSeconds);

    public virtual bool IsBlockedByBlocks => true;

    public virtual void OnBlocked(Vector2 correction)
    {
        position += correction;
        if (enemySprite is MovingAnimatedSprite animatedSprite)
            animatedSprite.SetPos(position);
        else if (enemySprite is MovingSprite movingSprite)
            movingSprite.SetPos(position);
    }

    public Vector2 position;
    public Vector2 startPos;
    protected Vector2 velocity;

    protected ISprite enemySprite = null!;
    protected abstract void Move(GameTime gameTime);

    // on-screen size of the largest frame, used to keep the whole sprite inside the window
    private readonly Vector2 size;

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Color originalColor = enemySprite.Color;
        if (IsDamaged) enemySprite.Color = Color.Red;
        enemySprite.Draw(spriteBatch);
        enemySprite.Color = originalColor;
    }

    public virtual void Update(GameTime gameTime)
    {
        UpdateDamage(gameTime);
        Move(gameTime);

        if (enemySprite is MovingSprite movingSprite)
        {
            movingSprite.Update(gameTime);
            return;
        }

        float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        position = ClampToWindow(position + velocity * seconds);

        if (enemySprite is MovingAnimatedSprite movingAnimatedSprite)
            movingAnimatedSprite.SetPos(position);

        enemySprite.Update(gameTime);
    }

    // Uses inherited constructor
    protected AbstractEnemy(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color,
    Rectangle[] sourceRectangles, float frameDuration, float scale = 1.0f)
    {
        foreach (Rectangle frame in sourceRectangles)
            size = Vector2.Max(size, new Vector2(frame.Width, frame.Height) * scale);

        this.position = ClampToWindow(position);
        this.startPos = position;
        this.velocity = velocity;
        enemySprite = new MovingAnimatedSprite(texture, ref position, ref velocity, color, sourceRectangles, frameDuration, scale);
    }

    protected AbstractEnemy(ref Vector2 position, ref Vector2 velocity, Rectangle sourceRectangle, float scale = 1.0f)
    {
        size = new Vector2(sourceRectangle.Width, sourceRectangle.Height) * scale;

        this.position = ClampToWindow(position);
        this.velocity = velocity;
    }

    /// <summary>
    /// Moves a point so the sprite drawn there stays fully inside GameConstants.WINDOW_SIZE.
    /// </summary>
    protected Vector2 ClampToWindow(Vector2 point)
    {
        Rectangle window = GameConstants.WINDOW_SIZE;
        Vector2 min = new Vector2(window.Left, window.Top);
        Vector2 max = Vector2.Max(min, new Vector2(window.Right, window.Bottom) - size);
        return Vector2.Clamp(point, min, max);
    }
}
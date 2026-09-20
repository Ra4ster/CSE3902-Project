
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Sprite;

/// <summary>
/// Moving, non-animating sprite.
/// </summary>
public class MovingSprite : ISprite

{
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    private Vector2 position;
    private Texture2D texture;
    public Vector2 Velocity { get; set; }
    public Color Color { get; set; }
    private float scale;
    public readonly Rectangle sourceRectangle;

    public MovingSprite(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color, ref Rectangle sourceRectangle, float scale = 1f)
    {
        this.position = position;
        Color = color;
        this.texture = texture;
        Velocity = velocity;
        this.sourceRectangle = sourceRectangle;
        this.scale = scale;
    }

    public void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, position, sourceRectangle, Color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

    public void Update(GameTime gameTime)
    {
        float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (position.X < Game.WINDOW_SIZE.Left || position.X > Game.WINDOW_SIZE.Right - (sourceRectangle.Width * scale))
            Velocity = new Vector2(Velocity.X * -1.0f, Velocity.Y);

        position.X += Velocity.X * seconds;

        if (position.Y > Game.WINDOW_SIZE.Bottom - (sourceRectangle.Height * scale) || position.Y < Game.WINDOW_SIZE.Top)
            Velocity = new Vector2(Velocity.X, Velocity.Y * -1.0f);

        position.Y += Velocity.Y * seconds;
    }

    public void SetPos(int x, int y)
    {
        position.X = x;
        position.Y = y;
    }
}
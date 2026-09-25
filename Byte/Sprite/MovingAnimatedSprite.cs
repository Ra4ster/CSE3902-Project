using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Byte.Sprite;

/// <summary>
/// Moving, animating sprite
/// </summary>
public class MovingAnimatedSprite : ISprite
{
    public bool Loop { get; set; } = true;
    public bool IsFinished { get; set; } = false;
    private Vector2[]? frameOrigins;
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    protected Vector2 position;
    protected Vector2 velocity;
    protected Texture2D texture;
    public Color Color { get; set; }
    protected Color color { get => Color; set => Color = value; }

    public float scale;
    public Vector2 Origin { get; set; } = Vector2.Zero;

    private Rectangle[] sourceRectangles;

    public int currentFrame;
    private float animationTimer = 0;
    private float frameDuration;

    public MovingAnimatedSprite(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color,
        Rectangle[] sourceRectangles, float frameDuration, float scale = 1.0f, SpriteEffects spriteEffects = SpriteEffects.None)
    {
        this.position = position;
        this.velocity = velocity;
        this.texture = texture;
        Color = color;
        this.sourceRectangles = sourceRectangles;
        this.frameDuration = frameDuration;
        this.scale = scale;
        Effects = spriteEffects;
    }

    public MovingAnimatedSprite(Texture2D texture, Vector2 position, Vector2 velocity, Color color,
        Rectangle[] sourceRectangles, float frameDuration, float scale = 1.0f)
        : this(texture, ref position, ref velocity, color, sourceRectangles, frameDuration, scale) { }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Vector2 frameOrigin = Origin;
        if(frameOrigins != null)
        {
            frameOrigin = frameOrigins[currentFrame];
        }

        spriteBatch.Draw(texture, position, sourceRectangles[currentFrame], Color, 0f, frameOrigin, scale, Effects, 0f);
    }

    public virtual void Update(GameTime gameTime)
    {
        float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

        position += velocity * seconds;

      
        animationTimer += seconds;

        if (animationTimer >= frameDuration)
        {
            animationTimer -= frameDuration;
            if(currentFrame < sourceRectangles.Length - 1)
            {
                currentFrame++;
            }else if (Loop)
            {
                currentFrame = 0;
            }
            else
            {
                IsFinished = true;
            }
            
        }
    }

    public void SetPos(Vector2 position)
    {
        this.position = position;

    }
    public void MoveOrigin(Vector2 origin)
    {

        Origin = origin;
    }
    public void SetFrameOrigin(Vector2[] Origins)
    {
        frameOrigins = Origins;
    }
}
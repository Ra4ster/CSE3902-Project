
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Sprite;

/// <summary>
/// Moving, animating sprite
/// </summary>
public class MovingAnimatedSprite : ISprite
{
    public bool Loop { get; set; } = true;
    public bool IsFinished { get; set; } = false;
    private Vector2[]? frameOrigins;
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    private Vector2 position;
    private Vector2 velocity;
    private Texture2D texture;
    public Color Color { get; set; }

    private float scale;
    public Vector2 Origin { get; set; } = Vector2.Zero;

    private Rectangle[] sourceRectangles;

    public int currentFrame;
    private float animationTimer = 0;
    private float frameDuration;

    public MovingAnimatedSprite(Texture2D texture, Vector2 position,  Vector2 velocity, Color color,
    Rectangle[] sourceRectangles, float frameDuration, float scale = 1.0f)
    {
        this.texture = texture;
        this.position = position;
        this.velocity = velocity;
        Color = color;
        this.sourceRectangles = sourceRectangles;
        this.frameDuration = frameDuration;
        this.scale = scale;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Vector2 frameOrigin = Origin;
        if(frameOrigins != null)
        {
            frameOrigin = frameOrigins[currentFrame];
        }

        spriteBatch.Draw(texture, position, sourceRectangles[currentFrame], Color, 0f, frameOrigin, scale, Effects, 0f);
    }

    public void Update(GameTime gameTime)
    {
        float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

      
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
        //position.X = x;
        //position.Y = y;
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
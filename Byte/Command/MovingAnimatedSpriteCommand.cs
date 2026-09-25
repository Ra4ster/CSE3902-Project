
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Moving and animated sprite.
/// </summary>
public class MovingAnimatedSpriteCommand : ICommand
{
    private Game game;
    private Texture2D texture;
    private Vector2 position;
    private Vector2 velocity;
    private Color color;
    private Rectangle[] sourceRects;
    private float frameDuration;
    private float scale;

    public MovingAnimatedSpriteCommand(Game game, Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color, Rectangle[] sourceRects, float frameDuration, float scale = 1f)
    {
        this.game = game;
        this.texture = texture;
        this.position = position;
        this.velocity = velocity;
        this.color = color;
        this.sourceRects = sourceRects;
        this.frameDuration = frameDuration;
        this.scale = scale;
    }

    public void Execute(GameTime gameTime) => game.linkSprite = new MovingAnimatedSprite(texture, position, velocity, color, sourceRects, frameDuration, scale);
}
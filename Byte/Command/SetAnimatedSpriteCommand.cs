
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Sprite;

public class SetAnimatedSpriteCommand : ICommand
{
    private Game game;

    private Texture2D texture;
    private Vector2 position;
    private Color color;
    private Rectangle[] sourceRects;
    private float frameDuration;
    private float scale;

    public SetAnimatedSpriteCommand(Game game, Texture2D texture, ref Vector2 position, Color color, Rectangle[] sourceRects, float frameDuration, float scale = 1f)
    {
        this.game = game;
        this.texture = texture;
        this.position = position;
        this.color = color;
        this.sourceRects = sourceRects;
        this.frameDuration = frameDuration;
        this.scale = scale;
    }

    public void Execute(GameTime gameTime) => game.ActiveSprite = new AnimatedSprite(texture, ref position, color, sourceRects, frameDuration, scale);
}
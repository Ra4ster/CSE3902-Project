
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Sprite;

public class SetStaticSpriteCommand : ICommand
{
    private Game game;
    private Texture2D texture;
    private Vector2 position;
    private Color color;
    private Rectangle rectangle;
    private float scale;

    public SetStaticSpriteCommand(Game game, Texture2D texture, ref Vector2 position, Color color, ref Rectangle rectangle, float scale = 1f)
    {
        this.game = game;
        this.texture = texture;
        this.position = position;
        this.color = color;
        this.rectangle = rectangle;
        this.scale = scale;
    }

    public void Execute() => game.ActiveSprite = new StaticSprite(texture, position, color, rectangle, scale);
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SetTextSpriteCommand : ICommand
{
    private Game game;

    private SpriteFont spriteFont;
    private string text;
    private Vector2 position;
    private Color color;
    private float scale;

    public SetTextSpriteCommand(Game game, SpriteFont spriteFont, string text, ref Vector2 position, Color color, float scale = 1f)
    {
        this.game = game;
        this.spriteFont = spriteFont;
        this.text = text;
        this.position = position;
        this.color = color;
        this.scale = scale;
    }
    public void Execute()
    {
        throw new NotImplementedException();
    }
}
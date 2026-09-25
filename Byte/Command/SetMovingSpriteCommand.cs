using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SetMovingSpriteCommand : ICommand
{
    private Game game;
    private Texture2D texture;
    private Vector2 position;
    private Color color;
    private Vector2 velocity;
    private Rectangle sourceRect;
    private float scale;

    public SetMovingSpriteCommand(Game game, Texture2D texture, ref Vector2 position, Color color, ref Vector2 velocity, ref Rectangle sourceRect, float scale = 1f)
    {
        this.game = game;
        this.texture = texture;
        this.position = position;
        this.color = color;
        this.velocity = velocity;
        this.sourceRect = sourceRect;
        this.scale = scale;
    }

    public void Execute(GameTime gameTime) => game.linkSprite = new MovingSprite(texture, ref position, ref velocity, color, ref sourceRect, scale);
}
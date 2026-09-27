using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Block;

// shared logic for every block - a sprite drawn at a fixed spot
public abstract class AbstractBlock : IBlock
{
    private readonly ISprite sprite;

    public Vector2 Position { get; }

    protected AbstractBlock(ISprite sprite, Vector2 position)
    {
        this.sprite = sprite;
        Position = position;
    }

    public void Update(GameTime gameTime) => sprite.Update(gameTime);

    public void Draw(SpriteBatch spriteBatch) => sprite.Draw(spriteBatch);
}

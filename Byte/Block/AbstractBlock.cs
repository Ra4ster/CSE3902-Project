using Byte.Sprite;
using Byte.Collidable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Block;

// shared logic for every block - a sprite drawn at a fixed spot
public abstract class AbstractBlock : IBlock, ICollidable
{
    private readonly ISprite sprite;
    private const int BlockSize = GameConstants.TILE_SIZE * 6;

    public Vector2 Position { get; }
    public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, BlockSize, BlockSize);
    public CollisionProperties Properties { get; } = new CollisionProperties
    {
        Type = CollidableTypes.Block,
        IsStatic = true,
        BlocksPlayer = true
    };

    protected AbstractBlock(ISprite sprite, Vector2 position)
    {
        this.sprite = sprite;
        Position = position;
    }

    public void Update(GameTime gameTime) => sprite.Update(gameTime);

    public virtual void Draw(SpriteBatch spriteBatch) => sprite.Draw(spriteBatch);
}

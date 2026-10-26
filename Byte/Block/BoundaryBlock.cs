using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Block
{
    internal class BoundaryBlock : AbstractBlock
    {
        public BoundaryBlock(ISprite sprite, Vector2 position) : base(sprite, position) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            return; // This will override to not draw.
        }
    }
}
using Byte.Sprite;
using Microsoft.Xna.Framework;

namespace Byte.Block;

public class SquareBlock : AbstractBlock
{
    public SquareBlock(ISprite sprite, Vector2 position) : base(sprite, position) { }
}

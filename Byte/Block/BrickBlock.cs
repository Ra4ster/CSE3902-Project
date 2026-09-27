using Byte.Sprite;
using Microsoft.Xna.Framework;

namespace Byte.Block;

public class BrickBlock : AbstractBlock
{
    public BrickBlock(ISprite sprite, Vector2 position) : base(sprite, position) { }
}

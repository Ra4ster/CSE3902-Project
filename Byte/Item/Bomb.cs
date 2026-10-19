using Byte.Sprite;
using Microsoft.Xna.Framework;

namespace Byte.Item
{
    internal class Bomb : AbstractItem
    {
        public Bomb(ISprite sprite, Vector2 position, Point size) : base(sprite, position, size) { }
    }
}

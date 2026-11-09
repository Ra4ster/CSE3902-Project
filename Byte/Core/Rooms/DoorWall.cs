using Byte.Drawing;
using Byte.Sprite;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Rooms
{
    public sealed class DoorWall : IDrawable
    {
        private readonly StaticSprite sprite;

        internal DoorWall(StaticSprite sprite)
        {
            this.sprite = sprite;
        }

        public void Draw(SpriteBatch painter) => sprite.Draw(painter);
    }
}

using Byte.Collidable;
using Microsoft.Xna.Framework;

namespace Byte.Item
{
    public interface IItem : ICollidable, Byte.Drawing.IDrawable
    {
        void Update(GameTime gametime);
        Vector2 Position { get; set; }
        bool IsCollected { get; }
        void Collect();
    }
}

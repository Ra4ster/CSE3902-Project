using Byte.Collidable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Item
{
    public interface IItem : ICollidable
    {
        void Update(GameTime gametime);
        void Draw(SpriteBatch spriteBatch);
        Vector2 Position { get; set; }
        bool IsCollected { get; }
        void Collect();
    }
}

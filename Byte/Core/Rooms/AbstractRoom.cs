
using Byte.Block;
using Byte.Item;
using Byte.Sprite;
using Byte.Sprite.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Rooms
{
    internal abstract class AbstractRoom : IRoom
    {
        public IReadOnlyList<AbstractEnemy> Enemies => throw new NotImplementedException();
        public IReadOnlyList<AbstractBlock> Blocks => throw new NotImplementedException();
        public IReadOnlyList<IItem> Items => throw new NotImplementedException();

        public required string name;

        public AbstractRoom(string name)
        {
            this.name = name;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gametime)
        {
            throw new NotImplementedException();
        }
    }
}
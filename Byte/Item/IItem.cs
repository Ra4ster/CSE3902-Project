using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Item
{
    public interface IItem
    {
        void Update(GameTime gametime);
        void Draw(SpriteBatch spriteBatch);
        Vector2 Position { get; set; }

        public bool IsCollected { get; set; }
    }
}

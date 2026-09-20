using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Item
{
    internal class ItemManager
    {
        private List<IItem> items;
        private int currentIndex;

        public void NextItem()
        {
            if (items.Count > 0)
            {
                currentIndex = (currentIndex + 1) % items.Count;
            }
        }

        public void PrevItem()
        {
            if (items.Count > 0)
            {
                currentIndex = (currentIndex - 1 + items.Count) % items.Count;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (items.Count > 0 && currentIndex < items.Count)
            {
                items[currentIndex].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (items.Count > 0 && currentIndex < items.Count)
            {
                items[currentIndex].Draw(spriteBatch);
            }
        }
    }
}

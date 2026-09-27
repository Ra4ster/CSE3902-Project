using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Byte.Block;

// holds all the blocks and draws whichever one is selected, t/y cycle through them
public class BlockManager
{
    private readonly List<IBlock> blocks = new List<IBlock>();
    private int currentIndex;

    public void AddBlock(IBlock block) => blocks.Add(block);

    public void NextBlock()
    {
        if (blocks.Count > 0)
        {
            currentIndex = (currentIndex + 1) % blocks.Count;
        }
    }

    public void PreviousBlock()
    {
        if (blocks.Count > 0)
        {
            currentIndex = (currentIndex - 1 + blocks.Count) % blocks.Count;
        }
    }

    // go back to the first block when the game resets
    public void Reset() => currentIndex = 0;

    public void Update(GameTime gameTime)
    {
        if (blocks.Count > 0)
        {
            blocks[currentIndex].Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (blocks.Count > 0)
        {
            blocks[currentIndex].Draw(spriteBatch);
        }
    }
}

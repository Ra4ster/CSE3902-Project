using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Block;

// a tile that just sits there and draws, no interaction this sprint
public interface IBlock
{
    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);
}

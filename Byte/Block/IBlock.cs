using Microsoft.Xna.Framework;

namespace Byte.Block;

// a tile that just sits there and draws, no interaction this sprint
public interface IBlock : Byte.Drawing.IDrawable
{
    void Update(GameTime gameTime);

}

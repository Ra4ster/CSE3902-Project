using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.States;

public interface IGameState
{
    void Enter();

    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);
}

using Microsoft.Xna.Framework;

namespace Byte.Controller;

public interface IController
{
    /// <summary>
    /// For each input, executes its command.
    /// </summary>
    void Update(GameTime gameTime);
}
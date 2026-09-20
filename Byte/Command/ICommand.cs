
using Microsoft.Xna.Framework;

public interface ICommand
{
    /// <summary>
    /// Executes the command assigned to this.
    /// </summary>
    public void Execute(GameTime gameTime);
}
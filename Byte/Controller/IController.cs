namespace Sprint0.Controller;

using Microsoft.Xna.Framework;

public interface IController
{
    /// <summary>
    /// For each input, executes its command.
    /// </summary>
    void Update();
}
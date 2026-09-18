namespace Byte.Controller;

public interface IController
{
    /// <summary>
    /// For each input, executes its command.
    /// </summary>
    void Update();
}
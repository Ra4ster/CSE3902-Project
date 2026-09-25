using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Byte.Controller;

/// <summary>
/// Controls key input.
/// </summary>
public class KeyboardController : IController
{
    private Dictionary<Keys, ICommand> keyboardCommands;

    public KeyboardState currentState;

    private KeyboardState previousState;

    public KeyboardController(Dictionary<Keys, ICommand> keyboardCommands)
    {
        this.keyboardCommands = keyboardCommands;
        currentState = Keyboard.GetState();
    }

    public void Update(GameTime gameTime)
    {
        previousState = currentState;
        currentState = Keyboard.GetState();

        foreach (KeyValuePair<Keys, ICommand> pair in keyboardCommands)
        {
            if (currentState.IsKeyDown(pair.Key))
            {
                pair.Value.Execute(gameTime);
            }
        }
    }

    public void AddKey(Keys key, ICommand handler) => keyboardCommands.Add(key, handler);
}
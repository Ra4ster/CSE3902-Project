using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Byte.Controller;

/// <summary>
/// Controls key input.
/// </summary>
public class KeyboardController : IController
{
    private Dictionary<Keys, ICommand> keyboardCommands;
    private HashSet<Keys> pressOnlyCommands = new HashSet<Keys>();

    public KeyboardState currentState;

    private KeyboardState previousState;

    public KeyboardController(Dictionary<Keys, ICommand> keyboardCommands)
    {
        this.keyboardCommands = keyboardCommands;
        currentState = Keyboard.GetState();
    }

    public KeyboardController(Dictionary<Keys, ICommand> keyboardCommands, Keys[] pressOnlyKeys) : this(keyboardCommands)
    {
        foreach (Keys k in pressOnlyKeys)
            pressOnlyCommands.Add(k);
    }

    public void Update(GameTime gameTime)
    {
        previousState = currentState;
        currentState = Keyboard.GetState();

        foreach (KeyValuePair<Keys, ICommand> pair in keyboardCommands)
        {
            if (currentState.IsKeyDown(pair.Key))
            {
                if (!pressOnlyCommands.Contains(pair.Key))
                    pair.Value.Execute(gameTime);
                else if (!previousState.IsKeyDown(pair.Key))
                    pair.Value.Execute(gameTime);
            }
        }
    }

    public void AddKey(Keys key, ICommand handler) => keyboardCommands.Add(key, handler);

    public void SetPressOnly(Keys key)
    {
        pressOnlyCommands.Add(key);
    }
}
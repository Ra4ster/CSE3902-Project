using Byte.Controller;
using Microsoft.Xna.Framework.Input;

namespace Byte.States;

public static class StateInput
{
    public static KeyboardController PressOnly(Dictionary<Keys, ICommand> bindings)
    {
        ArgumentNullException.ThrowIfNull(bindings);
        return new KeyboardController(bindings, bindings.Keys.ToArray());
    }
}

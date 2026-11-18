using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Byte.Controller
{
    public sealed class InputControllers
    {
        public Dictionary<Keys, ICommand> KeyBindings { get; } = [];
        public Dictionary<Rectangle, ICommand> LeftClickBindings { get; } = [];
        public Dictionary<Rectangle, ICommand> RightClickBindings { get; } = [];
        public KeyboardController Keyboard { get; }
        public MouseController Mouse { get; }

        public InputControllers(Keys[] pressOnlyKeys)
        {
            Keyboard = new KeyboardController(KeyBindings, pressOnlyKeys);
            Mouse = new MouseController(LeftClickBindings, RightClickBindings);
        }

        public void Clear()
        {
            KeyBindings.Clear();
            LeftClickBindings.Clear();
            RightClickBindings.Clear();
        }

        public void Update(GameTime gameTime)
        {
            Keyboard.Update(gameTime);
            Mouse.Update(gameTime);
        }
    }
}

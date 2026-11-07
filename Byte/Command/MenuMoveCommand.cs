using Byte.States;
using Microsoft.Xna.Framework;

namespace Byte.Command
{
    public class MenuMoveCommand : ICommand
    {
        private readonly IMenu menu;
        private readonly int delta;

        public MenuMoveCommand(IMenu menu, int delta)
        {
            this.menu = menu;
            this.delta = delta;
        }

        public void Execute(GameTime gameTime) => menu.MoveSelection(delta);
    }
}

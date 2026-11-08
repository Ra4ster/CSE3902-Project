using Byte.States;
using Microsoft.Xna.Framework;

namespace Byte.Command
{
    public class MenuConfirmCommand : ICommand
    {
        private readonly IMenu menu;

        public MenuConfirmCommand(IMenu menu) => this.menu = menu;

        public void Execute(GameTime gameTime) => menu.ConfirmSelection();
    }
}

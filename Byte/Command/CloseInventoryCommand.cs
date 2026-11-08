using Byte.States;
using Microsoft.Xna.Framework;

namespace Byte.Command
{
    public class CloseInventoryCommand : ICommand
    {
        private readonly InventoryState inventoryState;

        public CloseInventoryCommand(InventoryState inventoryState) => this.inventoryState = inventoryState;

        public void Execute(GameTime gameTime) => inventoryState.Close();
    }
}

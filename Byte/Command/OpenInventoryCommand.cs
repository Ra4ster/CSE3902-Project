using Byte.Core;
using Microsoft.Xna.Framework;

namespace Byte.Command
{
    internal class OpenInventoryCommand : ICommand
    {
        private readonly GameStateManager gameStateManager;

        public OpenInventoryCommand(GameStateManager stateManager)
        {
            gameStateManager = stateManager ?? throw new System.ArgumentNullException(nameof(stateManager));
        }

        public void Execute(GameTime gametime)
        {
            gameStateManager.ToggleInventory();
        }
    }
}
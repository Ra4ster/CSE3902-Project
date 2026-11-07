using System;
using System.Collections.Generic;
using System.Text;

using Byte.InventoryAndHud;

namespace Byte.Core
{
    public class GameStateManager
    {
        public GameState CurrentState { get; set; } = GameState.Playing;

        public void ToggleInventory()
        {
            if (CurrentState == GameState.Playing)
            {
                CurrentState = GameState.TransitionToInventory;
            }
            else if (CurrentState == GameState.Inventory)
            {
                CurrentState = GameState.TransitionToPlaying;
            }
        }
    }
}

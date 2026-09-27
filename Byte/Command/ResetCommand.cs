using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Byte.Block;
using Byte.Player;

namespace Byte.Command
{
    public class ResetCommand : ICommand
    {
        Link player;
        BlockManager blockManager;
        public ResetCommand(Game game, Link link, BlockManager blockManager) {
            player = link;
            this.blockManager = blockManager;
        }
        public void Execute(GameTime gametime)
        {
            // Reset player values to initial state
            player.Position = Link.startPos;
            player.Direction = CardinalDirections.South;
            player.IsAttacking = false;
            player.GetNewState(new PlayerIdleState());

            // reset blocks back to the first one
            blockManager.Reset();
        }
    }
}

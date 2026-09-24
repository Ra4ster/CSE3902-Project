using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Byte.Player;

namespace Byte.Command
{
    public class ResetCommand : ICommand
    {
        Link player;
        public ResetCommand(Game game, Link link) {
            player = link;
        }
        public void Execute(GameTime gametime)
        {
            // Reset player values to initial state
            player.Position = Link.startPos;
            player.Direction = CardinalDirections.South;
            player.IsAttacking = false;
            player.GetNewState(new PlayerIdleState());
        }
    }
}

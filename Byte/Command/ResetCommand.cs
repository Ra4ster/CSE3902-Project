using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Byte.Block;
using Byte.Player;
using Byte.Projectile;

namespace Byte.Command
{
    public class ResetCommand : ICommand
    {
        Link player;
        BlockManager blockManager;
        ProjectileManager projectileManager;
        public ResetCommand(Game game, Link link, BlockManager blockManager, ProjectileManager projectileManager) {
            player = link;
            this.blockManager = blockManager;
            this.projectileManager = projectileManager;
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

            // remove every projectile still in flight
            projectileManager.Clear();
        }
    }
}

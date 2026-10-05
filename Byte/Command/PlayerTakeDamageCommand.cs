using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Byte.Player;

namespace Byte.Command
{
    public class PlayerTakeDamageCommand : ICommand
    {
        Link player;
        public PlayerTakeDamageCommand(Link link) {
            player = link;
        }
        public void Execute(GameTime gameTime)
        {
            player.TakeDamage();
        }
    }
}

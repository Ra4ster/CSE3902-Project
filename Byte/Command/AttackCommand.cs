using Byte.Player;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Command
{
    public class AttackCommand : ICommand
    {
        private Link player;
        Game gameGet;



        public AttackCommand(Game game, Link link)
        {
            player = link;
            gameGet = game;
        }

        public void Execute(GameTime gameTime)
        {

            player.Attack();
           
            
            
            
        }
    }
}

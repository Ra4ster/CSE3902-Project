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
            

            player.MovementSpeed = new Vector2(0, 0);
            if (player.state is not PlayerAttackState)
            {
                player.GetNewState(new PlayerAttackState());
            }
            
        }
    }
}

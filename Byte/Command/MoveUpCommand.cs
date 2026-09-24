using Byte.Player;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Byte.Command
{
    public class MoveUpCommand : ICommand
    {
        private Link player;
        Game gameGet; 
        


        public MoveUpCommand(Game game, Link link) {
            player= link;
            gameGet = game;
            player.MovementSpeed = new Vector2(0, -200);
        }
         
        public void Execute(GameTime gameTime)
        {
            if (player.IsAttacking)
            {
                return;
            }
            player.Direction = CardinalDirections.North;
            
            player.MovementSpeed = new Vector2(0, -200);
            if (player.state is not PlayerMoveUpState)
            {
                player.GetNewState(new PlayerMoveUpState());
            }
            player.Position += player.MovementSpeed *(float)gameTime.ElapsedGameTime.TotalSeconds;
            
        }
    }
}

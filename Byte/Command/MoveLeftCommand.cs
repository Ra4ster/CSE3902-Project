using System;
using System.Collections.Generic;
using System.Text;
using Byte.Player;
using Microsoft.Xna.Framework;

namespace Byte.Command
{
    public class MoveLeftCommand : ICommand
    {
        private Link player;
        Game gameGet;
        
        float speed = 2.0f;

        public MoveLeftCommand(Game game, Link link)
        {
            player = link;
            gameGet = game;
            
        }

        public void Execute(GameTime gameTime)
        {

            player.Direction = CardinalDirections.West;

            player.MovementSpeed = new Vector2(-200, 0);
            if (player.state is not PlayerMoveLeftState)
            {
                player.GetNewState(new PlayerMoveLeftState());
            }
            player.Position += player.MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    
    }
}

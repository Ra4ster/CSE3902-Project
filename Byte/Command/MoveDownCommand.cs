using Byte.Player;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Command
{
    public class MoveDownCommand : ICommand
    {
        private Link player;
        Game gameGet;

        

        public MoveDownCommand(Game game, Link link)
        {
            player = link;
            gameGet = game;
        }

        public void Execute(GameTime gameTime)
        {
            if (player.IsAttacking)
            {
                return;
            }



            player.Direction = CardinalDirections.South;

            player.MovementSpeed = new Vector2(0, 200);
            if (player.state is not PlayerMoveDownState)
            {
                player.GetNewState(new PlayerMoveDownState());
            }
            player.Position += player.MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}

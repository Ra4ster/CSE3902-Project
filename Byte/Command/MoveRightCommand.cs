using Byte.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Command
{
    public class MoveRightCommand : ICommand
    {
        private Link player;
        Game gameGet;
    

        public MoveRightCommand(Game game, Link link)
        {
            player = link;
            gameGet = game;
            player.MovementSpeed = new Vector2(0, -200);
        }

        public void Execute(GameTime gameTime)
        {
            // check if player is attacking and stop command if so
            if (player.IsAttacking)
            {
                return;
            }
            KeyboardState keyboardState = Keyboard.GetState();

            // check if player is moving up or down - cant move right/left during
            if (keyboardState.IsKeyDown(Keys.W) ||
                keyboardState.IsKeyDown(Keys.S))
            {
                return;
            }
            player.Direction = CardinalDirections.East;

            // update player direction and begin moving
            player.MovementSpeed = new Vector2(200,0);
            if (player.state is not PlayerMoveRightState)
            {
                player.GetNewState(new PlayerMoveRightState());
            }
            player.Position += player.MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}

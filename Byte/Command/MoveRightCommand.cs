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
        float speed = 2.0f;

        public MoveRightCommand(Game game, Link link)
        {
            player = link;
            gameGet = game;
            player.MovementSpeed = new Vector2(0, -200);
        }

        public void Execute(GameTime gameTime)
        {
            if (player.IsAttacking)
            {
                return;
            }
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.W) ||
                keyboardState.IsKeyDown(Keys.S))
            {
                return;
            }
            player.Direction = CardinalDirections.East;

            player.MovementSpeed = new Vector2(200,0);
            if (player.state is not PlayerMoveRightState)
            {
                player.GetNewState(new PlayerMoveRightState());
            }
            player.Position += player.MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}

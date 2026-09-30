using Byte.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Command
{
    public class MoveLeftCommand : ICommand
    {
        private Link player;
       
        Game gameGet;
        
      

        public MoveLeftCommand(Game game, Link link)
        {
            player = link;
            gameGet = game;
            
        }

        public void Execute(GameTime gameTime)
        {
            // Check if player is attacking - disable movement
            if (player.IsAttacking)
            {
                return;
            }
            // check keyboard state
            KeyboardState keyboardState = Keyboard.GetState();

            // if w or s is pressed - prioritize their movement - in original no diagonal - defaults to vertical movement
            if (keyboardState.IsKeyDown(Keys.W) ||
                keyboardState.IsKeyDown(Keys.S) ||
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.Down))
            {
                return;
            }
            // update direction
            player.Direction = CardinalDirections.West;

            //check if already moving left - if not create new moving left state
            player.MovementSpeed = new Vector2(-200, 0);
            if (player.state is not PlayerMoveLeftState)
            {
                player.GetNewState(new PlayerMoveLeftState());
            }
            // update position based on direction and time
            player.Position += player.MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    
    }
}

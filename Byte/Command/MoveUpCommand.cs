using Byte.Player;
using Microsoft.Xna.Framework;

namespace Byte.Command
{
    public class MoveUpCommand : ICommand
    {
        private readonly Link player;
        readonly Game gameGet;



        public MoveUpCommand(Game game, Link link)
        {
            player = link;
            gameGet = game;

        }

        public void Execute(GameTime gameTime)
        {
            // Check if player is attacking and stop if so
            if (player.IsAttacking)
            {
                return;
            }

            // update direction and move player
            player.Direction = CardinalDirections.North;

            player.Velocity = new Vector2(0, -player.MovementSpeed);
            if (player.state is not PlayerMoveUpState)
            {
                player.GetNewState(new PlayerMoveUpState());
            }


        }
    }
}

using Byte.Player;
using Microsoft.Xna.Framework;

namespace Byte.Command
{
    public class MoveDownCommand : ICommand
    {
        private readonly Link player;
        readonly Game gameGet;



        public MoveDownCommand(Game game, Link link)
        {
            player = link;
            gameGet = game;
        }

        public void Execute(GameTime gameTime)
        {
            // check if player is attacking - cant move during
            if (player.IsAttacking)
            {
                return;
            }



            //update player direction and move player
            player.Direction = CardinalDirections.South;

            player.Velocity = new Vector2(0, player.MovementSpeed);
            if (player.state is not PlayerMoveDownState)
            {
                player.GetNewState(new PlayerMoveDownState());
            }

        }
    }
}

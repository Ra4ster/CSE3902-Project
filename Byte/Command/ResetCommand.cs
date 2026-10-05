using Microsoft.Xna.Framework;

namespace Byte.Command
{
    // reloads the whole level, every object is rebuilt from the dungeon
    public class ResetCommand : ICommand
    {
        private readonly Game game;

        public ResetCommand(Game game)
        {
            this.game = game;
        }

        public void Execute(GameTime gameTime)
        {
            game.RequestReset();
        }
    }
}

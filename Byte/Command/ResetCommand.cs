using Microsoft.Xna.Framework;

namespace Byte.Command
{
    // reloads the whole level, every object is rebuilt from the dungeon
    public class ResetCommand : ICommand
    {
        private readonly Program program;

        public ResetCommand(Program program)
        {
            this.program = program;
        }

        public void Execute(GameTime gameTime)
        {
            program.RequestReset();
        }
    }
}

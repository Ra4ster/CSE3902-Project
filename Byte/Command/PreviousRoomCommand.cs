using Microsoft.Xna.Framework;

namespace Byte.Command
{
    // debug only, jumps to the previous room without using a door
    public class PreviousRoomCommand : ICommand
    {
        private readonly Program program;

        public PreviousRoomCommand(Program program)
        {
            this.program = program;
        }

        public void Execute(GameTime gameTime) => program.PreviousRoom();
    }
}

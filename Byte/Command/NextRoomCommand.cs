using Microsoft.Xna.Framework;

namespace Byte.Command
{
    // debug only, jumps to the next room without using a door
    public class NextRoomCommand : ICommand
    {
        private readonly Program program;

        public NextRoomCommand(Program program)
        {
            this.program = program;
        }

        public void Execute(GameTime gameTime) => program.NextRoom();
    }
}

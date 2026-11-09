using Microsoft.Xna.Framework;

namespace Byte.Command
{
    public sealed class InteractWithDoorCommand : ICommand
    {
        private readonly Program program;

        public InteractWithDoorCommand(Program program)
        {
            this.program = program;
        }

        public void Execute(GameTime gameTime) => program.InteractWithDoor();
    }
}

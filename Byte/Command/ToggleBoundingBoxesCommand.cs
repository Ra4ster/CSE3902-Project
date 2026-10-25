using Microsoft.Xna.Framework;

namespace Byte.Command
{
    // debug only, shows or hides every hitbox
    public class ToggleBoundingBoxesCommand : ICommand
    {
        public void Execute(GameTime gameTime) => Program.DrawBoundingBoxes = !Program.DrawBoundingBoxes;
    }
}

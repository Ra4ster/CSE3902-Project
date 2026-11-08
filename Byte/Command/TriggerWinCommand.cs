using Byte.States;
using Microsoft.Xna.Framework;

namespace Byte.Command
{
    public class TriggerWinCommand : ICommand
    {
        public void Execute(GameTime gameTime) => GameEvents.RaiseTriforcePickedUp();
    }
}

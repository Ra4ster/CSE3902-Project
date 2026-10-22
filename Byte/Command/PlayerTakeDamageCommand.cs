using Microsoft.Xna.Framework;
using Byte.Player;

namespace Byte.Command
{
    public class PlayerTakeDamageCommand : ICommand
    {
        readonly Link player;
        public PlayerTakeDamageCommand(Link link)
        {
            player = link;
        }
        public void Execute(GameTime gameTime)
        {
            player.TakeDamage();
        }
    }
}

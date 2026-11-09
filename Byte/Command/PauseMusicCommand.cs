using Byte.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Byte.Command
{
    public class PauseMusicCommand : ICommand
    {
        private float cooldownTimer = 0f;
        private const float CooldownDuration = 0.1f;

        public void Execute(GameTime gameTime)
        {
            cooldownTimer -=
                (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (cooldownTimer > 0f)
                return;

            if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Resume();
            }
            else
            {
                Program.soundManager.PauseMusic();
            }

            cooldownTimer = CooldownDuration;
        }
    }
}
using Byte.Block;
using Microsoft.Xna.Framework;
using System;

namespace Byte.Command
{
    public class NextBlockCommand : ICommand
    {
        // controller runs held keys every frame so this spaces out the repeats
        private static readonly TimeSpan RepeatDelay = TimeSpan.FromMilliseconds(250);

        private readonly BlockManager blockManager;
        private TimeSpan lastExecuted = TimeSpan.MinValue;

        public NextBlockCommand(BlockManager blockManager)
        {
            this.blockManager = blockManager;
        }

        public void Execute(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - lastExecuted < RepeatDelay)
            {
                return;
            }
            lastExecuted = gameTime.TotalGameTime;
            blockManager.NextBlock();
        }
    }
}

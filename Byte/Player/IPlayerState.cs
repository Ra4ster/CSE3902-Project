using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Byte.Player
{
    public interface IPlayerState
    {
        // Update the player based on the current state
        public IPlayerState Update(Link link, GameTime gameTime);

        // draw the current state
        public void Draw(Link link);
    }
}

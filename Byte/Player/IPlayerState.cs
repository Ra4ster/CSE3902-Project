using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Byte.Player
{
    public interface IPlayerState
    {
        public IPlayerState Update(Link link, GameTime gameTime);

        public void Draw(Link link);
    }
}

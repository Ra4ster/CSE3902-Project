using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Player
{
    public interface IPlayerState
    {
        public IPlayerState Update();

        public void Draw();
    }
}

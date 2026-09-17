using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Player
{
    public class PlayerIdleState : IPlayerState

    {
        public IPlayerState Update(Link link)
        {
            
            if (link.Direction == CardinalDirections.South)
            {

                link.sourceRects[0] = link.IdleFrames[0];
            }
            else if (link.Direction == CardinalDirections.North)
            {
                link.sourceRects[0] = link.IdleFrames[2];

            } else if(link.Direction == CardinalDirections.East || link.Direction == CardinalDirections.West){
                link.sourceRects[0] = link.IdleFrames[1];

            }

            return this;
        }

        public void Draw()
        {
            
        }
    }
}

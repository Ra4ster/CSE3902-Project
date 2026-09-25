using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Byte.Sprite;

namespace Byte.Player
{
    public class PlayerIdleState : IPlayerState

    {
        public PlayerIdleState()
        {

        }
        public IPlayerState Update(Link link, GameTime gameTime)
        {

            if (link.Direction == CardinalDirections.South)
            {
                link.sourceRects[0] = link.IdleFrames[0];


            }
            else if (link.Direction == CardinalDirections.North)
            {
                link.sourceRects[0] = link.IdleFrames[2];



            }
            else if (link.Direction == CardinalDirections.East || link.Direction == CardinalDirections.West)
            {
                link.sourceRects[0] = link.IdleFrames[1];



            }
            link.currentSprite = new StaticSprite(link.SpriteSheet, link.Position, Color.White, link.sourceRects[0], 6.0f);

            if (link.Direction == CardinalDirections.West)
            {
                link.currentSprite.Effects = SpriteEffects.FlipHorizontally;
            }
            return this;
        }

        public void Draw(Link link)
        {
            link.currentSprite.Draw(link.spriteBatch);
        }
    }
}

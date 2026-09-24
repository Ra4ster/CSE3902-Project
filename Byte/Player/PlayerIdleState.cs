using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Sprite;

namespace Byte.Player
{
    public class PlayerIdleState : IPlayerState

    {
        // contains the frames for idle state
        public Rectangle[] IdleFrames =
       {
            new Rectangle(1, 11, 16, 16), // Idle South
            new Rectangle(35, 11, 16, 16), // Idle East  - flip for West
            new Rectangle(69, 11, 16,16) // idle North,
        };
        public PlayerIdleState()
        {
            
        }
        public IPlayerState Update(Link link,GameTime gameTime)
        {
            
            // update gameStart so new sprite is only made when game starts
                link.isGameStart = false;

            // check given direction and update sourceRect to correct frame
                if (link.Direction == CardinalDirections.South)
                {
                    link.sourceRects[0] = IdleFrames[0];


                }
                else if (link.Direction == CardinalDirections.North)
                {
                    link.sourceRects[0] = IdleFrames[2];



                }
                else if (link.Direction == CardinalDirections.East || link.Direction == CardinalDirections.West)
                {
                    link.sourceRects[0] = IdleFrames[1];



                }
                // Create sprite for idle
                link.currentSprite = new StaticSprite(link.SpriteSheet, link.Position, Color.White, link.sourceRects[0], 6.0f);

            // if facing west flip the sprite
                if (link.Direction == CardinalDirections.West)
                {
                    link.currentSprite.Effects = SpriteEffects.FlipHorizontally;
                }
                // update sprite origin to keep aligned when using animations
                link.currentSprite.MoveOrigin(Link.LinkOrigin);
            
            
            return this;
        }

        public void Draw(Link link)
        {
            // draw in current spritebatch
            link.currentSprite.Draw(link.spriteBatch);
        }
    }
}

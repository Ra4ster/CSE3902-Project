using Microsoft.Xna.Framework;
using Sprint0.Sprite;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Player
{
    public class PlayerMoveLeftState : IPlayerState
    {

        Rectangle[] westFrames =
        {
            new Rectangle(35, 11, 16, 16),
            new Rectangle(52,11,16,16)
        };
        public PlayerMoveLeftState()
        {


        }
        public IPlayerState Update(Link link, GameTime gametime)
        {
            if (link.currentSprite is not MovingAnimatedSprite)
            {
                link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                     link.Position,
                     link.MovementSpeed,
                    Color.White,
                    westFrames,
                    .1f,
                    6.0f);
                link.currentSprite.Effects = SpriteEffects.FlipHorizontally;
            }
            if (link.currentSprite is MovingAnimatedSprite)
            {
                MovingAnimatedSprite sprite = (MovingAnimatedSprite)link.currentSprite;

                sprite.SetPos(link.Position);
                sprite.Update(gametime);
                sprite.Effects = SpriteEffects.FlipHorizontally;
            }

            return this;
        }
        public void Draw(Link link)
        {
            link.currentSprite.Draw(link.spriteBatch);
        }
    }
}

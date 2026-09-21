using Microsoft.Xna.Framework;
using Sprint0.Sprite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Player
{
    public class PlayerMoveDownState : IPlayerState
    {

        Rectangle[] southFrames =
        {
            new Rectangle(1, 11, 16, 16),
            new Rectangle(18,11,16,16)
        };
        public PlayerMoveDownState()
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
                    southFrames,
                    .1f,
                    6.0f);
            }
            if (link.currentSprite is MovingAnimatedSprite)
            {
                MovingAnimatedSprite sprite = (MovingAnimatedSprite)link.currentSprite;

                sprite.SetPos(link.Position);
                sprite.Update(gametime);
            }

            return this;
        }
        public void Draw(Link link)
        {
            link.currentSprite.Draw(link.spriteBatch);
        }
    }
}

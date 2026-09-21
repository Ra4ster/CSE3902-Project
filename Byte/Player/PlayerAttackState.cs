using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Player
{
    public class PlayerAttackState : IPlayerState
    {
        public PlayerAttackState() { }

        Rectangle[] attackSouthFrames =
        {
            new Rectangle(1,47,16,16),
            new Rectangle(18,47,16,26),
            new Rectangle(35,47,16,22),
            new Rectangle(52,47,16,18)

        };
     
        Rectangle[] attackEastWestFrames =
       {
            new Rectangle(1,77,16,16),
            new Rectangle(18,77,26,16),
            new Rectangle(46,77,22,16),
            new Rectangle(70,77,18,16)

        };
        Rectangle[] attackNorthFrames =
       {
            new Rectangle(1,109,16,16),
            new Rectangle(18,97,16,27),
            new Rectangle(36,98,16,26),
            new Rectangle(52,106,16,18)

        };



        public IPlayerState Update(Link link, GameTime gametime)
        {
            if (link.currentSprite is not MovingAnimatedSprite)
            {
                if (link.Direction == CardinalDirections.North)
                {
                    link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                         link.Position,
                         link.MovementSpeed,
                        Color.White,
                        attackNorthFrames,
                        .1f,
                        6.0f);
                }
                else if (link.Direction == CardinalDirections.South)
                {
                    link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                         link.Position,
                         link.MovementSpeed,
                        Color.White,
                        attackSouthFrames,
                        .1f,
                        6.0f);
                }
                else if (link.Direction == CardinalDirections.East)
                {

                    link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                         link.Position,
                         link.MovementSpeed,
                        Color.White,
                        attackEastWestFrames,
                        .1f,
                        6.0f);
                }
                else
                {
                    link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                         link.Position,
                         link.MovementSpeed,
                        Color.White,
                        attackEastWestFrames,
                        .1f,
                        6.0f);
                    link.currentSprite.Effects = SpriteEffects.FlipHorizontally;
                }
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

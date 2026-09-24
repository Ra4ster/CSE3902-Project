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

        // contains frames for attacking south
        Rectangle[] attackSouthFrames =
        {
            new Rectangle(1,47,16,14),
            new Rectangle(19,47,14,26),
            new Rectangle(35,47,15,22),
            new Rectangle(52,47,15,18)

        };

        // contains frames for attacking east or west(east flipped)

        Rectangle[] attackEastWestFrames =
       {
            new Rectangle(1,77,16,16),
            new Rectangle(18,77,26,16),
            new Rectangle(46,77,22,16),
            new Rectangle(70,77,18,16)

        };
        // contains frames for attacking north

        Rectangle[] attackNorthFrames =
       {
            new Rectangle(1,109,16,16),
            new Rectangle(18,97,14,27),
            new Rectangle(36,98,14,26),
            new Rectangle(52,106,14,18)

        };



        public IPlayerState Update(Link link, GameTime gametime)
        {
            // check if link is already in an attacking animation
            if (link.currentSprite is not MovingAnimatedSprite)
            {
                // depending on direction set link attack animation
                if (link.Direction == CardinalDirections.North)
                {
                    link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                         link.Position,
                         link.MovementSpeed,
                        Color.White,
                        attackNorthFrames,
                        .1f,
                        6.0f);
                    // for north update frame origin to prevent sprite moving during animation
                    link.currentSprite.SetFrameOrigin(new Vector2[]
                    {
                        new Vector2(8, 15),
                        new Vector2(7, 25),
                        new Vector2(7, 23),
                        new Vector2(7, 14)
                    });
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
                    // to prevent weird movement when attacking west - update sprite origins
                    link.currentSprite.SetFrameOrigin(new Vector2[]
                    {
                        new Vector2(13, 13),
                        new Vector2(18, 13),
                        new Vector2(15,13),
                        new Vector2(13,13)
                    });
                    // flip sprite for west
                    link.currentSprite.Effects = SpriteEffects.FlipHorizontally;
                }
                link.currentSprite.MoveOrigin(Link.LinkOrigin);

                // set loop to false so animation plays once
                link.currentSprite.Loop = false;
                
            }
            if (link.currentSprite is MovingAnimatedSprite)
            {
                MovingAnimatedSprite sprite = (MovingAnimatedSprite)link.currentSprite;
                
                
                // update current sprite
                sprite.SetPos(link.Position);
                sprite.Update(gametime);

                // if animation finishes update IsAttacking, player state, and movementSpeed
                if (sprite.IsFinished)
                {
                    link.IsAttacking = false;
                    link.MovementSpeed = Vector2.Zero;
                    link.GetNewState(new PlayerIdleState());
                    link.state.Update(link,gametime);
                }

            }



            return this;
        }

        public void Draw(Link link)
        {

            link.currentSprite.Draw(link.spriteBatch);
        }
    }
}

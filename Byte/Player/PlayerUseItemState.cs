using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Player
{
    public class PlayerUseItemState : IPlayerState
    {
        public PlayerUseItemState() { }

        // contains frames for using item south
        public Rectangle[] useItemSouthFrames =
        {
            new Rectangle(1,47,16,14),
            

        };

        // contains frames for attacking east or west(east flipped)

        public Rectangle[] useItemEastWestFrames =
       {
            new Rectangle(1,77,16,16),
            

        };
        // contains frames for attacking north

        public Rectangle[] useItemNorthFrames =
       {
            new Rectangle(1,109,16,16),
           

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
                        useItemNorthFrames,
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
                        useItemSouthFrames,
                        .1f,
                        6.0f);
                }
                else if (link.Direction == CardinalDirections.East)
                {

                    link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                         link.Position,
                         link.MovementSpeed,
                        Color.White,
                        useItemEastWestFrames,
                        .1f,
                        6.0f);
                }
                else
                {
                    link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                         link.Position,
                         link.MovementSpeed,
                        Color.White,
                        useItemEastWestFrames,
                        .1f,
                        6.0f);
                    // to prevent weird movement when using item west - update sprite origins
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
                    link.state.Update(link, gametime);
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

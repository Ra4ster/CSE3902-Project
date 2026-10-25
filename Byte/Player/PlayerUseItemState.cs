using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Player
{
    public class PlayerUseItemState : IPlayerState
    {
        public PlayerUseItemState() { }

        private readonly float useItemDuration = 0.1f;
        private readonly float scale = 6.0f;

        private double timer = 0.0;
        private readonly double maxUseItemDuration = 100.0;

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
            timer += gametime.ElapsedGameTime.TotalMilliseconds;
            // check if link is already in an attacking animation
            if (link.currentSprite is not MovingAnimatedSprite)
            {
                // depending on direction set link attack animation
                if (link.Direction == CardinalDirections.North)
                {
                    link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                         link.Position,
                         link.Velocity,
                        Color.White,
                        useItemNorthFrames,
                        useItemDuration,
                        scale);
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
                         link.Velocity,
                        Color.White,
                        useItemSouthFrames,
                        useItemDuration,
                        scale);
                }
                else if (link.Direction == CardinalDirections.East)
                {

                    link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                         link.Position,
                         link.Velocity,
                        Color.White,
                        useItemEastWestFrames,
                        useItemDuration,
                        scale);
                }
                else
                {
                    link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                         link.Position,
                         link.Velocity,
                        Color.White,
                        useItemEastWestFrames,
                        useItemDuration,
                        scale);
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
                link.currentSprite.loopAnimation = false;

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
                    link.Velocity = Vector2.Zero;
                    link.GetNewState(new PlayerIdleState());
                    _ = link.state.Update(link, gametime);
                }
                //This is to fix a bug where the sprite did not always return back to idle state. Now if it is not updated back to idle state from Sprite.IsFinished, this forces it to.
                else if (timer >= maxUseItemDuration)
                {
                    link.IsAttacking = false;
                    link.Velocity = Vector2.Zero;
                    link.GetNewState(new PlayerIdleState());
                    _ = link.state.Update(link, gametime);
                    timer = 0;
                }

            }



            return this;
        }

        public void Draw(Link link)
        {
            link.currentSprite!.Draw(link.spriteBatch!);
        }
    }
}

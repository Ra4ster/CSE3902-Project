using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Player
{
    public class PlayerAttackState : IPlayerState
    {
        public PlayerAttackState() { }

        private readonly float attackDuration = 0.1f;
        private readonly float scale = 6.0f;

        private double timer = 0.0;

        private bool hasStarted = false;


        // contains frames for attacking south
        public Rectangle[] attackSouthFrames =
        {
            new Rectangle(1,47,16,14),
            new Rectangle(19,47,14,26),
            new Rectangle(35,47,15,22),
            new Rectangle(52,47,15,18)

        };

        // contains frames for attacking east or west(east flipped)

        public Rectangle[] attackEastWestFrames =
       {
            new Rectangle(1,77,16,16),
            new Rectangle(18,77,26,16),
            new Rectangle(46,77,22,16),
            new Rectangle(70,77,18,16)

        };
        // contains frames for attacking north

        public Rectangle[] attackNorthFrames =
       {
            new Rectangle(1,109,16,16),
            new Rectangle(18,97,14,27),
            new Rectangle(36,98,14,26),
            new Rectangle(52,106,14,18)

        }; 

        // attackDuration * number of frames * 1000 milliseconds per second 
        private readonly double maxAttackDuration = 400.0;






        public IPlayerState Update(Link link, GameTime gametime)
        {
            timer += gametime.ElapsedGameTime.TotalMilliseconds;
            // check if link is already in an attacking animation
            if (!hasStarted) // link.currentSprite is not MovingAnimatedSprite && 
            {
                // depending on direction set link attack animation
                if (link.Direction == CardinalDirections.North)
                {
                    link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                         link.Position,
                         link.Velocity,
                        Color.White,
                        attackNorthFrames,
                        attackDuration,
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
                        attackSouthFrames,
                        attackDuration,
                        scale);
                }
                else if (link.Direction == CardinalDirections.East)
                {

                    link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                         link.Position,
                         link.Velocity,
                        Color.White,
                        attackEastWestFrames,
                        attackDuration,
                        scale);
                }
                else
                {
                    link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                         link.Position,
                         link.Velocity,
                        Color.White,
                        attackEastWestFrames,
                        attackDuration,
                        scale);
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
                link.currentSprite.loopAnimation = false;
                hasStarted = true;

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
                else if (timer >= maxAttackDuration)
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

            link.currentSprite.Draw(link.spriteBatch!);
        }
    }
}

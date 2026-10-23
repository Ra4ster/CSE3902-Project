using Microsoft.Xna.Framework;

namespace Byte.Player
{
    public class PlayerMoveDownState : IPlayerState
    {
        // frames for walking south animation
        readonly Rectangle[] southFrames =
        {
            new Rectangle(1, 11, 16, 16),
            new Rectangle(18,11,16,16)
        };
        public PlayerMoveDownState()
        {


        }
        public IPlayerState Update(Link link, GameTime gametime)
        {
            link.Velocity = new Vector2(0, link.MovementSpeed);
            // check if in walking sprite already
            if (link.currentSprite is not MovingAnimatedSprite)
            {
                // create new walking sprite
                link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                     link.Position,
                     link.Velocity,
                    Color.White,
                    southFrames,
                    .1f,
                    6.0f);

            }
            if (link.currentSprite is MovingAnimatedSprite)
            {
                // update current position
                MovingAnimatedSprite sprite = (MovingAnimatedSprite)link.currentSprite;

                sprite.SetPos(link.Position);
                sprite.Update(gametime);
            }
            // update origin for sprite
            link.currentSprite.MoveOrigin(Link.LinkOrigin);
            return this;
        }
        public void Draw(Link link)
        {
            link.currentSprite.Draw(link.spriteBatch!);
        }
    }
}

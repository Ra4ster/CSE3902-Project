using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Player
{
    public class PlayerMoveLeftState : IPlayerState
    {
        // contains frames for walking west 
        readonly Rectangle[] westFrames =
        {
            new Rectangle(35, 11, 16, 16),
            new Rectangle(52,11,16,16)
        };
        public PlayerMoveLeftState()
        {


        }
        public IPlayerState Update(Link link, GameTime gametime)
        {
            link.Velocity = new Vector2(-link.MovementSpeed, 0);
            // check if already walking
            if (link.currentSprite is not MovingAnimatedSprite)
            {
                //start walking if not already
                link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                     link.Position,
                     link.Velocity,
                    Color.White,
                    westFrames,
                    .1f,
                    6.0f);
                // flip for west walking
                link.currentSprite.Effects = SpriteEffects.FlipHorizontally;
            }
            if (link.currentSprite is MovingAnimatedSprite)
            {
                MovingAnimatedSprite sprite = (MovingAnimatedSprite)link.currentSprite;

                sprite.SetPos(link.Position);
                sprite.Update(gametime);
                sprite.Effects = SpriteEffects.FlipHorizontally;
            }
            link.currentSprite.MoveOrigin(Link.LinkOrigin);
            return this;
        }
        public void Draw(Link link)
        {
            link.currentSprite.Draw(link.spriteBatch);
        }
    }
}

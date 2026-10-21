using Microsoft.Xna.Framework;

namespace Byte.Player
{
    public class PlayerMoveUpState : IPlayerState
    {

        // frames for north walking
        readonly Rectangle[] northFrames =
        {
            new Rectangle(69, 11, 16,16),
            new Rectangle(86,11,16,16)
        };
        public PlayerMoveUpState()
        {


        }
        public IPlayerState Update(Link link, GameTime gametime)
        {
            link.Velocity = new Vector2(0, -link.MovementSpeed);
            // Check if already walking if not start
            if (link.currentSprite is not MovingAnimatedSprite)
            {
                link.currentSprite = new MovingAnimatedSprite(link.SpriteSheet,
                     link.Position,
                     link.Velocity,
                    Color.White,
                    northFrames,
                    .1f,
                    6.0f);
            }
            if (link.currentSprite is MovingAnimatedSprite)
            {
                MovingAnimatedSprite sprite = (MovingAnimatedSprite)link.currentSprite;

                sprite.SetPos(link.Position);
                sprite.Update(gametime);
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

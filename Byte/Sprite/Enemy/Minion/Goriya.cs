
using Byte.Projectile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy.Minion
{
    internal class Goriya : WaitingPatrolEnemy
    {
        private static readonly Rectangle DOWN_FRAME = new Rectangle(223, 12, 15, 15);
        private static readonly Rectangle UP_FRAME = new Rectangle(240, 12, 15, 15);
        private static readonly Rectangle[] SIDE_FRAMES = { new Rectangle(257, 12, 15, 15), new Rectangle(274, 12, 15, 15) };
        private static readonly Rectangle[] ALL_FRAMES = { DOWN_FRAME, UP_FRAME, SIDE_FRAMES[0], SIDE_FRAMES[1] };

        private const float WALK_FLIP_INTERVAL = 0.15f;
        private const float THROW_INTERVAL = 3.0f;

        private readonly MovingAnimatedSprite downSprite;
        private readonly MovingAnimatedSprite upSprite;
        private readonly MovingAnimatedSprite sideSprite;

        private readonly Func<Vector2, Vector2, IProjectile> throwBoomerang;
        private readonly Vector2 centerOffset;

        private Facing facing = Facing.Down;
        private float flipTimer;
        private float throwTimer;
        private IProjectile? boomerang;

        internal Goriya(Texture2D texture,
            ref Vector2 position, ref Vector2 velocity,
            Color color,
            float frameDuration, float scale,
            Vector2[] patrolPath, float patrolSpeed,
            float waitDuration,
            Func<Vector2, Vector2, IProjectile> throwBoomerang)
            : base(texture, ref position, ref velocity, color, ALL_FRAMES, frameDuration, scale, patrolPath, patrolSpeed, waitDuration)
        {
            this.throwBoomerang = throwBoomerang;
            centerOffset = new Vector2(DOWN_FRAME.Width, DOWN_FRAME.Height) * scale / 2.0f;

            downSprite = new MovingAnimatedSprite(texture, this.position, Vector2.Zero, color, [DOWN_FRAME], frameDuration, scale);
            upSprite = new MovingAnimatedSprite(texture, this.position, Vector2.Zero, color, [UP_FRAME], frameDuration, scale);
            sideSprite = new MovingAnimatedSprite(texture, this.position, Vector2.Zero, color, SIDE_FRAMES, frameDuration, scale);

            enemySprite = downSprite;
        }

        protected override void Move(GameTime gameTime)
        {
            if (boomerang != null)
            {
                velocity = Vector2.Zero;
                return;
            }
            base.Move(gameTime);
            UpdateFacing();
            AnimateVerticalWalk((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void Update(GameTime gameTime)
        {
            if (boomerang is { IsExpired: true })
                boomerang = null;

            if (boomerang != null)
            {
                velocity = Vector2.Zero;
                return;
            }
            base.Update(gameTime);

            if (boomerang is { IsExpired: true })
                boomerang = null;
            if (boomerang != null)
                return;

            throwTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (throwTimer >= THROW_INTERVAL)
            {
                throwTimer = 0f;
                boomerang = throwBoomerang(position + centerOffset, FacingDirection());
                velocity = Vector2.Zero;
            }
        }

        private void UpdateFacing()
        {
            if (velocity == Vector2.Zero)
                return;

            Facing newFacing = facing;
            if (Math.Abs(velocity.X) > Math.Abs(velocity.Y))
                newFacing = velocity.X > 0 ? Facing.Right : Facing.Left;
            else
                newFacing = velocity.Y > 0 ? Facing.Down : Facing.Up;

            if (newFacing == facing)
                return;

            facing = newFacing;
            flipTimer = 0f;
            enemySprite = facing switch
            {
                Facing.Down => downSprite,
                Facing.Up => upSprite,
                _ => sideSprite
            };
            enemySprite.Effects = facing == Facing.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        private void AnimateVerticalWalk(float seconds)
        {
            if (facing is not (Facing.Up or Facing.Down) || velocity == Vector2.Zero)
                return;

            flipTimer += seconds;
            if (flipTimer >= WALK_FLIP_INTERVAL)
            {
                flipTimer -= WALK_FLIP_INTERVAL;
                enemySprite.Effects ^= SpriteEffects.FlipHorizontally;
            }
        }

        private Vector2 FacingDirection() => facing switch
        {
            Facing.Up => new Vector2(0, -1),
            Facing.Down => new Vector2(0, 1),
            Facing.Left => new Vector2(-1, 0),
            _ => new Vector2(1, 0)
        };
    }
}
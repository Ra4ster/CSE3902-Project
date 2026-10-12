
using Byte.Projectile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy.Minion
{
    internal class Goriya : WaitingPatrolEnemy
    {
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
            float? frameDuration, float? scale,
            Vector2[]? patrolPath, float patrolSpeed,
            float? waitDuration,
            Func<Vector2, Vector2, IProjectile> throwBoomerang)
            : base(texture, ref position, ref velocity, color,
            EnemyConstants.Goriya.ALL_FRAMES,
            frameDuration ?? EnemyConstants.DEFAULT_FRAME_DURATION,
            scale ?? GameConstants.SCALE,
            patrolPath ?? EnemyConstants.Goriya.REL_PATHS, patrolSpeed,
            waitDuration ?? EnemyConstants.Goriya.WAIT_DURATION)
        {
            this.throwBoomerang = throwBoomerang;
            centerOffset = new Vector2(EnemyConstants.Goriya.DOWN_FRAME.Width, EnemyConstants.Goriya.DOWN_FRAME.Height) * (scale ?? GameConstants.SCALE) / 2.0f;
            Vector2 v = Vector2.Zero;

            downSprite = new MovingAnimatedSprite(texture, ref this.position, ref v, color, [EnemyConstants.Goriya.DOWN_FRAME], frameDuration ?? EnemyConstants.DEFAULT_FRAME_DURATION, scale ?? GameConstants.SCALE);
            upSprite = new MovingAnimatedSprite(texture, ref this.position, ref v, color, [EnemyConstants.Goriya.UP_FRAME], frameDuration ?? EnemyConstants.DEFAULT_FRAME_DURATION, scale ?? GameConstants.SCALE);
            sideSprite = new MovingAnimatedSprite(texture, ref this.position, ref v, color, EnemyConstants.Goriya.SIDE_FRAMES, frameDuration ?? EnemyConstants.DEFAULT_FRAME_DURATION, scale ?? GameConstants.SCALE);

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
            if (throwTimer >= EnemyConstants.Goriya.THROW_INTERVAL)
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
            if (flipTimer >= EnemyConstants.Goriya.WALK_FLIP_INTERVAL)
            {
                flipTimer -= EnemyConstants.Goriya.WALK_FLIP_INTERVAL;
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
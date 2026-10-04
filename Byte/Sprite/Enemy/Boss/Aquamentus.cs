
using Byte.Projectile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Byte.Sprite.Enemy
{
    internal class Aquamentus : WaitingPatrolEnemy
    {
        private static Rectangle[] sourceRects =
        {
            new Rectangle(2, 12, 23, 31),
            new Rectangle(27, 12, 23, 31)
        };

        private const float FIRE_INTERVAL = 2.0f;

        // where the mouth is on the unscaled sprite, fireballs spawn here
        private static readonly Vector2 MOUTH_OFFSET = new Vector2(2, 8);

        // aquamentus faces left and spits three fireballs in a spread
        private static readonly Vector2[] FIREBALL_DIRECTIONS =
        {
            new Vector2(-1, -0.3f),
            new Vector2(-1, 0),
            new Vector2(-1, 0.3f)
        };

        private readonly ProjectileFactory projectileFactory;
        private readonly ProjectileManager fireballs = new ProjectileManager();
        private readonly Vector2 mouthOffset;
        private float fireTimer;

        internal Aquamentus(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color, float frameDuration, float scale, Vector2[] patrolPath, float patrolSpeed, float waitDuration, ProjectileFactory projectileFactory)
            : base(texture, ref position, ref velocity, color, sourceRects, frameDuration, scale, patrolPath, patrolSpeed, waitDuration)
        {
            this.projectileFactory = projectileFactory;
            mouthOffset = MOUTH_OFFSET * scale;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            fireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fireTimer >= FIRE_INTERVAL)
            {
                fireTimer = 0f;
                Fire();
            }

            fireballs.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            fireballs.Draw(spriteBatch);
        }

        private void Fire()
        {
            foreach (Vector2 direction in FIREBALL_DIRECTIONS)
            {
                fireballs.Add(projectileFactory.CreateFireball(position + mouthOffset, direction));
            }
        }
    }
}

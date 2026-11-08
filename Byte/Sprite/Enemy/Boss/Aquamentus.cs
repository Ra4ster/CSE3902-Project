
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    internal sealed class Aquamentus : WaitingPatrolEnemy
    {
        private readonly Action<Vector2, Vector2> spawnFireball;

        private readonly Vector2 mouthOffset;
        private float fireTimer;

        internal Aquamentus(
            Texture2D texture, ref Vector2 position, ref Vector2 velocity,
            Color color,
            float frameDuration, float? scale,
            Vector2[]? patrolPath, float patrolSpeed,
            float? waitDuration,
            Action<Vector2, Vector2> spawnFireball)
            : base(texture, ref position, ref velocity, color,
            EnemyConstants.Aquamentus.SOURCE_RECTS, frameDuration,
            scale ?? GameConstants.SCALE,
            patrolPath ?? EnemyConstants.Aquamentus.REL_PATHS, patrolSpeed,
            waitDuration ?? EnemyConstants.Aquamentus.WAIT_DURATION)
        {
            this.spawnFireball = spawnFireball;
           
            mouthOffset = EnemyConstants.Aquamentus.MOUTH_OFFSET * (scale ?? GameConstants.SCALE);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            fireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fireTimer >= EnemyConstants.Aquamentus.FIRE_INTERVAL)
            {
                fireTimer = 0f;
                Fire();
                PlayRandomRoar();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void Fire()
        {
            foreach (Vector2 direction in EnemyConstants.Aquamentus.FIREBALL_DIRECTIONS)
                spawnFireball(position + mouthOffset, direction);
        }
        private void PlayRandomRoar()
        {
            int randomNumber = Random.Shared.Next(1, 4);

            switch (randomNumber)
            {
                case 1:
                    Program.soundManager.PlaySound("Boss_Scream1");
                    break;

                case 2:
                    Program.soundManager.PlaySound("Boss_Scream2");
                    break;

                case 3:
                    Program.soundManager.PlaySound("Boss_Scream3");
                    break;
            }
        }
    }

}

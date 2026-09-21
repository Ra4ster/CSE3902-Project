
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    internal class WaitingPatrolEnemy : PatrolEnemy
    {
        private float waitDuration;
        private float waitTimer = 0.0f;
        private bool waiting = false;

        internal WaitingPatrolEnemy(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color, Rectangle[] sourceRectangles, float frameDuration, float scale, Vector2[] patrolPath, float patrolSpeed, float waitDuration)
            : base(texture, ref position, ref velocity, color, sourceRectangles, frameDuration, scale, patrolPath, patrolSpeed)
        {
            this.waitDuration = waitDuration;
        }


        protected override void Move(GameTime gameTime)
        {
            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Waiting at waypoint
            if (waiting)
            {
                velocity = Vector2.Zero;
                waitTimer += seconds;

                if (waitTimer >= waitDuration)
                {
                    waitTimer = 0f;
                    waiting = false;

                    currentWaypoint = (currentWaypoint + 1) % patrolPath.Length;
                }

                return;
            }

            Vector2 target = patrolPath[currentWaypoint];

            float dx = target.X - position.X;
            float dy = target.Y - position.Y;

            const float threshold = 2f;

            if (Math.Abs(dx) + Math.Abs(dy) <= threshold)
            {
                position = target;
                velocity = Vector2.Zero;
                waiting = true;
                return;
            }

            if (Math.Abs(dx) > threshold)
                velocity = new Vector2(Math.Sign(dx) * patrolSpeed, 0f);
            else if (Math.Abs(dy) > threshold)
            {
                position.X = target.X;
                velocity = new Vector2(0f, Math.Sign(dy) * patrolSpeed);
            }
            else
                velocity = Vector2.Zero;
        }
    }
}
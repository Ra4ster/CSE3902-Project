using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    /// <summary>
    /// An enemy that patrols cardinal directions.
    /// </summary>
    /// <seealso cref="SeekingEnemy"/>
    internal class PatrolEnemy : AbstractEnemy
    {
        protected readonly Vector2[] patrolPath;
        protected int currentWaypoint;
        protected float patrolSpeed { get; }

        internal PatrolEnemy(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color,
            Rectangle[] sourceRectangles, float frameDuration, float scale, Vector2[] patrolPath, float patrolSpeed)
            : base(texture, ref position, ref velocity, color, sourceRectangles, frameDuration, scale)
        {
            this.patrolPath = Array.ConvertAll(patrolPath, ClampToWindow);
            this.patrolSpeed = patrolSpeed;
            currentWaypoint = 0;
        }

        protected override void Move(GameTime gameTime)
        {
            Vector2 target = patrolPath[currentWaypoint];

            float dx = target.X - position.X;
            float dy = target.Y - position.Y;

            const float threshold = 2f;

            if (Math.Abs(dx) + Math.Abs(dy) <= threshold)
            {
                position = target;
                currentWaypoint = (currentWaypoint + 1) % patrolPath.Length;

                target = patrolPath[currentWaypoint];
                dx = target.X - position.X;
                dy = target.Y - position.Y;
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Sprite.Enemy
{
    internal class PatrolEnemy : AbstractEnemy
    {
        private readonly Vector2[] patrolPath;
        private int currentWaypoint;
        protected float patrolSpeed { get; }

        /// <summary>
        /// Initializer for patroller
        /// </summary>
        /// <param name="texture">Texture of sprite</param>
        /// <param name="position">See MA Sprite</param>
        /// <param name="velocity">See MA Sprite</param>
        /// <param name="color">See MA Sprite</param>
        /// <param name="sourceRectangles">See MA Sprite</param>
        /// <param name="frameDuration">See MA Sprite</param>
        /// <param name="scale">See MA Sprite</param>
        /// <param name="patrolPath">Path of points to patrol around</param>
        /// <param name="patrolSpeed">Speed of movement between points</param>
        internal PatrolEnemy(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color,
            Rectangle[] sourceRectangles, float frameDuration, float scale, Vector2[] patrolPath, float patrolSpeed)
            : base(texture, ref position, ref velocity, color, sourceRectangles, frameDuration, scale)
        {
            this.patrolPath = patrolPath;
            this.patrolSpeed = patrolSpeed;
            currentWaypoint = 0;
        }

        protected override void Move(GameTime gameTime)
        {
            Vector2 direction = patrolPath[currentWaypoint] - position;

            if (direction.LengthSquared() < 25f)
            {
                currentWaypoint++;

                if (currentWaypoint >= patrolPath.Length)
                    currentWaypoint = 0;

                direction = patrolPath[currentWaypoint] - position;
            }

            if (direction != Vector2.Zero)
            {
                direction.Normalize();
                velocity = direction * patrolSpeed;
            }

            base.Update(gameTime);
        }
    }
}

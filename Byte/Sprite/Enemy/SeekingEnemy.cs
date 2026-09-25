using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Byte.Sprite;

/// <summary>
/// An enemy that seeks paths using Euclidean distance.
/// </summary>
/// <seealso cref="PatrolEnemy"/>
public class SeekingEnemy : AbstractEnemy
{
    private readonly Vector2[] patrolPath;

    private Vector2 direction = Vector2.Zero;
    private int currentWaypoint;
    protected float patrolSpeed { get; }

    internal SeekingEnemy(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color,
        Rectangle[] sourceRectangles, float frameDuration, float scale, Vector2[] patrolPath, float patrolSpeed)
        : base(texture, ref position, ref velocity, color, sourceRectangles, frameDuration, scale)
    {
        this.patrolPath = patrolPath;
        this.patrolSpeed = patrolSpeed;
        currentWaypoint = 0;
    }

    protected override void Move(GameTime gameTime)
    {
        Vector2 target = patrolPath[currentWaypoint];

        float dx = target.X - position.X;
        float dy = target.Y - position.Y;
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

        if (distance < 2.0f) // Small threshold distance
        {
            currentWaypoint = (currentWaypoint + 1) % patrolPath.Length;
            target = patrolPath[currentWaypoint];
        }

        if (distance <= 0f)
            velocity = Vector2.Zero;
        else
        {
            direction.X = target.X - position.X;
            direction.Y = target.Y - position.Y;
            direction.Normalize();
            velocity.X = direction.X * patrolSpeed;
            velocity.Y = direction.Y * patrolSpeed;
        }
    }
}
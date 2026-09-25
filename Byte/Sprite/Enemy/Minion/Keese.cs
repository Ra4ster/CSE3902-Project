
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Keese : SeekingEnemy
{
    private static readonly Rectangle[] sourceRects = {
        new Rectangle(183, 11, 16, 16),
        new Rectangle(200, 11, 16, 16)
    };

    internal Keese(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color, float frameDuration, float scale, Vector2[] patrolPath, float patrolSpeed) : base(texture, ref position, ref velocity, color, sourceRects, frameDuration, scale, patrolPath, patrolSpeed)
    {
    }
}
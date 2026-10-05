
using Microsoft.Xna.Framework;

public static class GameConstants
{
    public const float SCALE = 10.0f;
    public const int TILE_SIZE = 16;

    public static readonly Rectangle WINDOW_SIZE = new Rectangle(0, 0, 1600, 1600);
    public static readonly Rectangle MIN_WINDOW_SIZE = new Rectangle(0, 0, 800, 600);
    public static readonly Color BG_COLOR = Color.SkyBlue;

    public static readonly Vector2 LINK_START_POS = new Vector2(220.0f, 220.0f);
}
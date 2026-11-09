
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte
{
    public class BackgroundManager
    {
        private BackgroundManager() {/* Eager Singleton */}

        private const int START_X = 1;
        private const int START_Y = 192;
        private const int WIDTH = 192;
        private const int HEIGHT = 112;
        private const int GAP = 3;
        private const int COLUMNS = 6;
        private const string ROOM_PREFIX = "room";

        private static Vector2 SCALE_VEC = new Vector2(GameConstants.SCALE);

        public static readonly BackgroundManager _instance = new BackgroundManager();
        public static BackgroundManager Instance => _instance;

        /// <summary>
        /// Outputs a rectangle coordinates for a room name.
        /// </summary>
        /// <param name="bgName">Valid room name, i.e. "room1", "room5", etc.</param>
        /// <returns>Rectangle bounds of the room background in GameAssets.BlockSheet</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no background exists with that bg name.</exception>
        public Rectangle Map(string bgName)
        {
            ArgumentNullException.ThrowIfNull(bgName);

            if (!bgName.StartsWith(ROOM_PREFIX, StringComparison.OrdinalIgnoreCase) ||
                !int.TryParse(
                    bgName.AsSpan(ROOM_PREFIX.Length),
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out int roomNum) ||
                roomNum < 1)
            {
                throw new KeyNotFoundException($"No room texture is named {bgName}");
            }

            int roomIndex = roomNum - 1;
            int column = roomIndex % COLUMNS;
            int row = roomIndex / COLUMNS;
            int x = START_X + (WIDTH + GAP) * column;
            int y = START_Y + (HEIGHT + GAP) * row;
            return new Rectangle(x, y, WIDTH, HEIGHT);
        }

        /// <summary>
        /// Draws `bgName` to the screen; see Map() for details.
        /// </summary>
        /// <param name="bgName">Name of bg (i.e. room1, room3)</param>
        /// <param name="painter">SpriteBatch for drawing</param>
        public void Draw(string bgName, SpriteBatch painter)
        {
            Rectangle box = Map(bgName);
            Texture2D blockSheet = GameAssets.Instance.BlockSheet;
            if (box.Left < 0 || box.Top < 0 ||
                box.Right > blockSheet.Width || box.Bottom > blockSheet.Height)
            {
                throw new InvalidDataException(
                    $"Background '{bgName}' maps outside the block sheet ({blockSheet.Width}x{blockSheet.Height}).");
            }

            painter.Draw(blockSheet, Vector2.Zero, box, Color.White, 0f, Vector2.Zero, SCALE_VEC, SpriteEffects.None, 0f);
        }
    }
}
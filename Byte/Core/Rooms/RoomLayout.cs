using Byte.InventoryAndHud;
using Microsoft.Xna.Framework;

namespace Byte.Rooms;

public static class RoomLayout
{
    public static Vector2 RoomOffset(IRoom room)
    {
        ArgumentNullException.ThrowIfNull(room);

        const float centeringDivisor = 2f;
        Rectangle bounds = room.Bounds;
        float playHeight = GameConstants.WINDOW_SIZE.Height - InventoryAndHUDGraphics.HudHeight;
        return new Vector2(
            (GameConstants.WINDOW_SIZE.Width - bounds.Width) / centeringDivisor,
            InventoryAndHUDGraphics.HudHeight + Math.Max(0f, (playHeight - bounds.Height) / centeringDivisor)
        );
    }

    public static Rectangle RoomViewport(IRoom room)
    {
#pragma warning disable S109 // Magic numbers should not be used
        Vector2 offset = RoomLayout.RoomOffset(room);
        int frameMargin = 2 * (int)RoomGrid.TILE_WORLD_SIZE;
        return new Rectangle(
            (int)offset.X - frameMargin,
            (int)offset.Y - frameMargin,
            room.Bounds.Width + 2 * frameMargin,
            room.Bounds.Height + 2 * frameMargin);
#pragma warning restore S109 // Magic numbers should not be used
    }
}

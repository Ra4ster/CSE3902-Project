using Byte.Item;
using Byte.Rooms;

namespace Byte.States;

public static class GameEvents
{
    public static event Action? TriforcePickedUp;

    public static event Action? LinkDied;

    public static event Action<IRoom>? RoomCleared;

    public static event Action<IItem>? ItemPickedUp;

    public static void RaiseTriforcePickedUp() => TriforcePickedUp?.Invoke();

    public static void RaiseLinkDied() => LinkDied?.Invoke();

    public static void RaiseRoomCleared(IRoom room) => RoomCleared?.Invoke(room);

    public static void RaiseItemPickedUp(IItem item) => ItemPickedUp?.Invoke(item);
}

using Byte.Core.Rooms;
using Byte.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.States;

public sealed class RoomTransitionState : IGameState
{
    private readonly Program game;
    private readonly Door door;

    private IRoom fromRoom = null!;
    private IRoom toRoom = null!;
    private Vector2 arrivalTile;
    private Vector2 linkStart;
    private Vector2 linkEnd;
    private Vector2 scrollDirection;
    private float duration;
    private float elapsed;

    public RoomTransitionState(Program game, Door door)
    {
        this.game = game;
        this.door = door;
    }

    private bool IsScroll => scrollDirection != Vector2.Zero;

    private float Progress => MathHelper.Clamp(elapsed / duration, 0f, GameStateConstants.FullProgress);

    public void Enter()
    {
        fromRoom = game.Rooms.CurrentRoom;
        linkStart = game.link.Position + Program.RoomOffset(fromRoom);

        arrivalTile = game.Rooms.EnterDoor(door);
        toRoom = game.Rooms.CurrentRoom;
        linkEnd = Program.LinkPositionAt(arrivalTile) + Program.RoomOffset(toRoom);

        scrollDirection = ScrollDirection(door.doorDirection);
        duration = IsScroll ? GameStateConstants.RoomScrollDuration : GameStateConstants.RoomFadeDuration;
        elapsed = 0f;
    }

    public void Update(GameTime gameTime)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (elapsed < duration)
            return;

        game.PlaceLink(arrivalTile);
        game.StateMachine.ChangeState(new GameplayState(game));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsScroll)
            DrawScroll();
        else
            DrawFade(spriteBatch);
        game.DrawHud(0f);
    }

    private void DrawScroll()
    {
        float t = Progress;
        Vector2 fromShift = -scrollDirection * RoomSize(fromRoom) * t;
        Vector2 toShift = scrollDirection * RoomSize(toRoom) * (GameStateConstants.FullProgress - t);
        Rectangle camera = game.RoomViewport(toRoom);

        DrawRoom(fromRoom, fromShift, camera);
        DrawRoom(toRoom, toShift, camera);

        Vector2 linkAt = Vector2.Lerp(linkStart + fromShift, linkEnd + toShift, t);
        DrawLinkAt(linkAt, camera);
    }

    private void DrawFade(SpriteBatch spriteBatch)
    {
        float t = Progress;
        bool showingOldRoom = t < GameStateConstants.HalfProgress;
        IRoom room = showingOldRoom ? fromRoom : toRoom;
        Rectangle camera = game.RoomViewport(room);

        DrawRoom(room, Vector2.Zero, camera);
        DrawLinkAt(showingOldRoom ? linkStart : linkEnd, camera);

        float darkness = showingOldRoom
            ? t / GameStateConstants.HalfProgress
            : (GameStateConstants.FullProgress - t) / GameStateConstants.HalfProgress;
        game.BeginScreen();
        game.Overlay.FillScreen(spriteBatch, new Color(Color.Black, darkness));
        game.EndBatch();
    }

    private void DrawRoom(IRoom room, Vector2 shift, Rectangle camera)
    {
        game.BeginWorld(Program.RoomOffset(room) + shift, camera);
        room.Draw(game.Painter);
        game.EndBatch();
    }

    private void DrawLinkAt(Vector2 windowPosition, Rectangle camera)
    {
        game.BeginWorld(windowPosition - game.link.Position, camera);
        game.link.state.Draw(game.link);
        game.EndBatch();
    }

    private static Vector2 RoomSize(IRoom room) => new(room.Bounds.Width, room.Bounds.Height);

    private static Vector2 ScrollDirection(DoorDirection direction) => direction switch
    {
        DoorDirection.North => -Vector2.UnitY,
        DoorDirection.South => Vector2.UnitY,
        DoorDirection.East => Vector2.UnitX,
        DoorDirection.West => -Vector2.UnitX,
        _ => Vector2.Zero
    };
}

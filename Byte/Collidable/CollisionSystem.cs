using System.Collections.ObjectModel;
using Byte.Player;
using Byte.Projectile;
using Byte.Rooms;
using Byte.Sounds;
using Microsoft.Xna.Framework;

namespace Byte.Collidable
{

    public class CollisionSystem
    {
        private readonly Link _link;
        private readonly CollisionDispatcher _dispatcher;
        private Door? pendingDoor;

        public CollisionSystem(
            IEnumerable<IRoom> rooms,
            Link player,
            CollisionDispatcher dispatcher
        )
        {
            this._link = player;
            this._dispatcher = dispatcher;

            foreach (IRoom room in rooms)
            {
                foreach (Door door in room.Doors)
                {
                    door.Properties.OnCollide = _ =>
                    {
                        if (door.CanEnter)
                            pendingDoor = door;
                    };
                }
            }
        }

        public Door? ProcessCollisions(IRoom currentRoom)
        {
            // Resolve barriers first, so body and sword contacts use the corrected position.
            foreach (ICollidable block in currentRoom.Blocks.OfType<ICollidable>())
                DispatchCollisions(new Collection<ICollidable> { _link, block });
            Vector2 roomOffset = RoomLayout.RoomOffset(currentRoom);
            Rectangle visibleWorldBounds = new(
                (int)-roomOffset.X,
                (int)-roomOffset.Y,
                GameConstants.WINDOW_SIZE.Width,
                GameConstants.WINDOW_SIZE.Height);
            LinkCollisionHandler.ClampToRoom(_link, visibleWorldBounds);
            LinkCollisionHandler.ClampToRoom(_link, currentRoom.Bounds, currentRoom.Doors);

            foreach (ICollidable block in currentRoom.Blocks.OfType<ICollidable>())
            {
                foreach (IProjectile projectile in ProjectileManager.Instance.Projectiles)
                    DispatchCollisions(new Collection<ICollidable> { projectile, block });
                foreach (ICollidable enemy in currentRoom.Enemies)
                    DispatchCollisions(new Collection<ICollidable> { enemy, block });
            }

            foreach (Bomb bomb in ProjectileManager.Instance.Projectiles.OfType<Bomb>())
            {
                if (!bomb.IsExploding)
                    continue;

                foreach (Door bombableDoor in currentRoom.Doors)
                    if (bomb.BoundingBox.Intersects(bombableDoor.InteractionBounds(currentRoom.Bounds)) &&
                        bombableDoor.TryBlowOpen())
                        SoundManager.Instance.PlaySound("Secret");
            }

            var collidables = new List<ICollidable> { _link };
            collidables.AddRange(currentRoom.Enemies);
            collidables.AddRange(currentRoom.Items);
            collidables.AddRange(currentRoom.Doors);
            collidables.AddRange(ProjectileManager.Instance.Projectiles);

            _link.Sword.Update(_link);
            if (!_link.Sword.BoundingBox.IsEmpty)
                collidables.Add(_link.Sword);
            DispatchCollisions(collidables);

            Door? requestedDoor = pendingDoor;
            pendingDoor = null;
            return requestedDoor;
        }

        private void DispatchCollisions(IReadOnlyList<ICollidable> collidables)
        {
            var detector = new CollisionDetector();
            foreach (var collision in detector.DetectCollisions(collidables))
                _dispatcher.Dispatch(collision.A, collision.B, collision.Side);
        }

    };
}
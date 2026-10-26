using Byte.Rooms;
using Byte.Sprite.Enemy;
using Microsoft.Xna.Framework;


namespace Byte.Collidable
{
    public static class EnemyCollisionHandler
    {
        public static void Register(CollisionDispatcher dispatcher)
        {
            ArgumentNullException.ThrowIfNull(dispatcher);
            dispatcher.Register(CollidableTypes.Enemy, CollidableTypes.Block, ResolveBlock);
            dispatcher.Register(CollidableTypes.Enemy, CollidableTypes.Door, ResolveDoor);
            dispatcher.Register(CollidableTypes.Enemy, CollidableTypes.Enemy, ResolveEnemy);
        }
        

        private static void ResolveBlock(ICollidable enemy, ICollidable block, CollisionSide side)
        {
            if (enemy is not AbstractEnemy target || !block.Properties.BlocksPlayer)
                return;

            Rectangle overlap = Rectangle.Intersect(target.BoundingBox, block.BoundingBox);
            if (overlap.Width <= 0 || overlap.Height <= 0)
                return;

            Vector2 correction = side switch
            {
                CollisionSide.Right => new Vector2(-overlap.Width, 0),
                CollisionSide.Left => new Vector2(overlap.Width, 0),
                CollisionSide.Bottom => new Vector2(0, -overlap.Height),
                CollisionSide.Top => new Vector2(0, overlap.Height),
                _ => Vector2.Zero
            };
            target.OnBlocked(correction);
        }

        private static void ResolveDoor(ICollidable enemy, ICollidable door, CollisionSide side)
        {
            if (enemy is not AbstractEnemy target)
                return;

            Rectangle overlap = Rectangle.Intersect(target.BoundingBox, door.BoundingBox);
            if (overlap.Width <= 0 || overlap.Height <= 0)
                return;

            Vector2 correction = side switch
            {
                CollisionSide.Right => new Vector2(-overlap.Width, 0),
                CollisionSide.Left => new Vector2(overlap.Width, 0),
                CollisionSide.Bottom => new Vector2(0, -overlap.Height),
                CollisionSide.Top => new Vector2(0, overlap.Height),
                _ => Vector2.Zero
            };
            target.OnBlocked(correction);
        }

        private static void ResolveEnemy(ICollidable enemy, ICollidable enemyTwo, CollisionSide side)
        {

            if (enemy is not AbstractEnemy target)
                return;

            Rectangle overlap = Rectangle.Intersect(target.BoundingBox, enemyTwo.BoundingBox);
            if (overlap.Width <= 0 || overlap.Height <= 0)
                return;

            Vector2 correction = side switch
            {
                CollisionSide.Right => new Vector2(-overlap.Width, 0),
                CollisionSide.Left => new Vector2(overlap.Width, 0),
                CollisionSide.Bottom => new Vector2(0, -overlap.Height),
                CollisionSide.Top => new Vector2(0, overlap.Height),
                _ => Vector2.Zero
            };
            target.OnBlocked(correction);


        }
    }


}
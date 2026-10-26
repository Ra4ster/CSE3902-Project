using Microsoft.Xna.Framework;

namespace Byte.Collidable
{
    public enum CollisionSide
    {
        None,
        Top,
        Bottom,
        Left,
        Right
    }
    public class CollisionProperties
    {
        public bool DamagesPlayer = false;

        public bool DamagesEnemies = false;

        public bool IsStatic = false;

        public bool BlocksPlayer = false;


        public Action<ICollidable>? OnCollide { get; set; }

        public string Type = CollidableTypes.Base;
    }
    public interface ICollidable
    {
        Rectangle BoundingBox { get; }
        CollisionProperties Properties { get; }
    }
}

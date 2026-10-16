using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Byte.Collidable
{
    public enum CollisionSide
    { 
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

        public Action<ICollidable>? OnCollide { get; set; }

        public string Type = CollidableTypes.Base;
    }
    public interface ICollidable
    {
        Rectangle BoundingBox { get; }
        CollisionProperties Properties { get; }
    }
}

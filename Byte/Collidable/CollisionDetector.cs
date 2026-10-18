using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Byte.Collidable
{
    public class CollisionDetector
    {
        public List<(ICollidable A, ICollidable B, Rectangle Intersection, CollisionSide Side)>
            DetectCollisions(IList<ICollidable> objects)
        {
            var collisions = new List<(ICollidable A, ICollidable B, Rectangle Intersection, CollisionSide Side)>();
            for (int i = 0; i < objects.Count; i++)
            {
                for (int j = i + 1; j < objects.Count; j++)
                {
                    var a = objects[i];
                    var b = objects[j];
                    if (a.BoundingBox.Intersects(b.BoundingBox))
                    {
                        var intersection = Rectangle.Intersect(a.BoundingBox, b.BoundingBox);
                        CollisionSide side = GetCollisionSide(a.BoundingBox, b.BoundingBox, intersection);
                        collisions.Add((a, b, intersection, side));
                    }
                }
            }
            return collisions;
        }
        private CollisionSide GetCollisionSide(
            Rectangle a,
            Rectangle b,
            Rectangle intersection)
        {
            if (intersection.Width < intersection.Height)
            {
                return a.Center.X < b.Center.X
                    ? CollisionSide.Right
                    : CollisionSide.Left;
            }

            return a.Center.Y < b.Center.Y
                ? CollisionSide.Bottom
                : CollisionSide.Top;
        }
    }

}

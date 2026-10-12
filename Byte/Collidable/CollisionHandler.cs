using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Byte.Collidable
{
    public delegate void CollisionHandler(ICollidable a,ICollidable b,CollisionSide side);
}

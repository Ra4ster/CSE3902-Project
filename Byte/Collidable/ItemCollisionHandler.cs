using Byte.Item;
using Byte.Player;
using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Collidable
{
    public static class ItemCollisionHandler
    {
        public static void Register(CollisionDispatcher dispatcher)
        {
            dispatcher.Register(CollidableTypes.Player, CollidableTypes.ItemPickup, ResolveItemPickup);
        }

        private static void ResolveItemPickup(ICollidable player, ICollidable item, CollisionSide side)
        {
            if (player is Link && item is IItem collectibleItem)
            {
                collectibleItem.IsCollected = true;
            }
        }
    }
}
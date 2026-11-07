using Byte.Collidable;
using Byte.Command;
using Byte.Item;
using Byte.Projectile;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.InventoryAndHud
{
    public sealed class InventoryManager : IDisposable
    {
        private readonly InventoryData inventoryData;

        public InventoryManager(InventoryData invData)
        {
            this.inventoryData = invData;
            LinkCollisionHandler.PickUpItemEvent += HandleItemPickUp;
            UseProjectileCommand.OnProjectileFired += HandleProjectileFired;
        }

        public void Dispose()
        {
            LinkCollisionHandler.PickUpItemEvent -= HandleItemPickUp;
            UseProjectileCommand.OnProjectileFired -= HandleProjectileFired;
        }

        public bool CanUseItem(IProjectile projectile)
        {
            return projectile switch
            {
                Projectile.Bomb => inventoryData.Bombs > 0,
                Projectile.Arrow => inventoryData.HasBow && inventoryData.Rupees > 0,
                Projectile.Boomerang => inventoryData.HasBoomerang,
                _ => true
            };
        }

        private void HandleItemPickUp(IItem item)
        {
            switch (item)
            {
                case Rupee:
                    inventoryData.Rupees++;
                    break;
                case Item.Bomb:
                    inventoryData.Bombs++;
                    inventoryData.HasBomb = true; 
                    break;
                case Item.Boomerang:
                    inventoryData.HasBoomerang = true;
                    break;
                case Bow:
                    inventoryData.HasBow = true;
                    break;
            }
        }

        private void HandleProjectileFired(IProjectile item)
        {
            if (item is Projectile.Bomb)
            {
                inventoryData.Bombs--;
                if (inventoryData.Bombs <= 0)
                {
                    inventoryData.Bombs = 0;
                    inventoryData.HasBomb = false;
                }
            }

            if (item is Projectile.Arrow)
            {
                inventoryData.Rupees--;
                if (inventoryData.Rupees <= 0)
                {
                    inventoryData.Rupees = 0;
                }
            }
        }
    }
}
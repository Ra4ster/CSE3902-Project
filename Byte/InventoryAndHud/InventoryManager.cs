using Byte.Collidable;
using Byte.Item;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.InventoryAndHud;


public class InventoryManager
{
    private readonly InventoryData inventoryData;
    public InventoryManager(InventoryData invData) 
    {
        this.inventoryData = invData;

        LinkCollisionHandler.PickUpItemEvent += HandleItemPickUp;
    }

    private void HandleItemPickUp(IItem item)
    {
        switch (item)
        {
            case Rupee:
                inventoryData.Rupees++;
                break;
            case Bomb:
                inventoryData.Bombs++;
                break;
            case Boomerang:
                inventoryData.HasBoomerang = true;
                break;
        }
    }

}


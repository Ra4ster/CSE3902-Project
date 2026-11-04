using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.InventoryAndHud;

public class SelectionController
{
    private readonly InventoryData inventory;
    public ItemType EquippedItem { get; private set; } = ItemType.None;

    public Point CursorGridPosition { get; private set; } = new Point(0, 0);

    public SelectionController(InventoryData inventoryData)
    {
        this.inventory = inventoryData;

        SetInitialEquippedItem();
    }

    private void SetInitialEquippedItem()
    {
        if (inventory.HasBoomerang) EquippedItem = ItemType.Boomerang;
        else if (inventory.HasBomb) EquippedItem = ItemType.Bomb;
        else if (inventory.HasBow) EquippedItem = ItemType.Bow;
    }

    public List<ItemType> GetAvailableItems()
    {
        List<ItemType> items = new List<ItemType>();
        if (inventory.HasBoomerang) items.Add(ItemType.Boomerang);
        if (inventory.HasBomb) items.Add(ItemType.Bomb);
        if (inventory.HasBow) items.Add(ItemType.Bow);
        return items;
    }

    public void GoToNextItem()
    {
        List<ItemType> available = GetAvailableItems();
        if (available.Count == 0) return;

        int currentIndex = available.IndexOf(EquippedItem);
        int nextIndex = (currentIndex + 1) % available.Count;
        EquippedItem = available[nextIndex];
    }

    public void HandleInput(KeyboardState currentKey, KeyboardState previousKey)
    {

        if (currentKey.IsKeyDown(Keys.Right) && previousKey.IsKeyUp(Keys.Right))
        {
            GoToNextItem();
        }
    }
}


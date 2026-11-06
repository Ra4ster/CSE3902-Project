using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Byte.InventoryAndHud;

public class SelectionController
{
    private readonly InventoryData inventory;

    public ItemType EquippedItem { get; private set; } = ItemType.None;
    public ItemType EquippedAItem { get; private set; } = ItemType.Sword;

    public Point CursorGridPosition { get; private set; } = new Point(0, 0);

    public SelectionController(InventoryData inventoryData)
    {
        this.inventory = inventoryData;
        SetInitialEquippedItems();
    }

    private void SetInitialEquippedItems()
    {
        if (inventory.HasBoomerang) EquippedItem = ItemType.Boomerang;
        else if (inventory.HasBomb) EquippedItem = ItemType.Bomb;
        else if (inventory.HasBow) EquippedItem = ItemType.Bow;

        if (inventory.HasSword) EquippedAItem = ItemType.Sword;
    }

    public void MoveCursor(int dx, int dy)
    {
        int newX = MathHelper.Clamp(CursorGridPosition.X + dx, 0, 3);
        int newY = MathHelper.Clamp(CursorGridPosition.Y + dy, 0, 1);
        CursorGridPosition = new Point(newX, newY);
    }

    public void EquipSelectedAtCursor()
    {
        if (CursorGridPosition == new Point(0, 0) && inventory.HasBoomerang)
            EquippedItem = ItemType.Boomerang;
        else if (CursorGridPosition == new Point(1, 0) && inventory.HasBomb)
            EquippedItem = ItemType.Bomb;
        else if (CursorGridPosition == new Point(2, 0) && inventory.HasBow)
            EquippedItem = ItemType.Bow;
    }

    public void HandleInput(KeyboardState currentKey, KeyboardState previousKey)
    {
        if (currentKey.IsKeyDown(Keys.Right) && previousKey.IsKeyUp(Keys.Right)) MoveCursor(1, 0);
        if (currentKey.IsKeyDown(Keys.Left) && previousKey.IsKeyUp(Keys.Left)) MoveCursor(-1, 0);
        if (currentKey.IsKeyDown(Keys.Up) && previousKey.IsKeyUp(Keys.Up)) MoveCursor(0, -1);
        if (currentKey.IsKeyDown(Keys.Down) && previousKey.IsKeyUp(Keys.Down)) MoveCursor(0, 1);

        if (currentKey.IsKeyDown(Keys.Enter) && previousKey.IsKeyUp(Keys.Enter))
        {
            EquipSelectedAtCursor();
        }
    }
}
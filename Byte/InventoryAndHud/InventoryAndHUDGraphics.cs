using Byte.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.InventoryAndHud;

internal class InventoryAndHUDGraphics
{
    private readonly Texture2D spriteSheet;

    private readonly Texture2D blackPixel;
    private readonly Texture2D itemSheet;
    private readonly SpriteFont font;
    private readonly InventoryData inventory;
    private readonly SelectionController selectionController;

    private static readonly Rectangle BoomerangSourceRect = new Rectangle(129, 3, 5, 8);
    private static readonly Rectangle BombSourceRect = new Rectangle(136, 0, 8, 16);
    private static readonly Rectangle BowSourceRect = new Rectangle(144, 0, 8, 16);

    private float screenPosition = 0f;

    private readonly float maxOffset = GameConstants.WINDOW_SIZE.Height - 600;

    public InventoryAndHUDGraphics(Texture2D itemSheet, Texture2D spriteSheet, SpriteFont font, InventoryData inventory, SelectionController selectionController)
    {
        this.spriteSheet = spriteSheet;
        this.font = font;
        this.inventory = inventory;
        this.selectionController = selectionController;
        this.itemSheet = itemSheet;

        blackPixel = new Texture2D(spriteSheet.GraphicsDevice, 1, 1);
        blackPixel.SetData(new[] { Color.Black });
    }

    public GameState UpdateAnimation(GameTime gameTime, GameState currentState)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (currentState == GameState.TransitionToInventory)
        {
            screenPosition += 1000f * delta;
            if (screenPosition >= maxOffset)
            {
                screenPosition = maxOffset;
                return GameState.Inventory; 
            }
        }
        else if (currentState == GameState.TransitionToPlaying)
        {
            screenPosition -= 1000f * delta;
            if (screenPosition <= 0f)
            {
                screenPosition = 0f;
                return GameState.Playing; 
            }
        }
        else if (currentState == GameState.Inventory)
        {
            screenPosition = maxOffset;
        }
        else if (currentState == GameState.Playing)
        {
            screenPosition = 0f;
        }

        return currentState;
    }

    public void Draw(SpriteBatch spriteBatch, GameState currentState)
    {
        Rectangle bgRect = new Rectangle(0, 0, (int)(256 * GameConstants.SCALE), (int)(GameConstants.WINDOW_SIZE.Y));
        spriteBatch.Draw(blackPixel, bgRect, Color.Black);

        Vector2 inventoryPosition = new Vector2(0, -maxOffset + screenPosition);
        DrawInventoryScreen(spriteBatch, inventoryPosition);

        Vector2 hudPosition = new Vector2(0, screenPosition);
        DrawHUD(spriteBatch, hudPosition);
    }
    private void DrawHUD(SpriteBatch spriteBatch, Vector2 hudPosition)
    {

        Rectangle hudBgRect = new Rectangle(258, 11, 256, 55);
        spriteBatch.Draw(spriteSheet, hudPosition, hudBgRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);

        Vector2 scaleOffset = new Vector2(GameConstants.SCALE, GameConstants.SCALE);

        spriteBatch.DrawString(font, $"X{inventory.Rupees}", hudPosition + new Vector2(128f, 32f) * scaleOffset, Color.White);
        spriteBatch.DrawString(font, $"X{inventory.Keys}", hudPosition + new Vector2(128f, 48f) * scaleOffset, Color.White);
        spriteBatch.DrawString(font, $"X{inventory.Bombs}", hudPosition + new Vector2(128f, 64f) * scaleOffset, Color.White);

        int fullHearts = inventory.CurrentHalfHearts / 2;
        bool hasHalfHeart = (inventory.CurrentHalfHearts % 2) != 0;

        Vector2 heartStartPos = hudPosition + new Vector2(175, 30) * scaleOffset;

        Rectangle fullHeartRect = new Rectangle(645, 117, 8, 8);
        Rectangle halfHeartRect = new Rectangle(636, 117, 8, 8);
        Rectangle emptyHeartRect = new Rectangle(627, 117, 8, 8);
        if (selectionController.EquippedItem == ItemType.Boomerang && inventory.HasBoomerang)
        {
            Vector2 bItemPos = hudPosition + new Vector2(104f, 24f) * scaleOffset; // Adjust to your B-box coordinates
            spriteBatch.Draw(itemSheet, bItemPos, BoomerangSourceRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
        }
        else if (selectionController.EquippedItem == ItemType.Bomb && inventory.HasBomb)
        {
            Vector2 bItemPos = hudPosition + new Vector2(104f, 24f) * scaleOffset;
            spriteBatch.Draw(itemSheet, bItemPos, BombSourceRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
        }
        int maxContainers = inventory.MaxHalfHearts / 2;
        for (int i = 0; i < maxContainers; i++)
        {
            Rectangle heartRect;
            int currentHeartValue = inventory.CurrentHalfHearts - (i * 2);

            if (currentHeartValue >= 2)
                heartRect = fullHeartRect;
            else if (currentHeartValue == 1)
                heartRect = halfHeartRect;
            else
                heartRect = emptyHeartRect;

            Vector2 heartPos = heartStartPos + new Vector2(i * 8f, 0) * scaleOffset;
            spriteBatch.Draw(spriteSheet, heartPos, heartRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
        }



    }

    private void DrawInventoryScreen(SpriteBatch spriteBatch, Vector2 inventoryPosition)
    {
        Vector2 scaleOffset = new Vector2(GameConstants.SCALE, GameConstants.SCALE);


        Rectangle invBgRect = new Rectangle(1, 11, 255, 87);
        Rectangle mapBgRect = new Rectangle(258, 112, 255, 87);
        Vector2 HUDPosition = inventoryPosition + new Vector2(0, invBgRect.Height * GameConstants.SCALE);

        spriteBatch.Draw(spriteSheet, inventoryPosition, invBgRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
        spriteBatch.Draw(spriteSheet, HUDPosition, mapBgRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);

        if (selectionController.EquippedItem == ItemType.Boomerang && inventory.HasBoomerang)
        {
        }
        else if (selectionController.EquippedItem == ItemType.Bomb && inventory.HasBomb)
        {

        }

        if (inventory.HasBoomerang)
        {
            Vector2 drawPos = inventoryPosition + new Vector2(125f, 40f) * scaleOffset;
            spriteBatch.Draw(itemSheet, drawPos, BoomerangSourceRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
        }

        if (inventory.HasBomb)
        {
            Vector2 drawPos = inventoryPosition + new Vector2(145f, 40f) * scaleOffset;
            spriteBatch.Draw(itemSheet, drawPos, BombSourceRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
        }

        if (inventory.HasBow)
        {
            Vector2 drawPos = inventoryPosition + new Vector2(165f, 40f) * scaleOffset;
            spriteBatch.Draw(itemSheet, drawPos, BowSourceRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
        }
            if (inventory.HasMap)
        {
        }
        if (inventory.HasCompass)
        {
        }

        foreach (Point room in inventory.VisitedRooms)
        {
            Vector2 roomMapDrawPos = inventoryPosition + new Vector2(120f + (room.X * 8f), 100f + (room.Y * 8f)) * scaleOffset;

        }
    }
}


using Byte.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.InventoryAndHud
{
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
        private static readonly Rectangle SwordSourceRect = new Rectangle(104, 0, 8, 16);

        private static readonly Rectangle CursorSelectRectangle = new Rectangle(519, 137, 16, 16);
        public const float MaxSlideOffset = 174f * GameConstants.SCALE;
        public const float HudHeight = 55f * GameConstants.SCALE;

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

        public void Draw(SpriteBatch spriteBatch, float slideOffset)
        {
            if (slideOffset > 0f)
            {
                Rectangle bgRect = new Rectangle(
                    0,
                    (int)(-MaxSlideOffset + slideOffset),
                    GameConstants.WINDOW_SIZE.Width,
                    (int)MaxSlideOffset
                );
                spriteBatch.Draw(blackPixel, bgRect, Color.Black);
            }

            Vector2 inventoryPosition = new Vector2(0, -MaxSlideOffset + slideOffset);
            DrawInventoryScreen(spriteBatch, inventoryPosition);

            Vector2 hudPosition = new Vector2(0, slideOffset);
            DrawHUD(spriteBatch, hudPosition);
        }

        private Rectangle GetItemSourceRect(ItemType itemType)
        {
            return itemType switch
            {
                ItemType.Boomerang => BoomerangSourceRect,
                ItemType.Bomb => BombSourceRect,
                ItemType.Bow => BowSourceRect,
                ItemType.Sword => SwordSourceRect,
                _ => Rectangle.Empty
            };
        }

        private void DrawHUD(SpriteBatch spriteBatch, Vector2 hudPosition)
        {
            Rectangle hudBgRect = new Rectangle(258, 11, 256, 55);
            spriteBatch.Draw(spriteSheet, hudPosition, hudBgRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);

            Vector2 scaleOffset = new Vector2(GameConstants.SCALE, GameConstants.SCALE);

            spriteBatch.DrawString(font, $"X{inventory.Rupees}", hudPosition + new Vector2(95f, 16f) * scaleOffset, Color.White);
            spriteBatch.DrawString(font, $"X{inventory.Keys}", hudPosition + new Vector2(95f, 32f) * scaleOffset, Color.White);
            spriteBatch.DrawString(font, $"X{inventory.Bombs}", hudPosition + new Vector2(95f, 40f) * scaleOffset, Color.White);

            Rectangle bSourceRect = GetItemSourceRect(selectionController.EquippedItem);
            if (bSourceRect != Rectangle.Empty)
            {
                Vector2 itemPos = hudPosition + new Vector2(126f, 24f) * scaleOffset;
                spriteBatch.Draw(itemSheet, itemPos, bSourceRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
            }

            Rectangle aSourceRect = GetItemSourceRect(selectionController.EquippedAItem);
            if (aSourceRect != Rectangle.Empty)
            {
                Vector2 itemPos = hudPosition + new Vector2(153f, 24f) * scaleOffset;
                spriteBatch.Draw(itemSheet, itemPos, aSourceRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
            }

            Vector2 heartStartPos = hudPosition + new Vector2(176, 24) * scaleOffset;
            Rectangle fullHeartRect = new Rectangle(645, 117, 8, 8);
            Rectangle halfHeartRect = new Rectangle(636, 117, 8, 8);
            Rectangle emptyHeartRect = new Rectangle(627, 117, 8, 8);

            int maxContainers = inventory.MaxHalfHearts / 2;
            for (int i = 0; i < maxContainers; i++)
            {

                int currentHeartValue = inventory.CurrentHalfHearts - (i * 2);

                Rectangle heartRect;
                if (currentHeartValue >= 2)
                    heartRect = fullHeartRect;
                else if (currentHeartValue == 1)
                    heartRect = halfHeartRect;
                else
                    heartRect = emptyHeartRect;

                int row = i / 8;
                int col = i % 8;
                Vector2 heartPos = heartStartPos + new Vector2(col * 8f, row * 8f) * scaleOffset;
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


            Rectangle bSlotItem = GetItemSourceRect(selectionController.EquippedItem);
            if (bSlotItem != Rectangle.Empty)
            {
                Vector2 bSlotPos = inventoryPosition + new Vector2(65f, 50f) * scaleOffset;
                spriteBatch.Draw(itemSheet, bSlotPos, bSlotItem, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
            }


            Vector2 cursorOffset = new Vector2(125f + (selectionController.CursorGridPosition.X * 24f), 45f + (selectionController.CursorGridPosition.Y * 24f));
            spriteBatch.Draw(spriteSheet, inventoryPosition + cursorOffset * GameConstants.SCALE, CursorSelectRectangle, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
            if (inventory.HasBoomerang)
            {
                Vector2 drawPos = inventoryPosition + new Vector2(128f, 45f) * scaleOffset;
                spriteBatch.Draw(itemSheet, drawPos, BoomerangSourceRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
            }

            if (inventory.HasBomb)
            {
                Vector2 drawPos = inventoryPosition + new Vector2(152f, 45f) * scaleOffset;
                spriteBatch.Draw(itemSheet, drawPos, BombSourceRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
            }

            if (inventory.HasBow)
            {
                Vector2 drawPos = inventoryPosition + new Vector2(176f, 45f) * scaleOffset;
                spriteBatch.Draw(itemSheet, drawPos, BowSourceRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
            }

            Rectangle roomRect = new Rectangle(645, 117, 8, 8);
            foreach (Point room in inventory.VisitedRooms)
            {
                Vector2 roomMapDrawPos = HUDPosition + new Vector2(120f + (room.X * 8f), 32f + (room.Y * 8f)) * scaleOffset;
                spriteBatch.Draw(spriteSheet, roomMapDrawPos, roomRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
            }
        }
    }
}
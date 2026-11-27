using System;
using Byte.Core;
using Byte.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.InventoryAndHud
{
    internal sealed class InventoryAndHUDGraphics
    {
        private readonly Texture2D spriteSheet;
        private readonly Texture2D itemSheet;
        private readonly Texture2D blackPixel;
        private readonly Texture2D mapPixel;
        private readonly SpriteFont font;
        private readonly InventoryData inventory;
        private readonly SelectionController selectionController;
        private readonly LowerMap lowerMap;
        private readonly UpperMap upperMap;



        private static readonly Vector2 ScaleVector = new Vector2(GameConstants.SCALE, GameConstants.SCALE);

        // Rectangle coordinates
        private static readonly Rectangle HudBgRect = new Rectangle(258, 11, 256, 55);
        private static readonly Rectangle InvBgRect = new Rectangle(1, 11, 255, 87);
        private static readonly Rectangle MapBgRect = new Rectangle(258, 112, 255, 87);

        private static readonly Rectangle BoomerangSourceRect = new Rectangle(129, 3, 5, 8);
        private static readonly Rectangle BombSourceRect = new Rectangle(136, 0, 8, 16);
        private static readonly Rectangle BowSourceRect = new Rectangle(144, 0, 8, 16);
        private static readonly Rectangle SwordSourceRect = new Rectangle(104, 0, 8, 16);
        private static readonly Rectangle CursorSelectRectangle = new Rectangle(519, 137, 16, 16);

        private static readonly Rectangle FullHeartRect = new Rectangle(645, 117, 8, 8);
        private static readonly Rectangle HalfHeartRect = new Rectangle(636, 117, 8, 8);
        private static readonly Rectangle EmptyHeartRect = new Rectangle(627, 117, 8, 8);


        private const int LowerMapPos = 4;
        private const int LowerMapWidth = 87;
        private const int LowerMapHeight = 47;
        private const int HeartsPerRow = 8;
        private const int HeartSize = 8;
        private const int HalfHeartsPerContainer = 2;

        //Offsets in relation to the inventory or HUD. 
        private static readonly Vector2 RupeesOffset = new Vector2(95f, 16f);
        private static readonly Vector2 KeysOffset = new Vector2(95f, 32f);
        private static readonly Vector2 BombsOffset = new Vector2(95f, 40f);
        private static readonly Vector2 BItemHudOffset = new Vector2(126f, 24f);
        private static readonly Vector2 AItemHudOffset = new Vector2(153f, 24f);
        private static readonly Vector2 HeartsStartOffset = new Vector2(176f, 24f);

        private static readonly Vector2 BItemInvOffset = new Vector2(65f, 50f);
        private static readonly Vector2 ItemGridOrigin = new Vector2(128f, 45f);
        private static readonly Vector2 CursorOrigin = new Vector2(125f, 45f);
        private static readonly Vector2 ItemGridSpacing = new Vector2(24f, 24f);
        private static readonly Vector2 UpperMapScaledOffset = new Vector2(864f, 516f);

        public const float MaxSlideOffset = 174f * GameConstants.SCALE;
        public const float HudHeight = 55f * GameConstants.SCALE;

        public InventoryAndHUDGraphics(
            Texture2D itemSheet,
            Texture2D spriteSheet,
            SpriteFont font,
            InventoryData inventory,
            SelectionController selectionController,
            LevelRepresentation level,
            UpperMap upperMap)
        {
            this.itemSheet = itemSheet;
            this.spriteSheet = spriteSheet;
            this.font = font;
            this.inventory = inventory;
            this.selectionController = selectionController;
            this.upperMap = upperMap;

            GraphicsDevice device = this.spriteSheet.GraphicsDevice;
            blackPixel = CreateSolidPixel(device, Color.Black);
            mapPixel = CreateSolidPixel(device, Color.White);

            Rectangle lowerMapBounds = new Rectangle(
                (int)(LowerMapPos * GameConstants.SCALE),
                (int)(LowerMapPos * GameConstants.SCALE),
                (int)(LowerMapWidth * GameConstants.SCALE),
                (int)(LowerMapHeight * GameConstants.SCALE));

            lowerMap = new LowerMap(level, mapPixel, lowerMapBounds);
        }

        public void Draw(SpriteBatch spriteBatch, float slideOffset, string currentRoomName)
        {
            Vector2 inventoryPosition = new Vector2(0f, -MaxSlideOffset + slideOffset);
            Vector2 hudPosition = new Vector2(0f, slideOffset);

            if (slideOffset > 0f)
            {
                Rectangle bgRect = new Rectangle(
                    0,
                    (int)inventoryPosition.Y,
                    GameConstants.WINDOW_SIZE.Width,
                    (int)MaxSlideOffset);
                spriteBatch.Draw(blackPixel, bgRect, Color.Black);
            }

            DrawInventoryScreen(spriteBatch, inventoryPosition);
            DrawHUD(spriteBatch, hudPosition, currentRoomName);
        }

        private void DrawHUD(SpriteBatch spriteBatch, Vector2 hudPosition, string currentRoomName)
        {
            DrawScaled(spriteBatch, spriteSheet, hudPosition, HudBgRect);
            lowerMap.Draw(spriteBatch, currentRoomName, hudPosition, inventory.HasMap);

            DrawCounters(spriteBatch, hudPosition);
            DrawEquippedSlots(spriteBatch, hudPosition);
            DrawHearts(spriteBatch, hudPosition);
        }

        private void DrawCounters(SpriteBatch spriteBatch, Vector2 hudPosition)
        {
            spriteBatch.DrawString(font, $"X{inventory.Rupees}", hudPosition + RupeesOffset * ScaleVector, Color.White);
            spriteBatch.DrawString(font, $"X{inventory.Keys}", hudPosition + KeysOffset * ScaleVector, Color.White);
            spriteBatch.DrawString(font, $"X{inventory.Bombs}", hudPosition + BombsOffset * ScaleVector, Color.White);
        }

        private void DrawEquippedSlots(SpriteBatch spriteBatch, Vector2 hudPosition)
        {
            DrawItemIfEquipped(spriteBatch, selectionController.EquippedItem, hudPosition + BItemHudOffset * ScaleVector);
            DrawItemIfEquipped(spriteBatch, selectionController.EquippedAItem, hudPosition + AItemHudOffset * ScaleVector);
        }

        private void DrawHearts(SpriteBatch spriteBatch, Vector2 hudPosition)
        {
            Vector2 heartStartPos = hudPosition + HeartsStartOffset * ScaleVector;
            int maxContainers = inventory.MaxHalfHearts / HalfHeartsPerContainer;

            for (int i = 0; i < maxContainers; i++)
            {
                int remainingHalfHearts = inventory.CurrentHalfHearts - (i * HalfHeartsPerContainer);
                Rectangle heartRect = remainingHalfHearts >= HalfHeartsPerContainer
                    ? FullHeartRect
                    : remainingHalfHearts == 1
                        ? HalfHeartRect
                        : EmptyHeartRect;

                int row = i / HeartsPerRow;
                int col = i % HeartsPerRow;
                Vector2 offset = new Vector2(col * HeartSize, row * HeartSize) * ScaleVector;
                DrawScaled(spriteBatch, spriteSheet, heartStartPos + offset, heartRect);
            }
        }

        private void DrawInventoryScreen(SpriteBatch spriteBatch, Vector2 inventoryPosition)
        {
            Vector2 mapPanelPosition = inventoryPosition + new Vector2(0f, InvBgRect.Height * GameConstants.SCALE);

            DrawScaled(spriteBatch, spriteSheet, inventoryPosition, InvBgRect);
            DrawScaled(spriteBatch, spriteSheet, mapPanelPosition, MapBgRect);

            upperMap.Draw(spriteBatch, inventoryPosition + UpperMapScaledOffset);

            DrawItemIfEquipped(spriteBatch, selectionController.EquippedItem, inventoryPosition + BItemInvOffset * ScaleVector);
            DrawCursor(spriteBatch, inventoryPosition);
            DrawSelectableItems(spriteBatch, inventoryPosition);
        }

        private void DrawCursor(SpriteBatch spriteBatch, Vector2 inventoryPosition)
        {
            Point gridPos = selectionController.CursorGridPosition;
            Vector2 cursorOffset = CursorOrigin + new Vector2(
                gridPos.X * ItemGridSpacing.X,
                gridPos.Y * ItemGridSpacing.Y);

            DrawScaled(spriteBatch, spriteSheet, inventoryPosition + cursorOffset * ScaleVector, CursorSelectRectangle);
        }

        private void DrawSelectableItems(SpriteBatch spriteBatch, Vector2 inventoryPosition)
        {
            if (inventory.HasBoomerang)
                DrawGridItem(spriteBatch, inventoryPosition, col: 0, BoomerangSourceRect);

            if (inventory.HasBomb)
                DrawGridItem(spriteBatch, inventoryPosition, col: 1, BombSourceRect);

            if (inventory.HasBow)
                DrawGridItem(spriteBatch, inventoryPosition, col: 2, BowSourceRect);
        }

        private void DrawGridItem(SpriteBatch spriteBatch, Vector2 inventoryPosition, int col, Rectangle sourceRect)
        {
            Vector2 gridPos = ItemGridOrigin + new Vector2(col * ItemGridSpacing.X, 0f);
            DrawScaled(spriteBatch, itemSheet, inventoryPosition + gridPos * ScaleVector, sourceRect);
        }

        private void DrawItemIfEquipped(SpriteBatch spriteBatch, ItemType itemType, Vector2 position)
        {
            Rectangle sourceRect = GetItemSourceRect(itemType);
            if (sourceRect != Rectangle.Empty)
            {
                DrawScaled(spriteBatch, itemSheet, position, sourceRect);
            }
        }

        private static void DrawScaled(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle sourceRect)
        {
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, Vector2.Zero, GameConstants.SCALE, SpriteEffects.None, 0f);
        }

        private static Texture2D CreateSolidPixel(GraphicsDevice device, Color color)
        {
            Texture2D pixel = new Texture2D(device, 1, 1);
            pixel.SetData(new[] { color });
            return pixel;
        }

        private static Rectangle GetItemSourceRect(ItemType itemType)
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
    }
}
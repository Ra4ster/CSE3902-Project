using Byte.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.InventoryAndHud;

internal class InventoryAndHUDGraphics
{
    private readonly Texture2D spriteSheet;
    private readonly SpriteFont font;
    private readonly InventoryData inventory;
    private readonly SelectionController selectionController;

    private float screenPosition = 0f;

    private readonly float maxOffset = GameConstants.WINDOW_SIZE.Y * 0.66f;

    public InventoryAndHUDGraphics(Texture2D spriteSheet, SpriteFont font, InventoryData inventory, SelectionController selectionController)
    {
        this.spriteSheet = spriteSheet;
        this.font = font;
        this.inventory = inventory;
        this.selectionController = selectionController;
    }

    public void UpdateAnimation(GameTime gameTime, GameState currentState)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (currentState == GameState.TransitionToInventory)
        {
            screenPosition += 1000f * delta;
            if (screenPosition >= maxOffset) screenPosition = screenPosition;
        }
        else if (currentState == GameState.TransitionToPlaying)
        {
            screenPosition -= 1000f * delta;
            if (screenPosition <= 0f) screenPosition = 0f;
        }
        else if (currentState == GameState.Inventory)
        {
            screenPosition = maxOffset;
        }
        else if (currentState == GameState.Playing)
        {
            screenPosition = 0f;
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameState currentState)
    {

        if (screenPosition > 0f)
        {
            Vector2 inventoryPosition = new Vector2(0, -maxOffset + screenPosition);
            DrawInventoryScreen(spriteBatch, inventoryPosition);
        }

        Vector2 hudPosition = new Vector2(0, screenPosition);
        DrawHUD(spriteBatch, hudPosition);
    }

    private void DrawHUD(SpriteBatch spriteBatch, Vector2 hudPosition)
    {
        throw new NotImplementedException();
    }

    private void DrawInventoryScreen(SpriteBatch spriteBatch, Vector2 inventoryPosition)
    {
        throw new NotImplementedException();
    }
}


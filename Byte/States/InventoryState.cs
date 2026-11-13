using Byte.Command;
using Byte.Controller;
using Byte.InventoryAndHud;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Byte.States;

public sealed class InventoryState : IGameState
{
    private enum Phase { Opening, Open, Closing }

    private const float SlideSpeed = InventoryAndHUDGraphics.MaxSlideOffset / GameStateConstants.InventorySlideDuration;

    private readonly Program game;
    private IController controls = null!;
    private Phase phase;
    private float slideOffset;

    public InventoryState(Program game) => this.game = game;

    public void Enter()
    {
        phase = Phase.Opening;
        slideOffset = 0f;

        SelectionController selection = game.Selection;
        ICommand cursorUp = new MoveCursorCommand(selection, 0, -1);
        ICommand cursorDown = new MoveCursorCommand(selection, 0, 1);
        ICommand cursorLeft = new MoveCursorCommand(selection, -1, 0);
        ICommand cursorRight = new MoveCursorCommand(selection, 1, 0);
        controls = StateInput.PressOnly(new Dictionary<Keys, ICommand>
        {
            [Keys.I] = new CloseInventoryCommand(this),
            [Keys.Up] = cursorUp,
            [Keys.W] = cursorUp,
            [Keys.NumPad8] = cursorUp,
            [Keys.Down] = cursorDown,
            [Keys.S] = cursorDown,
            [Keys.NumPad2] = cursorDown,
            [Keys.Left] = cursorLeft,
            [Keys.A] = cursorLeft,
            [Keys.NumPad4] = cursorLeft,
            [Keys.Right] = cursorRight,
            [Keys.D] = cursorRight,
            [Keys.NumPad6] = cursorRight,
            [Keys.Enter] = new SelectCursorCommand(selection),
            [Keys.Q] = new QuitCommand(game),
        });
    }

    public void Close()
    {
        if (phase == Phase.Open || phase == Phase.Opening)
            phase = Phase.Closing;
    }

    public void Update(GameTime gameTime)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        float step = SlideSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        switch (phase)
        {
            case Phase.Opening:
                slideOffset = Math.Min(slideOffset + step, InventoryAndHUDGraphics.MaxSlideOffset);
                if (slideOffset >= InventoryAndHUDGraphics.MaxSlideOffset)
                    phase = Phase.Open;
                break;

            case Phase.Open:
                controls.Update(gameTime);
                break;

            case Phase.Closing:
                slideOffset -= step;

                if (slideOffset <= 0f)
                {
                    slideOffset = 0f;
                    game.StateMachine.ChangeState(new GameplayState(game));
                }
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        game.DrawScene(new Vector2(0f, slideOffset));
        game.DrawHud(slideOffset);
    }
}

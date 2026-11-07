using Byte.Command;
using Byte.Controller;
using Byte.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Byte.States;

public sealed class GameOverState : IGameState, IMenu
{
    private enum Phase { Dying, Fading, Menu }

    private static readonly Vector2[] SpinOrder =
    [
        CardinalDirections.South, CardinalDirections.West, CardinalDirections.North, CardinalDirections.East
    ];

    private static readonly string[] Options = [GameStateConstants.RetryText, GameStateConstants.QuitText];
    private const int RetryIndex = 0;

    private readonly Program game;
    private IController controls = null!;
    private Phase phase;
    private float elapsed;
    private int selectedIndex;

    public GameOverState(Program game) => this.game = game;

    public void Enter()
    {
        phase = Phase.Dying;
        elapsed = 0f;
        selectedIndex = RetryIndex;

        Link link = game.link;
        link.Velocity = Vector2.Zero;
        link.IsAttacking = false;
        link.GetNewState(new PlayerIdleState());

        controls = StateInput.PressOnly(new Dictionary<Keys, ICommand>
        {
            [Keys.Up] = new MenuMoveCommand(this, -1),
            [Keys.W] = new MenuMoveCommand(this, -1),
            [Keys.Down] = new MenuMoveCommand(this, 1),
            [Keys.S] = new MenuMoveCommand(this, 1),
            [Keys.Enter] = new MenuConfirmCommand(this),
            [Keys.Q] = new QuitCommand(game),
        });
    }

    public void MoveSelection(int delta) =>
        selectedIndex = (selectedIndex + delta + Options.Length) % Options.Length;

    public void ConfirmSelection()
    {
        if (selectedIndex == RetryIndex)
            game.RequestReset();
        else
            game.Exit();
    }

    public void Update(GameTime gameTime)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        switch (phase)
        {
            case Phase.Dying:
                SpinLink(gameTime);
                if (elapsed >= GameStateConstants.DeathSpinDuration)
                    NextPhase(Phase.Fading);
                break;
            case Phase.Fading:
                if (elapsed >= GameStateConstants.DeathFadeDuration)
                    NextPhase(Phase.Menu);
                break;
            case Phase.Menu:
                controls.Update(gameTime);
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        switch (phase)
        {
            case Phase.Dying:
                DrawRedRoom(spriteBatch, Math.Min(elapsed / GameStateConstants.DeathSpinDuration, GameStateConstants.FullProgress));
                break;
            case Phase.Fading:
                DrawRedRoom(spriteBatch, GameStateConstants.FullProgress);
                game.BeginScreen();
                game.Overlay.FillScreen(spriteBatch, new Color(Color.Black, elapsed / GameStateConstants.DeathFadeDuration));
                game.EndBatch();
                break;
            case Phase.Menu:
                DrawMenu(spriteBatch);
                break;
        }
        game.DrawHud(0f);
    }

    private void NextPhase(Phase next)
    {
        phase = next;
        elapsed = 0f;
    }

    private void SpinLink(GameTime gameTime)
    {
        int frame = (int)(elapsed / GameStateConstants.DeathSpinFrameTime) % SpinOrder.Length;
        game.link.Direction = SpinOrder[frame];
        _ = game.link.state.Update(game.link, gameTime);
    }

    private void DrawRedRoom(SpriteBatch spriteBatch, float tintProgress)
    {
        game.DrawScene(Vector2.Zero);
        game.BeginScreen();
        game.Overlay.FillScreen(spriteBatch,
            new Color(GameStateConstants.DeathTintColor, GameStateConstants.DeathTintMaxAlpha * tintProgress));
        game.EndBatch();
        game.DrawLink(Vector2.Zero);
    }

    private void DrawMenu(SpriteBatch spriteBatch)
    {
        game.BeginScreen();
        game.Overlay.FillScreen(spriteBatch, Color.Black);
        _ = game.Overlay.DrawCenteredText(spriteBatch, GameStateConstants.GameOverText, GameStateConstants.CenterY,
            GameStateConstants.GameOverTextColor, GameStateConstants.TitleTextScale);

        for (int i = 0; i < Options.Length; i++)
        {
            float y = GameStateConstants.CenterY + GameStateConstants.LineSpacing * (i + 1);
            float left = game.Overlay.DrawCenteredText(spriteBatch, Options[i], y,
                GameStateConstants.MenuTextColor, GameStateConstants.TextScale);
            if (i == selectedIndex)
                game.Overlay.DrawTextAt(spriteBatch, GameStateConstants.MenuCursorText,
                    left - GameStateConstants.MenuCursorOffsetX, y, GameStateConstants.MenuTextColor, GameStateConstants.TextScale);
        }
        game.EndBatch();
    }
}

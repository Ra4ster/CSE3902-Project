using Byte.Command;
using Byte.Controller;
using Byte.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Byte.States;

public sealed class WinState : IGameState
{
    private enum Phase { Flashing, Curtains, Complete }

    private const int FlashFramesPerCycle = 2;
    private const float CurtainHalf = 0.5f;
    private const int HintLine = 2;

    private readonly Program game;
    private IController controls = null!;
    private Phase phase;
    private float elapsed;

    public WinState(Program game) => this.game = game;

    public void Enter()
    {
        phase = Phase.Flashing;
        elapsed = 0f;

        Link link = game.link;
        link.Velocity = Vector2.Zero;
        link.IsAttacking = false;
        link.Direction = CardinalDirections.South;
        link.GetNewState(new PlayerIdleState());

        controls = StateInput.PressOnly(new Dictionary<Keys, ICommand>
        {
            [Keys.Enter] = new ResetCommand(game),
            [Keys.Q] = new QuitCommand(game),
        });
    }

    public void Update(GameTime gameTime)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        switch (phase)
        {
            case Phase.Flashing:
                _ = game.link.state.Update(game.link, gameTime);
                if (elapsed >= GameStateConstants.WinFlashDuration)
                    NextPhase(Phase.Curtains);
                break;
            case Phase.Curtains:
                if (elapsed >= GameStateConstants.WinCurtainDuration)
                    NextPhase(Phase.Complete);
                break;
            case Phase.Complete:
                controls.Update(gameTime);
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        switch (phase)
        {
            case Phase.Flashing:
                game.DrawScene(Vector2.Zero);
                if (IsFlashFrame())
                    FillRoom(spriteBatch, new Color(GameStateConstants.WinFlashColor, GameStateConstants.WinFlashAlpha));
                break;
            case Phase.Curtains:
                game.DrawScene(Vector2.Zero);
                DrawCurtains(spriteBatch, Math.Min(elapsed / GameStateConstants.WinCurtainDuration, GameStateConstants.FullProgress));
                break;
            case Phase.Complete:
                DrawCompleteScreen(spriteBatch);
                break;
        }
        game.DrawHud(0f);
    }

    private void NextPhase(Phase next)
    {
        phase = next;
        elapsed = 0f;
    }

    private bool IsFlashFrame() => (int)(elapsed / GameStateConstants.WinFlashInterval) % FlashFramesPerCycle == 0;

    private void FillRoom(SpriteBatch spriteBatch, Color color)
    {
        game.BeginScreen();
        game.Overlay.Fill(spriteBatch, game.RoomViewport(game.Rooms.CurrentRoom), color);
        game.EndBatch();
    }

    private void DrawCurtains(SpriteBatch spriteBatch, float progress)
    {
        Rectangle room = game.RoomViewport(game.Rooms.CurrentRoom);
        int panelWidth = (int)Math.Ceiling(room.Width * CurtainHalf * progress);

        game.BeginScreen();
        game.Overlay.Fill(spriteBatch, new Rectangle(room.Left, room.Top, panelWidth, room.Height), Color.Black);
        game.Overlay.Fill(spriteBatch, new Rectangle(room.Right - panelWidth, room.Top, panelWidth, room.Height), Color.Black);
        game.EndBatch();
    }

    private void DrawCompleteScreen(SpriteBatch spriteBatch)
    {
        game.BeginScreen();
        game.Overlay.FillScreen(spriteBatch, Color.Black);
        _ = game.Overlay.DrawCenteredText(spriteBatch, GameStateConstants.WinTitleText, GameStateConstants.CenterY,
            GameStateConstants.WinTitleColor, GameStateConstants.TitleTextScale);
        _ = game.Overlay.DrawCenteredText(spriteBatch, GameStateConstants.WinSubtitleText,
            GameStateConstants.CenterY + GameStateConstants.LineSpacing, Color.White, GameStateConstants.TextScale);
        _ = game.Overlay.DrawCenteredText(spriteBatch, GameStateConstants.WinHintText,
            GameStateConstants.CenterY + GameStateConstants.LineSpacing * HintLine, Color.White, GameStateConstants.TextScale);
        game.EndBatch();
    }
}

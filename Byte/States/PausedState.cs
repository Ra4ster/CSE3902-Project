using Byte.Command;
using Byte.Controller;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Byte.States;

public sealed class PausedState : IGameState
{
    private readonly Program game;
    private IController controls = null!;

    public PausedState(Program game) => this.game = game;

    public void Enter()
    {
        controls = StateInput.PressOnly(new Dictionary<Keys, ICommand>
        {
            [Keys.P] = new ChangeStateCommand(game.StateMachine, () => new GameplayState(game)),
            [Keys.Q] = new QuitCommand(game),
        });
    }

    public void Update(GameTime gameTime) => controls.Update(gameTime);

    public void Draw(SpriteBatch spriteBatch)
    {
        game.DrawScene(Vector2.Zero);
        game.DrawHud(0f);

        game.BeginScreen();
        game.Overlay.FillScreen(spriteBatch, GameStateConstants.PauseDimColor);
        _ = game.Overlay.DrawCenteredText(spriteBatch, GameStateConstants.PausedText,
            GameStateConstants.CenterY, Color.White, GameStateConstants.TitleTextScale);
        _ = game.Overlay.DrawCenteredText(spriteBatch, GameStateConstants.PausedHintText,
            GameStateConstants.CenterY + GameStateConstants.LineSpacing, Color.White, GameStateConstants.TextScale);
        game.EndBatch();
    }
}

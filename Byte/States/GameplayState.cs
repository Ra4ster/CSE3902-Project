using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.States;

public sealed class GameplayState : IGameState
{
    private readonly Program game;

    public GameplayState(Program game) => this.game = game;

    public void Enter()
    {
    }

    public void Update(GameTime gameTime)
    {
        game.UpdateGameplayInput(gameTime);
        if (game.StateMachine.HasPendingChange)
            return;

        game.UpdateWorld(gameTime);

        if (game.link.Health <= 0)
        {
            GameEvents.RaiseLinkDied();
            game.StateMachine.ChangeState(new GameOverState(game));
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        game.DrawScene(Vector2.Zero);
        game.DrawHud(0f);
    }
}

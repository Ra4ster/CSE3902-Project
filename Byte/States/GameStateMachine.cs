using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.States;

public sealed class GameStateMachine
{
    private IGameState? pendingState;

    public IGameState? Current { get; private set; }

    public bool HasPendingChange => pendingState != null;

    public void Start(IGameState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        pendingState = null;
        Activate(state);
    }

    public void ChangeState(IGameState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        pendingState = state;
    }

    public void Update(GameTime gameTime)
    {
        Current?.Update(gameTime);
        if (pendingState is IGameState next)
        {
            pendingState = null;
            Activate(next);
        }
    }

    public void Draw(SpriteBatch spriteBatch) => Current?.Draw(spriteBatch);

    private void Activate(IGameState state)
    {
        Current = state;
        state.Enter();
    }
}

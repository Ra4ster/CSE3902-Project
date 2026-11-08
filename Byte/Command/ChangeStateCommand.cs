using Byte.States;
using Microsoft.Xna.Framework;

namespace Byte.Command
{
    public class ChangeStateCommand : ICommand
    {
        private readonly GameStateMachine stateMachine;
        private readonly Func<IGameState> createState;

        public ChangeStateCommand(GameStateMachine stateMachine, Func<IGameState> createState)
        {
            this.stateMachine = stateMachine;
            this.createState = createState;
        }

        public void Execute(GameTime gameTime) => stateMachine.ChangeState(createState());
    }
}

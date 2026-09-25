
using Microsoft.Xna.Framework;

public class QuitCommand : ICommand
{
    private Game game;

    public QuitCommand(Game game) => this.game = game;

    public void Execute(GameTime gameTime) => game.Exit();
}
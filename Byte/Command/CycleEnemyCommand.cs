using Byte.Sprite.Enemy;
using Microsoft.Xna.Framework;

/// <summary>
/// CycleEnemyCommand is a <b>DEBUG-only</b> command which forces enemyManager to display one enemy at a time.
/// </summary>
class CycleEnemyCommand : ICommand
{
    private readonly EnemyManager enemyManager;

    public CycleEnemyCommand(EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
    }

    public void Execute(GameTime gameTime)
    {
        enemyManager.NextEnemy();
    }
}
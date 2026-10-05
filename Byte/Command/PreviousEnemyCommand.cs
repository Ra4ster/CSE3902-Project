using Byte.Sprite.Enemy;
using Microsoft.Xna.Framework;

namespace Byte.Command
{
    class PreviousEnemyCommand : ICommand
    {
        private readonly EnemyManager enemyManager;

        public PreviousEnemyCommand(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
        }

        public void Execute(GameTime gameTime)
        {
            enemyManager.PreviousEnemy();
        }
    }
}
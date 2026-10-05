using Byte.Sprite.Enemy;
using Microsoft.Xna.Framework;

namespace Byte.Command
{
    class NextEnemyCommand : ICommand
    {
        private readonly EnemyManager enemyManager;

        public NextEnemyCommand(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
        }

        public void Execute(GameTime gameTime)
        {
            enemyManager.NextEnemy();
        }
    }
}

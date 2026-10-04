
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    public class EnemyManager
    {
        private readonly List<AbstractEnemy> enemies = new List<AbstractEnemy>();

        // Debug-only int; used for cycling through enemies.
        private int? selectedEnemyIndex = 0;

        public void Add(List<AbstractEnemy> enemies)
        {
            this.enemies.AddRange(enemies);
        }

        public void NextEnemy()
        {
            if (enemies.Count == 0)
                return;

            selectedEnemyIndex = selectedEnemyIndex switch
            {
                null => 0,
                int index when index == enemies.Count - 1 => null,
                int index => index + 1
            };
        }

        public void PreviousEnemy()
        {
            if (enemies.Count == 0)
                return;

            selectedEnemyIndex = selectedEnemyIndex switch
            {
                null => enemies.Count - 1,
                0 => null,
                int index => index - 1
            };
        }

        public void Draw(SpriteBatch spritePainter)
        {
            if (selectedEnemyIndex is int index)
                enemies[index].Draw(spritePainter);
            else
                foreach (AbstractEnemy enemy in enemies)
                    enemy.Draw(spritePainter);
        }

        public void Update(GameTime gameTime)
        {
            if (selectedEnemyIndex is int index)
                enemies[index].Update(gameTime);
            else
                foreach (AbstractEnemy enemy in enemies)
                    enemy.Update(gameTime);
        }
    }
}
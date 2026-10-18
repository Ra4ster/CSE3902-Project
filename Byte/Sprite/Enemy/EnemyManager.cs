
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    // holds all the enemies and shows whichever one is selected, o/p cycle through them
    public class EnemyManager
    {
        private readonly List<AbstractEnemy> enemies = new List<AbstractEnemy>();
        private int selectedEnemyIndex = 0;

        public void Add(List<AbstractEnemy> enemies)
        {
            this.enemies.AddRange(enemies);
        }

        public void NextEnemy()
        {
            if (enemies.Count > 0)
                selectedEnemyIndex = (selectedEnemyIndex + 1) % enemies.Count;
        }

        public void PreviousEnemy()
        {
            if (enemies.Count > 0)
                selectedEnemyIndex = (selectedEnemyIndex - 1 + enemies.Count) % enemies.Count;
        }

        public void Draw(SpriteBatch spritePainter)
        {
            AbstractEnemy? enemy = GetSelectedEnemy();
            if (enemy == null) return;

            enemy.Draw(spritePainter);
            if (Game.DrawBoundingBoxes)
                Game.BoundingBoxDrawer.Draw(spritePainter, enemy.BoundingBox, Color.Red);
        }

        public void Update(GameTime gameTime)
        {
            enemies.RemoveAll(enemy => enemy.IsDefeated);
            if (enemies.Count == 0) return;
            selectedEnemyIndex %= enemies.Count;
            enemies[selectedEnemyIndex].UpdateDamage(gameTime);
            enemies[selectedEnemyIndex].Update(gameTime);
        }

        public AbstractEnemy? GetSelectedEnemy()
        {
            if (enemies.Count == 0)
                return null;

            return enemies[selectedEnemyIndex].IsDefeated ? null : enemies[selectedEnemyIndex];
        }
    }
}

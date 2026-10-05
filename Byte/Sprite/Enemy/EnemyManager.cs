
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
            if (enemies.Count > 0)
                enemies[selectedEnemyIndex].Draw(spritePainter);
        }

        public void Update(GameTime gameTime)
        {
            if (enemies.Count > 0)
                enemies[selectedEnemyIndex].Update(gameTime);
        }
    }
}


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    public class EnemyManager
    {
        private List<AbstractEnemy> enemies = new List<AbstractEnemy>();

        public void Add(List<AbstractEnemy> enemies)
        {
            this.enemies = enemies;
        }

        public void Draw(SpriteBatch spritePainter)
        {
            foreach (AbstractEnemy enemy in enemies)
                enemy.Draw(spritePainter);
        }

        public void Update(GameTime gameTime)
        {
            foreach (AbstractEnemy enemy in enemies)
                enemy.Update(gameTime);
        }
    }
}
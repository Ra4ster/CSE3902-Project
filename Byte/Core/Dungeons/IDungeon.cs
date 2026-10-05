
using Byte.Block;
using Byte.Item;
using Byte.Projectile;
using Byte.Sprite.Enemy;

/// <summary>
/// Extensible dungeon interface; this will allow for multiple dungeons to be created and, more importantly, for entity serialization.
/// </summary>
public interface IDungeon
{
    public EnemyManager CreateEnemies(ProjectileManager projectileManager);
    public ItemManager CreateItems();
    public BlockManager CreateBlocks();

}
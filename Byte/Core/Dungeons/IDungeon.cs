
using Byte.Block;
using Byte.Item;
using Byte.Sprite.Enemy;

/// <summary>
/// Extensible dungeon interface; this will allow for multiple dungeons to be created and, more importantly, for entity serialization.
/// </summary>
public interface IDungeon
{
    public EnemyManager CreateEnemies();
    public ItemManager CreateItems();
    public BlockManager CreateBlocks();

}
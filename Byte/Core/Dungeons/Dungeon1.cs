
using Byte.Block;
using Byte.Item;
using Byte.Projectile;
using Byte.Sprite.Enemy;
using Microsoft.Xna.Framework;

class Dungeon1 : IDungeon
{

    public BlockManager CreateBlocks()
    {
        BlockManager blockManager = new BlockManager();
        BlockFactory blockFactory = new BlockFactory(GameAssets.Instance.BlockSheet);
        Vector2 blockPos = new Vector2(1200, 300);

        blockManager.AddBlock(blockFactory.CreateSquareBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateKeystoneBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateStatueLeftBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateStatueRightBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateVoidBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateSpeckledFloorBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateWaterBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateStairsBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateBrickBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateLadderBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateSandBlock(blockPos));
        return blockManager;
    }

    public EnemyManager CreateEnemies(ProjectileManager projectileManager)
    {

        List<AbstractEnemy> enemies = new List<AbstractEnemy> {
            EnemyFactory.CreateEnemy(EnemyType.Stalfos, at: new Vector2(100, 100)),
            EnemyFactory.CreateEnemy(EnemyType.Keese, at: new Vector2(500, 600)),
            EnemyFactory.CreateEnemy(EnemyType.Gel, at: new Vector2(300, 800)),
            EnemyFactory.CreateEnemy(EnemyType.Goriya, at: new Vector2(400, 400), projectileManager: projectileManager),
            EnemyFactory.CreateEnemy(EnemyType.Aquamentus, at: new Vector2(800, 900), projectileManager: projectileManager),
            EnemyFactory.CreateEnemy(EnemyType.Wallmaster, at: new Vector2(1200, 1000)),
            EnemyFactory.CreateEnemy(EnemyType.BladeTrap, at: new Vector2(GameConstants.WINDOW_SIZE.Right, GameConstants.WINDOW_SIZE.Bottom))
        };

        EnemyManager e_man = new EnemyManager();
        e_man.Add(enemies);

        return e_man;
    }

    public ItemManager CreateItems()
    {
        ItemFactory itemFactory = new ItemFactory();
        ItemManager itemManager = new ItemManager();
        Vector2 spawnPos = new Vector2(700, 700);

        itemManager.AddItem(itemFactory.CreateRupee(spawnPos));
        itemManager.AddItem(itemFactory.CreateHeart(spawnPos));
        itemManager.AddItem(itemFactory.CreateBoomerang(spawnPos));
        itemManager.AddItem(itemFactory.CreateBow(spawnPos));
        itemManager.AddItem(itemFactory.CreateBomb(spawnPos));

        return itemManager;
    }
}
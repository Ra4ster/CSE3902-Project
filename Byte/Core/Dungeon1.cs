
using Byte.Block;
using Byte.Item;
using Byte.Sprite;
using Byte.Sprite.Enemy;
using Byte.Sprite.Enemy.Minion;
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

    public EnemyManager CreateEnemies()
    {
        List<AbstractEnemy> enemies = [
            EnemyFactory.Instance.Create<Stalfos>(
            patrolPath: [
                new Vector2(100, 100),
                new Vector2(100, 300),
                new Vector2(300, 300),
                new Vector2(300, 100)
            ], speed: 200f),
            EnemyFactory.Instance.Create<Keese>(
                null,
                pos: new Vector2(500, 600), 200f),
            EnemyFactory.Instance.Create<Gel>(
            patrolPath: [
                new Vector2(300, 800),
                new Vector2(800, 800)
            ], speed: 200f, waitDuration: 1.5f),
            EnemyFactory.Instance.Create<Aquamentus>(
            patrolPath: [
                new Vector2(800, 900),
                new Vector2(1000, 900)
            ], speed: 200f, waitDuration: 1.0f, frameDuration: 0.5f)
        ];

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
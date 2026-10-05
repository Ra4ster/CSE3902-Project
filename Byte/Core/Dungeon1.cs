
using Byte.Block;
using Byte.Item;
using Byte.Projectile;
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

    public EnemyManager CreateEnemies(ProjectileManager projectileManager)
    {

        EnemyDescription stalfosDesc = new EnemyDescription
        {
            Type = EnemyType.Stalfos,
            PatrolPath =
            [
                new Vector2(100, 100),
                new Vector2(100, 300),
                new Vector2(300, 300),
                new Vector2(300, 100)
            ],
            Speed = 200f
        };
        EnemyDescription keeseDesc = new EnemyDescription
        {
            Type = EnemyType.Keese,
            Position = new Vector2(500, 600),
            Speed = 200f
        };
        EnemyDescription gelDesc = new EnemyDescription
        {
            Type = EnemyType.Gel,
            PatrolPath = [
                new Vector2(300, 800),
                new Vector2(800, 800)
            ],
            Speed = 200f,
            WaitDuration = 1.5f
        };
        EnemyDescription goriyaDesc = new EnemyDescription
        {
            Type = EnemyType.Goriya,
            PatrolPath = [new Vector2(400, 400), new Vector2(800, 400), new Vector2(800, 700)],
            Speed = 150f,
            WaitDuration = 0.5f
        };
        EnemyDescription aquamentusDesc = new EnemyDescription
        {
            Type = EnemyType.Aquamentus,
            PatrolPath = [
                new Vector2(800, 900),
                new Vector2(1000, 900)
            ],
            Speed = 200f,
            WaitDuration = 1.0f,
            FrameDuration = 0.5f
        };

        List<AbstractEnemy> enemies = new List<AbstractEnemy> {
            EnemyFactory.Instance.CreateEnemy(ref stalfosDesc, projectileManager),
            EnemyFactory.Instance.CreateEnemy(ref keeseDesc, projectileManager),
            EnemyFactory.Instance.CreateEnemy(ref gelDesc, projectileManager),
            EnemyFactory.Instance.CreateEnemy(ref goriyaDesc, projectileManager),
            EnemyFactory.Instance.CreateEnemy(ref aquamentusDesc, projectileManager) };

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
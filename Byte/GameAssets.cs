using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

internal sealed class GameAssets
{
    private static GameAssets? instance;

    public static GameAssets Instance => instance
        ?? throw new InvalidOperationException("GameAssets.Load must be called before accessing assets.");

    public Texture2D EnemySheet { get; }
    public Texture2D BossSheet { get; }
    public Texture2D LinkSheet { get; }
    public Texture2D BlockSheet { get; }
    public Texture2D ItemSheet { get; }

    private GameAssets(Texture2D enemySheet, Texture2D bossSheet, Texture2D linkSheet, Texture2D blockSheet, Texture2D itemSheet)
    {
        EnemySheet = enemySheet;
        BossSheet = bossSheet;
        LinkSheet = linkSheet;
        BlockSheet = blockSheet;
        ItemSheet = itemSheet;
    }

    public static void Load(ContentManager content)
    {
        instance = new GameAssets(
            content.Load<Texture2D>("AssetSheets/enemySheet"),
            content.Load<Texture2D>("AssetSheets/bossSheet"),
            content.Load<Texture2D>("AssetSheets/linkSheet2"),
            content.Load<Texture2D>("AssetSheets/blockSheet"),
            content.Load<Texture2D>("AssetSheets/items")
        );

    }
}
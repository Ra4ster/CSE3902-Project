using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Block;

// makes every block type, only place the sheet coordinates should live
public class BlockFactory
{
    // the single tiles are in a little palette on the sheet
    // 16x16 tiles spaced 17px apart starting at (984, 11)
    private const int TILE_SIZE = 16;
    private const int TILE_PITCH = 17;
    private const int PALETTE_X = 984;
    private const int PALETTE_Y = 11;
    private const float BLOCK_SCALE = 6.0f;

    private readonly Texture2D blockSheet;

    public BlockFactory(Texture2D blockSheet)
    {
        this.blockSheet = blockSheet;
    }

    private StaticSprite CreateSprite(int column, int row, Vector2 position)
    {
        Rectangle source = new Rectangle(
            PALETTE_X + TILE_PITCH * column,
            PALETTE_Y + TILE_PITCH * row,
            TILE_SIZE, TILE_SIZE);
        return new StaticSprite(blockSheet, position, Color.White, source, BLOCK_SCALE);
    }

    public IBlock CreateSquareBlock(Vector2 position) => new SquareBlock(CreateSprite(0, 0, position), position);

    public IBlock CreateKeystoneBlock(Vector2 position) => new KeystoneBlock(CreateSprite(1, 0, position), position);

    public IBlock CreateStatueLeftBlock(Vector2 position) => new StatueLeftBlock(CreateSprite(2, 0, position), position);

    public IBlock CreateStatueRightBlock(Vector2 position) => new StatueRightBlock(CreateSprite(3, 0, position), position);

    public IBlock CreateVoidBlock(Vector2 position) => new VoidBlock(CreateSprite(0, 1, position), position);

    public IBlock CreateSpeckledFloorBlock(Vector2 position) => new SpeckledFloorBlock(CreateSprite(1, 1, position), position);

    public IBlock CreateWaterBlock(Vector2 position) => new WaterBlock(CreateSprite(2, 1, position), position);

    public IBlock CreateStairsBlock(Vector2 position) => new StairsBlock(CreateSprite(3, 1, position), position);

    public IBlock CreateBrickBlock(Vector2 position) => new BrickBlock(CreateSprite(0, 2, position), position);

    public IBlock CreateLadderBlock(Vector2 position) => new LadderBlock(CreateSprite(1, 2, position), position);

    public IBlock CreateSandBlock(Vector2 position) => new SandBlock(CreateSprite(2, 2, position), position);
}

using Byte.Block;
using Byte.Rooms;
using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Factory;

// makes every block type, only place the sheet coordinates should live
public class BlockFactory
{
    private const int TILE_PITCH = 17;
    private const int PALETTE_X = 984;
    private const int PALETTE_Y = 11;

    private readonly Texture2D blockSheet;

    public BlockFactory(Texture2D blockSheet)
    {
        this.blockSheet = blockSheet;
    }

    public IBlock Create(string type, Vector2 position) => type switch
    {
        "Square" => CreateSquareBlock(position),
        "Keystone" => CreateKeystoneBlock(position),
        "StatueLeft" => CreateStatueLeftBlock(position),
        "StatueRight" => CreateStatueRightBlock(position),
        "Void" => CreateVoidBlock(position),
        "SpeckledFloor" => CreateSpeckledFloorBlock(position),
        "Water" => CreateWaterBlock(position),
        "Stairs" => CreateStairsBlock(position),
        "Brick" => CreateBrickBlock(position),
        "Ladder" => CreateLadderBlock(position),
        "Sand" => CreateSandBlock(position),
        _ => throw new InvalidDataException($"Invalid block data of type: {type}")
    };

    private StaticSprite CreateSprite(int column, int row, Vector2 position)
    {
        Rectangle source = new Rectangle(
            PALETTE_X + TILE_PITCH * column,
            PALETTE_Y + TILE_PITCH * row,
            RoomGrid.TILE_SIZE, RoomGrid.TILE_SIZE);
        return new StaticSprite(blockSheet, position, Color.White, source, RoomGrid.BLOCK_SCALE);
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

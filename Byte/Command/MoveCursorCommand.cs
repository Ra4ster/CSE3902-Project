using Microsoft.Xna.Framework;
using Byte.Core;

namespace Byte.InventoryAndHud;

public class MoveCursorCommand : ICommand
{
    private readonly SelectionController selectionController;
    private readonly int moveX;
    private readonly int moveY;

    public MoveCursorCommand(SelectionController selectionController, int moveX, int moveY)
    {
        this.selectionController = selectionController;
        this.moveX = moveX;
        this.moveY = moveY;
    }

    public void Execute(GameTime gameTime)
    {
        selectionController.MoveCursor(moveX, moveY);
    }
}
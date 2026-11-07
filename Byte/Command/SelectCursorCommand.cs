using Microsoft.Xna.Framework;
using Byte.Core;

namespace Byte.InventoryAndHud;

public class SelectCursorCommand : ICommand
{
    private readonly SelectionController selectionController;

    public SelectCursorCommand(SelectionController selectionController)
    {
        this.selectionController = selectionController;
    }

    public void Execute(GameTime gameTime)
    {
        selectionController.EquipSelectedAtCursor();
    }
}
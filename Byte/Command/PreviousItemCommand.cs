using Byte.Item;
using Byte.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

public class PreviousItemCommand : ICommand
{

    private readonly ItemManager itemManager;

    public PreviousItemCommand(ItemManager itemManager)
    {
        this.itemManager = itemManager;

    }
    public void Execute(GameTime gameTime)
    {
        itemManager.PreviousItem();
    }
}




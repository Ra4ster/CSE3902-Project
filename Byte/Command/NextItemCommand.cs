using Byte.Item;
using Byte.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

public class NextItemCommand : ICommand
{

    private readonly ItemManager itemManager;

    public NextItemCommand(ItemManager itemManager)
    {
        this.itemManager = itemManager;

    }
    public void Execute(GameTime gameTime)
    {
        itemManager.NextItem();
    }
}




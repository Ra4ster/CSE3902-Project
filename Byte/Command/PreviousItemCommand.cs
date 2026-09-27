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

    private double cooldownTimer = 0f;
    private readonly double cooldownTime = 0.2f;

    public PreviousItemCommand(ItemManager itemManager)
    {
        this.itemManager = itemManager;

    }
    public void Execute(GameTime gameTime)
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }

        itemManager.PreviousItem();


        cooldownTimer = cooldownTime;
    }
}




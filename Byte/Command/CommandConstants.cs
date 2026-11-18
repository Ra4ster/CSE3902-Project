using Byte.Controller;
using Byte.InventoryAndHud;
using Byte.Player;
using Byte.Projectile;
using Byte.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Byte.Command;

public static class CommandConstants
{
    internal static readonly Keys[] PressOnlyKeys =
    [
        Keys.E,
        Keys.Enter,
        Keys.R,
        Keys.H,
        Keys.OemOpenBrackets,
        Keys.OemCloseBrackets,
        Keys.I,
        Keys.P,
        Keys.T
    ];

    public static void Configure(
        InputControllers inputControllers,
        Program game,
        Link link,
        ProjectileManager projectileManager,
        InventoryManager inventoryManager,
        InventoryData inventoryData,
        SelectionController selectionController)
    {
        ArgumentNullException.ThrowIfNull(inputControllers);
        ArgumentNullException.ThrowIfNull(game);
        ArgumentNullException.ThrowIfNull(link);
        ArgumentNullException.ThrowIfNull(projectileManager);
        ArgumentNullException.ThrowIfNull(inventoryManager);
        ArgumentNullException.ThrowIfNull(inventoryData);
        ArgumentNullException.ThrowIfNull(selectionController);

        inputControllers.Clear();
        Dictionary<Keys, ICommand> keys = inputControllers.KeyBindings;

        ICommand openInventory = new ChangeStateCommand(
            game.StateMachine,
            () => new InventoryState(game));
        ICommand pause = new ChangeStateCommand(
            game.StateMachine,
            () => new PausedState(game));
        ICommand triggerWin = new TriggerWinCommand();
        ICommand attack = new AttackCommand(game, link);
        ICommand moveUp = new MoveUpCommand(game, link);
        ICommand moveDown = new MoveDownCommand(game, link);
        ICommand moveLeft = new MoveLeftCommand(game, link);
        ICommand moveRight = new MoveRightCommand(game, link);
        ICommand quit = new QuitCommand(game);
        ICommand reset = new ResetCommand(game);
        ICommand takeDamage = new PlayerTakeDamageCommand(link);
        ICommand nextRoom = new NextRoomCommand(game);
        ICommand previousRoom = new PreviousRoomCommand(game);
        ICommand toggleBoundingBoxes = new ToggleBoundingBoxesCommand();
        ICommand pauseMusic = new PauseMusicCommand();
        ICommand interactWithDoor = new InteractWithDoorCommand(game);
        ICommand useEquippedItem = CreateProjectileCommand(
            link,
            projectileManager,
            inventoryManager,
            inventoryData,
            selectionController);

        keys.Add(Keys.D0, quit);
        keys.Add(Keys.Q, quit);
        keys.Add(Keys.W, moveUp);
        keys.Add(Keys.S, moveDown);
        keys.Add(Keys.A, moveLeft);
        keys.Add(Keys.D, moveRight);
        keys.Add(Keys.Up, moveUp);
        keys.Add(Keys.Down, moveDown);
        keys.Add(Keys.Left, moveLeft);
        keys.Add(Keys.Right, moveRight);
        keys.Add(Keys.Z, attack);
        keys.Add(Keys.N, attack);
        keys.Add(Keys.R, reset);
        keys.Add(Keys.E, takeDamage);
        keys.Add(Keys.Enter, interactWithDoor);
        keys.Add(Keys.OemCloseBrackets, nextRoom);
        keys.Add(Keys.OemOpenBrackets, previousRoom);
        keys.Add(Keys.H, toggleBoundingBoxes);
        keys.Add(Keys.M, pauseMusic);
        keys.Add(Keys.I, openInventory);
        keys.Add(Keys.P, pause);
        keys.Add(Keys.T, triggerWin);
        keys.Add(Keys.B, useEquippedItem);

        inputControllers.LeftClickBindings.Add(GameConstants.WINDOW_SIZE, previousRoom);
        inputControllers.RightClickBindings.Add(GameConstants.WINDOW_SIZE, nextRoom);
    }

    private static ICommand CreateProjectileCommand(
        Link link,
        ProjectileManager projectileManager,
        InventoryManager inventoryManager,
        InventoryData inventoryData,
        SelectionController selectionController)
    {
        ProjectileFactory projectileFactory = new(
            GameAssets.Instance.LinkSheet,
            GameAssets.Instance.BossSheet);

        Func<IProjectile, bool> canUse = inventoryManager.CanUseItem;
        Func<Vector2, Vector2, IProjectile?> createProjectile = (position, direction) =>
        {
            if (selectionController.EquippedItem == ItemType.Bomb && !inventoryData.HasBomb)
            {
                selectionController.UnequipItem();
                return null;
            }
            if (selectionController.EquippedItem == ItemType.Bow && !inventoryData.HasBow)
            {
                selectionController.UnequipItem();
                return null;
            }

            if (selectionController.EquippedItem == ItemType.None)
                return null;

            return selectionController.EquippedItem switch
            {
                ItemType.Bow => projectileFactory.CreateArrow(position, direction),
                ItemType.Bomb => projectileFactory.CreateBomb(position),
                ItemType.Boomerang => projectileFactory.CreateBoomerang(position, direction),
                _ => null
            };
        };

        return new UseProjectileCommand(
            link,
            projectileManager,
            createProjectile!,
            canUse);
    }
}
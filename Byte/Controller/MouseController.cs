using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sprint0.Controller;

/// <summary>
/// Controls mouse input (excluding scroll/move).
/// </summary>
public class MouseController : IController
{
    private Dictionary<Rectangle, ICommand> leftClickMapping;
    private Dictionary<Rectangle, ICommand> rightClickMapping;

    private MouseState CurrentState { get; set; }
    private MouseState PreviousState { get; set; }

    public MouseController(Dictionary<Rectangle, ICommand> leftClickMapping, Dictionary<Rectangle, ICommand> rightClickMapping)
    {
        this.leftClickMapping = leftClickMapping;
        this.rightClickMapping = rightClickMapping;
        CurrentState = Mouse.GetState();
    }

    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = Mouse.GetState();

        // Checks only for NEW commands, not holding down a mouse state.
        foreach (KeyValuePair<Rectangle, ICommand> mouseCmd in leftClickMapping)
        {
            if (CurrentState.LeftButton == ButtonState.Pressed &&
            PreviousState.LeftButton != ButtonState.Pressed &&
            mouseCmd.Key.Contains(new Point(CurrentState.X, CurrentState.Y)))
            {
                mouseCmd.Value.Execute();
            }
        }

        foreach (KeyValuePair<Rectangle, ICommand> mouseCmd in rightClickMapping)
        {
            if (CurrentState.RightButton == ButtonState.Pressed &&
            PreviousState.RightButton != ButtonState.Pressed &&
            mouseCmd.Key.Contains(new Point(CurrentState.X, CurrentState.Y)))
            {
                mouseCmd.Value.Execute();
            }
        }
    }
}
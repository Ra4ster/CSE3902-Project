namespace Byte.States;

public interface IMenu
{
    void MoveSelection(int delta);

    void ConfirmSelection();
}

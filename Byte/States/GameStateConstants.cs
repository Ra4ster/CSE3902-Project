using Microsoft.Xna.Framework;

namespace Byte.States;

public static class GameStateConstants
{
    public const float TextScale = 1.75f;
    public const float TitleTextScale = 3.0f;
    public static readonly float CenterY = GameConstants.WINDOW_SIZE.Height * 0.5f;
    public const float LineSpacing = 110f;
    public const float FullProgress = 1f;
    public const float HalfProgress = 0.5f;

    public const float RoomScrollDuration = 0.9f;
    public const float RoomFadeDuration = 0.6f;

    public const float InventorySlideDuration = 0.75f;

    public static readonly Color PauseDimColor = new(0, 0, 0, 150);
    public const string PausedText = "PAUSED";
    public const string PausedHintText = "PRESS P TO RESUME";

    public const float DeathSpinDuration = 1.6f;
    public const float DeathSpinFrameTime = 0.08f;
    public const float DeathFadeDuration = 0.8f;
    public const float DeathTintMaxAlpha = 0.6f;
    public static readonly Color DeathTintColor = new(200, 0, 0);
    public const string GameOverText = "GAME OVER";
    public const string RetryText = "RETRY";
    public const string QuitText = "QUIT";
    public const string MenuCursorText = ">";
    public const float MenuCursorOffsetX = 50f;
    public static readonly Color GameOverTextColor = Color.Red;
    public static readonly Color MenuTextColor = Color.White;

    public const float WinFlashDuration = 2.4f;
    public const float WinFlashInterval = 0.12f;
    public const float WinFlashAlpha = 0.75f;
    public const float WinCurtainDuration = 1.5f;
    public static readonly Color WinFlashColor = Color.White;
    public const string WinTitleText = "YOU GOT THE TRIFORCE!";
    public const string WinSubtitleText = "LEVEL 1 COMPLETE";
    public const string WinHintText = "PRESS ENTER TO PLAY AGAIN";
    public static readonly Color WinTitleColor = Color.Gold;
}

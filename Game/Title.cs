using System.Numerics;
using Raylib_CsLo;

namespace BombFinder;



public class TitleScreen
{
    public const int TITLE_SCREEN_WIDTH = 400;
    public const int TITLE_SCREEN_HEIGHT = 450;
    private static TitleScreen _instance;
    public static TitleScreen Instance => _instance;

    static TitleScreen()
    {
        _instance = new TitleScreen();
    }


    private List<GameButton> _buttons = new List<GameButton>();
    private Music click;
    private TitleScreen()
    {
        var scd = TITLE_SCREEN_WIDTH / 2 - 100;
        _buttons.Add(new GameButton(new Rectangle(scd, 55, 200, 50), "Easy", GameButton.GameButtonActions.Easy));
        _buttons.Add(new GameButton(new Rectangle(scd, 135, 200, 50), "Medium", GameButton.GameButtonActions.Medium));
        _buttons.Add(new GameButton(new Rectangle(scd, 215, 200, 50), "Hard", GameButton.GameButtonActions.Hard));
        _buttons.Add(new GameButton(new Rectangle(scd, 295, 200, 50), "Credits", GameButton.GameButtonActions.Credits));
        _buttons.Add(new GameButton(new Rectangle(scd, 375, 200, 50), "Exit", GameButton.GameButtonActions.Exit));

        click = Raylib.LoadMusicStream("sounds/click.mp3");
    }
    public void SetupTitleScreen()
    {
        Raylib.SetWindowSize(TITLE_SCREEN_WIDTH, TITLE_SCREEN_HEIGHT);
    }
    public void DrawScreen()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib.BLACK);
        Raylib.DrawText($"Bomb Finder!", TITLE_SCREEN_WIDTH / 2 - Raylib.MeasureText($"Bomb Finder!", 32) / 2, 5, 32, Raylib.WHITE);

        _buttons.ForEach(x => x.Draw());

        Raylib.EndDrawing();
    }


    public GameButton.GameButtonActions TestMouseClick()
    {
        var pos = Raylib.GetMousePosition();

        if (Raylib.IsMouseButtonPressed(Raylib.MOUSE_LEFT_BUTTON))
        {
            foreach (var button in _buttons)
                if (button.TestCollision(pos))
                {
                    Raylib.PlayMusicStream(click);
                    return button.Action;
                }
        }

        return GameButton.GameButtonActions.Nothing;
    }




}
using System.Numerics;
using Raylib_CsLo;

namespace BombFinder;

public class TitleScreen
{
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
        var scd = Constants.TITLE_SCREEN_WIDTH / 2 - 100;
        _buttons.Add(new GameButton(new Rectangle(scd, 55, 200, 50), "Easy", GameButtonActions.Easy));
        _buttons.Add(new GameButton(new Rectangle(scd, 135, 200, 50), "Medium", GameButtonActions.Medium));
        _buttons.Add(new GameButton(new Rectangle(scd, 215, 200, 50), "Hard", GameButtonActions.Hard));
        _buttons.Add(new GameButton(new Rectangle(scd, 295, 200, 50), "Credits", GameButtonActions.Credits));
        _buttons.Add(new GameButton(new Rectangle(scd, 375, 200, 50), "Exit", GameButtonActions.Exit));

        click = Raylib.LoadMusicStream("sounds/click.mp3");
    }
    public void SetupTitleScreen()
    {
        Raylib.SetWindowSize(Constants.TITLE_SCREEN_WIDTH, Constants.TITLE_SCREEN_HEIGHT);
        SoundManager.Instance.PlayMusic(GameMusic.Title);
    }

    public void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib.BLACK);
        Raylib.DrawText(Constants.GAME_TITLE, Constants.TITLE_SCREEN_WIDTH / 2 - Raylib.MeasureText(Constants.GAME_TITLE, 32) / 2, 5, 32, Raylib.WHITE);

        var pos = Raylib.GetMousePosition();
        _buttons.ForEach(x =>
        {
            x.TestHover(pos);
            x.Draw();
        });

        Raylib.EndDrawing();
    }


    public GameButtonActions TestInput()
    {
        var pos = Raylib.GetMousePosition();

        if (Raylib.IsMouseButtonPressed(Raylib.MOUSE_LEFT_BUTTON))
        {
            foreach (var button in _buttons)
                if (button.TestCollision(pos))
                {
                    SoundManager.Instance.PlaySound(GameSounds.Click);
                    return button.Action;
                }
        }

        return GameButtonActions.Nothing;
    }




}
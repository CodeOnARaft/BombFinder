using Raylib_CsLo;

namespace BombFinder;


public class Credits
{
    private static Credits _instance;
    public static Credits Instance => _instance;
    static Credits()
    {
        _instance = new Credits();
    }

    public void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib.BLACK);
        Raylib.DrawText(Constants.GAME_TITLE, Constants.TITLE_SCREEN_WIDTH / 2 - Raylib.MeasureText(Constants.GAME_TITLE, 32) / 2, 5, 32, Raylib.WHITE);


        Raylib.EndDrawing();
    }

    public GameButtonActions TestInput()
    {

        if (Raylib.IsMouseButtonPressed(Raylib.MOUSE_LEFT_BUTTON)
        || Raylib.IsMouseButtonPressed(Raylib.MOUSE_RIGHT_BUTTON)
        || Raylib.IsKeyReleased(KeyboardKey.KEY_SPACE)
        || Raylib.IsKeyReleased(KeyboardKey.KEY_ESCAPE)
        || Raylib.IsKeyReleased(KeyboardKey.KEY_ENTER))
            return GameButtonActions.Exit;


        return GameButtonActions.Nothing;
    }

    public void SetupCreditsScreen()
    {
        SoundManager.Instance.PlayMusic(GameMusic.Credits);
    }
}
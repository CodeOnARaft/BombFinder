using Raylib_CsLo;

namespace BombFinder;


public class Credits
{
    private static Credits _instance;
    public static Credits Instance => _instance;

    private float _halfscreen = Constants.TITLE_SCREEN_WIDTH / 2;
    static Credits()
    {
        _instance = new Credits();
    }

    public void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib.BLACK);
        DrawCenteredText(Constants.GAME_TITLE, 32, 5, Raylib.WHITE);
        DrawCenteredText("Programming and Graphics", 22, 60, Raylib.BLUE);
        DrawCenteredText("Kevin Hann", 22, 85, Raylib.GREEN);
        DrawCenteredText("Sound and Music", 22, 130, Raylib.BLUE);
        DrawCenteredText("From OpenGameArt.Org", 22, 160, Raylib.BLUE);
        DrawCenteredText("Lokif", 22, 185, Raylib.GREEN);
        DrawCenteredText("Joth", 22, 210, Raylib.GREEN);
        DrawCenteredText("onky", 22, 235, Raylib.GREEN);
        DrawCenteredText("Brandon Morris", 16, 260, Raylib.GREEN);

        DrawCenteredText("Made With", 22, 290, Raylib.BLUE);
        Raylib.DrawTexture(TextureManager.Instance.GetTexture(GameTextures.RaylibLogo), Constants.HALF_TITLE_SCREEN_WIDTH - 50, 315, Raylib.WHITE);
        Raylib.EndDrawing();
    }

    private void DrawCenteredText(string text, int size, int y, Color color)
    {
        Raylib.DrawText(text, Constants.HALF_TITLE_SCREEN_WIDTH - Raylib.MeasureText(text, size) / 2, y, size, color);

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
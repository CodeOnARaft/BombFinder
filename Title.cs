using System.Numerics;
using Raylib_CsLo;

namespace BombFinder;



public class TitleScreen
{
    public enum TitleScreenAction
    {
        Nothing,
        Credits,
        Exit,
        Easy,
        Medium,
        Hard,
    }
    private static TitleScreen _instance;
    public static TitleScreen Instance => _instance;

    public const int TITLE_SCREEN_WIDTH = 400;
    public const int TITLE_SCREEN_HEIGHT = 500;


    static TitleScreen()
    {
        _instance = new TitleScreen();
    }


    private List<TitleButton> _buttons = new List<TitleButton>();
    private Music click;
    protected TitleScreen()
    {
        var scd = TITLE_SCREEN_WIDTH / 2;
        _buttons.Add(new TitleButton(new Rectangle(scd, 75, 200, 50), "Easy", TitleScreenAction.Easy));
        _buttons.Add(new TitleButton(new Rectangle(scd, 175, 200, 50), "Medium", TitleScreenAction.Medium));
        _buttons.Add(new TitleButton(new Rectangle(scd, 275, 200, 50), "Hard", TitleScreenAction.Hard));
        _buttons.Add(new TitleButton(new Rectangle(scd, 375, 200, 50), "Credits", TitleScreenAction.Credits));
        _buttons.Add(new TitleButton(new Rectangle(scd, 475, 200, 50), "Exit", TitleScreenAction.Exit));

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
        Raylib.DrawText($"Bomb Finder!", 180, 5, 32, Raylib.WHITE);

        _buttons.ForEach(x => x.Draw());

        Raylib.EndDrawing();
    }


    public TitleScreenAction TestMouseClick()
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

        return TitleScreenAction.Nothing;
    }



    private class TitleButton
    {

        private Rectangle _position;
        private string _text;
        TitleScreenAction _action;

        public TitleScreenAction Action => _action;
        public TitleButton(Rectangle postion, string text, TitleScreenAction action)
        {
            _position = postion;
            _text = text;
            _action = action;
        }

        public void Draw()
        {
            Raylib.DrawRectangleLines((int)_position.X, (int)_position.Y, (int)_position.width, (int)_position.height, Raylib.WHITE);
            Raylib.DrawText(_text, (int)(_position.X + _position.width / 2 - 20), (int)(_position.Y + _position.height / 2 - 20), 20, Raylib.WHITE);
        }

        public bool TestCollision(Vector2 point)
        {
            return Raylib.CheckCollisionPointRec(point, _position);
        }
    }

}
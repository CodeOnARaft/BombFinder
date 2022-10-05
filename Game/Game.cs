using Raylib_CsLo;

namespace BombFinder;

public class Game
{

    static Game()
    {
        _instance = new Game();
    }

    private static Game _instance;
    public static Game Instance => _instance;

    private GameScenes _scene;
    private Board _board;
    public void Play()
    {
        _scene = GameScenes.Title;

        Raylib.InitWindow(Constants.TITLE_SCREEN_WIDTH, Constants.TITLE_SCREEN_HEIGHT, Constants.GAME_TITLE);
        Raylib.SetTargetFPS(60);
        var image = Raylib.LoadImage("bombfinder.ico");
        Raylib.SetWindowIcon(image);

        TitleScreen.Instance.SetupTitleScreen();

        // Main game loop
        var done = false;
        while (!done && !Raylib.WindowShouldClose()) // Detect window close button or ESC key
        {
            switch (_scene)
            {

                case GameScenes.Credits:
                    Credits.Instance.Draw();
                    var creditActions = Credits.Instance.TestInput();
                    if (creditActions == GameButtonActions.Exit)
                    {
                        _scene = GameScenes.Title;
                        TitleScreen.Instance.SetupTitleScreen();
                    }
                    break;

                case GameScenes.Title:
                    TitleScreen.Instance.Draw();
                    var action = TitleScreen.Instance.TestInput();
                    switch (action)
                    {
                        case GameButtonActions.Exit:
                            done = true;
                            break;

                        case GameButtonActions.Easy:
                            _board = new Board(GameDifficulties.Easy);
                            _scene = GameScenes.Game;
                            break;

                        case GameButtonActions.Medium:
                            _board = new Board(GameDifficulties.Medium);
                            _scene = GameScenes.Game;
                            break;

                        case GameButtonActions.Hard:
                            _board = new Board(GameDifficulties.Hard);
                            _scene = GameScenes.Game;
                            break;
                        case GameButtonActions.Credits:
                            _scene = GameScenes.Credits;
                            Credits.Instance.SetupCreditsScreen();
                            break;

                        default:
                            break;
                    }
                    break;

                case GameScenes.Game:
                    _board.Draw();
                    var boardAction = _board.TestInput();
                    if (boardAction == GameButtonActions.Exit)
                    {
                        _scene = GameScenes.Title;
                        TitleScreen.Instance.SetupTitleScreen();
                    }
                    break;
            }

        }
        Raylib.CloseWindow();
    }
}
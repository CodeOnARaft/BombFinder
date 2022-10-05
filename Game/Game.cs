using Raylib_CsLo;

namespace BombFinder;

public class Game
{

    static Game()
    {
        _instace = new Game();
    }

    private static Game _instace;
    public static Game Instance => _instace;

  

    private GameScenes currentScene;
    private Board currentBoard;
    public void Play()
    {
        currentScene = GameScenes.Title;

        Raylib.InitWindow(TitleScreen.TITLE_SCREEN_WIDTH, TitleScreen.TITLE_SCREEN_HEIGHT, Constants.GAME_TITLE);
        Raylib.SetTargetFPS(60);
        TitleScreen.Instance.SetupTitleScreen();

        // Main game loop
        var done = false;
        while (!done && !Raylib.WindowShouldClose()) // Detect window close button or ESC key
        {
            switch (currentScene)
            {
                case GameScenes.Title:
                    TitleScreen.Instance.DrawScreen();
                    var action = TitleScreen.Instance.TestMouseClick();
                    switch (action)
                    {
                        case GameButtonActions.Exit:
                            done = true;
                            break;

                        case GameButtonActions.Easy:
                            currentBoard = new Board(GameDifficulties.Easy);
                            currentScene = GameScenes.Game;
                            break;

                        case GameButtonActions.Medium:
                            currentBoard = new Board(GameDifficulties.Medium);
                            currentScene = GameScenes.Game;
                            break;

                        case GameButtonActions.Hard:
                            currentBoard = new Board(GameDifficulties.Hard);
                            currentScene = GameScenes.Game;
                            break;

                        default:
                            break;
                    }
                    break;

                case GameScenes.Game:
                    currentBoard.DrawBoard();
                    var boardAction = currentBoard.TestMouseClick();
                    if (boardAction == GameButtonActions.Exit)
                    {
                        currentScene = GameScenes.Title;
                        TitleScreen.Instance.SetupTitleScreen();
                    }
                    break;
            }

        }
        Raylib.CloseWindow();
    }
}
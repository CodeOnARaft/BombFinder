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

    private enum Scene
    {
        Title,
        Game
    }

    private Scene currentScene;
    private Board currentBoard;
    public void Play()
    {
        currentScene = Scene.Title;
       
        Raylib.InitWindow( TitleScreen.TITLE_SCREEN_WIDTH,  TitleScreen.TITLE_SCREEN_HEIGHT, "Bomb Finder!");
        Raylib.SetTargetFPS(60);
        TitleScreen.Instance.SetupTitleScreen();
        currentBoard = new Board(Board.Difficulties.Medium);

        // Main game loop
        var done = false;
        while (!done && !Raylib.WindowShouldClose()) // Detect window close button or ESC key
        {
            switch (currentScene)
            {
                case Scene.Title:
                    TitleScreen.Instance.DrawScreen();
                    var action = TitleScreen.Instance.TestMouseClick();
                    switch (action)
                    {
                        case TitleScreen.TitleScreenAction.Exit:                        
                            done = true;
                            break;

                        case TitleScreen.TitleScreenAction.Easy:
                            currentBoard = new Board(Board.Difficulties.Easy);
                            currentScene = Scene.Game;
                            break;

                        case TitleScreen.TitleScreenAction.Medium:
                            currentBoard = new Board(Board.Difficulties.Medium);
                            currentScene = Scene.Game;
                            break;

                        case TitleScreen.TitleScreenAction.Hard:
                            currentBoard = new Board(Board.Difficulties.Hard);
                            currentScene = Scene.Game;
                            break;

                        default:
                            break;
                    }
                    break;

                case Scene.Game:
                    currentBoard.DrawBoard();
                    currentBoard.TestMouseClick();
                    break;
            }

        }
        Raylib.CloseWindow();
    }
}
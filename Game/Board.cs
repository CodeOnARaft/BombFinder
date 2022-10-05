
using Raylib_CsLo;

namespace BombFinder;

public class Board
{
    Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();

    int numXTiles = 9;
    int numYTiles = 9;
    int numBombs = 9;

    GameTile[,]? _tiles;


    private GameDifficulties _difficulty;
    private int selectedSquares = 0;
    private DateTime _startTime;
    private DateTime _endTime;
    private GameStates _gameState;
    private GameButton _backToMenuButton;
    private GameButton _resetButton;

    public Board(GameDifficulties difficulty)
    {
        _difficulty = difficulty;

        SetupBoard();
    }

    private void SetupBoard()
    {

        switch (_difficulty)
        {
            case GameDifficulties.Easy:
                numXTiles = 9;
                numYTiles = 9;
                numBombs = 10;
                Raylib.SetWindowSize(numXTiles * Constants.SQUARE_SIZE + Constants.BOARD_OFFSET * 2, 600);
                break;
            case GameDifficulties.Medium:
                numXTiles = 16;
                numYTiles = 16;
                numBombs = 40;
                Raylib.SetWindowSize(numXTiles * Constants.SQUARE_SIZE + Constants.BOARD_OFFSET * 2, 700);
                break;
            case GameDifficulties.Hard:
                numXTiles = 30;
                numYTiles = 16;
                numBombs = 99;
                Raylib.SetWindowSize(numXTiles * Constants.SQUARE_SIZE + Constants.BOARD_OFFSET * 2, 800);
                break;

        }

        _tiles = new GameTile[numXTiles, numYTiles];
        for (int x = 0; x < numXTiles; x++)
            for (int y = 0; y < numYTiles; y++)
            {
                int posX = (x * Constants.SQUARE_SIZE) + Constants.BOARD_OFFSET;
                int posY = (y * Constants.SQUARE_SIZE) + Constants.BOARD_OFFSET_Y;
                _tiles[x, y] = new GameTile(new System.Numerics.Vector2(posX, posY));
            }

        // Select Bomb Locations
        int need = numBombs;
        var rnd = new Random();
        while (need > 0)
        {
            var x = rnd.Next(0, numXTiles);
            var y = rnd.Next(0, numYTiles);

            if (!_tiles[x, y].IsBomb)
            {
                _tiles[x, y].Bombs = GameTile.BOMB;
                need--;
            }
        }

        // Get Bomb counts for empty tiles
        for (int x = 0; x < numXTiles; x++)
            for (int y = 0; y < numYTiles; y++)
            {
                if (_tiles[x, y].IsBomb) continue;

                UpdateBombCount(x, y, -1, -1);
                UpdateBombCount(x, y, 0, -1);
                UpdateBombCount(x, y, 1, -1);

                UpdateBombCount(x, y, -1, 0);
                UpdateBombCount(x, y, 1, 0);

                UpdateBombCount(x, y, -1, 1);
                UpdateBombCount(x, y, 0, 1);
                UpdateBombCount(x, y, 1, 1);
            }

        var btnY = numYTiles * Constants.SQUARE_SIZE + Constants.BOARD_OFFSET_Y + 10;
        _backToMenuButton = new GameButton(new Rectangle(Constants.BOARD_OFFSET, btnY, 175, Constants.BUTTON_STANDARD_HEIGHT), "Back to Menu", GameButtonActions.Exit);
        _resetButton = new GameButton(new Rectangle(Constants.BOARD_OFFSET + 180, btnY, 100, Constants.BUTTON_STANDARD_HEIGHT), "Reset", GameButtonActions.Reset);

        _startTime = DateTime.Now;
        _gameState = GameStates.Playing;
    }

    private void UpdateBombCount(int x, int y, int x1, int y1)
    {
        if (x + x1 < 0 || x + x1 >= numXTiles) return;
        if (y + y1 < 0 || y + y1 >= numYTiles) return;

        if (_tiles[x + x1, y + y1].IsBomb)
            _tiles[x, y].Bombs++;
    }

    public void DrawBoard()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib.BLACK);

        for (int x = 0; x < numXTiles; x++)
            for (int y = 0; y < numYTiles; y++)
                _tiles[x, y].Draw();

        var span = _gameState == GameStates.Playing ? DateTime.Now.Subtract(_startTime) : _endTime.Subtract(_startTime);
        Raylib.DrawText($"Time: {span.ToString("ss")}, Bombs: {numBombs - selectedSquares}", 5, 5, 20, Raylib.WHITE);
        _backToMenuButton.Draw();
        _resetButton.Draw();

        switch (_gameState)
        {
            case GameStates.Won:
                DrawWon();
                break;

            case GameStates.Lost:
                DrawLost();
                break;
        }

        Raylib.EndDrawing();
    }

    public void DrawWon()
    {
        Raylib.DrawRectangle(Constants.BOARD_OFFSET, Constants.BOARD_OFFSET_Y, 100, 100, Raylib.GREEN);
    }

    public void DrawLost()
    {
        Raylib.DrawRectangle(Constants.BOARD_OFFSET, Constants.BOARD_OFFSET_Y, 100, 100, Raylib.RED);

    }

    public GameButtonActions TestMouseClick()
    {
        var pos = Raylib.GetMousePosition();

        if (Raylib.IsMouseButtonPressed(Raylib.MOUSE_LEFT_BUTTON))
        {
            if (_backToMenuButton.TestCollision(pos)) return _backToMenuButton.Action;

            if (_resetButton.TestCollision(pos))
            {
                SetupBoard();
                return _resetButton.Action;
            }
        }
        // if we are outside the board just skip
        if (pos.X < Constants.BOARD_OFFSET || pos.Y < Constants.BOARD_OFFSET_Y) return GameButtonActions.Nothing;

        // Get the tile we are over (zero based)
        var tileX = (int)Math.Floor((pos.X - Constants.BOARD_OFFSET) / Constants.SQUARE_SIZE);
        var tileY = (int)Math.Floor((pos.Y - Constants.BOARD_OFFSET_Y) / Constants.SQUARE_SIZE);

        // if we are outside the board just skip
        if (tileX >= numXTiles || tileY >= numYTiles) return GameButtonActions.Nothing;


        if (Raylib.IsMouseButtonPressed(Raylib.MOUSE_LEFT_BUTTON))
        {
            switch (_gameState)
            {
                case GameStates.Playing:
                    if (!_tiles[tileX, tileY].IsCovered) return GameButtonActions.Nothing;
                    _tiles[tileX, tileY].Uncover();

                    if (_tiles[tileX, tileY].IsBomb)
                    {
                        _gameState = GameStates.Lost;
                        _endTime = DateTime.Now;
                    }
                    else
                    {

                        if (_tiles[tileX, tileY].Bombs == 0)
                        {
                            stack.Push(new Tuple<int, int>(tileX, tileY));
                            UncoverBlanks();
                        }
                    }
                    break;
            }
        }
        else if (Raylib.IsMouseButtonPressed(Raylib.MOUSE_RIGHT_BUTTON))
        {
            if (_gameState == GameStates.Playing)
            {
                var selectedDelta = _tiles[tileX, tileY].CycleCoveredType();
                selectedSquares += selectedDelta;
            }
        }

        return GameButtonActions.Nothing;
    }

    private void UncoverBlanks()
    {
        while (stack.Any())
        {
            var item = stack.Pop();
            var x = item.Item1;
            var y = item.Item2;

            TestForStack(x, y - 1);
            TestForStack(x - 1, y);
            TestForStack(x + 1, y);
            TestForStack(x, y + 1);
        }
    }

    private void TestForStack(int xx, int yy)
    {
        if (xx >= 0 && yy >= 0 && xx < numXTiles && yy < numYTiles && _tiles[xx, yy].Bombs >= 0 && _tiles[xx, yy].IsCovered)
        {
            _tiles[xx, yy].Uncover();
            if (_tiles[xx, yy].Bombs == 0)
                stack.Push(new Tuple<int, int>(xx, yy));
        }
    }
}
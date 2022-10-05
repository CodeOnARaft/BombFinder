
using Raylib_CsLo;

namespace BombFinder;

public class Board
{
    public const int SQUARE_SIZE = 32;
    public const int BOARD_OFFSET = 10;
    public const int BOARD_OFFSET_Y = 35;

    const int BOMB = -1;

    static Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();

    int numXTiles = 9;
    int numYTiles = 9;
    int numBombs = 9;

    GameTile[,]? _tiles;
    public enum Difficulties
    {
        Easy,
        Medium,
        Hard
    }

    public enum BoardActons
    {
        Nothing,
        ExitGame
    }


    private Difficulties _difficulty;
    private int selectedSquares = 0;
    private DateTime startTime;

    public Board(Difficulties difficulty)
    {
        _difficulty = difficulty;

        SetupBoard();
        startTime = DateTime.Now;
    }

    private void SetupBoard()
    {
        switch (_difficulty)
        {
            case Difficulties.Easy:
                numXTiles = 9;
                numYTiles = 9;
                numBombs = 10;
                Raylib.SetWindowSize(numXTiles * 32 + BOARD_OFFSET * 2, 600);
                break;
            case Difficulties.Medium:
                numXTiles = 16;
                numYTiles = 16;
                numBombs = 40;
                Raylib.SetWindowSize(numXTiles * 32 + BOARD_OFFSET * 2, 700);
                break;
            case Difficulties.Hard:
                numXTiles = 30;
                numYTiles = 16;
                numBombs = 99;
                Raylib.SetWindowSize(numXTiles * 32 + BOARD_OFFSET * 2, 800);
                break;

        }

        _tiles = new GameTile[numXTiles, numYTiles];
        for (int x = 0; x < numXTiles; x++)
            for (int y = 0; y < numYTiles; y++)
            {
                int posX = (x * SQUARE_SIZE) + BOARD_OFFSET;
                int posY = (y * SQUARE_SIZE) + BOARD_OFFSET_Y;
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
                _tiles[x,y].Draw();

        var span = DateTime.Now.Subtract(startTime);
        Raylib.DrawText($"Time: {span.ToString("ss")}, Bombs: {numBombs - selectedSquares}", 5, 5, 20, Raylib.WHITE);
        Raylib.EndDrawing();
    }
    public GameButton.GameButtonActions TestMouseClick()
    {
        var pos = Raylib.GetMousePosition();
        if (pos.X >= BOARD_OFFSET && pos.Y >= BOARD_OFFSET_Y)
        {
            // Get the tile we are over (zero based)
            var tileX = (int)Math.Floor((pos.X - BOARD_OFFSET) / SQUARE_SIZE);
            var tileY = (int)Math.Floor((pos.Y - BOARD_OFFSET_Y) / SQUARE_SIZE);

            // if we are outside the board just skip
            if (tileX >= numXTiles || tileY >= numYTiles) return GameButton.GameButtonActions.Nothing;


            if (Raylib.IsMouseButtonPressed(Raylib.MOUSE_LEFT_BUTTON))
            {
                if (!_tiles[tileX, tileY].IsCovered) return GameButton.GameButtonActions.Nothing;

                if (!_tiles[tileX, tileY].IsBomb)
                {
                    _tiles[tileX, tileY].Uncover();
                    if (_tiles[tileX, tileY].Bombs == 0)
                    {
                        stack.Push(new Tuple<int, int>(tileX, tileY));
                        UncoverBlanks();
                    }
                }

            }
            else if (Raylib.IsMouseButtonPressed(Raylib.MOUSE_RIGHT_BUTTON))
            {
               var selectedDelta =_tiles[tileX,tileY].CycleCoveredType();
               selectedSquares += selectedDelta;

            }
        }

        return GameButton.GameButtonActions.Nothing;
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
        if (xx >= 0 && yy >= 0 && xx < numXTiles && yy < numYTiles && _tiles[xx, yy].Bombs >= 0 && _tiles[xx,yy].IsCovered)
        {
            _tiles[xx, yy].Uncover();
            if (_tiles[xx, yy].Bombs == 0)
                stack.Push(new Tuple<int, int>(xx, yy));
        }
    }
}
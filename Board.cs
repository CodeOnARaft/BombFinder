
using Raylib_CsLo;

namespace BombFinder;

public class Board
{
    const int SQUARE_SIZE = 32;
    const int BOARD_OFFSET = 10;
    const int BOARD_OFFSET_Y = 35;

    const int COVERED_TEXTURE = 0;
    const int BLANK_TEXTURE = 1;
    const int SELECTED_TEXTURE = 2;
    const int QUESTION_TEXTURE = 3;

    const int BOMB = -1;

    int numXTiles = 9;
    int numYTiles = 9;
    int numBombs = 9;

    int[,]? mines;
    int[,]? covered;

    static Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();


    static Texture coveredTexture;
    static Texture blankTexture;

    static Texture selectedTexture;

    static Texture questionTexture;

    static Board()
    {
        var image = Raylib.LoadImage("images/covered.png");     // Loaded in CPU memory (RAM)
        coveredTexture = Raylib.LoadTextureFromImage(image);          // Image converted to texture, GPU memory (VRAM)

        image = Raylib.LoadImage("images/blank.png");     // Loaded in CPU memory (RAM)
        blankTexture = Raylib.LoadTextureFromImage(image);          // Image converted to texture, GPU memory (VRAM)

        image = Raylib.LoadImage("images/selected.png");     // Loaded in CPU memory (RAM)
        selectedTexture = Raylib.LoadTextureFromImage(image);          // Image converted to texture, GPU memory (VRAM)

        image = Raylib.LoadImage("images/question.png");     // Loaded in CPU memory (RAM)
        questionTexture = Raylib.LoadTextureFromImage(image);          // Image converted to texture, GPU memory (VRAM)

    }

    public enum Difficulties
    {
        Easy,
        Medium,
        Hard
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
                numBombs = 9;
                Raylib.SetWindowSize(600, 600);
                break;
            case Difficulties.Medium:
                numXTiles = 16;
                numYTiles = 16;
                numBombs = 40;
                Raylib.SetWindowSize(800, 700);
                break;
            case Difficulties.Hard:
                numXTiles = 30;
                numYTiles = 16;
                numBombs = 99;
                Raylib.SetWindowSize(1000, 800);
                break;

        }

        mines = new int[numXTiles, numYTiles];
        covered = new int[numXTiles, numYTiles];

        // Select Bomb Locations
        int need = numBombs;
        var rnd = new Random();
        while (need > 0)
        {
            var x = rnd.Next(0, numXTiles);
            var y = rnd.Next(0, numYTiles);

            if (mines[x, y] == 0)
            {
                mines[x, y] = BOMB;
                need--;
            }
        }


        for (int x = 0; x < numXTiles; x++)
            for (int y = 0; y < numYTiles; y++)
            {
                if (mines[x, y] == BOMB) continue;

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

        if (mines[x + x1, y + y1] == BOMB)
            mines[x, y]++;
    }

    public void DrawBoard()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib.BLACK);
        var texture = coveredTexture;

        for (int x = 0; x < numXTiles; x++)
            for (int y = 0; y < numYTiles; y++)
            {
                bool num = false;

                switch (covered[x, y])
                {
                    case COVERED_TEXTURE:
                        texture = coveredTexture;
                        break;

                    case BLANK_TEXTURE:
                        texture = blankTexture;
                        num = true;
                        break;

                    case SELECTED_TEXTURE:
                        texture = selectedTexture;
                        break;

                    case QUESTION_TEXTURE:
                        texture = questionTexture;
                        break;

                }
                int textX = (x * SQUARE_SIZE) + BOARD_OFFSET;
                int textY = (y * SQUARE_SIZE) + BOARD_OFFSET_Y;
                int textOff = -3 + SQUARE_SIZE / 2;
                Raylib.DrawTexture(texture, textX, textY, Raylib.WHITE);

                if (num && mines[x, y] != 0)
                {
                    Raylib.DrawText(mines[x, y].ToString(), textX + textOff, textY + textOff, 8, Raylib.RED);
                }
            }

        var span = DateTime.Now.Subtract(startTime);
        Raylib.DrawText($"Time: {span.ToString("ss")}, Bombs: {numBombs - selectedSquares}", 5, 5, 20, Raylib.WHITE);
        Raylib.EndDrawing();
    }
    public void TestMouseClick()
    {
        var pos = Raylib.GetMousePosition();
        if (pos.X >= BOARD_OFFSET && pos.Y >= BOARD_OFFSET_Y)
        {
            // Get the tile we are over (zero based)
            var tileX = (int)Math.Floor((pos.X - BOARD_OFFSET) / SQUARE_SIZE);
            var tileY = (int)Math.Floor((pos.Y - BOARD_OFFSET_Y) / SQUARE_SIZE);

            // if we are outside the board just skip
            if (tileX >= numXTiles || tileY >= numYTiles) return;


            if (Raylib.IsMouseButtonPressed(Raylib.MOUSE_LEFT_BUTTON))
            {
                if (covered[tileX, tileY] != 0) return;

                if (mines[tileX, tileY] != -1)
                {
                    covered[tileX, tileY] = 1;
                    if (mines[tileX, tileY] == 0)
                    {
                        stack.Push(new Tuple<int, int>(tileX, tileY));
                        UncoverBlanks();
                    }
                }

            }
            else if (Raylib.IsMouseButtonPressed(Raylib.MOUSE_RIGHT_BUTTON))
            {
                switch (covered[tileX, tileY])
                {
                    case COVERED_TEXTURE:
                        covered[tileX, tileY] = SELECTED_TEXTURE;
                        selectedSquares++;
                        break;

                    case SELECTED_TEXTURE:
                        covered[tileX, tileY] = QUESTION_TEXTURE;
                        selectedSquares--;
                        break;

                    case QUESTION_TEXTURE:
                        covered[tileX, tileY] = COVERED_TEXTURE;
                        break;

                    default:
                        return;
                }
            }
        }
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
        if (xx >= 0 && yy >= 0 && xx < numXTiles && yy < numYTiles && mines[xx, yy] >= 0 && covered[xx, yy] == 0)
        {
            covered[xx, yy] = 1;
            if (mines[xx, yy] == 0)
                stack.Push(new Tuple<int, int>(xx, yy));

        }
    }
}
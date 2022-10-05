using System.Numerics;
using Raylib_CsLo;

namespace BombFinder;

public class GameTile
{
    public const int BOMB = -1;
    private const int TEXT_OFF_SET = -3 + Constants.SQUARE_SIZE / 2;
    public int Bombs
    {
        get; set;
    }

    public bool IsBomb => Bombs == BOMB;
    public bool IsCovered => _texture == GameTextures.Covered;

    public bool IsBlank => _texture == GameTextures.Blank;

    private GameTextures _texture = GameTextures.Covered;
    private Vector2 _position;
    public GameTile(Vector2 position)
    {
        _position = position;
        _bounds = new Rectangle(_position.X, _position.Y, Constants.SQUARE_SIZE, Constants.SQUARE_SIZE);
    }

    private int X => (int)_position.X;
    private int Y => (int)_position.Y;
    private bool _hover = false;
    private Rectangle _bounds;

    public void Draw()
    {
        Raylib.DrawTexture(TextureManager.Instance.GetTexture(_texture), X, Y, Raylib.WHITE);

        if (_texture == GameTextures.Blank && Bombs != 0)
        {
            Raylib.DrawText(Bombs.ToString(), X + TEXT_OFF_SET, Y + TEXT_OFF_SET, 8, Raylib.RED);
        }

        if (_hover)
            Raylib.DrawRectangleLines((int)_position.X, (int)_position.Y, Constants.SQUARE_SIZE, Constants.SQUARE_SIZE, Raylib.YELLOW);
    }

    public void Uncover()
    {
        if (IsBomb)
            _texture = GameTextures.Bomb;
        else
            _texture = GameTextures.Blank;
    }

    public int CycleCoveredType()
    {
        int rvalue = 0;
        switch (_texture)
        {
            case GameTextures.Covered:
                _texture = GameTextures.Selected;
                rvalue = 1;
                break;

            case GameTextures.Selected:
                _texture = GameTextures.Question;
                rvalue = -1;
                break;

            case GameTextures.Question:
                _texture = GameTextures.Covered;
                break;
        }

        return rvalue;
    }

    public void TestHover(Vector2 point)
    {
        _hover = Raylib.CheckCollisionPointRec(point, _bounds);
    }
}
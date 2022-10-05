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
    public bool IsCovered => _texture == TextureManager.Textures.Covered;

    private TextureManager.Textures _texture = TextureManager.Textures.Covered;
    private Vector2 _position;
    public GameTile(Vector2 position)
    {
        _position = position;
    }

    private int X => (int)_position.X;
    private int Y => (int)_position.Y;

    public void Draw()
    {
        Raylib.DrawTexture(TextureManager.Instance.GetTexture(_texture), X, Y, Raylib.WHITE);
        if (_texture == TextureManager.Textures.Blank && Bombs != 0)
        {
            Raylib.DrawText(Bombs.ToString(), X + TEXT_OFF_SET, Y + TEXT_OFF_SET, 8, Raylib.RED);
        }
    }

    public void Uncover()
    {
        _texture = TextureManager.Textures.Blank;
    }

    public int CycleCoveredType()
    {
        int rvalue = 0;
        switch (_texture)
        {
            case TextureManager.Textures.Covered:
                _texture = TextureManager.Textures.Selected;
                rvalue = 1;
                break;

            case TextureManager.Textures.Selected:
                _texture = TextureManager.Textures.Question;
                rvalue = -1;
                break;

            case TextureManager.Textures.Question:
                _texture = TextureManager.Textures.Covered;
                break;
        }

        return rvalue;
    }
}
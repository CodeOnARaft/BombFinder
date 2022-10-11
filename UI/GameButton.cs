using System.Numerics;
using Raylib_CsLo;

namespace BombFinder;

public class GameButton
{


    private Rectangle _position;
    private string _text;
    GameButtonActions _action;

    public GameButtonActions Action => _action;

    private bool _hover = false;

    public Color NormalColor { get; set; }
    public Color HoverColor { get; set; }


    public GameButton(Rectangle postion, string text, GameButtonActions action)
    {
        _position = postion;
        _text = text;
        _action = action;

        NormalColor = Raylib.WHITE;
        HoverColor = Raylib.YELLOW;
    }

    public void Draw()
    {
        var color = _hover ? HoverColor : NormalColor;
        Raylib.DrawRectangleLines((int)_position.X, (int)_position.Y, (int)_position.width, (int)_position.height, color);
        var textSizeHalf = Raylib.MeasureText(_text, 20) / 2;
        Raylib.DrawText(_text, (int)(_position.X + _position.width / 2 - textSizeHalf), (int)(_position.Y + _position.height / 2 - 10), 20, color);
    }

    public bool TestCollision(Vector2 point)
    {
        return Raylib.CheckCollisionPointRec(point, _position);
    }

    public void TestHover(Vector2 point)
    {
        _hover = TestCollision(point);
    }
}

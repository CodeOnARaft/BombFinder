using System.Numerics;
using Raylib_CsLo;

namespace BombFinder;

public class GameButton
{
    public enum GameButtonActions
    {
        Nothing,
        Credits,
        Exit,
        Easy,
        Medium,
        Hard,
    }

    private Rectangle _position;
    private string _text;
    GameButtonActions _action;

    public GameButtonActions Action => _action;
    public GameButton(Rectangle postion, string text, GameButtonActions action)
    {
        _position = postion;
        _text = text;
        _action = action;
    }

    public void Draw()
    {
        Raylib.DrawRectangleLines((int)_position.X, (int)_position.Y, (int)_position.width, (int)_position.height, Raylib.WHITE);
        var textSizeHalf = Raylib.MeasureText(_text, 20) / 2;        
        Raylib.DrawText(_text, (int)(_position.X + _position.width / 2 - textSizeHalf), (int)(_position.Y + _position.height / 2 -10), 20, Raylib.WHITE);
    }

    public bool TestCollision(Vector2 point)
    {
        return Raylib.CheckCollisionPointRec(point, _position);
    }
}

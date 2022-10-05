using Raylib_CsLo;

namespace BombFinder;

public class TextureManager
{

    private static TextureManager _instance;
    public static TextureManager Instance => _instance;
    static TextureManager()
    {
        _instance = new TextureManager();
        _instance.LoadTextures();
    }

    private List<Texture> _textures = new List<Texture>();

    private void LoadTextures()
    {
        var image = Raylib.LoadImage("images/covered.png");
        _textures.Add(Raylib.LoadTextureFromImage(image));

        image = Raylib.LoadImage("images/blank.png");
        _textures.Add(Raylib.LoadTextureFromImage(image));

        image = Raylib.LoadImage("images/selected.png");
        _textures.Add(Raylib.LoadTextureFromImage(image));

        image = Raylib.LoadImage("images/question.png");
        _textures.Add(Raylib.LoadTextureFromImage(image));

         image = Raylib.LoadImage("images/bomb.png");
        _textures.Add(Raylib.LoadTextureFromImage(image));
    }

    public Texture GetTexture(GameTextures texture){
        return _textures[(int)texture];
    }

}
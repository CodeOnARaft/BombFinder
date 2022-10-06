using Raylib_CsLo;

namespace BombFinder;

public class SoundManager
{
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;

    private List<Sound> sounds = new List<Sound>();
    private List<Music> musics = new List<Music>();

    private Music? lastMusic = null;
    static SoundManager()
    {
        _instance = new SoundManager();
    }

    private SoundManager()
    {
        Raylib.InitAudioDevice();

        LoadSounds();
        LoadMusic();
    }

    private void LoadSounds()
    {
        sounds.Add(Raylib.LoadSound("sounds/click.wav"));
        sounds.Add(Raylib.LoadSound("sounds/win.wav"));
        sounds.Add(Raylib.LoadSound("sounds/lose.wav"));
        sounds.Add(Raylib.LoadSound("sounds/button.wav"));


    }

    private void LoadMusic()
    {
        musics.Add(Raylib.LoadMusicStream("sounds/title.ogg"));
        musics.Add(Raylib.LoadMusicStream("sounds/game.mp3"));
        
    }

    public void PlaySound(GameSounds sound)
    {
        Raylib.PlaySound(sounds[(int)sound]);
    }

    public void PlayMusic(GameMusic music)
    {
        StopMusic();
        lastMusic = musics[(int)music];
        Raylib.PlayMusicStream((Music)lastMusic);
        Raylib.SetMusicVolume((Music)lastMusic,0.35f);
    }

    public void UpdateMusicStream()
    {
        if (lastMusic != null)
            Raylib.UpdateMusicStream((Music)lastMusic);
    }
    public void StopMusic()
    {
        if (lastMusic != null)
            Raylib.StopMusicStream((Music)lastMusic);

        lastMusic = null;
    }
}
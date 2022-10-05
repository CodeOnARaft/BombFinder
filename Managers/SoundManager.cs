namespace BombFinder;

public class SoundManager
{
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;
    static SoundManager()
    {
        _instance = new SoundManager();
    }

    private SoundManager(){
        LoadSounds();
        LoadMusic();
    }

    private void LoadSounds(){

    }

    private void LoadMusic(){

    }

    public void PlaySound(GameSounds sound){

    }

    public void PlayMusic(GameMusic music){
        
    }
}
namespace BombFinder;

public enum GameScenes
{
    Title,
    Game,
    Credits
}

public enum GameDifficulties
{
    Easy,
    Medium,
    Hard
}

public enum GameStates
{
    Playing,
    Won,
    Lost
}

public enum GameButtonActions
{
    Nothing,
    Credits,
    Exit,
    Easy,
    Medium,
    Hard,
    Reset
}

public enum GameTextures
{
    Covered = 0,
    Blank = 1,
    Selected = 2,
    Question = 3,
    Bomb = 4
}

public enum GameSounds
{
    Click = 0,
    Explosion = 1,
    GameWon = 2,
    GameLost = 3
}

public enum GameMusic{
    Title = 0,
    Game = 1,
    Credits = 2
}
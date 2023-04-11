public class GameScore
{
    int _currentScore = 0;
    public int Add(int scoreToAdd)
    {
        _currentScore += scoreToAdd;
        return _currentScore;
    }

    public void Save(string playerName)
    {
        ServerData SD = new ServerData();
        SD.SaveScore(_currentScore, playerName);
    }

    public int Get()
    {
        return _currentScore;
    }

    public void Reset()
    {
        _currentScore = 0;
    }
}
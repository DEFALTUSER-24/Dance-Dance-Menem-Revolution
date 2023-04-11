public class GameScore
{
    int _currentScore = 0;
    public void Update(KeypressPrecision result)
    {
        switch (result)
        {
            case KeypressPrecision.Excellent:
                _currentScore += 1000;
                break;
            case KeypressPrecision.VeryGood:
                _currentScore += 080;
                break;
            case KeypressPrecision.Good:
                _currentScore += 400;
                break;
            case KeypressPrecision.Bad:
                _currentScore -= 200;
                break;
        }

        OnUpdate();
    }

    private void OnUpdate()
    {
        GameManager.instance.UpdateScoreUI();
    }

    public void Save(string playerName, int gameLevel)
    {
        ServerData SD = new ServerData();
        SD.SaveScore(_currentScore, playerName, gameLevel);
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
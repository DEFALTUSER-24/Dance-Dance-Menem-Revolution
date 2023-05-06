using UnityEngine;

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
                _currentScore += 800;
                break;
            case KeypressPrecision.Good:
                _currentScore += 400;
                break;
            case KeypressPrecision.Bad:
                _currentScore -= 200;
                break;
        }

        UI.instance.UpdateKeypressResult(result);

        Mathf.Clamp(_currentScore, 0, Mathf.Infinity);
    }

    //public void Save(string playerName, int gameLevel)
    public void Save(string playerName)
    {
        ServerRequest.instance.SaveScore(GameManager.instance.Score().Get(), playerName);
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
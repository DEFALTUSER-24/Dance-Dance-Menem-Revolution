using UnityEngine;
using UnityEngine.Localization.Components;

public class Score : MonoBehaviour
{
                        public  int                     actualScore;
    [SerializeField]    private LocalizeStringEvent     _lse;

    private void Update()
    {
        actualScore = GameManager.instance.Score().Get();
        _lse.RefreshString();
    }
}
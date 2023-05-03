using UnityEngine;

public class SetFps : MonoBehaviour
{
    [SerializeField] public int _framesPerSecond;

    private void Awake()
    {
        Application.targetFrameRate = _framesPerSecond;
    }
}
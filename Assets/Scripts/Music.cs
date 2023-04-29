using System.Collections;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource _au;

    private bool musicStarted;

    private void Start()
    {
        musicStarted = false;
        StartCoroutine(Begin());
    }

    IEnumerator Begin()
    {
        yield return new WaitForSeconds(GameManager.instance.beginLevelTime + GameManager.instance.songDelayTime);
        _au.Play();
        musicStarted = true;
    }

    public void ToggleMusic()
    {
        if (!musicStarted) return;

        if (_au.isPlaying)
            _au.Pause();
        else
            _au.Play();
    }
}
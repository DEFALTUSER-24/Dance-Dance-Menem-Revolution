using System.Collections;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource _au;

    private void Start()
    {
        StartCoroutine(Begin());
    }

    IEnumerator Begin()
    {
        yield return new WaitForSeconds(GameManager.instance.beginLevelTime + GameManager.instance.songDelayTime);
        _au.Play();
    }
}
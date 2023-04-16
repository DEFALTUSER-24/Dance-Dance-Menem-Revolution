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
        yield return new WaitForSeconds(GameManager.instance.beginLevelTime + 0.25f);
        _au.Play();
    }
}
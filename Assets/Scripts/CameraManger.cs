using System.Collections;
using UnityEngine;

public class CameraManger : MonoBehaviour
{
    [SerializeField]    private     GameObject      _actualCamera;
    [SerializeField]    private     GameObject[]    _cameras;
    [SerializeField]    private     float           _changeTimer;
                        private     int             _index      =       0;

    private void Start()
    {
        for (int i = 0; i < _cameras.Length; i++) _cameras[i].SetActive(false);

        _actualCamera = _cameras[_index];
        _actualCamera.SetActive(true);
        StartCoroutine(ChangeCorrutine());
    }

    IEnumerator ChangeCorrutine()
    {
        for (int i = 0; i < _cameras.Length; i++)
        {
            yield return new WaitForSeconds(GameManager.instance.beginLevelTime/_cameras.Length);
            ChangeMainCamera(i);
        }

        ChangeMainCamera(0);

        while (true)
        {
            for (int i = 0; i < _cameras.Length; i++)
            {
                yield return new WaitForSeconds(_changeTimer);
                ChangeMainCamera(i);
            }
        }
    }

    private void ChangeMainCamera(int i)
    {
        _actualCamera.SetActive(false);
        _actualCamera = _cameras[i];
        _actualCamera.SetActive(true);
    }

}
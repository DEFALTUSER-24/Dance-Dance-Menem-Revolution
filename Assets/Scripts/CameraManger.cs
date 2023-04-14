using System.Collections;
using UnityEngine;

public class CameraManger : MonoBehaviour
{
    [SerializeField]    private     GameObject      _actualCamera;
    [SerializeField]    private     GameObject[]    _cameras;
    [SerializeField]    private     float           _changeTimer;
                        private     int             _index = 0;

    private void Start()
    {
        for (int i = 0; i < _cameras.Length; i++) _cameras[i].SetActive(false);

        _actualCamera = _cameras[_index];
        _actualCamera.SetActive(true);
        StartCoroutine(ChangeCorrutine());
    }

    IEnumerator ChangeCorrutine()
    {
        while (true)
        {
            _index++;
            if (_index >= _cameras.Length) _index = 0;
            yield return new WaitForSeconds(_changeTimer);
            _actualCamera.SetActive(false);
            _actualCamera = _cameras[_index];
            _actualCamera.SetActive(true);
        }
    }
}
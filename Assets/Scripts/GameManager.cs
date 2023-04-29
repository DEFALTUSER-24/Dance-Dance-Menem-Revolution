using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Objects")]
    [SerializeField] private GameObject         _arrow;
    [SerializeField] private Canvas             _canvas;
    [SerializeField] private Canvas             _arrowCanvas;
    [SerializeField] private RectTransform      _field;

    [Header("Music")]
    public Music musicManager;

    [Header("Queues & Lists")]
    private Queue<ArrowKey> keys = new Queue<ArrowKey>();
    private Queue<Arrow> arrows = new Queue<Arrow>();
    private Queue<float> seconds = new Queue<float>();
    private List<float> eventTimes = new List<float>();
    private List<Arrow> eventArrows = new List<Arrow>();

    [Header("Currents")]
    public Arrow currentArrow = null;
    public float currentTimeEvent = 0;

    [Header("Time Resources")]
    public  float   beginLevelTime;
    public  float   delayTime;
    public  float   songDelayTime;
    private float   _startTimer;
    public  float   currentTimer;
    private int     _CurrentEvent;

    [Header("Player")]
    [SerializeField] private Menem menem;
    private GameScore score;

    private int destroyedKeys = 0;
    public bool gamePaused = false;
    //[SerializeField] private int currentLevel;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        score = new GameScore();
        score.Reset();

        gamePaused = false;
        Time.timeScale = 1;
    }

    private void Start()
    {
        _startTimer = Time.deltaTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI.instance.TogglePauseMenu();
            ToggleGamePause();
        }

        if (gamePaused) return;

        currentTimer = Time.time - _startTimer;

        if (arrows.Count == 0 && currentArrow == null) return;

        if (currentArrow == null)
        {
            currentArrow = arrows.Dequeue();
            currentTimeEvent = seconds.Dequeue();
        }
        else
        {
            if (_CurrentEvent < eventTimes.Count)
            {
                float e = eventTimes[_CurrentEvent];

                if (currentTimer >= e && _CurrentEvent < eventArrows.Count)
                {
                    eventArrows[_CurrentEvent].gameObject.SetActive(true);
                    _CurrentEvent++;
                }
            }
        }
    }

    internal void AddKeyEvent(float time, ArrowKey key)
    {
        eventTimes.Add(time + Time.time - delayTime);
        seconds.Enqueue(time + Time.time);
        keys.Enqueue(key);
    }

    public void CreateKeys()
    {
        int index = keys.Count;

        for (int i = 0; i < index; i++)
        {
            ArrowKey key = keys.Dequeue();
            Arrow arrowScript = Arrow.CreateArrow(_arrow, _arrowCanvas.transform, key);
            eventArrows.Add(arrowScript);
            arrows.Enqueue(arrowScript);
        }
    }

    public Vector3 GetField()
    {
        return _field.localPosition;
    }

    public GameScore Score()
    {
        return score;
    }

    public void DestroyCurrentKey(bool wasGood, bool wasPressed)
    {
        if (!currentArrow.isActiveAndEnabled) return;

        currentArrow.DestroyMe(wasGood, wasPressed);
        currentTimeEvent = 0;
        currentArrow = null;

        destroyedKeys++;

        if (destroyedKeys == eventArrows.Count) StartCoroutine(Finish());
    }

    private IEnumerator Finish()
    {
        menem.StopDancing();
        yield return new WaitForSeconds(2.5f);
        UI.instance.ShowGameOverMenu();
    }

    public void ToggleGamePause()
    {
        musicManager.ToggleMusic();
        Time.timeScale = gamePaused ? 1 : 0;
        gamePaused = !gamePaused;
    }
}
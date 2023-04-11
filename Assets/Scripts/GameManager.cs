using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Objects")]
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject field;
    [SerializeField] private Canvas canvas;

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
    public float delayTime;
    private float _startTimer;
    public float currentTimer;
    private int _CurrentEvent;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        _startTimer = Time.deltaTime;

        for (int i = 0; i < keys.Count; i++)
        {
            ArrowKey key = keys.Dequeue();
            Arrow arrowScript = Arrow.CreateArrow(arrow, canvas.transform, new Vector3(275, -117, 0), key);
            eventArrows.Add(arrowScript);
            arrows.Enqueue(arrowScript);
        }
        Debug.Log(seconds.Count);
        Debug.Log(arrows.Count);
        Debug.Log(keys.Count);
        Debug.Log(eventTimes.Count);
        Debug.Log(eventArrows.Count);
    }

    private void Update()
    {
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

                if (currentTimer >= e)
                {
                    eventArrows[_CurrentEvent].gameObject.SetActive(true);
                    _CurrentEvent++;
                }
            }
        }
    }

    internal void AddKeyEvent(float time, ArrowKey key)
    {
        eventTimes.Add(time - (delayTime / 2));
        seconds.Enqueue(time);
        keys.Enqueue(key);
    }

    public Vector3 GetField()
    {
        return new Vector3(-275, -117, 0);
    }
}
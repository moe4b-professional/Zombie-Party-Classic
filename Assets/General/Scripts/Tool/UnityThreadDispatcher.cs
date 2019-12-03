using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;

[DefaultExecutionOrder(ExecutionOrder)]
public class UnityThreadDispatcher : MonoBehaviour
{
    public const int ExecutionOrder = -400;

    public static UnityThreadDispatcher Instance { get; protected set; }

    public ConcurrentQueue<Action> Queue { get; protected set; }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnLoad()
    {
        Instance = Create();

        DontDestroyOnLoad(Instance.gameObject);
    }

    static UnityThreadDispatcher Create()
    {
        var gameObject = new GameObject();

        gameObject.name = Regex.Replace(nameof(UnityThreadDispatcher), "([a-z])([A-Z])", "$1 $2");

        return gameObject.AddComponent<UnityThreadDispatcher>();
    }


    protected virtual void Awake()
    {
        Queue = new ConcurrentQueue<Action>();
    }

    public static void Add(Action action)
    {
        Instance.Queue.Enqueue(action);
    }

    Action action;
    void Update()
    {
        while (!Queue.IsEmpty)
        {
            if(Queue.TryDequeue(out action))
                action();
        }
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

[DefaultExecutionOrder(ExecutionOrder)]
public class UnityThreadDispatcher : MonoBehaviour
{
    public const int ExecutionOrder = -400;

    public static UnityThreadDispatcher Instance { get; protected set; }

    public Queue<IEnumerator> Queue { get; protected set; }


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
        Queue = new Queue<IEnumerator>();
    }


    public static void Add(IEnumerator procedure)
    {
        Instance.Queue.Enqueue(procedure);
    }

    void Update()
    {
        lock (Queue)
        {
            while (Queue.Count > 0)
            {
                StartCoroutine(Queue.Dequeue());
            }
        }
    }


    void OnDestroy()
    {
        Instance = null;
    }
}
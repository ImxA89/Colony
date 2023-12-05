using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private List<Worker> _startWorkers;
    [SerializeField][Range(0f, 10f)] private float _scanDelayTime;

    private Queue<Worker> _freeWorkers;
    private List<Worker> _busyWorkers;
    private Queue<Copper> _resourses;
    private float _runtime;
    private int _copperCount;

    public event Func<List<Copper>> ResoursesScanned;

    private void Awake()
    {
        _freeWorkers = new Queue<Worker>();
        _busyWorkers = new List<Worker> ();
        _resourses = new Queue<Copper> ();
    }

    private void OnEnable()
    {
        foreach (Worker worker in _startWorkers)
        {
            worker.SpecifyBasePosition(transform.position);
            worker.ResourseDelivered += OnResourseDelivered;
            _freeWorkers.Enqueue(worker);
        }

        _startWorkers.Clear();
    }

    private void OnDisable()
    {
        foreach (Worker worker in _busyWorkers)
            worker.ResourseDelivered -= OnResourseDelivered;

        for (int i = 0; i < _freeWorkers.Count; i++)
            _freeWorkers.Dequeue().ResourseDelivered -= OnResourseDelivered;
    }

    private void Update()
    {
        _runtime += Time.deltaTime;

        if (_resourses.Count > 0 && _freeWorkers.Count > 0)
        {
            SendWorker();
        }
        else
        {
            if (_runtime >= _scanDelayTime)
                ScanResourse();
        }
    }

    private void SendWorker()
    {
        Worker worker = _freeWorkers.Dequeue();

        worker.TakeResourse(_resourses.Dequeue());
        _busyWorkers.Add(worker);
    }

    private void ScanResourse()
    {
        List<Copper> foundedResourses = ResoursesScanned();

        for (int i = 0; i < foundedResourses.Count; i++)
            _resourses.Enqueue(foundedResourses[i]);

        foundedResourses.Clear();
    }

    private void OnResourseDelivered(Worker worker)
    {
        _copperCount++;
        _freeWorkers.Enqueue(worker);
        _busyWorkers.Remove(worker);
    }
}

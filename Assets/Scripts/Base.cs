using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private List<Worker> _startWorkers;
    [SerializeField][Range(0f, 10f)] private float _scanDelayTime;

    private Scanner _scanner;
    private Queue<Worker> _freeWorkers;
    private List<Worker> _busyWorkers;
    private Queue<Copper> _resourses;
    private int _copperCount;


    private void Awake()
    {
        _freeWorkers = new Queue<Worker>();
        _busyWorkers = new List<Worker>();
        _resourses = new Queue<Copper>();

        _scanner = GetComponent<Scanner>();

        InitializeStartWorkers();
    }

    private void OnEnable()
    {
        _scanner.ResursesScaned += OnResourseScanned;
    }

    private void OnDisable()
    {
        foreach (Worker worker in _busyWorkers)
            worker.ResourseDelivered -= OnResourseDelivered;

        for (int i = 0; i < _freeWorkers.Count; i++)
            _freeWorkers.Dequeue().ResourseDelivered -= OnResourseDelivered;

        _scanner.ResursesScaned -= OnResourseScanned;
    }

    private void InitializeStartWorkers()
    {
        foreach (Worker worker in _startWorkers)
        {
            worker.SpecifyBasePosition(transform.position);
            worker.ResourseDelivered += OnResourseDelivered;
            _freeWorkers.Enqueue(worker);
        }

        _startWorkers.Clear();
    }

    private void SendWorkers()
    {
        while (_freeWorkers.Count > 0 && _resourses.Count > 0)
        {
            Worker worker = _freeWorkers.Dequeue();
            worker.TakeResourse(_resourses.Dequeue());
            _busyWorkers.Add(worker);
        }
    }

    private void OnResourseScanned(List<Copper> scannedResourses)
    {
        for (int i = 0; i < scannedResourses.Count; i++)
        {
            if (scannedResourses[i] != null)
                _resourses.Enqueue(scannedResourses[i]);
        }

        SendWorkers();
    }

    private void OnResourseDelivered(Worker worker)
    {
        _copperCount++;
        _freeWorkers.Enqueue(worker);
        _busyWorkers.Remove(worker);

        SendWorkers();
    }
}

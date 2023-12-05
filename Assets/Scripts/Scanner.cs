using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private ResourseCollector _resourseCollector;

    private int _scanDelayTime = 2;

    public event Action<List<Copper>> ResursesScaned;

    private void Start()
    {
        StartCoroutine(ScanResourse());
    }

    private IEnumerator ScanResourse()
    {
        WaitForSeconds delay = new WaitForSeconds(_scanDelayTime);
        List<Copper> scannedResourses = new List<Copper>();

        while (enabled)
        {
            scannedResourses = _resourseCollector.GiveResourses();
            ResursesScaned?.Invoke(scannedResourses);
            scannedResourses.Clear();
            yield return delay;
        }
    }
}

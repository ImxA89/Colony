using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private ResourseCollector _resourseCollector;

    private void OnEnable()
    {
        _base.ResoursesScanned += OnResourseScanned;
    }

    private void OnDisable()
    {
        _base.ResoursesScanned -= OnResourseScanned;
    }

    private List<Copper> OnResourseScanned()
    {
        List<Copper> resourses;

        return resourses = _resourseCollector.GiveResourses();
    }
}

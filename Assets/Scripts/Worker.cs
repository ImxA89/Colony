using System;
using UnityEngine;

[RequireComponent(typeof(WorkerMover))]
public class Worker : MonoBehaviour
{
    private bool _isResourseTaken;
    private WorkerMover _workerMover;
    private Vector3 _basePosition;
    private Transform _resurseForDelivery;
    private Vector3 _localPositionForTaken = new Vector3(0f, 0.5f, 0.5f);

    public event Action<Worker> ResourseDelivered;

    private void Awake()
    {
        _isResourseTaken = false;
        _workerMover = GetComponent<WorkerMover>();
        _workerMover.TakeTarget(_basePosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isResourseTaken == true && other.TryGetComponent<Base>(out Base otherBase) && CompareBases(otherBase))
            GiveResourse();

        if (_isResourseTaken == false && other.TryGetComponent<Copper>(out Copper copper) && CompareResourses(copper))
            PikeUpResourse();
    }

    public bool CompareResourses(Copper foundedResourse)
    {
        bool isOurBase = false;

        if (foundedResourse.transform == _resurseForDelivery)
            isOurBase = true;

        return isOurBase;
    }

    public bool CompareBases(Base foundedBase)
    {
        bool isOurBase = false;

        if (foundedBase.transform.position == _basePosition)
            isOurBase = true;

        return isOurBase;
    }

    public void SpecifyBasePosition(Vector3 basePosition)
    {
        _basePosition = basePosition;
    }

    public void TakeResourse(Copper copper)
    {
        _resurseForDelivery = copper.transform;
        _workerMover.TakeTarget(copper.transform.position);
        _isResourseTaken = false;
    }

    public void PikeUpResourse()
    {
        _resurseForDelivery.SetParent(transform);
        _resurseForDelivery.SetLocalPositionAndRotation(_localPositionForTaken, Quaternion.identity);
        _isResourseTaken = true;
        _resurseForDelivery.GetComponent<Collider>().enabled = false;
        _workerMover.TakeTarget(_basePosition);
    }

    public void GiveResourse()
    {
        Destroy(_resurseForDelivery.gameObject);
        _resurseForDelivery = null;
        ResourseDelivered?.Invoke(this);
        _isResourseTaken = false;
    }
}

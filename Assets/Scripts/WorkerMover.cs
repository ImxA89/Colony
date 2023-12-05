using UnityEngine;

public class WorkerMover : MonoBehaviour
{
    [SerializeField][Range(1f, 10f)] private float _speed;

    private float _minDistanse = 0.3f;
    private Vector3 _target;

    void Update()
    {
        if (Vector3.Distance(transform.position, _target) > _minDistanse)
            Move(_target);
    }

    public void TakeTarget(Vector3 target)
    {
        _target = target;
    }

    private void Move(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
    }
}

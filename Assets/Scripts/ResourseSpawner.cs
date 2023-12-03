using UnityEngine;

public class ResourseSpawner : MonoBehaviour
{
    [SerializeField] private Copper _copperPrefab;

    public Copper Spawn()
    {
        Copper copper = Instantiate(_copperPrefab,new Vector3(Random.Range(0f,99f),0,Random.Range(0f,99f)), Quaternion.identity);

        return copper;
    }
}

using System.Collections;
using UnityEngine;

public class UFOSpawner : MonoBehaviour
{
    [Range(1, 20)]
    public float minTimeToSpawnUFO;
    [Range(1, 40)]
    public float maxTimeToSpawnUFO;

    public bool startedSpawningUFO = false;

    private GameObject[] ufoIsAlive;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        ufoIsAlive = GameObject.FindGameObjectsWithTag("UFO");
        if (ufoIsAlive.Length == 0 && !startedSpawningUFO)
        {
            startedSpawningUFO = true;
            StartCoroutine(SpawnUFO());
        }
    }

    private IEnumerator SpawnUFO()
    {
        yield return new WaitForSeconds(Random.Range(minTimeToSpawnUFO, maxTimeToSpawnUFO));
        ObjectPooling.Instance.SpawnFromPool("UFO", new Vector3(0, Random.Range(6.0f, 26.0f), 0), Quaternion.identity);
        startedSpawningUFO = false;
        yield break;
    }
}

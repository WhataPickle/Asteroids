using System.Collections;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{

    public int startingAmount;

    public int increaseBy;

    public int currentAmount;

    [Range(0.0f, 10.0f)]
    public float timeBetweenWaves;

    public bool startedSpawning = false;

    private Vector3 topSpawn;
    private Vector3 rightSpawn;
    private Vector3 bottomSpawn;
    private Vector3 leftSpawn;

    private int sideToSpawn; //1 - top spawn, 2 - right spawn, 3 - bottom spawn, 4 - left spawn

    public int asteroidCount;

    GameObject[] gameObjects;

    void Start()
    {
    }


    void FixedUpdate()
    {
        while (currentAmount < startingAmount)
        {
            sideToSpawn = Random.Range(1, 5);
            switch (sideToSpawn)
            {
                case 1:
                    topSpawn = new Vector3(Random.Range(0, 30), 30, 0);
                    ObjectPooling.Instance.SpawnFromPool("BAsteroid", topSpawn, Quaternion.Euler(0,0,Random.Range(0,360)));
                    currentAmount++;
                    break;
                case 2:
                    rightSpawn = new Vector3(30, Random.Range(0, 30), 0);
                    ObjectPooling.Instance.SpawnFromPool("BAsteroid", rightSpawn, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                    currentAmount++;
                    break;
                case 3:
                    bottomSpawn = new Vector3(Random.Range(0, 30), 0, 0);
                    ObjectPooling.Instance.SpawnFromPool("BAsteroid", bottomSpawn, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                    currentAmount++;
                    break;
                case 4:
                    leftSpawn = new Vector3(0, Random.Range(0, 30), 0);
                    ObjectPooling.Instance.SpawnFromPool("BAsteroid", leftSpawn, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                    currentAmount++;
                    break;
            }
        }
        gameObjects = GameObject.FindGameObjectsWithTag("Asteroid"); //Finds all active asteroids and stores them into array
        if (gameObjects.Length == 0 && !startedSpawning)
        {
            startedSpawning = true;
            StartCoroutine(SpawnNewWave());
        }
    }

    private IEnumerator SpawnNewWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        startingAmount += increaseBy;
        currentAmount = 0;
        startedSpawning = false;
        yield break;
    }
}

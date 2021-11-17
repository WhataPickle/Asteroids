using System.Collections;
using UnityEngine;

public class UFOLogic : MonoBehaviour, IPooledObject
{
    [Range(0.0f, 1.0f)]
    public float moveSpeed;
    [Range(0, 500)]
    public int pointsAwarded;

    public int directionToMove; //If 0 - move to right, if 1 - move to left;

    public Transform enemyFirePoint;

    [Range(0.0f, 5.0f)]
    public float minSec;
    [Range(0.0f, 10.0f)]
    public float maxSec;

    private float shootingTime;

    public bool shooting;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void OnObjectSpawn()
    {
        directionToMove = Random.Range(0, 2);
        shooting = false;
        StopAllCoroutines();
    }

    void FixedUpdate()
    {
        if (directionToMove == 0)
        {
            transform.position += Vector3.right * moveSpeed;
        }
        else
        {
            transform.position += Vector3.left * moveSpeed;
        }
        if (transform.position.x > 30)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < 0)
        {
            transform.position = new Vector3(30, transform.position.y, transform.position.z);
        }
        if (!shooting)
        {
            StartCoroutine(StartFire());
        }
    }

    //Collision logic
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PBullet")
        {
            audioManager.Play("MExp");
            gameObject.SetActive(false); //Disables UFO if collided with player bullet
            GameObject.FindGameObjectWithTag("GameController").GetComponent<UiBehaviour>().currentScore += pointsAwarded;
        }
        if (other.tag == "Player")
        {
            gameObject.SetActive(false); //Disables UFO if collided with Player
        }
        if (other.tag == "Asteroid")
        {
            gameObject.SetActive(false); //Disables UFO if collided with Asteroid
        }
    }



    public IEnumerator StartFire()
    {
        shooting = true;
        ObjectPooling.Instance.SpawnFromPool("EBullet", enemyFirePoint.position, enemyFirePoint.rotation);
        audioManager.Play("Fire");
        shootingTime = Random.Range(minSec, maxSec);
        yield return new WaitForSeconds(shootingTime);
        shooting = false;
        yield break;
    }
}

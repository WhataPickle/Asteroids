using UnityEngine;

public class AsteroidLogic : MonoBehaviour, IPooledObject
{
    [Range(0.0f, 1.0f)]
    public float travelSpeed;
    [Range(0, 500)]
    public int pointsAwarded;

    public bool isBig;
    public bool isMedium;
    [Range(0,5)]
    public int mediumSplinters;
    public bool isSmall;
    [Range(0,5)]
    public int smallSplinters;

    [Range(0, 90)]
    public int spreadAngle;

    private float minAngle;
    private float maxAngle;
    private float randomAngle; //One of these should be true

    private float initialSpeed;
    private Vector3 currentPos;
    private Quaternion rot;

    private AudioManager audioManager;

    void Awake()
    {
        initialSpeed = travelSpeed;
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void OnObjectSpawn()
    {
        travelSpeed = Random.Range(initialSpeed * 0.5f, initialSpeed * 1.5f);
        currentPos = transform.position;
        rot = transform.rotation;
    }

    void FixedUpdate()
    {
        currentPos = transform.position;
        Vector3 velocity = new Vector3(0, travelSpeed, 0);
        currentPos += rot * velocity;
        transform.position = currentPos;
    }

    //Collision logic
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PBullet")
        {
            AsteroidSplit(); //Disables Asteroid if collided with player bullet
            GameObject.FindGameObjectWithTag("GameController").GetComponent<UiBehaviour>().currentScore += pointsAwarded;
        }
        if (other.tag == "Player")
        {
            AsteroidSplit(); //Disables Asteroid if collided with Player
        }
        if (other.tag == "UFO")
        {
            AsteroidSplit(); //Disables Asteroid if collided with UFO
        }
    }

    private void AsteroidSplit()
    {
        if (isBig)
        {
            audioManager.Play("LExp");
            for (int a = 0; a < mediumSplinters; a++)
            {
                SetNewAngle();
                ObjectPooling.Instance.SpawnFromPool("MAsteroid", transform.position, transform.rotation);
            }
            gameObject.SetActive(false);
        }
        else if (isMedium)
        {
            audioManager.Play("MExp");
            for (int b = 0; b < smallSplinters; b++)
            {
                SetNewAngle();
                ObjectPooling.Instance.SpawnFromPool("SAsteroid", transform.position, transform.rotation);
            }
            gameObject.SetActive(false);
        }
        else if (isSmall)
        {
            audioManager.Play("SExp");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("isBig, isMedium, isSmall booleans was not specified!");
        }
    }

    private void SetNewAngle()
    {
        minAngle = spreadAngle / 2 * -1;
        maxAngle = spreadAngle / 2;
        randomAngle = Random.Range(minAngle, maxAngle);
        transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + randomAngle);
    }
}

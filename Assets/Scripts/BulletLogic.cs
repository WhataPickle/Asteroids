using UnityEngine;

public class BulletLogic : MonoBehaviour, IPooledObject
{
    [Range(0.0f, 45.0f)]
    public float maxTravelDistance;
    [Range(0.0f, 1.0f)]
    public float travelSpeed;

    public bool isPlayerBullet;

    private Vector3 currentPos;
    private Quaternion rot;

    public float startingPosX; //Starting X position of this GameObject
    public float startingPosY; //Starting X position of this GameObject

    private float xOffset;
    private float yOffset;

    private float xDistance;
    private float yDistance;

    private float sqrt;

    private int flipped = 0; //Determine if bullet crossed the end of the screen

    public void OnObjectSpawn()
    {
        currentPos = transform.position;
        rot = transform.rotation;

        xOffset = 0;
        yOffset = 0;
        xDistance = 0;
        yDistance = 0;
        sqrt = 0;
    }


    void FixedUpdate()
    {
        Vector3 velocity = new Vector3(0, travelSpeed, 0);
        //**Varps bullet to the other end of the screen
        if(transform.position.x > 30)
        {
            xOffset = xDistance + startingPosX;
            currentPos = new Vector3(0, transform.position.y, transform.position.z);
            flipped++;
        }
        else if(transform.position.x < 0)
        {
            xOffset = xDistance - startingPosX;
            currentPos = new Vector3(30, transform.position.y, transform.position.z);
            flipped++;
        }
        if (transform.position.y > 30)
        {
            yOffset = yDistance + startingPosY;
            currentPos = new Vector3(transform.position.x, 0, transform.position.z);
            flipped++;
        }
        else if (transform.position.y < 0)
        {
            yOffset = yDistance - startingPosY;
            currentPos = new Vector3(transform.position.x, 30, transform.position.z);
            flipped++;
        }

        currentPos += rot * velocity;
        transform.position = currentPos;

        //Calculates travelled distance and disables GameObject if it's travelled distance = maxTravelDistance

        xDistance = transform.position.x - startingPosX + xOffset;
        yDistance = transform.position.y - startingPosY + yOffset;

        sqrt = Mathf.Sqrt(Mathf.Pow(xDistance, 2) + Mathf.Pow(yDistance, 2));
        if (sqrt > maxTravelDistance || flipped > 2)
        {
            flipped = 0;
            xOffset = 0;
            yOffset = 0;
            xDistance = 0;
            yDistance = 0;
            sqrt = 0;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isPlayerBullet)
        {
            gameObject.SetActive(false);
        }
        if (other.tag == "UFO" && isPlayerBullet)
        {
            gameObject.SetActive(false);
        }
        if (other.tag == "Asteroid" && isPlayerBullet)
        {
            gameObject.SetActive(false);
        }
    }
}

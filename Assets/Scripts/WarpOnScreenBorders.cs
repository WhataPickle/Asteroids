using UnityEngine;

public class WarpOnScreenBorders : MonoBehaviour
{
    public float screenSize;

    private void Start()
    {
        Camera camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        screenSize = camera.orthographicSize * 2;
    }

    void FixedUpdate()
    {
        if (transform.position.x > screenSize)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < 0)
        {
            transform.position = new Vector3(screenSize, transform.position.y, transform.position.z);
        }
        if (transform.position.y > screenSize)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        else if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, screenSize, transform.position.z);
        }
    }
}

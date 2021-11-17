using UnityEngine;

public class UFOFirePointFollow : MonoBehaviour
{

    [Range(0,90)]
    public float spreadAngle;

    private Transform target;
    private Quaternion lookRotation;
    private Vector3 direction;

    private float minAngle;
    private float maxAngle;

    private float randomAngle;

    void Start()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }        

        minAngle = spreadAngle / 2 * -1;
        maxAngle = spreadAngle / 2;
        randomAngle = Random.Range(minAngle, maxAngle);

        if (target != null)
        {
            direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            lookRotation = Quaternion.AngleAxis(angle - 90 + randomAngle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 1);
        }
    }


    void FixedUpdate()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
        if (target != null)
        {
            randomAngle = Random.Range(minAngle, maxAngle);
            direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            lookRotation = Quaternion.AngleAxis(angle - 90 + randomAngle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 1);
        }
    }
}

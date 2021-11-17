using System.Collections;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    //PlayerControl variables

    [Range(0.0f, 5.0f)]
    public float rotationSpeed;
    [Range(0.0f, 10.0f)]
    public float maxMoveSpeed;
    [Range(0.0f, 10.0f)]
    public float accelerationRate;

    public float moveSpeed;

    //Shooting variables
    public Transform playerFirePoint;
    [Range(1,40)]
    public int fireRate;
    [Range(1,20)]
    public int perSecond;

    public int currentAmountOfBullets;

    public bool shooting = false;

    //Respawn variables
    [Range(0.0f,5.0f)]
    public float invulnerabilityTime;
    public bool isRespawning = false;
    public bool isAccelerating = false;
    public UiBehaviour uiBehaviour;

    public MeshRenderer _capsule;
    public MeshRenderer _cylinder;
    public CapsuleCollider _collider;

    public AudioSource _thrust;
    public AudioManager audioManager;

    private Vector3 pos;
    private Quaternion rot;
    private Vector3 velocity;
    private Quaternion slidingRotation;
    [SerializeField]
    private float differenceInRotation;
    private float zAngle;

    void FixedUpdate()
    {
        pos = transform.position;
        rot = transform.rotation;
        zAngle = rot.eulerAngles.z;

        if (!uiBehaviour.controls)
        {
            KeyboardControls();
        }
        else
        {
            KeyboardAndMouseControls();
        }
        if (isAccelerating)
        {
            _thrust.enabled = true;
        }
        else
        {
            _thrust.enabled = false;
        }
    }

    private void Update()
    {
        if (!uiBehaviour.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !uiBehaviour.controls)
            {
                if (currentAmountOfBullets < fireRate)
                {
                    ObjectPooling.Instance.SpawnFromPool("PBullet", playerFirePoint.position, playerFirePoint.rotation);
                    currentAmountOfBullets++;
                    audioManager.Play("Fire");
                    if (!shooting)
                    {
                        StartCoroutine(FireRate());
                    }
                }
            }
            else if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) && uiBehaviour.controls)
            {
                if (currentAmountOfBullets < fireRate)
                {
                    ObjectPooling.Instance.SpawnFromPool("PBullet", playerFirePoint.position, playerFirePoint.rotation);
                    currentAmountOfBullets++;
                    audioManager.Play("Fire");
                    if (!shooting)
                    {
                        StartCoroutine(FireRate());
                    }
                }
            }
        }
    }

    //Collision logic
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EBullet")
        {
            audioManager.Play("SExp");
            isAccelerating = false;
            _thrust.enabled = false;
            Respawn(); //Disables Player if collided with enemy bullet
        }
        if (other.tag == "UFO")
        {
            audioManager.Play("MExp");
            isAccelerating = false;
            _thrust.enabled = false;
            Respawn(); //Disables Player if collided with UFO
        }
        if (other.tag == "Asteroid")
        {
            isAccelerating = false;
            _thrust.enabled = false;
            Respawn(); //Disables Player if collided with Asteroid
        }
    }

    public void Respawn()
    {
        if (uiBehaviour.currentLives > 0)
        {
            gameObject.transform.position = new Vector3(Random.Range(0, 30), Random.Range(0, 30), 0);
            StartCoroutine(RespawnInvulnerability());
            uiBehaviour.currentLives--;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void KeyboardControls()
    {
        //Keyboard rotation
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            zAngle += rotationSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            zAngle -= rotationSpeed;
        }
        rot = Quaternion.Euler(0, 0, zAngle);
        transform.rotation = rot;

        //Keyboard acceleration
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            isAccelerating = true;
            differenceInRotation = Quaternion.Angle(slidingRotation, rot);
            if (differenceInRotation > 90 || differenceInRotation < -90)
            {
                if (moveSpeed > 0)
                {
                    moveSpeed -= accelerationRate * Time.deltaTime * 2;
                    velocity = new Vector3(0, moveSpeed * Time.deltaTime, 0);
                    slidingRotation = Quaternion.Lerp(slidingRotation, transform.rotation, 0.6f * Time.deltaTime);
                    pos += slidingRotation * velocity;
                }
                else if (moveSpeed <= 0)
                {
                    slidingRotation = rot;
                }
            }
            else
            {
                if (moveSpeed < maxMoveSpeed)
                {
                    moveSpeed += accelerationRate * Time.deltaTime;
                    velocity = new Vector3(0, moveSpeed * Time.deltaTime, 0);
                    slidingRotation = Quaternion.Lerp(slidingRotation, transform.rotation, 1.5f * Time.deltaTime);
                    pos += slidingRotation * velocity;
                }
                else
                {
                    moveSpeed = maxMoveSpeed;
                    slidingRotation = Quaternion.Lerp(slidingRotation, transform.rotation, 1.5f * Time.deltaTime);
                    pos += slidingRotation * velocity;
                }
            }
        }
        else //Slide
        {
            isAccelerating = false;
            velocity = new Vector3(0, moveSpeed * Time.deltaTime, 0);
            pos += slidingRotation * velocity;
        }
        transform.position = pos;
    }

    public void KeyboardAndMouseControls()
    {
        //Mouse rotation
        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion mouseRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, mouseRotation, rotationSpeed * Time.deltaTime);

        //Mouse acceleration
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetMouseButton(1))
        {
            isAccelerating = true;
            differenceInRotation = Quaternion.Angle(slidingRotation, rot);
            if (differenceInRotation > 90 || differenceInRotation <-90)
            {
                if (moveSpeed > 0)
                {
                    moveSpeed -= accelerationRate * Time.deltaTime;
                    velocity = new Vector3(0, moveSpeed * Time.deltaTime, 0);
                    slidingRotation = Quaternion.Lerp(slidingRotation, transform.rotation, 0.6f * Time.deltaTime);
                    pos += slidingRotation * velocity;
                }
                else if(moveSpeed <= 0) 
                {
                    slidingRotation = rot;
                }
            }
            else
            {
                if (moveSpeed < maxMoveSpeed)
                {
                    moveSpeed += accelerationRate * Time.deltaTime;
                    velocity = new Vector3(0, moveSpeed * Time.deltaTime, 0);
                    slidingRotation = Quaternion.Lerp(slidingRotation, transform.rotation, 1.5f * Time.deltaTime);
                    pos += slidingRotation * velocity;
                }
                else
                {
                    moveSpeed = maxMoveSpeed;
                    slidingRotation = Quaternion.Lerp(slidingRotation, transform.rotation, 1.5f * Time.deltaTime);
                    pos += slidingRotation * velocity;
                }
            }
        }
        else //Slide
        {
            isAccelerating = false;
            velocity = new Vector3(0, moveSpeed * Time.deltaTime, 0);
            pos += slidingRotation * velocity;
        }
        transform.position = pos;
    }

    public IEnumerator FireRate()
    {
        shooting = true;
        yield return new WaitForSeconds(perSecond);
        shooting = false;
        currentAmountOfBullets = 0;
        yield break;
    }

    private IEnumerator RespawnInvulnerability()
    {
        isRespawning = true;
        _collider.enabled = false;
        StartCoroutine(RespawnFlashing());
        yield return new WaitForSeconds(invulnerabilityTime);
        StopCoroutine(RespawnFlashing());
        isRespawning = false;
        _capsule.enabled = true;
        _cylinder.enabled = true;
        _collider.enabled = true;
    }

    private IEnumerator RespawnFlashing()
    {
        while (isRespawning)
        {
            yield return new WaitForSeconds(0.5f);
            _capsule.enabled = false;
            _cylinder.enabled = false;
            yield return new WaitForSeconds(0.5f);
            _capsule.enabled = true;
            _cylinder.enabled = true;
        }
    }
}

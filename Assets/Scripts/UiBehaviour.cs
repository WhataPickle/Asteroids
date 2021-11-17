using UnityEngine;
using UnityEngine.UI;

public class UiBehaviour : MonoBehaviour
{
    public bool isStarted = false;
    public bool isPaused = true;

    public bool controls = false; //if false - keyboard controls; if true - keyboard + mouse controls

    public GameObject player;
    public GameObject gameController;

    public GameObject mainMenu;
    public GameObject gameplayUi;

    public Button _continue;
    public Toggle toggleControls;
    public Text toggleLabel;

    //Score and life counters

    public int currentScore;
    [Range(0, 10)]
    public int maxLives;
    public int currentLives;

    public Text scoreTxt;
    public Text livesTxt;

    private void Awake()
    {
        currentLives = maxLives;
    }

    private void Update()
    {
        if (isStarted)
        {
            scoreTxt.text = currentScore.ToString();
            livesTxt.text = currentLives.ToString();
            if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
            {
                isPaused = true;
                Time.timeScale = 0;
                mainMenu.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
            {
                isPaused = false;
                mainMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void NewGame()
    {
        if (!isStarted) //First game start
        {
            isStarted = true;
            isPaused = false;
            _continue.interactable = true;
            player.SetActive(true);
            gameController.GetComponent<AsteroidSpawner>().enabled = true;
            gameController.GetComponent<UFOSpawner>().enabled = true;
            mainMenu.SetActive(false);
            gameplayUi.SetActive(true);
        }
        else //Game restart
        {
            isPaused = false;
            player.SetActive(true);
            player.transform.position = new Vector3(15, 15, 0);
            //Finding all bullets, asteroids and UFOs and disabling them
            GameObject[] eBullets = GameObject.FindGameObjectsWithTag("EBullet");
            foreach(GameObject ebul in eBullets)
            {
                ebul.SetActive(false);
            }
            GameObject[] pBullets = GameObject.FindGameObjectsWithTag("PBullet");
            foreach (GameObject pbul in pBullets)
            {
                pbul.SetActive(false);
            }
            GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
            foreach (GameObject ast in asteroids)
            {
                ast.SetActive(false);
            }
            GameObject[] ufos = GameObject.FindGameObjectsWithTag("UFO");
            foreach (GameObject ufo in ufos)
            {
                ufo.SetActive(false);
            }
            //Resetting player variables
            PlayerControls pC = player.GetComponent<PlayerControls>();
            pC.moveSpeed = 0;
            pC.currentAmountOfBullets = 0;
            pC.shooting = false;
            pC.isRespawning = false;
            pC._capsule.enabled = true;
            pC._cylinder.enabled = true;
            pC._collider.enabled = true;
            pC.StopAllCoroutines();
            //Resetting UFO variables
            UFOSpawner ufoSpawner = gameController.GetComponent<UFOSpawner>();
            ufoSpawner.startedSpawningUFO = false;
            ufoSpawner.StopAllCoroutines();
            //Resetting Asteroid variables
            AsteroidSpawner astS = gameController.GetComponent<AsteroidSpawner>();
            astS.currentAmount = 0;
            astS.startingAmount = 0;
            astS.startedSpawning = false;
            astS.StopAllCoroutines();
            //Resetting life and score count
            currentScore = 0;
            scoreTxt.text = currentScore.ToString();
            livesTxt.text = maxLives.ToString();
            currentLives = maxLives;
            //Resuming the game
            mainMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Continue()
    {
        isPaused = false;
        mainMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ControlSchemes()
    {
        controls = toggleControls.isOn;
        ChangeLabel();
    }

    public void ChangeLabel()
    {
        if (!controls)
        {
            toggleLabel.text = "Keyboard";
        }
        else
        {
            toggleLabel.text = "Keyboard + mouse";
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}

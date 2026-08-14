using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UI;
using Unity.VisualScripting.Dependencies.Sqlite;

public class GameManager : MonoBehaviour
{
    //TRAIN
    [Tooltip("The transform of the train itself")]
    [SerializeField] Transform trainTile;

    [Tooltip("The position where train first appears")]
    [SerializeField] Vector3 trainStartPosition = new (-0.119999997f, 1.42999995f, 6.44000006f);

    [Tooltip("Amount of train prefabs that will spawn when the game starts")]
    [SerializeField] int initialNumber_OfTrainsSpawned = 10;

    [Tooltip("Where the next train prefab should spawn")]
    Vector3 nextTrainTile_SpawnPosition;

    [Tooltip("The rotation of the next train prefab")]
    Quaternion nextTrainTileRotation;

   [SerializeField] float zPositionIncrease = 30;

    //OBSTACLES
    [SerializeField] Transform doubleSeatObstacle;
    [SerializeField] Transform singleSeatObstacle;
    [SerializeField] Transform electricCableInPuddle;
    [SerializeField] int initialNumber_OfObstacles = 4;
    float obstMinXposition;
     float obstMaxXposition;
    float zPosIncrease;
    float zPosDecrease;

    //BATTERIES
    public static float batteryLifeTimer = 45;
    [SerializeField] Transform batteryickup;
    Quaternion batteryickupRotation;
    Vector3 batteryPickupPosition;

    

    float newBatteryPosition;

    [SerializeField] TMP_Text batteryLifeText;


    //SCORE
    [SerializeField] TMP_Text scoreText;

    [PrimaryKey, AutoIncrement] int playerID {  get; set; }

    public static int scoreAmount = 0;
    int scoreAmountIncrease = 1;
    public TMP_Text highScore;
    public TMP_Text finalScore;
    static int obstaclesPassedScore = 0;

    //PHASABILITY
    [SerializeField] Transform phasabilityDevice;
    float minPhaseXposition;
    float maxPhaseXposition;

    [SerializeField] Button LossButton;


    //INVENTORY
    public static InventoryManager InventoryManager { get; private set; }
    public List<IGameManager> gameManagerList = new List<IGameManager>();

    //BROOM
    [SerializeField] Transform broom;

    //KNIVES
    [SerializeField]Transform knifeObject;
    float knifeSpeed = 60f;
    public float timeBeforeSpawn = 20f;
  

    //PLAYER INFO
    string playerName = "Player";
   
    public static GameManager Instance;
   public bool isGameLost = false;


    [SerializeField] Button toMainMenu;
    [SerializeField] Button resumeButton;


    private void Awake()
    {
        if(Instance == null)
        {
          Instance = this;
        }

        else if(Instance != this) 
        {
            Destroy(this.gameObject);
        }

        batteryLifeTimer = 45f;
        scoreAmount = 0;
        SceneManager.sceneLoaded += NewSceneLoading;

    }

        float time = 0;
    float levelRuntime = 5;
    bool isGamePaused = false;

   [SerializeField] Transform bossObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        //Before the game starts, the next tile to spawn is the first one
        nextTrainTile_SpawnPosition = trainStartPosition;
        nextTrainTileRotation = Quaternion.identity;

        batteryPickupPosition = Vector3.zero;
        batteryickupRotation = Quaternion.identity;

        for (int i = 0; i < initialNumber_OfTrainsSpawned; i++)
        {
            if(i >= initialNumber_OfObstacles)
            {
              SpawnNextTile();
            }
        }

        batteryLifeText.text = "Battery Life: " + (int)batteryLifeTimer;
        scoreText.text = "Score :" + (int)scoreAmount;
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "MainMenu")
        {
            DontDestroyOnLoad(scoreText.gameObject);
            DontDestroyOnLoad(batteryLifeText.gameObject);
        }

        InventoryManager = GetComponent<InventoryManager>();

        gameManagerList.Add(InventoryManager);

        StartCoroutine(StartManager());
        highScore.gameObject.SetActive(false);
        finalScore.gameObject.SetActive(false);


        toMainMenu.gameObject.SetActive(false);
        toMainMenu.onClick.AddListener(ReturnToMainMenu);

        resumeButton.gameObject.SetActive(false);
        resumeButton.onClick.AddListener(ResumeGame);
    }

    void NewSceneLoading(Scene scene, LoadSceneMode m)
    {
        if(PlayerMovement.Instance != null)
        {
           PlayerMovement.Instance.transform.position = new Vector3(0, 3, 0);
        }


    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        isGameLost = true;
    }
    bool isBossDoneSpawning = false;
    // Update is called once per frame
    void Update()
    {
        Debug.LogWarning("Obstacles passed: "+obstaclesPassedScore.ToString());

        if(isGameLost == false)
        {
           if(Input.GetKeyDown(KeyCode.P))
           {
              isGamePaused = true;
           }
        }

        if(isGamePaused == true)
        {
            Time.timeScale = 0f;
            PlayerMovement.playerAudioSource.Pause();
            toMainMenu.gameObject.SetActive(true);
            resumeButton.gameObject.SetActive(true);
        }

        time = time + Time.deltaTime;
        if(time >= levelRuntime)
        {
            /*  Scene currentScene = SceneManager.GetActiveScene();
              if(currentScene.name == "Railway Level")
              {
                 SceneManager.LoadSceneAsync("SampleScene");
              }

              else if(currentScene.name == "SampleScene")
              {
                SceneManager.LoadSceneAsync("Railway Level");
              }

              time = 0;  */

              StartCoroutine(SpawnBoss()); 
 

            time = 0f;
        }


        batteryLifeTimer = batteryLifeTimer - Time.deltaTime;
        if (batteryLifeTimer <= 0)
        {

            Time.timeScale = 0f;
            isGameLost = true;

        }
        batteryLifeText.text = "Battery Life: " + (int)batteryLifeTimer;
        if (Time.timeScale == 0f && isGamePaused == false)
        {
            isGameLost = true;     
        }

        if(isGameLost == true)
        {
            GetPlayerData();
           SceneManager.LoadScene("MainMenu");
           Destroy(Instance.gameObject);
            Destroy(PlayerMovement.Instance.gameObject);
            Time.timeScale = 1f;    
        }

    }

    public void GetPlayerData()
    {
        if(PlayerPrefs.HasKey("SavedHighScore"))
        {
            if(scoreAmount > PlayerPrefs.GetInt("SavedHighScore"))
            {
                PlayerPrefs.SetInt("SavedHighScore", scoreAmount);
            }
        }

        else
        {
                PlayerPrefs.SetInt("SavedHighScore", scoreAmount);
        }
        PlayerPrefs.SetString("Final Score", scoreAmount.ToString());
        finalScore.text = "Your Score: "+scoreAmount.ToString();
        highScore.text = "High Score "+PlayerPrefs.GetInt("SavedHighScore").ToString();
        PlayerPrefs.Save(); 
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        PlayerMovement.playerAudioSource.Play();
        toMainMenu.gameObject.SetActive (false);
        isGamePaused = false;
        resumeButton.gameObject.SetActive (false);
    }

    public void SpawnNextTile()
    {      

       Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Railway Level")
        {
            Quaternion railwayRotation = Quaternion.Euler(0, 90, 0);
            nextTrainTile_SpawnPosition.y = 0;
            Transform newTrainTile = Instantiate(trainTile, nextTrainTile_SpawnPosition, railwayRotation);
        Transform nextTrainTile = newTrainTile.Find("Next Spawn Position");
        
        Vector3 newPosition = nextTrainTile.position;
        newPosition.z = newPosition.z + zPositionIncrease;
        
        nextTrainTile_SpawnPosition = newPosition;
        nextTrainTileRotation = nextTrainTile.rotation;

        SpawnPickups(newTrainTile);
            SpawnPuddles(newTrainTile);
            SpawnBrooms(newTrainTile);
        }

        else if (currentScene.name ==  "SampleScene")
        {
            Transform newTrainTile = Instantiate(trainTile, nextTrainTile_SpawnPosition, nextTrainTileRotation);
        Transform nextTrainTile = newTrainTile.Find("Next Spawn Position");
        
        Vector3 newPosition = nextTrainTile.position;
        newPosition.z = newPosition.z + zPositionIncrease;
        
        nextTrainTile_SpawnPosition = newPosition;
        nextTrainTileRotation = nextTrainTile.rotation;

        SpawnObstacles(newTrainTile);
        SpawnPuddles(newTrainTile);
        SpawnPickups(newTrainTile);
        SpawnPhasabilityDevice(newTrainTile);
        SpawnBrooms(newTrainTile);

        }



    }



    void SpawnPickups(Transform newTrainTile)
    {

        List<GameObject> batterySpawnPoints = new List<GameObject>();
        foreach (Transform child in newTrainTile)
        {
            if (child.gameObject.CompareTag("Battery"))
            {
                batterySpawnPoints.Add(child.gameObject);
            }
        }

        if(batterySpawnPoints.Count > 0)
        {
            for (int i = 0; i < batterySpawnPoints.Count; i++)
            {
                GameObject batterySpawnPositionObject = batterySpawnPoints[i];

                float randomizedXpos = Random.Range(-7.5f, 4.8f);
                Vector3 batterySpawnPosition = new(randomizedXpos, batterySpawnPositionObject.transform.position.y, batterySpawnPositionObject.transform.position.z);

                batterySpawnPositionObject.transform.position = batterySpawnPosition;
                Vector3 spawnPosition = batterySpawnPositionObject.transform.position;
                Scene currentScene = SceneManager.GetActiveScene();
                if (currentScene.name == "Railway Level")
                {
                    spawnPosition.y = 1.84f;
                }

                else
                {
                    spawnPosition.y = batterySpawnPositionObject.transform.position.y;
                }
               Transform newBatteryObject = Instantiate(batteryickup, spawnPosition, Quaternion.identity);
               newBatteryObject.SetParent(batterySpawnPositionObject.transform);
            }

        }

    }


    void SpawnPhasabilityDevice(Transform newTrainTile)
    {
        List<GameObject> phasabilityPickupsSpawns = new List<GameObject>();
        foreach(Transform child in newTrainTile)
        {
            if(child.gameObject.CompareTag("Phasability Device"))
            {
                phasabilityPickupsSpawns.Add(child.gameObject);
            }
        }

        if(phasabilityPickupsSpawns.Count > 0)
        {
            for(int i = 0; i<phasabilityPickupsSpawns.Count; i++)
            {
                GameObject phasabilityDeviceObject = phasabilityPickupsSpawns[i];

                minPhaseXposition = -5.33f;
                maxPhaseXposition = 4.73f;
                float randomizedXposition = Random.Range(minPhaseXposition, maxPhaseXposition);

                Vector3 phasabilityDevice_SpawnPos = new(randomizedXposition, phasabilityDeviceObject.transform.position.y, phasabilityDeviceObject.transform.position.z);
                phasabilityDeviceObject.transform.position = phasabilityDevice_SpawnPos;
                Vector3 phaseSpawnPos = phasabilityDeviceObject.transform.position;

                Transform newPhasabilityDevice = Instantiate(phasabilityDevice, phaseSpawnPos, Quaternion.identity);
                newPhasabilityDevice.SetParent(phasabilityDeviceObject.transform);
            }
        }
    }

    void SpawnObstacles(Transform newTrainTileObject)
    {
        //Store the doubleSeatObstacle game objects
        List<GameObject> doubleSeatSpawnPoints = new List<GameObject>();

        foreach(Transform childObject in newTrainTileObject) //Go through all the child objects of the train object
        {
            if(childObject.gameObject.CompareTag("Double Seat")) //If any of them have the tag, then...
            {
                //Put that object into the list
              doubleSeatSpawnPoints.Add(childObject.gameObject);
            }
        }

        if(doubleSeatSpawnPoints.Count > 0)
        {
            /* Variable to be used to choose which game object under the train object in the 
             * initialized list will be used to spawn the doubleSeatObstacle. In this case, the empty game objects */
           // int randomizedSpawnPoint = 0;
           for(int i = 0; i<doubleSeatSpawnPoints.Count; i++)
            {
            //The game object at the randomly chosen index will be assigned to a GameObject variable
            GameObject spawnPositionObject = doubleSeatSpawnPoints[i];

                obstMinXposition = -5.345429f;//-7f;
                obstMaxXposition = -0.73f;//0.60f;
            float obstXPosition = Random.Range(obstMinXposition, obstMaxXposition);

            Vector3 spawnPosObjectPosition = new(obstXPosition, spawnPositionObject.transform.position.y, spawnPositionObject.transform.position.z);
            spawnPositionObject.transform.position = spawnPosObjectPosition; 

            //That variable's position will be used to determine where the doubleSeatObstacle spawns
            Vector3 spawnPosition = spawnPositionObject.transform.position;


                //Spawn the doubleSeatObstacle
                Transform newObstacleObject = Instantiate(doubleSeatObstacle, spawnPosition, Quaternion.identity);

            newObstacleObject.SetParent(spawnPositionObject.transform);
            }
            


            {
                List<GameObject> singleSeatSpawnPoints = new List<GameObject>();
                foreach(Transform childObject in newTrainTileObject)
                {
                    if(childObject.CompareTag("Single Seat"))
                    {
                       singleSeatSpawnPoints.Add(childObject.gameObject);
                    }
                }

                if(singleSeatSpawnPoints.Count > 0)
                {
                    for(int i = 0; i < singleSeatSpawnPoints.Count; i++)
                    {
                        GameObject spawnPositionObject1 = singleSeatSpawnPoints[i];

                        obstMaxXposition = 6f;
                        obstMinXposition = -4f;
                        float obstXPosition1 = Random.Range(obstMinXposition, obstMaxXposition);

                        Vector3 spawnPosObjectPosition1 = new(obstXPosition1, spawnPositionObject1.transform.position.y, spawnPositionObject1.transform.position.z);
                        spawnPositionObject1.transform.position = spawnPosObjectPosition1;
                        Vector3 spawnPosition = spawnPositionObject1.transform.position;
                        

                        Transform newObstacleObject1 = Instantiate(singleSeatObstacle, spawnPosition, Quaternion.identity);
                        newObstacleObject1.SetParent(spawnPositionObject1.transform);
                    }
                }


            }
        } 
    }
    void SpawnPuddles(Transform newTrainTile)
        {
            List<GameObject> electricCableSpawnPoints = new List<GameObject>();
            foreach(Transform child in newTrainTile)
            {
                if(child.gameObject.CompareTag("Electric Cable"))
                {
                    electricCableSpawnPoints.Add(child.gameObject);   
                }
            }

            if(electricCableSpawnPoints.Count > 0)
            {

            for(int i = 0; i<electricCableSpawnPoints.Count; i++)
            {
                GameObject cableSpawnPositionObject = electricCableSpawnPoints[i];
                
                obstMinXposition = -4.56f;//10f;
                obstMaxXposition = 5.05f;
                float puddleXposition = Random.Range(obstMinXposition, obstMaxXposition);
                Vector3 cableSpawnPosition = new(puddleXposition, cableSpawnPositionObject.transform.position.y, cableSpawnPositionObject.transform.position.z);

                cableSpawnPositionObject.transform.position = cableSpawnPosition;
                Vector3 spawnPosition = cableSpawnPositionObject.transform.position;

                float cableYRotation = Random.Range(0, 180);
                Quaternion cableRotation = Quaternion.Euler(Quaternion.identity.x, cableYRotation, Quaternion.identity.z);

                Transform newCableObject = Instantiate(electricCableInPuddle, spawnPosition, cableRotation);
                newCableObject.SetParent(cableSpawnPositionObject.transform);
            }

            }
        }

    void SpawnBrooms(Transform newTrainTile)
    {
        foreach (Transform child in newTrainTile)
        {
            if(child.gameObject.CompareTag("Broom"))
            {
                GameObject broomSpawnObject = child.gameObject;
                float minBroomXpos = -5f;
                float maxBroomXpos = 5f;

                float broomXpos = Random.Range(minBroomXpos, maxBroomXpos);
                Vector3 broomSpawnPosition = new(broomXpos, broomSpawnObject.transform.position.y, broomSpawnObject.transform.position.z);

                broomSpawnObject.transform.position = broomSpawnPosition;
                Vector3 broomSpawnPoint = broomSpawnObject.transform.position;

                Transform newBroomObject = Instantiate(broom, broomSpawnPoint , Quaternion.identity);
                newBroomObject.SetParent(broomSpawnObject.transform);

            }
        }
    }

   /* void SpawnKnives(Transform newTrainTile)
    {
        float bossTimer = 20f;
        Debug.Log("KNIVES!!");
           List<GameObject> knivesSpawnPoints = new List<GameObject>();
        foreach(Transform child in newTrainTile)
        {
 
            if (child.gameObject.CompareTag("Boss Spawn"))
            {
                knivesSpawnPoints.Add(child.gameObject);
            }

            if(knivesSpawnPoints.Count > 0)
            {
                Debug.Log("Knife list initialized");
            for (int i = 0; i < knivesSpawnPoints.Count; i++) 
            {
                GameObject knifeSpawnObject = knivesSpawnPoints[i];

                    float minKnifeXpos = -4f;
                    float maxKnifeXpos = 4f;
                    float knifeXpos = Random.Range(minKnifeXpos, maxKnifeXpos);
                Vector3 knifeSpawnPosition = new Vector3(knifeXpos, knifeSpawnObject.transform.position.y, knifeSpawnObject.transform.position.z);
                    knifeSpawnObject.transform.position = knifeSpawnPosition;
                    Vector3 knifeSpawnPos = knifeSpawnObject.transform.position;

                Quaternion knifeRotation = Quaternion.Euler(90, 0, 0);
                Transform newestKnife = Instantiate(knifeObject, knifeSpawnPos, knifeRotation);
                    if (newestKnife)
                    {
                        Debug.LogWarning("No Knife");
                    }

                newestKnife.SetParent(knifeSpawnObject.transform);
                Rigidbody knifeRB = newestKnife.GetComponent<Rigidbody>();
                knifeRB.linearVelocity = knifeSpawnObject.transform.forward * knifeSpeed;
                Destroy(knifeRB, 4);
            }
            }
        }
    }  */

   public void RestartGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void ShowScore()
    {
        scoreAmount = scoreAmount + scoreAmountIncrease;
        scoreText.text = "Score: " + (int)scoreAmount;
    }

    IEnumerator StartManager()
    {
        foreach(IGameManager gameManager in gameManagerList)
        {
            gameManager.DoBeforeGameStart();
        }
        yield return null;
    }


    public static void IncreaseObstaclesPassedScore()
    {
        obstaclesPassedScore = obstaclesPassedScore + 1;
    }

    IEnumerator SpawnBoss()
    {
        Vector3 spawnPosition =  PlayerMovement.Instance.gameObject.transform.position;
        spawnPosition.y = 1;
        spawnPosition.x = Random.Range(-7.48f, 1.17f);
        spawnPosition.z = PlayerMovement.Instance.gameObject.transform.position.z + 5f;
        Transform newBoss = Instantiate(bossObject, spawnPosition, Quaternion.identity);
        //  newBoss.SetParent(PlayerMovement.Instance.gameObject.transform);
        yield return new WaitForSeconds(1);
    }



}

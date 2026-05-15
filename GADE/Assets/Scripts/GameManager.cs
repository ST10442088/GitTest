using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UI;

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
    public float batteryLifeTimer = 5;
    [SerializeField] Transform batteryickup;
    Quaternion batteryickupRotation;
    Vector3 batteryPickupPosition;

    float minNewBatteryPosition = 30;
    float maxNewBatteryPosition = 50;
    float newBatteryPosition;

    [SerializeField] TMP_Text batteryLifeText;


    //SCORE
    [SerializeField] TMP_Text scoreText;
    public float scoreAmount = 0;
    float scoreAmountIncrease = 1;
    public static GameManager gameManagerObject {  get; private set; }

    //PHASABILITY
    [SerializeField] Transform phasabilityDevice;

    [SerializeField] Button LossButton;


    //INVENTORY
    public static InventoryManager InventoryManager { get; private set; }
    public List<IGameManager> gameManagerList = new List<IGameManager>();



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

        InventoryManager = GetComponent<InventoryManager>();

        gameManagerList.Add(InventoryManager);

        StartCoroutine(StartManager());
    }

    // Update is called once per frame
    void Update()
    {
        batteryLifeTimer = batteryLifeTimer - Time.deltaTime;
        if (batteryLifeTimer <= 0)
        {

            Time.timeScale = 0f;

        }
        batteryLifeText.text = "Battery Life: " + (int)batteryLifeTimer;
        if (Time.timeScale == 0f)
        {
          LossButton.gameObject.SetActive(true);
        }

    }

    public void SpawnNextTile()
    {
        Transform newTrainTile = Instantiate(trainTile, nextTrainTile_SpawnPosition, nextTrainTileRotation);
        Transform nextTrainTile = newTrainTile.Find("Next Spawn Position");
        
        Vector3 newPosition = nextTrainTile.position;
        newPosition.z = newPosition.z + zPositionIncrease;//25F;//10;

        nextTrainTile_SpawnPosition = newPosition;//nextTrainTile.position;
        nextTrainTileRotation = nextTrainTile.rotation;

        SpawnObstacles(newTrainTile);
        SpawnElectricCables(newTrainTile);
        SpawnPickups(newTrainTile);
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
            Vector3 batterySpawnPosition = batterySpawnPositionObject.transform.position;

            float newBatteryXpos = Random.Range(-7, 7);
            batteryPickupPosition.x = newBatteryXpos;
            batterySpawnPosition.x = newBatteryXpos;
            batterySpawnPosition.y = -2;

            Transform newBatteryObject = Instantiate(batteryickup, batterySpawnPosition, Quaternion.identity);
            newBatteryObject.SetParent(batterySpawnPositionObject.transform);
            }

        }


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
                Vector3 phasabilityDevice_SpawnPos = phasabilityDeviceObject.transform.position;
                float randomizedXposition = Random.Range(-4.5f, 4.5f);
                phasabilityDevice_SpawnPos.x = randomizedXposition;
                Transform newPhasabilityDevice = Instantiate(phasabilityDevice, phasabilityDevice_SpawnPos, Quaternion.identity);
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

            obstMinXposition = -7f;
            obstMaxXposition = 0.60f;
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
              /*  spawnPositionObject1.transform.position = spawnPosObjectPosition1; 
                
                Vector3 spawnPosition1 = spawnPositionObject1.transform.position;*/

                Transform newObstacleObject1 = Instantiate(singleSeatObstacle, spawnPosObjectPosition1, Quaternion.identity);
                newObstacleObject1.SetParent(spawnPositionObject1.transform);
                    }
                }


            }
        } 
    }
    void SpawnElectricCables(Transform newTrainTile)
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
                /**/
                obstMinXposition = -4f;//10f;
                obstMaxXposition = 4.8f;
                float puddleXposition = Random.Range(obstMinXposition, obstMaxXposition);
                Vector3 cableSpawnPosition = new(puddleXposition, cableSpawnPositionObject.transform.position.y, cableSpawnPositionObject.transform.position.z);//cableSpawnPositionObject.transform.position;
                float cableYRotation = Random.Range(0, 180);
                Quaternion cableRotation = Quaternion.Euler(Quaternion.identity.x, cableYRotation, Quaternion.identity.z);
                Transform newCableObject = Instantiate(electricCableInPuddle, cableSpawnPosition, cableRotation);
                newCableObject.SetParent(cableSpawnPositionObject.transform);
            }
              //  int randomizedSpawnPoints = Random.Range(0, electricCableSpawnPoints.Count);

            }
        }

   public void RestartGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        batteryLifeTimer = 5;
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

}

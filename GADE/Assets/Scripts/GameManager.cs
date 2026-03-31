using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

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

    //OBSTACLES
    [SerializeField] Transform doubleSeatObstacle;
    [SerializeField] Transform singleSeatObstacle;
    [SerializeField] Transform electricCableInPuddle;
    [SerializeField] int initialNumber_OfObstacles = 4;

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
    float scoreAmount = 0;
    float scoreAmountIncrease = 5;

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
    }

    // Update is called once per frame
    void Update()
    {
        batteryLifeTimer = batteryLifeTimer - Time.deltaTime;
        if (batteryLifeTimer <= 0)
        {
            Time.timeScale = 0f;
            RestartGame();
            
        }
        batteryLifeText.text = "Battery Life: " + (int)batteryLifeTimer;

    }

    public void SpawnNextTile()
    {
        Transform newTrainTile = Instantiate(trainTile, nextTrainTile_SpawnPosition, nextTrainTileRotation);
        Transform nextTrainTile = newTrainTile.Find("Next Spawn Position");

        nextTrainTile_SpawnPosition = nextTrainTile.position;
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
            int randomizedSpawnPoints = Random.Range(0, batterySpawnPoints.Count);
            GameObject batterySpawnPositionObject = batterySpawnPoints[randomizedSpawnPoints];
            Vector3 batterySpawnPosition = batterySpawnPositionObject.transform.position;

            float newBatteryXpos = Random.Range(-7, 7);
            batteryPickupPosition.x = newBatteryXpos;
            batterySpawnPosition.x = newBatteryXpos;
            batterySpawnPosition.y = -2;

            Transform newBatteryObject = Instantiate(batteryickup, batterySpawnPosition, Quaternion.identity);
            newBatteryObject.SetParent(batterySpawnPositionObject.transform);
        }
    }


    void SpawnObstacles(Transform newTrainTileObject)
    {
        //Store the doubleSeatObstacle game objects
        List<GameObject> obstacleSpawnPoints = new List<GameObject>();

        foreach(Transform childObject in newTrainTileObject) //Go through all the child objects of the train object
        {
            if(childObject.gameObject.CompareTag("ObstacleSpawn")) //If any of them have the tag, then...
            {
                //Put that object into the list
              obstacleSpawnPoints.Add(childObject.gameObject);
            }
        }

        if(obstacleSpawnPoints.Count > 0)
        {
            /* Variable to be used to choose which game object under the train object in the 
             * initialized list will be used to spawn the doubleSeatObstacle. In this case, the empty game objects */
            int randomizedSpawnPoint = Random.Range(0, obstacleSpawnPoints.Count);

            
            //The game object at the randomly chosen index will be assigned to a GameObject variable
            GameObject spawnPositionObject = obstacleSpawnPoints[randomizedSpawnPoint];

            //That variable's position will be used to determine where the doubleSeatObstacle spawns
            Vector3 spawnPosition = spawnPositionObject.transform.position; 

            //Spawn the doubleSeatObstacle
            Transform newObstacleObject = Instantiate(doubleSeatObstacle, spawnPosition, Quaternion.identity);

            newObstacleObject.SetParent(spawnPositionObject.transform);

            {
                int randomizedSpawnPoint1 = Random.Range(0, obstacleSpawnPoints.Count);
                if (randomizedSpawnPoint1 == randomizedSpawnPoint)
                {
                    randomizedSpawnPoint1 = Random.Range(0, obstacleSpawnPoints.Count);
                }

                GameObject spawnPositionObject1 = obstacleSpawnPoints[randomizedSpawnPoint1];
                Vector3 spawnPosition1 = spawnPositionObject1.transform.position;

                Transform newObstacleObject1 = Instantiate(singleSeatObstacle, spawnPosition1, Quaternion.identity);
                newObstacleObject1.SetParent(spawnPositionObject1.transform);
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
                int randomizedSpawnPoints = Random.Range(0, electricCableSpawnPoints.Count);
                GameObject cableSpawnPositionObject = electricCableSpawnPoints[randomizedSpawnPoints];
                Vector3 cableSpawnPosition = cableSpawnPositionObject.transform.position;
                float cableYRotation = Random.Range(0, 180);
                Quaternion cableRotation = Quaternion.Euler(Quaternion.identity.x, cableYRotation, Quaternion.identity.z);
                Transform newCableObject = Instantiate(electricCableInPuddle, cableSpawnPosition, cableRotation);
                newCableObject.SetParent(cableSpawnPositionObject.transform);
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
}

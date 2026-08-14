using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float forwardMovement;
    [SerializeField] float forwardMovementSpeed = 25f;


    float swerveMovement;
    [SerializeField] float swerveSpeed = 10f;

    Rigidbody playerRB;
    bool isPlayerJumping;
    float jumpForce = 12f;

    [SerializeField] LayerMask GroundLayer;
    float playerDistanceToGround = 0.5f;

    int batteryLifeDecrease = 5;

    BoxCollider playerBoxColl;

    Renderer playerRenderer;

   public static PlayerMovement Instance;

  public static  AudioSource playerAudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else if (Instance != this)
        {
            Destroy(this.gameObject);
            if(playerAudioSource != null)
            {
              playerAudioSource.Stop();
            }

        }

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerBoxColl = GetComponent<BoxCollider>();
        playerRenderer = this.gameObject.GetComponent<Renderer>();
        playerAudioSource = GetComponent<AudioSource>();
        if (playerAudioSource != null )
        {
          playerAudioSource.Play();
        }

    }

   public IEnumerator PhaseThrough()
    {
        Color playerCarColor = playerRenderer.material.color;
        playerCarColor.a = 0.5f;
        playerRenderer.material.color = playerCarColor;

        float phaseDuration = 10;
        Physics.IgnoreLayerCollision(7, 8, true); 
        yield return new WaitForSeconds(phaseDuration);

        playerCarColor.a = 1f;
        playerRenderer.material.color = playerCarColor;
        Physics.IgnoreLayerCollision(7, 8, false);
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerJumping = Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Space);
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Railway Level")
        {
            forwardMovementSpeed = 10f;
        }


    }

    private void FixedUpdate()
    {
        swerveMovement = Input.GetAxis("Horizontal") * swerveSpeed;
       Vector3 playerMovement = new(swerveMovement * Time.deltaTime, 0, forwardMovementSpeed * Time.deltaTime);

        playerRB.MovePosition(this.transform.position + playerMovement);
        if (isPlayerJumping && IsPlayerGrounded() )
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if(IsPlayerGrounded() && !isPlayerJumping )
        {
            if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
              forwardMovementSpeed = 14f;
              Vector3 playerReverse = new(swerveMovement * Time.deltaTime, 0, -forwardMovementSpeed * Time.deltaTime);
              playerRB.MovePosition(this.transform.position + playerReverse);
            }

        }

        isPlayerJumping = false;
    }

    public bool IsPlayerGrounded()
    {

        Vector3 boxCenter = new Vector3( playerBoxColl.bounds.center.x, playerBoxColl.bounds.min.y, playerBoxColl.bounds.center.z);
        Vector3 halfExtents = new Vector3(playerBoxColl.size.x * 0.9f, 0, playerBoxColl.size.z * 0.9f);
        bool playerGrounded = Physics.CheckBox(boxCenter, halfExtents, Quaternion.identity, GroundLayer, QueryTriggerInteraction.Ignore);
        return playerGrounded;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Obstacle Collider"))
        {
            GameManager.batteryLifeTimer = GameManager.batteryLifeTimer - batteryLifeDecrease;
        }

    }


   /* GameObject SpawnObj()
    {
        Vector3 spawnPosition = this.transform.position;
        spawnPosition.z = this.transform.position.z - 5;

        GameObject newObj = Instantiate(cube, spawnPosition, Quaternion.identity);
       
        return newObj;
    }
    */
}

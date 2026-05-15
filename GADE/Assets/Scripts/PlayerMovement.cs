using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float forwardMovement;
    [SerializeField] float forwardMovementSpeed = 10f;


    float swerveMovement;
    [SerializeField] float swerveSpeed = 5f;

    Rigidbody playerRB;
    bool isPlayerJumping;
    float jumpForce = 6f;

    [SerializeField] LayerMask GroundLayer;
    float playerDistanceToGround = 0.5f;

    int batteryLifeDecrease = 5;

    BoxCollider playerBoxColl;
    [SerializeField] BoxCollider puddleBoxCollider;
    [SerializeField] LayerMask Player;
    [SerializeField] LayerMask Obstacles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerBoxColl = GetComponent<BoxCollider>();
    }

     public void PhaseThrough()
    {

        float phaseDuration = 7;
        float timer = 0;
            Renderer renderer = GetComponent<Renderer>();
            Color color = renderer.material.color;
      

        while(phaseDuration > timer)
        {
            Physics.IgnoreCollision(playerBoxColl, puddleBoxCollider, true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Obstacles"), true);
            color.a = 0.5f;
            timer = timer + Time.deltaTime;
        }

        if(timer >= phaseDuration)
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Obstacles"), false);
            Physics.IgnoreCollision(puddleBoxCollider, puddleBoxCollider, false);
            color.a = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerJumping = Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Space);

    }

    private void FixedUpdate()
    {
        swerveMovement = Input.GetAxis("Horizontal") * swerveSpeed;
        forwardMovementSpeed = 10f;
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
        // Vector3 boxBottom = new Vector3(playerBoxColl.bounds.center.x, playerBoxColl.bounds.min.y, playerBoxColl.bounds.center.z);

        Vector3 boxCenter = new Vector3( playerBoxColl.bounds.center.x, playerBoxColl.bounds.min.y, playerBoxColl.bounds.center.z);
        Vector3 halfExtents = new Vector3(playerBoxColl.size.x * 0.9f, 0.10f, playerBoxColl.size.z * 0.9f);
        bool playerGrounded = Physics.CheckBox(boxCenter, halfExtents, Quaternion.identity, GroundLayer, QueryTriggerInteraction.Ignore);
        return playerGrounded;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Obstacle Collider"))
        { 
            GameManager gameManagerObject = GameObject.FindFirstObjectByType<GameManager>();
            gameManagerObject.batteryLifeTimer = gameManagerObject.batteryLifeTimer - batteryLifeDecrease;
        }

    }

   
}

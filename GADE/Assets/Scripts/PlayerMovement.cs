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

    CapsuleCollider playerCapsColl;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerCapsColl = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerJumping = Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Space);

    }

    private void FixedUpdate()
    {
        swerveMovement = Input.GetAxis("Horizontal") * swerveSpeed;
       // this.transform.Translate(swerveMovement * Time.deltaTime, 0, forwardMovementSpeed * Time.deltaTime);
       Vector3 playerMovement = new(swerveMovement * Time.deltaTime, 0, forwardMovementSpeed * Time.deltaTime);
        playerRB.MovePosition(this.transform.position + playerMovement);

        if (isPlayerJumping && IsPlayerGrounded() )
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        isPlayerJumping = false;
    }

    public bool IsPlayerGrounded()
    {
        Vector3 capsuleBottom = new Vector3(playerCapsColl.bounds.center.x, playerCapsColl.bounds.min.y, playerCapsColl.bounds.center.z);
        bool playerGrounded = Physics.CheckCapsule(playerCapsColl.bounds.center, capsuleBottom, playerDistanceToGround, GroundLayer, QueryTriggerInteraction.Ignore);
        return playerGrounded;
    }
}

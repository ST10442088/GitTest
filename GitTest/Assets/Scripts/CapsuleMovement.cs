using UnityEngine;

public class CapsuleMovement : MonoBehaviour
{
    float forwardMovement;
    [SerializeField] float forwardMovementSpeed = 10f;


    float swerveMovement;
    [SerializeField] float swerveSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        swerveMovement = Input.GetAxis("Horizontal") * swerveSpeed;
        forwardMovement = Input.GetAxis("Vertical") * forwardMovementSpeed;

        this.transform.Translate(swerveMovement * Time.deltaTime, 0, forwardMovement * Time.deltaTime);
    }
}

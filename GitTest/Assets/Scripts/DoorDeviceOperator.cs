using UnityEngine;

public class DoorDeviceOperator : MonoBehaviour
{
    float radius = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            Collider[] nearbyHitColliders = Physics.OverlapSphere(transform.position, radius);
            foreach(Collider hitCollider in nearbyHitColliders)
            {
                hitCollider.SendMessage("OperateDoorDevice", SendMessageOptions.DontRequireReceiver);
            }
                
        }
    }
}

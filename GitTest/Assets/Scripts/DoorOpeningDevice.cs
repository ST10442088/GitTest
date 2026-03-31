using UnityEngine;

public class DoorOpeningDevice : MonoBehaviour
{
    bool isDoorOpen;
    [SerializeField] Vector3 doorPositionShift;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OperateDoorDevice()
    {
        if (isDoorOpen == true)
        {
            Vector3 doorPosition = transform.position - doorPositionShift;
            this.transform.position = doorPosition;

        }

        else
        {
            Vector3 doorPosition = transform.position + doorPositionShift;
            this.transform.position = doorPosition;
        }

        isDoorOpen = false;
    }
}

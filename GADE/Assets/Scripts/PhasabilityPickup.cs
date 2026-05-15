using UnityEngine;

public class PhasabilityPickup : MonoBehaviour
{
    [SerializeField] string powerupName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.InventoryManager.AddItems(powerupName);
            Destroy(this.gameObject);
        }
    }
}

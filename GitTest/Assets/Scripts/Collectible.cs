using UnityEngine;

public class Collectible : MonoBehaviour
{
   [SerializeField] string collectibleType;
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
        if (other.gameObject.name == "Capsule")
        {
        GameManagers.Inventory.AddItemsToInventoryList(collectibleType);
        Destroy(this.gameObject);
        }
      
    }
}

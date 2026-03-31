using UnityEngine;

public class TileEndBehaviour : MonoBehaviour
{
    float timeBeforeTileDestruction = 1.5f;
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
        if(other.gameObject.GetComponent<PlayerMovement>() == true)
        {
            //Get the first object that has the "GameManager" script attached to it
            GameManager gameManagerObject = GameObject.FindFirstObjectByType<GameManager>();
            
            //Call the spawn function of the script to spawn more train tiles after the end of the previous train has been reached
            gameManagerObject.SpawnNextTile();
            gameManagerObject.ShowScore();
            Destroy(transform.parent.gameObject, timeBeforeTileDestruction);
        }
    }
}

using UnityEngine;

public class Batteries : MonoBehaviour
{
    GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.CompareTag("Player"))
        if(GameManager.batteryLifeTimer > 0)
        {
              GameManager.batteryLifeTimer = 45;
              Debug.Log("Battery life refilled!");
              Destroy(this.gameObject);
        }


    }
}

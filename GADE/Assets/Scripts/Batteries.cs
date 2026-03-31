using UnityEngine;

public class Batteries : MonoBehaviour
{
    GameManager gameManager;
    float batteryLifetime = 35f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
     /*  batteryLifetime = batteryLifetime - Time.deltaTime;
        if (batteryLifetime <= 0)
        {
            Destroy(this.gameObject);
        } */
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.name == "Capsule")
        if(gameManager.batteryLifeTimer > 0)
        {
              gameManager.batteryLifeTimer = 45;
              Debug.Log("Battery life refilled!");
        }

       Destroy(this.gameObject);
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class PuddleDestruction : MonoBehaviour
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
        if(other.gameObject.name == "Capsule")
        {
            Time.timeScale = 0f;
            gameManager.RestartGame();
        }
    }
}

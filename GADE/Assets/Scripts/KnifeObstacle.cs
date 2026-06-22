using UnityEngine;

public class KnifeObstacle : MonoBehaviour
{
    [SerializeField]Transform knifeObject;
    float knifeSpeed = 40f;
    BoxCollider bossCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossCollider = GetComponent<BoxCollider>();
        bossCollider.enabled = false;
      
    }

    // Update is called once per frame
    void Update()
    {
        GameManager gameManagerObject = GameObject.FindFirstObjectByType<GameManager>();
        if (gameManagerObject.timeBeforeSpawn <= 0)
        {
            bossCollider.enabled=true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            if(other.gameObject.CompareTag("Player"))
            {
                Debug.LogWarning("Collided With Player");
                SpawnKnives();
               // gameManagerObject.batteryLifeTimer = gameManagerObject.batteryLifeTimer - 5;
            }
        
    }

    void SpawnKnives()
    {

        float minKnifeXpos = -4f;
        float maxKnifeXpos = 4f;
        float knifeXpos = Random.Range(minKnifeXpos, maxKnifeXpos);

        Quaternion knifeRotation = Quaternion.Euler(90, 0, 0);
        Transform newestKnife = Instantiate(knifeObject, this.transform.position, knifeRotation);
        Rigidbody knifeRB = newestKnife.GetComponent<Rigidbody>();
        knifeRB.linearVelocity = this.transform.forward * knifeSpeed;
        newestKnife.SetParent(this.transform);  
        Destroy(knifeRB, 4);
    }
}

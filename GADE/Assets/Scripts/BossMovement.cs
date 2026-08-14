using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this.gameObject.transform.Translate(0, 0, 12.8f * Time.deltaTime);
        StartCoroutine(MoveBoss());
    }

    IEnumerator MoveBoss()
    {
        /*  if (this.gameObject.transform.position.x > -5 && this.gameObject.transform.position.x < 0.6)
          {
              this.gameObject.transform.Translate(5 * Time.deltaTime, 0, 12.8f * Time.deltaTime);
          }

          if(this.gameObject.transform.position.x > 0.6 && this.gameObject.transform.position.x< 5.1)
          {
              this.gameObject.transform.Translate(-5 * Time.deltaTime, 0, 12.8f * Time.deltaTime);
          } */
        this.gameObject.transform.Translate(5 * Time.deltaTime, 0, 12.8f * Time.deltaTime);

        yield return new WaitForSeconds(1.5f);
               this.gameObject.transform.Translate(-5 * Time.deltaTime, 0, 12.8f * Time.deltaTime); 
       /* if (this.gameObject.transform.position.x > -5 && this.gameObject.transform.position.x < 0.6)
        {
            this.gameObject.transform.Translate(5 * Time.deltaTime, 0, 12.8f * Time.deltaTime);
        }

        if(this.gameObject.transform.position.x > 0.6 && this.gameObject.transform.position.x< 5.1)
        {
            this.gameObject.transform.Translate(-5 * Time.deltaTime, 0, 12.8f * Time.deltaTime);
        } */
        
    }
}

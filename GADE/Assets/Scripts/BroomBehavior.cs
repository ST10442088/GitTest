using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
public class BroomBehavior : MonoBehaviour
{
    Transform broomFollowTarget;
    bool hasPlayerCollected = false;

  [SerializeField]  Vector3 broomPositionOffset = new Vector3(0, 0.35f, 1.5f);
    Quaternion broomRotation = Quaternion.Euler(270, 0, 0);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        broomFollowTarget = GameObject.Find("Player Car").GetComponent<Transform>();

        this.gameObject.transform.rotation = broomRotation;

    }

    // Update is called once per frame

    private void Update()
    {

    }
    void LateUpdate()
    {
        if (hasPlayerCollected)
        {
        this.transform.position = broomFollowTarget.TransformPoint(broomPositionOffset);
        this.transform.LookAt(broomFollowTarget);
            this.gameObject.transform.rotation = broomRotation;
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            hasPlayerCollected = true;
        }

        else if (hasPlayerCollected && other.gameObject.CompareTag("Puddle"))
        {
            Destroy(other.gameObject);
        }
          
    }

    






}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingSpot : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,1,0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fielder")
        {
            Debug.Log("수비 성공");
        }
    }
}

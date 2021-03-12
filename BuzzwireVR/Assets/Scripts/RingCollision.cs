using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCollision : MonoBehaviour
{
    public GameObject gameController;
    Vector3 loc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger!");
        loc = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        Debug.Log(loc);
       
    }

    private void OnTriggerExit(Collider other)
    {
        gameController.GetComponent<GameControllerScript>().doControllerDetachOperations();
        gameController.GetComponent<GameControllerScript>().triggerMistakeFeedback();
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(loc, 0.005f);
    }*/
}

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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entry into "  + other.gameObject);
        gameController.GetComponent<GameControllerScript>().doControllerReattachOperations(other.gameObject.tag);
        gameController.GetComponent<GameControllerScript>().stopMistakeFeedback();
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Collision with " + other.gameObject);

        loc = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        //Debug.Log(loc);
       
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit from " + other.gameObject);
        Debug.Log("Collider tag - " + other.gameObject.tag);
        gameController.GetComponent<GameControllerScript>().doControllerDetachOperations(other.gameObject.tag, loc);
        gameController.GetComponent<GameControllerScript>().triggerMistakeFeedback();
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(loc, 0.005f);
    }*/
}

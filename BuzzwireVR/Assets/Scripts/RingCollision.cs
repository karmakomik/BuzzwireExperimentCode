using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCollision : MonoBehaviour
{
    public GameObject gameController;
    Collider currCollider;
    Collider oldCollider;
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
        if (currCollider != null)
        {
            //oldCollider = currCollider;
            currCollider.enabled = false;
            delayEnableOldCollider(currCollider);
            currCollider = other;            
        }
        else
        {            
            currCollider = other;
        }
        Debug.Log("Entry into "  + other.gameObject);
        gameController.GetComponent<GameControllerScript>().doControllerReattachOperations(other.gameObject.tag);
        gameController.GetComponent<GameControllerScript>().stopMistakeFeedback();
    }


    public void delayEnableOldCollider(Collider collider)
    {
        StartCoroutine(delayEnableColliderCoRoutine(collider));
    }

    //Placing this here because I disable all coroutines in gamemanager during every transition
    public IEnumerator delayEnableColliderCoRoutine(Collider collider)
    {
        int seconds = 1;
        while (seconds > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            seconds--;
        }
        collider.enabled = true;
        //Debug.Log("delayResetNewTasksFlag");
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Collision with " + other.gameObject);
        loc = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        //Debug.Log(loc);       
    }

    private void OnTriggerExit(Collider other)
    {
        currCollider = null;
        Debug.Log("Exit from " + other.gameObject);
        Debug.Log("Collider tag - " + other.gameObject.tag);
        gameController.GetComponent<GameControllerScript>().doControllerDetachOperations((CapsuleCollider)other, other.gameObject.tag, loc);
        gameController.GetComponent<GameControllerScript>().triggerMistakeFeedback();
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(loc, 0.005f);
    }*/
}

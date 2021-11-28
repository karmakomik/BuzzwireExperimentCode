using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCollision : MonoBehaviour
{
    public GameObject gameController;
    Collider currCollider;
    Collider oldCollider;
    Vector3 loc;
    public GameObject startStopLight;
    int numCollidersInContact;

    // Start is called before the first frame update
    void Start()
    {
        numCollidersInContact = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Number of colliders in contact - " + numCollidersInContact);
        ++numCollidersInContact;
        //Debug.Log("Object in contact - " + other.gameObject);
        if (other.tag != "StartZone" && other.tag != "StopZone" && gameController.GetComponent<GameControllerScript>().feedbackEnabled)
        {
            /*if (currCollider != null)
            {
                //currCollider.enabled = false;
                //delayEnableOldCollider(currCollider);
                currCollider = other;
            }
            else
            {
                currCollider = other;
            }*/
            if ((gameController.GetComponent<GameControllerScript>().currLevel == 2 || gameController.GetComponent<GameControllerScript>().currLevel == 3) && other.gameObject.name == "Part6")
            {
                Debug.Log("Entry into " + other.gameObject);
                other.gameObject.transform.parent.Find("StopCylinder").GetComponent<Collider>().enabled = true;
            }

            
            gameController.GetComponent<GameControllerScript>().doControllerReattachOperations(other.gameObject.tag);
            gameController.GetComponent<GameControllerScript>().stopMistakeFeedback();
        }
        else if (other.tag == "StopZone")
        {
            gameController.GetComponent<GameControllerScript>().doControllerReattachOperations("null");
            gameController.GetComponent<GameControllerScript>().feedbackEnabled = false;
            gameController.GetComponent<GameControllerScript>().startStopRefController.SetActive(true);
            gameController.GetComponent<GameControllerScript>().startStopRefController.transform.position = gameController.GetComponent<GameControllerScript>().stopPositions[gameController.GetComponent<GameControllerScript>().currLevel - 1];
            gameController.GetComponent<GameControllerScript>().solidRightHandController.SetActive(false);
            gameController.GetComponent<GameControllerScript>().ghostRightHandController.SetActive(true);
        }
        else if (other.tag == "StartZone")
        {
            gameController.GetComponent<GameControllerScript>().doControllerReattachOperations("null");
            gameController.GetComponent<GameControllerScript>().feedbackEnabled = false;
            gameController.GetComponent<GameControllerScript>().startStopRefController.SetActive(false);            
            gameController.GetComponent<GameControllerScript>().solidRightHandController.SetActive(true);
            gameController.GetComponent<GameControllerScript>().ghostRightHandController.SetActive(false);
        }
    }


    /*public void delayEnableOldCollider(Collider collider)
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
    }*/

    private void OnTriggerStay(Collider other)
    {

        if (other.tag != "StartZone" && other.tag != "StopZone")
        {
            startStopLight.SetActive(false);
            //Debug.Log("Collision with " + other.gameObject);
            loc = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            //Debug.Log(loc);       
        }
        else if(other.tag == "StartZone")
        {
            startStopLight.SetActive(true);
            gameController.GetComponent<GameControllerScript>().stopMistakeFeedback();
            if (gameController.GetComponent<GameControllerScript>().client != null && !gameController.GetComponent<GameControllerScript>().tutorialPhase)
                gameController.GetComponent<GameControllerScript>().client.Write("M;1;;;LeftSwitchPressed;Left Switch Pressed\r\n");
        }
        else if (other.tag == "StopZone")
        {
            startStopLight.SetActive(true);
            gameController.GetComponent<GameControllerScript>().stopMistakeFeedback();
            if (gameController.GetComponent<GameControllerScript>().client != null && !gameController.GetComponent<GameControllerScript>().tutorialPhase)
                gameController.GetComponent<GameControllerScript>().client.Write("M;1;;;RightSwitchPressed;Right Switch Pressed\r\n");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        --numCollidersInContact;
        if (other.tag != "StartZone" && other.tag != "StopZone" && gameController.GetComponent<GameControllerScript>().feedbackEnabled)
        {
            startStopLight.SetActive(false);

            Debug.Log("Exit from " + other.gameObject);
            Debug.Log("Collider tag - " + other.gameObject.tag);
            if (numCollidersInContact < 1)
            {
                Debug.Log("Number of colliders in contact is less than 1. Triggering feedback");
                //currCollider = null;
                gameController.GetComponent<GameControllerScript>().doControllerDetachOperations((CapsuleCollider)other, other.gameObject.tag, loc);
                gameController.GetComponent<GameControllerScript>().triggerMistakeFeedback();
            }
        }
        else if (other.tag == "StartZone")
        {
            if(!gameController.GetComponent<GameControllerScript>().tutorialPhase) other.enabled = false;
            gameController.GetComponent<GameControllerScript>().feedbackEnabled = true;
            gameController.GetComponent<GameControllerScript>().startStopRefController.SetActive(false);
            gameController.GetComponent<GameControllerScript>().solidRightHandController.SetActive(true);
            gameController.GetComponent<GameControllerScript>().ghostRightHandController.SetActive(false);
        }
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(loc, 0.005f);
    }*/
}

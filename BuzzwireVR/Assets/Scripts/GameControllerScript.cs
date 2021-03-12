using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public GameObject hookRoot;
    public GameObject hookChildWithColliders;
    public Quaternion hookRootDefaultRot;
    public Vector3 hookRootDefaultPos;
    public GameObject righthandController;
    public GameObject ghostRightHandController;
    public GameObject solidRightHandController;
    public GameObject snapSlot;
    bool checkSnapCondition;

    [Header("Materials")]
    public Material lightOffMat;
    public Material lightOnMat;

    public GameObject[] lights;
    // Start is called before the first frame update
    void Start()
    {
        hookRootDefaultRot = hookRoot.transform.localRotation;
        hookRootDefaultPos = hookRoot.transform.localPosition;
        checkSnapCondition = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (checkSnapCondition)
        {
            if (Vector3.Distance(solidRightHandController.transform.position, ghostRightHandController.transform.position) < 0.01f)
            {
                Debug.Log("Snap!");
            }
        }
    }

    public void doControllerDetachOperations()
    {
        hookRoot.transform.SetParent(null);
        hookChildWithColliders.GetComponent<Rigidbody>().isKinematic = false;
        righthandController.SetActive(false);
        solidRightHandController.SetActive(true);
        ghostRightHandController.SetActive(true);
        snapSlot.SetActive(true);
        checkSnapCondition = true;
    }

    public void doControllerReattachOperations()
    {
        hookRoot.transform.SetParent(righthandController.transform);
        hookRoot.transform.localPosition = hookRootDefaultPos;
        hookRoot.transform.localRotation = hookRootDefaultRot;
        hookChildWithColliders.GetComponent<Rigidbody>().isKinematic = true;
        righthandController.SetActive(true);
        ghostRightHandController.SetActive(false);
        solidRightHandController.SetActive(false);
        snapSlot.SetActive(false);
        checkSnapCondition = false;
    }

    public void triggerMistakeFeedback()
    {
        foreach(GameObject light in lights)
        {
            light.GetComponent<MeshRenderer>().material = lightOnMat;
        }
    }

    public void stopMistakeFeedback()
    {
        foreach (GameObject light in lights)
        {
            light.GetComponent<MeshRenderer>().material = lightOffMat;
        }
    }
}

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
    // Start is called before the first frame update
    void Start()
    {
        hookRootDefaultRot = hookRoot.transform.localRotation;
        hookRootDefaultPos = hookRoot.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void doControllerDetachOperations()
    {
        hookRoot.transform.SetParent(null);
        hookChildWithColliders.GetComponent<Rigidbody>().isKinematic = false;
        righthandController.SetActive(false);
        solidRightHandController.SetActive(true);
        ghostRightHandController.SetActive(true);
        snapSlot.SetActive(true);
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
    }
}

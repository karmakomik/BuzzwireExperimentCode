using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public GameObject hookRoot;
    public GameObject originalHookModel;
    public GameObject hookChildWithColliders;

    public Quaternion hookRootDefaultRot;
    public Vector3 hookRootDefaultPos;
    public Quaternion solidRightHandControllerDefaultRot;
    public Vector3 solidRightHandControllerDefaultPos;

    public GameObject righthandController;
    public GameObject ghostRightHandController;
    public GameObject solidRightHandController;
    public GameObject snapSlot;
    public GameObject rightHandAnchor;
    bool checkSnapCondition;
    Vector3 detachPt;

    bool isDetached = false;
    enum Direction { xDir, yDir, zDir};
    string currDragDir;

    [Header("Materials")]
    public Material lightOffMat;
    public Material lightOnMat;

    public GameObject[] lights;
    // Start is called before the first frame update
    void Start()
    {
        hookRootDefaultRot = hookRoot.transform.localRotation;
        hookRootDefaultPos = hookRoot.transform.localPosition;
        solidRightHandControllerDefaultRot = solidRightHandController.transform.localRotation;
        solidRightHandControllerDefaultPos = solidRightHandController.transform.localPosition;

        checkSnapCondition = false;
    }

    float translateFactor = 0.001f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //print("Down");
            rightHandAnchor.transform.Translate(Vector3.down * translateFactor);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            //print("Up");
            rightHandAnchor.transform.Translate(Vector3.up * translateFactor);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //print("Left");
            rightHandAnchor.transform.Translate(Vector3.left * translateFactor);

        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            //print("Right");
            rightHandAnchor.transform.Translate(Vector3.right * translateFactor);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            //print("Minus");
            rightHandAnchor.transform.Translate(Vector3.forward * translateFactor);
            //rightHandAnchor.transform.RotateAroundLocal()
        }

        if (Input.GetKey(KeyCode.X))
        {
            //print("Minus");
            rightHandAnchor.transform.Translate(Vector3.back * translateFactor);
        }

    }

    private void FixedUpdate()
    {
        if (checkSnapCondition)
        {
            if (Vector3.Distance(solidRightHandController.transform.position, ghostRightHandController.transform.position) < 0.01f)
            {
                //Debug.Log("Snap!");
            }
        }
        if(isDetached)
        {
            //Debug.Log("isDetached = true");
            if(currDragDir == "x-axis")
            {
                solidRightHandController.transform.position = new Vector3(ghostRightHandController.transform.position.x, solidRightHandController.transform.position.y, solidRightHandController.transform.position.z);
                //solidRightHandController.transform.rotation = rotatePointAroundPivot()
                //solidRightHandController.transform.rotation = Quaternion.Euler(new Vector3(0,0,ghostRightHandController.transform.rotation.eulerAngles.z));
            }
            else if (currDragDir == "y-axis")
            {
                solidRightHandController.transform.position = new Vector3(solidRightHandController.transform.position.x, ghostRightHandController.transform.position.y, solidRightHandController.transform.position.z);

            }
            else if (currDragDir == "z-axis")
            {
                solidRightHandController.transform.position = new Vector3(solidRightHandController.transform.position.x, solidRightHandController.transform.position.y, ghostRightHandController.transform.position.z);

            }
        }
        else
        {
            //Debug.Log("isDetached = false");
        }
    }

    public void doControllerDetachOperations(string tag, Vector3 _detachPt)
    {
        Debug.Log("isDetached = true, collision with " + tag);
        isDetached = true;
        currDragDir = tag; //x-dir, y-dir or z-dir
        //hookRoot.transform.SetParent(null);
        //originalHookModel.SetActive(false);
        //hookChildWithColliders.GetComponent<Rigidbody>().isKinematic = false;
        //righthandController.SetActive(false);
        //righthandController.GetComponent<MeshRenderer>().enabled = false;
        solidRightHandController.SetActive(true);
        solidRightHandController.transform.SetParent(null);
        ghostRightHandController.SetActive(true);
        snapSlot.SetActive(true);
        checkSnapCondition = true;
        detachPt = _detachPt;
    }

    public Vector3 rotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    public void doControllerReattachOperations(string tag)
    {
        Debug.Log("isDetached = false, collision with " + tag);
        isDetached = false;
        //originalHookModel.SetActive(true);
        /*hookRoot.transform.SetParent(righthandController.transform);
        hookRoot.transform.localPosition = hookRootDefaultPos;
        hookRoot.transform.localRotation = hookRootDefaultRot;
        hookChildWithColliders.GetComponent<Rigidbody>().isKinematic = true;*/
        //righthandController.SetActive(true);
        //righthandController.GetComponent<MeshRenderer>().enabled = true;
        ghostRightHandController.SetActive(false);


        //solidRightHandController.SetActive(false);
        solidRightHandController.transform.SetParent(hookRoot.transform);
        solidRightHandController.transform.localRotation = solidRightHandControllerDefaultRot;
        solidRightHandController.transform.localPosition = solidRightHandControllerDefaultPos;

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

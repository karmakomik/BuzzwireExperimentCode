using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleTCP;

public class GameControllerScript : MonoBehaviour
{
    SimpleTcpClient client;

    public GameObject env;
    public AudioSource beepsound;
    public GameObject hookRoot;
    //public GameObject originalHookModel;
    //public GameObject hookChildWithColliders;

    public Quaternion hookRootDefaultRot;
    public Vector3 hookRootDefaultPos;
    public Quaternion solidRightHandControllerDefaultRot;
    public Vector3 solidRightHandControllerDefaultPos;

    //public GameObject righthandController;
    public GameObject ghostRightHandController;
    public GameObject solidRightHandController;
    //public GameObject snapSlot;
    public GameObject rightHandAnchor;
    //bool checkSnapCondition;
    Vector3 detachPt;
    Vector3 offsetGhostDistance;
    Vector3 oldGhostPos;
    GameObject detachPivot;

    bool isDetached = false;
    enum Direction { xDir, yDir, zDir};
    CapsuleCollider currCollider;
    string currDragDir;
    public Vector3 offsetPivotAng;

    [Header("Materials")]
    public Material lightOffMat;
    public Material lightOnMat;

    public GameObject mistakeLight;
    public GameObject startLight;
    // Start is called before the first frame update
    void Start()
    {
        beepsound.mute = true;
        //hookRootDefaultRot = hookRoot.transform.localRotation;
        //hookRootDefaultPos = hookRoot.transform.localPosition;
        solidRightHandControllerDefaultRot = solidRightHandController.transform.localRotation;
        solidRightHandControllerDefaultPos = solidRightHandController.transform.localPosition;
        detachPivot = new GameObject("DetachPivot");        
        oldGhostPos = ghostRightHandController.transform.position;
        //checkSnapCondition = false;



        client = new SimpleTcpClient().Connect("127.0.0.1", 8089);
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

    public void gotoNextLevel()
    {
        env.transform.Rotate(0, -90, 0);
    }

    public void stopFeedback()
    {
        
    }

    private void FixedUpdate()
    {
        /*if (checkSnapCondition)
        {
            if (Vector3.Distance(solidRightHandController.transform.position, ghostRightHandController.transform.position) < 0.01f)
            {
                //Debug.Log("Snap!");
            }
        }*/

        offsetGhostDistance = ghostRightHandController.transform.position - oldGhostPos;
        oldGhostPos = ghostRightHandController.transform.position;

        if (isDetached)
        {
            StartCoroutine(Haptics(1, 1, 0.1f, true, false));
            if(client!=null)
                client.Write("M;1;;;BuzzWireHit;Buzz wire was hit\r\n");
            //Debug.Log("isDetached = true");
            if (currDragDir == "x-axis")
            {
                //Vector3 projectedPos = new Vector3(ghostRightHandController.transform.position.x, solidRightHandController.transform.position.y, solidRightHandController.transform.position.z);
                //if (projectedPos.x < currCollider.bounds.max.x && projectedPos.x > currCollider.bounds.min.x)
                //    solidRightHandController.transform.position = projectedPos;
                Vector3 projectedPos = detachPivot.transform.position + new Vector3(offsetGhostDistance.x, 0, 0);
                if (projectedPos.x < currCollider.bounds.max.x && projectedPos.x > currCollider.bounds.min.x)
                    detachPivot.transform.position = projectedPos;                   

                //detachPivot.transform.eulerAngles = ghostRightHandController.transform.eulerAngles;// + offsetPivotAng;
 ;
            }
            else if (currDragDir == "y-axis")
            {
                //solidRightHandController.transform.position = new Vector3(solidRightHandController.transform.position.x, ghostRightHandController.transform.position.y, solidRightHandController.transform.position.z);
                Vector3 projectedPos = detachPivot.transform.position + new Vector3(0,offsetGhostDistance.y, 0);
                if (projectedPos.y < currCollider.bounds.max.y && projectedPos.y > currCollider.bounds.min.y)
                    detachPivot.transform.position = projectedPos;
                //detachPivot.transform.eulerAngles = ghostRightHandController.transform.eulerAngles;
            }
            else if (currDragDir == "z-axis")
            {
                //solidRightHandController.transform.position = new Vector3(solidRightHandController.transform.position.x, solidRightHandController.transform.position.y, ghostRightHandController.transform.position.z);
                Vector3 projectedPos = detachPivot.transform.position + new Vector3(0, 0, offsetGhostDistance.z);
                if (projectedPos.z < currCollider.bounds.max.z && projectedPos.z > currCollider.bounds.min.z)
                    detachPivot.transform.position = projectedPos;
            }
        }
        else
        {
            //Debug.Log("isDetached = false");
        }
    }

    public void doControllerDetachOperations(CapsuleCollider _collider, string tag, Vector3 _detachPt)
    {
        currCollider = _collider;
        Debug.Log("isDetached = true, collision with " + tag);
        isDetached = true;
        currDragDir = tag; //x-dir, y-dir or z-dir
        //hookRoot.transform.SetParent(null);
        //originalHookModel.SetActive(false);
        //hookChildWithColliders.GetComponent<Rigidbody>().isKinematic = false;
        //righthandController.SetActive(false);
        //righthandController.GetComponent<MeshRenderer>().enabled = false;

        //snapSlot.SetActive(true);
        //checkSnapCondition = true;
        detachPt = _detachPt;
        detachPivot.transform.position = detachPt;

        solidRightHandController.SetActive(true);
        solidRightHandController.transform.SetParent(detachPivot.transform);
        ghostRightHandController.SetActive(true);
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

        //snapSlot.SetActive(false);
        //checkSnapCondition = false;
    }

    public void triggerMistakeFeedback()
    {
        //StartCoroutine(Haptics(1, 1, 0.1f, true, false));
        beepsound.mute = false;
        //mistakeLight.GetComponent<MeshRenderer>().material = lightOnMat;
        mistakeLight.SetActive(true);

    }

    public void stopMistakeFeedback()
    {
        beepsound.mute = true;
        //StartCoroutine(Haptics(1, 1, 0.1f, true, false));
        /*foreach (GameObject light in lights)
        {
            light.GetComponent<MeshRenderer>().material = lightOffMat;
        }*/
        mistakeLight.SetActive(false);
    }

    IEnumerator Haptics(float frequency, float amplitude, float duration, bool rightHand, bool leftHand)
    {
        if (rightHand) OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.RTouch);
        if (leftHand) OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.LTouch);

        yield return new WaitForSeconds(duration);

        if (rightHand) OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        if (leftHand) OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
    }
}

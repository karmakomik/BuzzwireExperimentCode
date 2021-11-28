using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using SimpleTCP;
using UnityEngine.UI;

public class BuzzWireServiceScript : MonoBehaviour
{
    public AudioSource beepsound;
    public AudioSource goSound;
    public GameObject testArduinoSerialControllerObj, trainingArduinoSerialControllerObj;
    public SerialController testArduinoSerialController;
    public SerialController trainingArduinoSerialController;
    SimpleTcpClient client;
    string prevMessage;
    bool trainingPhase;

    //public GameObject testArduinoObj;
    //public GameObject trainingArduinoObj;

    public Image leftSwitchIndicator, rightSwitchIndicator, mistakeIndicator;
    public GameObject baselineOverIndicator;
    public GameObject restOverIndicator;
    public TMPro.TMP_Text modeTxt;
    public TMPro.TMP_Text iMotionsConnText;

    public string receivedstring;

    private void Awake()
    {
        testArduinoSerialController.portName = PlayerPrefs.GetString("testCOMPort", "not_set");
        Debug.Log("Test COM port set - " + testArduinoSerialController.portName);

        trainingArduinoSerialController.portName = PlayerPrefs.GetString("trainingCOMPort", "not_set");
        Debug.Log("Training COM port set - " + trainingArduinoSerialController.portName);

        testArduinoSerialControllerObj.SetActive(true);
        trainingArduinoSerialControllerObj.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        trainingPhase = false;
        beepsound.mute = true;
        //testArduinoSerialController = testArduinoObj.GetComponent<SerialController>();
        //testArduinoSerialController = trainingArduinoObj.GetComponent<SerialController>();
        leftSwitchIndicator.color = Color.gray;
        rightSwitchIndicator.color = Color.gray;
        mistakeIndicator.color = Color.gray;
        client = new SimpleTcpClient().Connect("127.0.0.1", 8089);
    }

    public void startTask()
    {
        client.Write("M;1;;;StartTask;Task has started\r\n");
    }

    public void endTask()
    {
        client.Write("M;1;;;EndTask;Task has ended\r\n");
    }

    /*public void startLevel(int level)
    {
        modeTxt.text = "Training Mode On";
        
        trainingPhase = true;
        if (level == 1)
        {
            if (client != null)
                client.Write("M;1;;;level_1_started;Level 1 started\r\n");
        }
        if (level == 2)
        {
            if (client != null)
                client.Write("M;1;;;level_2_started;Level 2 started\r\n");
        }
        if (level == 3)
        {
            if (client != null)
                client.Write("M;1;;;level_3_started;Level 3 started\r\n");
        }
        if (level == 4)
        {
            if (client != null)
                client.Write("M;1;;;level_4_started;Level 4 started\r\n");
        }
    }*/

    public void startTest(int stage)
    {
        //goSound.Play();
        if (stage == 1)
        {
            modeTxt.text = "Pre Test Active";
            trainingPhase = false;
            if (client != null)
                client.Write("M;1;;;pre_test_started;Test (pre) started\r\n");
        }
        if (stage == 2)
        {
            modeTxt.text = "Post Test Active";
            trainingPhase = false;
            if (client != null)
                client.Write("M;1;;;post_test_started;Test (post) started\r\n");
        }
    }

    public void startBaseline()
    {
        if (client != null)
            client.Write("M;1;;;baseline_started;Baseline started\r\n");

        StartCoroutine(startBaselineCounterCoroutine());
    }

    public IEnumerator startBaselineCounterCoroutine()
    {
        modeTxt.text = "Baseline Active";
        Debug.Log("Baseline started");
        int seconds = 180;
        while (seconds > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            baselineOverIndicator.GetComponentInChildren<Text>().text = "" + seconds + "s";
            seconds--;
        }
        if (client != null)
            client.Write("M;1;;;baseline_over;Baseline over\r\n");

        //baselineOverIndicator.SetActive(true);
        baselineOverIndicator.GetComponentInChildren<Text>().text = "Baseline over";
        //Debug.Log("delayResetNewTasksFlag");
    }

    public void startRest()
    {
        StartCoroutine(startRestCoroutine());
    }

    public IEnumerator startRestCoroutine()
    {
        int seconds = 30;
        while (seconds > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            restOverIndicator.GetComponentInChildren<Text>().text = "" + seconds + "s";
            seconds--;
        }
        if (client != null)
            client.Write("M;1;;;rest_over;Rest over\r\n");
        goSound.Play();
        restOverIndicator.GetComponentInChildren<Text>().text = "Rest over";
        //Debug.Log("delayResetNewTasksFlag");
    }

    /*public void stopBaseline()
    {
        if (client != null)
            client.Write("M;1;;;baseline_stopped;Baseline stopped\r\n");
    }*/

    public void startTraining()
    {
        trainingPhase = true;
    }

    public void startRest(int level)
    {
        modeTxt.text = "Level " + level + " Active";
        if (level == 1)
        {
            startRest();
            if (client != null)
            {                
                client.Write("M;1;;;level_1_started;Level 1 started\r\n");
                client.Write("M;1;;;level_1_rest_started;Level 1 rest started\r\n");
            }
        }
        if (level == 2)
        {
            startRest();
            if (client != null)
            {
                client.Write("M;1;;;level_2_started;Level 2 started\r\n");
                client.Write("M;1;;;level_2_rest_started;Level 2 rest started\r\n");
            }
        }
        if (level == 3)
        {
            startRest();
            if (client != null)
            {
                client.Write("M;1;;;level_3_started;Level 3 started\r\n");
                client.Write("M;1;;;level_3_rest_started;Level 3 rest started\r\n");
            }
        }
        if (level == 4)
        {
            startRest();
            if (client != null)
            {
                client.Write("M;1;;;level_4_started;Level 4 started\r\n");
                client.Write("M;1;;;level_4_rest_started;Level 4 rest started\r\n");
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (client == null)
        {
            iMotionsConnText.color = Color.red;
            iMotionsConnText.text = "iMotions Disconnected";
        }
        else
        {
            iMotionsConnText.color = Color.green;
            iMotionsConnText.text = "iMotions Connected";
        }
        string message;

        if (trainingPhase)
        {
            message = trainingArduinoSerialController.ReadSerialMessage();
        }
        else
        {
            message = testArduinoSerialController.ReadSerialMessage();
        }

        if (message == null)
        {
            leftSwitchIndicator.color = Color.gray;
            rightSwitchIndicator.color = Color.gray;
            mistakeIndicator.color = Color.gray;
            beepsound.mute = true;
            return;
        }

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
        {
            //Debug.Log("Message arrived: " + message);
            if (message == "1")
            {
                mistakeIndicator.color = Color.red;
                if (trainingPhase)
                {
                    beepsound.mute = false;
                    //beepsound.Play();
                    //beepsound.PlayOneShot(beepsound.GetComponent<AudioClip>());
                    StartCoroutine(Haptics(1, 1, 0.1f, true, false));
                }

                if (client != null)
                    client.Write("M;1;;;BuzzWireHit;Buzz wire was hit\r\n");
            }
            else
            {
                beepsound.mute = true;
            }

            if (message == "+")
            {
                leftSwitchIndicator.color = Color.green;
                if (client != null)
                    client.Write("M;1;;;LeftSwitchPressed;Left Switch Pressed\r\n");
            }
            else if (message == "*")
            {
                rightSwitchIndicator.color = Color.green;
                if (client != null)
                    client.Write("M;1;;;RightSwitchPressed;Right Switch Pressed\r\n");
            }
            else
            {
                leftSwitchIndicator.color = Color.gray;
                rightSwitchIndicator.color = Color.gray;
            }
            message = "";
        }
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

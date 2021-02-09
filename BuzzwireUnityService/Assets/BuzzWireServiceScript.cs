using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using SimpleTCP;

public class BuzzWireServiceScript : MonoBehaviour
{
    public AudioSource beepsound;
    public SerialController serialController;
    SimpleTcpClient client;
    string prevMessage;
    bool feedbackEnabled;

    public string receivedstring;
    // Start is called before the first frame update
    void Start()
    {
        feedbackEnabled = true;
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        client = new SimpleTcpClient().Connect("127.0.0.1", 8089);
    }

    public void toggleFeedback()
    {
        feedbackEnabled = !feedbackEnabled;
    }

    public void startTask()
    {
        client.Write("M;1;;;StartTask;Task has started\r\n");
    }

    public void endTask()
    {
        client.Write("M;1;;;EndTask;Task has ended\r\n");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.A))
        {
            //beepsound.Play();
            //StartCoroutine(Haptics(1, 1, 0.1f, true, false));
        }

        string message = serialController.ReadSerialMessage();

        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
        {
            Debug.Log("Message arrived: " + message);
            if (message == "1")
            {
                //if (feedbackEnabled)
                {
                    beepsound.mute = false;
                    //beepsound.Play();
                    //beepsound.PlayOneShot(beepsound.GetComponent<AudioClip>());
                    StartCoroutine(Haptics(1, 1, 0.1f, true, false));
                    
                }
                client.Write("M;1;;;BuzzWireHit;Buzz wire was hit\r\n");
            }
            else if (message == "0")
            {
                beepsound.mute = true;
            }
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

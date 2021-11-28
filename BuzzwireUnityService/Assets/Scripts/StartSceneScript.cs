using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StartSceneScript : MonoBehaviour
{
    public TMP_InputField testCOMPort;
    public TMP_InputField trainingCOMPort;

    // Start is called before the first frame update
    void Start()
    {
        testCOMPort.text = PlayerPrefs.GetString("testCOMPort", "not_set");
        trainingCOMPort.text = PlayerPrefs.GetString("trainingCOMPort", "not_set");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTestCOMPort()
    {
        //Debug.Log("Current com port - " + testCOMPort.text);
        PlayerPrefs.SetString("testCOMPort", testCOMPort.text);
        PlayerPrefs.Save();
    }

    public void setTrainingCOMPort()
    {
        //Debug.Log("Current com port - " + testCOMPort.text);
        PlayerPrefs.SetString("trainingCOMPort", trainingCOMPort.text);
        PlayerPrefs.Save();
    }

    public void startExperiment()
    {        
        SceneManager.LoadScene("BuzzwirePhysical");
    }

}

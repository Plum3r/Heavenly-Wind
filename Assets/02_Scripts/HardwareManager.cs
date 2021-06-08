using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Management;

public class HardwareManager : MonoBehaviour
{
    public Object VRScene;
    public Object PCScene;
    public Text ErrorText;

    public void PlayWithoutVR()
        {
        SceneManager.LoadSceneAsync(PCScene.name); // PC Menu Scene
        }

    public void PlayWithVR()
        {
        StartCoroutine(StartXR());
        }

    public IEnumerator StartXR()
        {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
            Debug.LogWarning("Initializing XR Failed.");
            ErrorText.text = "Initializing XR Failed.";
            }
        else
            {
            Debug.Log("Initialization Finished.Starting XR Subsystems...");
            //Try to start all subsystems and check if they were all successfully started ( thus HMD prepared).
            bool loaderSuccess = XRGeneralSettings.Instance.Manager.activeLoader.Start();
            if (loaderSuccess)
                {
                Debug.Log("All Subsystems Started!");
                SceneManager.LoadSceneAsync(VRScene.name); // VR Menu Scene
                }
            else
                {
                Debug.LogWarning("Starting Subsystems Failed.");
                ErrorText.text = "Starting Subsystems Failed.";
                }
            }
        }
    }

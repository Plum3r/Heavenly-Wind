using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeRun : MonoBehaviour
{
    public bool ResetSettings;

    private void Start()
        {        
        if(ResetSettings)
            {
            PlayerPrefs.SetInt("FirstRun", 0);
            }
        if (PlayerPrefs.GetInt("FirstRun") == 0)
            {
            PlayerPrefs.SetFloat("Music", 0.05f);
            PlayerPrefs.SetFloat("SFX", 0.2f);
            PlayerPrefs.SetFloat("Environement", 0.05f);           

            PlayerPrefs.SetInt("FirstRun", 1);
            }
        }
    }

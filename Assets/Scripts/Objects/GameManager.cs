using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Set target frame rate
        Application.targetFrameRate = 72;

        // Disable unnecessary features
        QualitySettings.vSyncCount = 0; // Disable VSync
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

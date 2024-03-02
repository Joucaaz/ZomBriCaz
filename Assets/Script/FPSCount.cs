using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCount : MonoBehaviour
{

    public TextMeshProUGUI textFPS;
    private float fpsAccumulator = 0f;
    private int frameCounter = 0;
    private float timeCounter = 0f;

    private void Update()
    {
        frameCounter++;
        fpsAccumulator += Time.unscaledDeltaTime;

        if (timeCounter > 1f) // Mesurer chaque seconde
        {
            int currentFps = (int)(frameCounter / fpsAccumulator);
            textFPS.text = currentFps.ToString();

            frameCounter = 0;
            fpsAccumulator = 0f;
            timeCounter = 0f;
        }

        timeCounter += Time.unscaledDeltaTime;
    }


    // private float FPS;
    // public TextMeshProUGUI textFPS;
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     InvokeRepeating("GetFPS", 1, 1);
    // }

    // void GetFPS(){
    //     FPS = (int)(1f / Time.unscaledDeltaTime);
    //     textFPS.text = FPS.ToString(); 
    // }



    // public TextMeshProUGUI textFPS;
    // private float fpsAccumulator = 0f;
    // private float fpsNextPeriod = 0f;
    // private int currentFps;

    // void Start()
    // {
    //     fpsNextPeriod = Time.realtimeSinceStartup + 1f;
    // }

    // void Update()
    // {
    //     fpsAccumulator += Time.unscaledDeltaTime;
        
    //     if (Time.realtimeSinceStartup > fpsNextPeriod)
    //     {
    //         currentFps = (int)(1f / (fpsAccumulator / 60f));
    //         fpsAccumulator = 0f;
    //         fpsNextPeriod += 1f;

    //         textFPS.text = currentFps.ToString();
    //     }
}


using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    public float slowMotionTimescale;

    private float startTimescale;
    private float startFixedDeltaTime;

    void Start()
    {
        startTimescale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
    }

    public void StartSlowMotion()
    {
        Time.timeScale = slowMotionTimescale;
        Time.fixedDeltaTime = startFixedDeltaTime * slowMotionTimescale;
    }

    public void StopSlowMotion()
    {
        Time.timeScale = startTimescale;
        Time.fixedDeltaTime = startFixedDeltaTime;
    }
}
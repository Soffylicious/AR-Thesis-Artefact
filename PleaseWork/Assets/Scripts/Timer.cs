using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public Text fpsText;
    private float startTime;
    private float deltaTime;
    private float nextActionTime = 0.0f;
    public float period = 5f;
    private bool timerStopped;
    private CubeController cubes; 

    // Update is called once per frame
    void Update()
    {
        if (timerStopped)
        {
            return;
        }
        float t = Time.time - startTime; // Time since the timer started
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        string minutes = ((int) t / 60).ToString("00");
        string seconds = (t % 60).ToString("00");
        string miliseconds = ((int) (t * 100f) % 100).ToString("00");
        timerText.text = minutes + ":" + seconds + ":" + miliseconds;
        fpsText.text = Mathf.Ceil(fps).ToString("00") + "fps";
    }

    public void StartTimer()
    {
        startTime = Time.time; //Time since the application started
    }

    public void StopTimer()
    {
        timerStopped = true;
        timerText.color = Color.yellow;
    }
    
}
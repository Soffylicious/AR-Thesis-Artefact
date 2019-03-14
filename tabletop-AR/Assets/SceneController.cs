using UnityEngine;
using GoogleARCore;

public class SceneController : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        QuitOnConnectionErrors();
    }

    // Update is called once per frame
    void Update()
    {
        // Check the ARCore tracking state. ARCore needs to capture and process enough
        // information to start tracking the user's movements. 
        // The session status must be Tracking in order to access the Frame.
        if (Session.Status != SessionStatus.Tracking)
        {
            int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            return;
        }
        // So that the screen doesnt time out if we are tracking
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    
    /**
     * Method checks if the permissions to use the camera is granted and if
     * the ARCore library can connect to the ARCore Services. 
     */
    void QuitOnConnectionErrors()
    {
        if (Session.Status ==  SessionStatus.ErrorPermissionNotGranted)
        {
            StartCoroutine(CodelabUtils.ToastAndExit(
                "Camera permission is needed to run this application.", 5));
        }
        else if (Session.Status.IsError())
        {
            // This covers a variety of errors.  See reference for details
            // https://developers.google.com/ar/reference/unity/namespace/GoogleARCore
            StartCoroutine(CodelabUtils.ToastAndExit(
                "ARCore encountered a problem connecting. Please restart the app.", 5));
        }
    }
}
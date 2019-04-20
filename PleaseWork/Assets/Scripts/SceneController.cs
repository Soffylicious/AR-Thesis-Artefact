using GoogleARCore;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public Camera firstPersonCamera;
    public CubeController myCubes;

    private bool processTouches = true;

    // Use this for initialization
    void Start()
    {
        QuitOnConnectionErrors();
    }

    // Update is called once per frame
    void Update()
    {
        // The session status must be Tracking in order to access the Frame.
        if (Session.Status != SessionStatus.Tracking)
        {
            int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            return;
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (processTouches)
        {
            ProcessTouches();
        }
    }

    void ProcessTouches()
    {
        Touch touch;
        if (Input.touchCount != 1 ||
            (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        TrackableHit hit;
        TrackableHitFlags raycastFilter =
            TrackableHitFlags.PlaneWithinBounds |
            TrackableHitFlags.PlaneWithinPolygon;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            SetSelectedPlane(hit);
        }
    }

    void SetSelectedPlane(TrackableHit hit)
    {
        Debug.Log("Selected plane centered at " + (hit.Trackable as DetectedPlane).CenterPose.position);
        myCubes.SetSelectedPlane(hit);
        processTouches = false;
    }

    void QuitOnConnectionErrors()
    {
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
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
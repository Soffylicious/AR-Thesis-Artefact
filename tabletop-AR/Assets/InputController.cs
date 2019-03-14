using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;


public class InputController : MonoBehaviour {
	private Camera m_Camera;
	public GameObject cube; 

	// Use this for initialization
	void Start ()
	{
		if (Camera.main != null)
		{
			m_Camera = Camera.main;	
		}
		else
		{
			StartCoroutine(CodelabUtils.ToastAndExit(
				"No camera found.", 5));
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var touch in Input.touches)
		{
			ScreenPositionTouched(touch.position);
			// Make a new object
			// To prevent "button-bounce" and to spawn objects at the beginning of the touch 
			if (Input.touchCount > 1 && touch.phase == TouchPhase.Began)
			{
				add3DObject();
			}
		}
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

	void add3DObject()
	{
		Instantiate(cube, transform.position + transform.forward * 0.7f, Random.rotation);
	}
	/**
	 * Where you have touched the screen.
	 * Ones we shoot a ray we want to find out if it hit an
	 * AR object.
	 * If it does hit an object it returns which objects it was. 
	 */
	private void ScreenPositionTouched(Vector2 screenPosition)
	{
			var ray = m_Camera.ScreenPointToRay(screenPosition);
			var position = new RaycastHit();
			// If we hit an object, apply force to the object
			if(Physics.Raycast(ray, out position))
			{
				//The direction of force is the direction of the ray (makes the object spin if we hit an angle)
				// The second parameter, positionInfo, is the position of the hit 
				position.rigidbody.AddForceAtPosition(ray.direction, position.point);
			}
	}
	
	
}

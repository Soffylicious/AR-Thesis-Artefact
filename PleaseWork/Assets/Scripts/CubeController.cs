using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class CubeController : MonoBehaviour
{
    private DetectedPlane detectedPlane;
    public Camera firstPersonCamera;
    private TrackableHit hit;
    private const int numberOfCubes = 6;
    private Anchor anchor;
    private const double circleRadius = 0.25;
    private bool flicker;
    private bool everyOther;
    public Material cubeMaterial;
    private bool alphaPerFrame;
    private float startAlphaTime;
    public float flickerSpeed = 10f;
    private float r = 0.6f, g = 0.6f, b = 0.6f;
    private int frames;
    
    public Text logText;
    private readonly Color presetColor = new Color(0.6f, 0.6f, 0.6f, 1);
    private const string CUBE_NAME = "Cube1";
    
    // Use this for initialization
    void Start()
    {
        
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // The tracking state must be FrameTrackingState.Tracking
        // in order to access the Frame.
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        // If there is no plane, then return
        if (detectedPlane == null)
        {
            return;
        }

        // Check for the plane being subsumed.
        // If the plane has been subsumed switch attachment to the subsuming plane.
        while (detectedPlane.SubsumedBy != null)
        {
            detectedPlane = detectedPlane.SubsumedBy;
        }

        if (flicker)
        {
            frames++;
            
            if (frames % 10 == 0) { //If the remainder of the current frame divided by 5 is 0 run the function.
                Frame10Update();
            }
        }
    }

    // in ScoreboardController.cs
    public void SetSelectedPlane(TrackableHit hit)
    {
        detectedPlane = hit.Trackable as DetectedPlane;
        this.hit = hit;
        RenderCubes();
    }

    private void RenderCubes()
    {
        var xyList = ListOfCirclePoints(numberOfCubes, circleRadius, 0, 0);
        int i = 0;
        foreach (Renderer cubePrefab in GetComponentsInChildren<Renderer>())
        {
            cubePrefab.enabled = true;
            cubePrefab.material.color = presetColor;
            cubePrefab.transform.position =
                new Vector3(xyList.ElementAt(i).Key, xyList.ElementAt(i).Value, 0);
            i++;
        }

        Pose newPose = new Pose(new Vector3(hit.Pose.position.x, 0, hit.Pose.position.z), hit.Pose.rotation);
        anchor = detectedPlane.CreateAnchor(newPose);
        // Attach the scoreboard to the anchor.
        transform.SetParent(anchor.transform);
    }

    private static List<KeyValuePair<float, float>> ListOfCirclePoints(int points, double radius, int centerX,
        int centerY)
    {
        var list = new List<KeyValuePair<float, float>>();
        double slice = 2 * Math.PI / points;
        for (int i = 0; i < points; i++)
        {
            double angle = slice * i;
            float newX = (float) (centerX + radius * Math.Cos(angle));
            float newY = (float) (centerY + radius * Math.Sin(angle));
            list.Add(new KeyValuePair<float, float>(newX, newY));
        }

        foreach (var element in list)
        {
            Debug.Log(element);
        }

        return list;
    }

    public void StopFlickering()
    {
        flicker = false;
        logText.text = "r: " + r + ", g: " + g + ", b: " + b; 
    }

    public void StartFlickering()
    {
        cubeMaterial = transform.Find(CUBE_NAME).GetComponent<Renderer>().material;
        cubeMaterial.color = presetColor;
        flicker = true;


    }
    private void SetNewColor()
    {
        r += flickerSpeed;
        g += flickerSpeed;
        b += flickerSpeed;
        cubeMaterial.color = new Color(r, g, b);
        Debug.Log("New color: " + cubeMaterial.color);
        everyOther = false;
    }
    
    private void SetOldColor()
    {
        cubeMaterial.color = presetColor;
        Debug.Log("Old color set: " + cubeMaterial.color);
        everyOther = true;
    }

    private void Frame10Update()
    {
        if (everyOther)
        {
            SetNewColor();
        } else
        {
            SetOldColor();
        }
    }
}
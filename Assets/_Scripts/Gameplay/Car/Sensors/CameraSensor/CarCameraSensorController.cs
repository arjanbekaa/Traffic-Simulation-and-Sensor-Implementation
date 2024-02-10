using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraSensorController : MonoBehaviour
{
    public List<CarCameraModel> cameras;
    private CarCameraModel currentCarCameraModelBeingDisplayed;

    void Start()
    {
        // Set up initial state
        SwitchCamera(cameras[0]); // Assuming the first camera in the list as default
    }

    void Update()
    {
        // Switch cameras with arrow keys
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchCamera(GetPreviousCamera());
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchCamera(GetNextCamera());
        }
    }

    public void SwitchCameraByType(CarCameraType carCameraType)
    {
        if (currentCarCameraModelBeingDisplayed.carCameraType == carCameraType)
        {
            return;
        }
        
        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i].carCameraType == carCameraType)
            {
                currentCarCameraModelBeingDisplayed = cameras[i];
                cameras[i].cameraInstance.enabled = true;
                MenuManager.instance.ChangeCameraText(currentCarCameraModelBeingDisplayed.cameraName);
            }
            else
            {
                cameras[i].cameraInstance.enabled = false;
            }
        }
    }
    
    void SwitchCamera(CarCameraModel newCamera)
    {
        if (currentCarCameraModelBeingDisplayed != null)
        {
            if (currentCarCameraModelBeingDisplayed == newCamera)
            {
                return;
            }
            else
            {
                currentCarCameraModelBeingDisplayed.cameraInstance.enabled = false;
            }
        }

        // Enable the selected camera
        currentCarCameraModelBeingDisplayed = newCamera;

        if (currentCarCameraModelBeingDisplayed != null)
        {
            // Set the UI Image to display the selected camera's feed
            currentCarCameraModelBeingDisplayed.cameraInstance.enabled = true;
            MenuManager.instance.ChangeCameraText(currentCarCameraModelBeingDisplayed.cameraName);
        }
        else
        {
            Debug.LogWarning("Camera not found");
        }
    }

    CarCameraModel GetNextCamera()
    {
        int currentIndex = cameras.IndexOf(currentCarCameraModelBeingDisplayed);
        int nextIndex = (currentIndex + 1) % cameras.Count;
        return cameras[nextIndex];
    }

    CarCameraModel GetPreviousCamera()
    {
        int currentIndex = cameras.IndexOf(currentCarCameraModelBeingDisplayed);
        int previousIndex = (currentIndex - 1 + cameras.Count) % cameras.Count;
        return cameras[previousIndex];
    }
}

[System.Serializable]
public enum CarCameraType
{
    Front,
    Behind,
    FrontRightSide,
    FrontLeftSide,
    BehindRightSide,
    BehindLeftSide,
    // Add more camera types as needed
}

[System.Serializable]
public class CarCameraModel
{
    public string cameraName;
    public CarCameraType carCameraType;
    public Camera cameraInstance;
}

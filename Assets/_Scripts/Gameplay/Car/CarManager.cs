using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CarManager : MonoBehaviour
{
    public CarProximitySensorController carProximitySensorController;
    public CarVoiceSensorController carVoiceSensorController;
    public CarCameraSensorController carCameraSensorController;
    public CarProximitySensorSoundController carProximitySensorSoundController;
    public CarRadarSensorController carRadarSensorController;
    public GameObject carLightsHolder;

    public void ToggleCarLights(bool status)
    {
        if(carLightsHolder != null)
            carLightsHolder.SetActive(status);
    }
}

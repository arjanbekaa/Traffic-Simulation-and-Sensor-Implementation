using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarProximitySensorController : MonoBehaviour
{
    public Action<List<ProximitySensorResult>> OnProximitySensorResultAction;

    // List of sensor models with directions, distances, and collision audio
    public List<ProximitySensorModel> sensorModels;
    public bool debugLines = false;

    private CarManager _carManager;

    private void Start()
    {
        _carManager = GetComponentInParent<CarManager>();
        for (int i = 0; i < sensorModels.Count; i++)
        {
            sensorModels[i].Init(_carManager);
        }
    }
    
    void Update()
    {
        List<ProximitySensorResult> proximitySensorResults = new List<ProximitySensorResult>();
        foreach (var sensorModel in sensorModels)
        {
            // Check for collision
            proximitySensorResults.Add(sensorModel.CheckCollision());
        }

        if (OnProximitySensorResultAction != null)
            OnProximitySensorResultAction(proximitySensorResults);
    }

    public Action<List<ProximitySensorResult>> GetOnProximitySensorResultAction()
    {
        return OnProximitySensorResultAction;
    }
    
    public void Toggle3DSensorsFeedback(bool status)
    {
        for (int i = 0; i < sensorModels.Count; i++)
        {
            sensorModels[i].feedbackSensorsHolder3D.SetActive(status);
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < sensorModels.Count; i++)
        {
            sensorModels[i].DrawGizmos();
        }
    }

    private void OnDestroy()
    {
        OnProximitySensorResultAction = null;
    }
}

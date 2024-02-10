using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if !UNITY_WEBGL
using UnityEngine.Windows.Speech;
#endif

public class CarVoiceSensorController : MonoBehaviour
{
    public List<VoiceCommandModel> voiceCommandModels = new List<VoiceCommandModel>();
    
#if !UNITY_WEBGL
    private KeywordRecognizer recognizer;
#endif

    private CarManager _carManager;

    private void Start()
    {
        _carManager = GetComponentInParent<CarManager>();

        // Check if voice recognition is enabled and the platform is not WebGL
        #if !UNITY_WEBGL
            recognizer = new KeywordRecognizer(voiceCommandModels.Select(v => v.voiceTrigger).ToArray());
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        #else
            Debug.LogWarning("Voice recognition is not supported or disabled. Voice commands will not be available.");
        #endif
    }

    #if !UNITY_WEBGL
    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string command = args.text;
        HandleVoiceCommand(command);
    }
    #endif

    private void HandleVoiceCommand(string command)
    {
        foreach (VoiceCommandModel cmdData in voiceCommandModels)
        {
            if (command.ToLower().Contains(cmdData.voiceTrigger.ToLower()))
            {
                ExecuteCommand(cmdData.command);
                break;
            }
        }
    }

    private void ExecuteCommand(VoiceCommandType command)
    {
        switch (command)
        {
            case VoiceCommandType.TurnOnTheCar:
                // Replace this with your actual code to turn on the car
                Debug.Log("Car turned on!");
                break;
            case VoiceCommandType.TurnOffTheCar:
                // Replace this with your actual code to turn off the car
                Debug.Log("Car turned off!");
                break;
            case VoiceCommandType.TurnOnLights:
                // Replace this with your actual code to turn on the lights
                _carManager.ToggleCarLights(true);
                break;
            case VoiceCommandType.TurnOffLights:
                // Replace this with your actual code to turn off the lights
                _carManager.ToggleCarLights(false);
                break;
            case VoiceCommandType.OpenWindows:
                // Replace this with your actual code to open the windows
                Debug.Log("Windows opened!");
                break;
            case VoiceCommandType.CloseWindows:
                // Replace this with your actual code to close the windows
                Debug.Log("Windows closed!");
                break;
            case VoiceCommandType.TurnOn3DFeedbackSensors:
                _carManager.carProximitySensorController.Toggle3DSensorsFeedback(true);
                break;
            case VoiceCommandType.TurnOff3DFeedbackSensors:
                _carManager.carProximitySensorController.Toggle3DSensorsFeedback(false);
                break;
            // Add more cases as needed
        }
    }

    void OnDestroy()
    {
        #if !UNITY_WEBGL
        if (recognizer != null)
            recognizer.Stop();
        #endif
    }
}

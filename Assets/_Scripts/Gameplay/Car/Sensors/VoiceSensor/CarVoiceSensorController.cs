using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;


public class CarVoiceSensorController : MonoBehaviour
{
    public List<VoiceCommandModel> voiceCommandModels = new List<VoiceCommandModel>();
    private KeywordRecognizer recognizer;

    private CarManager _carManager;

    private void Start()
    {
        _carManager = GetComponentInParent<CarManager>();
        recognizer = new KeywordRecognizer(voiceCommandModels.Select(v => v.voiceTrigger).ToArray());
        recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
        recognizer.Start();
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string command = args.text;
        HandleVoiceCommand(command);
    }

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
        recognizer.Stop();
    }
}

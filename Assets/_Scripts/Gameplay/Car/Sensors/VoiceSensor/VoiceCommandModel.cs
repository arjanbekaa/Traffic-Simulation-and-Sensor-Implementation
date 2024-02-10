[System.Serializable]
public class VoiceCommandModel
{
    public VoiceCommandType command;
    public string voiceTrigger;
}

[System.Serializable]
public enum VoiceCommandType
{
    TurnOnTheCar,
    TurnOffTheCar,
    TurnOnLights,
    TurnOffLights,
    OpenWindows,
    CloseWindows,
    TurnOn3DFeedbackSensors,
    TurnOff3DFeedbackSensors,
}

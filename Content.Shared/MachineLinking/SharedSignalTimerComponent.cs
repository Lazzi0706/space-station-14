using Robust.Shared.Serialization;

namespace Content.Shared.MachineLinking;

[Serializable, NetSerializable]
public enum SignalTimerUiKey : byte
{
    Key
}

/// <summary>
/// Represents a SignalTimerComponent state that can be sent to the client
/// </summary>
[Serializable, NetSerializable]
public sealed class SignalTimerBoundUserInterfaceState : BoundUserInterfaceState
{
    public string CurrentText;
    public string CurrentDescriptionText; //SS220-brig-timer-description
    public string CurrentDelayMinutes;
    public string CurrentDelaySeconds;
    public bool ShowText;
    public TimeSpan TriggerTime;
    public bool TimerStarted;
    public bool HasAccess;

    public SignalTimerBoundUserInterfaceState(string currentText,
        string currentDescriptionText,
        string currentDelayMinutes,
        string currentDelaySeconds,
        bool showText,
        TimeSpan triggerTime,
        bool timerStarted,
        bool hasAccess)
    {
        CurrentText = currentText;
        CurrentDescriptionText = currentDescriptionText; //SS220-brig-timer-description
        CurrentDelayMinutes = currentDelayMinutes;
        CurrentDelaySeconds = currentDelaySeconds;
        ShowText = showText;
        TriggerTime = triggerTime;
        TimerStarted = timerStarted;
        HasAccess = hasAccess;
    }
}

[Serializable, NetSerializable]
public sealed class SignalTimerTextChangedMessage : BoundUserInterfaceMessage
{
    public string Text { get; }

    public SignalTimerTextChangedMessage(string text)
    {
        Text = text;
    }
}

//SS220-brig-timer-description begin
[Serializable, NetSerializable]
public sealed class SignalTimerDescriptionTextChangedMessage : BoundUserInterfaceMessage
{
    public string Text { get; }

    public SignalTimerDescriptionTextChangedMessage(string text)
    {
        Text = text;
    }
}
//SS220-brig-timer-description end

[Serializable, NetSerializable]
public sealed class SignalTimerDelayChangedMessage : BoundUserInterfaceMessage
{
    public TimeSpan Delay { get; }
    public SignalTimerDelayChangedMessage(TimeSpan delay)
    {
        Delay = delay;
    }
}

[Serializable, NetSerializable]
public sealed class SignalTimerStartMessage : BoundUserInterfaceMessage
{

}

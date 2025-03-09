using Robust.Shared.Serialization;

namespace Content.Shared.SS220.Attachables.Events;

[Serializable, NetSerializable]
public sealed class AttachablesContainerMessage : BoundUserInterfaceMessage
{

    public AttachablesContainerMessage() { }
}

[Serializable, NetSerializable]
public enum AttachablesContainerUiKey : byte
{
    Key
}

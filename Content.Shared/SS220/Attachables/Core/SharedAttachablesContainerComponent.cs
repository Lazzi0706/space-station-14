using Content.Shared.Containers.ItemSlots;

namespace Content.Shared.SS220.Attachables.Core;

public abstract partial class SharedAttachablesContainerComponent : Component
{
    [DataField]
    public Dictionary<string, ItemSlot> Slots = new();
}

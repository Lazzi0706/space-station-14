using Content.Shared.SS220.Attachables.Gun.Container;

namespace Content.Shared.SS220.Attachables.Gun.Attachment;

[RegisterComponent]
public sealed partial class GunAttachmentComponent : Component
{
    [DataField]
    public GunBonusModifier GunBonusModifier = new();
}

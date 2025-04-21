namespace Content.Shared.SS220.Attachables.Core;

[RegisterComponent]
public abstract partial class SharedAttachablesContainerComponent : Component
{
    [DataField]
    public float AttachTime = 2.5f;

    [DataField]
    public float DeattachTime = 2.5f;
}

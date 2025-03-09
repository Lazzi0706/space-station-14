
namespace Content.Shared.SS220.Attachables;

[RegisterComponent]
public sealed partial class AttachableComponent : Component
{
    [DataField]
    public float AttachDelay = 1.5f;

    [DataField]
    public float DeattachDelay = 1.5f;

}

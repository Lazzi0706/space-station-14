using Content.Shared.Containers.ItemSlots;
using Content.Shared.SS220.Attachables.Core;
using Robust.Shared.Serialization;

namespace Content.Shared.SS220.Attachables.Gun.Container;

[RegisterComponent]
public sealed partial class GunAttachablesContainerComponent : SharedAttachablesContainerComponent
{
    [DataField]
    public Dictionary<string, ItemSlot> Slots = new();

    [ViewVariables]
    public GunBonusModifier GunBonusModifierTable = new();
}

[DataDefinition, Serializable, NetSerializable]
public sealed partial class GunBonusModifier
{
    public GunBonusModifier() { }

    [DataField, ViewVariables]
    public float AngleIncrease = 0f;

    [DataField, ViewVariables]
    public float AngleDecay = 0f;

    [DataField, ViewVariables]
    public float MaxAngle = 0f;

    [DataField, ViewVariables]
    public float MinAngle = 0f;

    [DataField, ViewVariables]
    public int ShotsPerBurst = 0;

    [DataField, ViewVariables]
    public float FireRate = 0f;

    [DataField, ViewVariables]
    public float ProjectileSpeed = 0f;

    public void ApplyModifiers(GunBonusModifier mod, bool increace)
    {
        AngleIncrease += increace ? mod.AngleDecay : -mod.AngleIncrease;
        AngleDecay += increace ? mod.AngleDecay : -mod.AngleDecay;
        MaxAngle += increace ? mod.MaxAngle : -mod.MaxAngle;
        MinAngle += increace ? mod.MinAngle : -mod.MinAngle;
        ShotsPerBurst += increace ? mod.ShotsPerBurst : -mod.ShotsPerBurst;
        FireRate += increace ? mod.FireRate : -mod.FireRate;
        ProjectileSpeed += increace ? mod.ProjectileSpeed : -mod.ProjectileSpeed;
    }
}

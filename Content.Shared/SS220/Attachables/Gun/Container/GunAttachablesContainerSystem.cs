using Content.Shared.Containers.ItemSlots;
using Content.Shared.SS220.Attachables.Core;
using Content.Shared.SS220.Attachables.Gun.Attachment;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Containers;

namespace Content.Shared.SS220.Attachables.Gun.Container;

public sealed partial class GunAttachablesContainerSystem : SharedAttachablesContainerSystem
{
    [Dependency] private readonly ItemSlotsSystem _itemSlot = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly SharedGunSystem _gun = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GunAttachablesContainerComponent, ComponentInit>(OnComponentInit);

        SubscribeLocalEvent<GunAttachablesContainerComponent, EntInsertedIntoContainerMessage>(OnItemSlotInsert);
        SubscribeLocalEvent<GunAttachablesContainerComponent, EntRemovedFromContainerMessage>(OnItemSlotEject);

        SubscribeLocalEvent<GunAttachablesContainerComponent, GunRefreshModifiersEvent>(OnGunRefreshModifiers);
    }

    public void OnComponentInit(Entity<GunAttachablesContainerComponent> ent, ref ComponentInit args)
    {
        foreach (var (id, slot) in ent.Comp.Slots)
        {
            _itemSlot.AddItemSlot(ent.Owner, id, slot);
            _container.GetContainer(ent.Owner, id).OccludesLight = false;
        }
    }
    public void OnItemSlotInsert(Entity<GunAttachablesContainerComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        if (!TryComp<GunAttachmentComponent>(args.Entity, out var modifiers))
            return;

        ent.Comp.GunBonusModifierTable.ApplyModifiers(modifiers.GunBonusModifier, true);

        _gun.RefreshModifiers(ent.Owner);
    }
    public void OnItemSlotEject(Entity<GunAttachablesContainerComponent> ent, ref EntRemovedFromContainerMessage args)
    {
        if (!TryComp<GunAttachmentComponent>(args.Entity, out var modifiers))
            return;

        ent.Comp.GunBonusModifierTable.ApplyModifiers(modifiers.GunBonusModifier, false);

        _gun.RefreshModifiers(ent.Owner);
    }

    public void OnGunRefreshModifiers(Entity<GunAttachablesContainerComponent> ent, ref GunRefreshModifiersEvent args)
    {
        if (!TryComp<GunComponent>(ent.Owner, out var initial))
            return;

        args.AngleDecay += args.AngleDecay * ent.Comp.GunBonusModifierTable.AngleDecay;
        args.AngleIncrease += args.AngleIncrease * ent.Comp.GunBonusModifierTable.AngleIncrease;
        args.FireRate += args.FireRate * ent.Comp.GunBonusModifierTable.FireRate;
        args.MaxAngle += args.MaxAngle * ent.Comp.GunBonusModifierTable.MaxAngle;
        args.MinAngle += args.MinAngle * ent.Comp.GunBonusModifierTable.MinAngle;
        args.ProjectileSpeed += args.ProjectileSpeed * ent.Comp.GunBonusModifierTable.ProjectileSpeed;
        args.ShotsPerBurst += ent.Comp.GunBonusModifierTable.ShotsPerBurst;
    }
}

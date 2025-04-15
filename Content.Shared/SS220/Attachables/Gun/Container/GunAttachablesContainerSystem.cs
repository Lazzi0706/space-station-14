using Content.Shared.Containers.ItemSlots;
using Content.Shared.SS220.Attachables.Core;
using Content.Shared.Weapons.Ranged.Events;

namespace Content.Shared.SS220.Attachables.Gun.Container;

public sealed partial class GunAttachablesContainerSystem : SharedAttachablesContainerSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GunAttachablesContainerComponent, ItemSlotInsertAttemptEvent>(OnItemSlotInsert);
        SubscribeLocalEvent<GunAttachablesContainerComponent, ItemSlotEjectAttemptEvent>(OnItemSlotEject);

        SubscribeLocalEvent<GunAttachablesContainerComponent, GunRefreshModifiersEvent>(OnGunRefreshModifiers);
    }

    public void OnItemSlotInsert(Entity<GunAttachablesContainerComponent> ent, ref ItemSlotInsertAttemptEvent args) { }
    public void OnItemSlotEject(Entity<GunAttachablesContainerComponent> ent, ref ItemSlotEjectAttemptEvent args) { }
    public void OnGunRefreshModifiers(Entity<GunAttachablesContainerComponent> ent, ref GunRefreshModifiersEvent args) { }
}

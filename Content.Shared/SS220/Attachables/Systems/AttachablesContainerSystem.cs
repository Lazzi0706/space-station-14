
using Content.Shared.Actions;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Hands;
using Content.Shared.SS220.Attachables.Events;
using Content.Shared.Toggleable;
using Robust.Shared.Containers;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Shared.SS220.Attachables.Systems;

public sealed partial class AttachablesContainerSystem : EntitySystem
{
    [Dependency] private readonly ItemSlotsSystem _itemSlotsSystem = default!;
    [Dependency] private readonly SharedContainerSystem _sharedContainerSystem = default!;
    [Dependency] private readonly ActionContainerSystem _actionContainerSystem = default!;

    [Dependency] private readonly SharedUserInterfaceSystem _sharedUserInterfaceSystem = default!;
    [Dependency] private readonly SharedActionsSystem _sharedActionsSystem = default!;
    
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AttachablesContainerComponent, MapInitEvent>(OnMapInit);

        SubscribeLocalEvent<AttachablesContainerComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<AttachablesContainerComponent, ComponentRemove>(OnComponentRemove);

        SubscribeLocalEvent<AttachablesContainerComponent, GotEquippedHandEvent>(GotEquipped);
        SubscribeLocalEvent<AttachablesContainerComponent, AttachablesContainerOpenEvent>(OnToggleAction);

        SubscribeLocalEvent<AttachablesContainerComponent, EntInsertedIntoContainerMessage>(OnEntInserted);
        SubscribeLocalEvent<AttachablesContainerComponent, EntRemovedFromContainerMessage>(OnEntRemoved);

    }

    #region Main component events

    public void OnMapInit(EntityUid uid, AttachablesContainerComponent component, MapInitEvent args)
    {
        _actionContainerSystem.EnsureAction(uid, ref component.ToggleActionEntity, component.ToggleAction);
    }

    public void OnComponentInit(EntityUid uid, AttachablesContainerComponent component, ComponentInit args)
    {
        foreach (var (id, slot) in component.AllowedSlots)
        {
            _itemSlotsSystem.AddItemSlot(uid, id, slot);
            // _itemSlotsSystem.SetLock(uid, slot, true);
            _sharedContainerSystem.GetContainer(uid, id).OccludesLight = false;
        }
    }

    public void OnComponentRemove(EntityUid uid, AttachablesContainerComponent component, ComponentRemove args)
    {
        foreach (var (id, slot) in component.AllowedSlots)
        {
            _itemSlotsSystem.RemoveItemSlot(uid, slot);
        }
    }

    #endregion

    #region Messages

    public void GotEquipped(Entity<AttachablesContainerComponent> ent, ref GotEquippedHandEvent args)
    {
        _sharedActionsSystem.AddAction(args.User, ref ent.Comp.ToggleActionEntity, ent.Comp.ToggleAction, ent.Owner);
    }

    public void OnToggleAction(EntityUid uid, AttachablesContainerComponent component, AttachablesContainerOpenEvent args)
    {
        if (!TryComp(args.Performer, out ActorComponent? actor))
            return;

        _sharedUserInterfaceSystem.TryToggleUi(uid, AttachablesContainerUiKey.Key, actor.PlayerSession);
    }

    public void OnEntInserted(EntityUid uid, AttachablesContainerComponent component, EntInsertedIntoContainerMessage args) { }
    public void OnEntRemoved(EntityUid uid, AttachablesContainerComponent component, EntRemovedFromContainerMessage args) { }

    #endregion
}

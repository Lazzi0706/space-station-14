using Content.Shared.Damage.Components;
using Content.Shared.Whitelist;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Timing;
using Content.Shared.SS220.Buckle; // ss220-flesh-kudzu-damage-fix
using Content.Shared.SS220.Vehicle.Components; // ss220-flesh-kudzu-damage-fix

namespace Content.Shared.Damage.Systems;

public sealed class DamageContactsSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;
    [Dependency] private readonly EntityWhitelistSystem _whitelistSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<DamageContactsComponent, StartCollideEvent>(OnEntityEnter);
        SubscribeLocalEvent<DamageContactsComponent, EndCollideEvent>(OnEntityExit);

        //SS220 Add stand still time
        SubscribeLocalEvent<DamagedByContactComponent, MoveEvent>(OnMove);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<DamagedByContactComponent>();

        while (query.MoveNext(out var ent, out var damaged))
        {
            if (_timing.CurTime < damaged.NextSecond ||
                _timing.CurTime < damaged.LastMovement + damaged.StandStillTime) //SS220 Add stand still time
                continue;
            damaged.NextSecond = _timing.CurTime + TimeSpan.FromSeconds(1);

            if (damaged.Damage != null)
                _damageable.TryChangeDamage(ent, damaged.Damage, ignoreResistances: damaged.IgnoreResistances, interruptsDoAfters: false); //SS220 Add IgnoreResistances param
        }
    }

    private void OnEntityExit(EntityUid uid, DamageContactsComponent component, ref EndCollideEvent args)
    {
        var otherUid = args.OtherEntity;

        if (!TryComp<PhysicsComponent>(otherUid, out var body))
            return;

        var damageQuery = GetEntityQuery<DamageContactsComponent>();
        foreach (var ent in _physics.GetContactingEntities(otherUid, body))

        {
            if (ent == uid)
                continue;

            if (damageQuery.HasComponent(ent))
                return;
        }

        RemComp<DamagedByContactComponent>(otherUid);

        // ss220-flesh-kudzu-damage-fix-start
        if (TryComp<VehicleComponent>(otherUid, out var comp))
        {
            if (comp.Rider != null)
            {
                var riderId = comp.Rider.Value;
                RemComp<DamagedByContactComponent>(riderId);
            }
        }
        // ss220-flesh-kudzu-damage-fix-end
    }

    private void OnEntityEnter(EntityUid uid, DamageContactsComponent component, ref StartCollideEvent args)
    {
        var otherUid = args.OtherEntity;

        // ss220-flesh-kudzu-damage-fix-start
        if (TryComp<VehicleComponent>(otherUid, out var comp))
        {
            if (comp.Rider != null)
            {
                var riderId = comp.Rider.Value;
                var damagedByContactRider = EnsureComp<DamagedByContactComponent>(riderId);
                damagedByContactRider.Damage = component.Damage;
            }
        }
        // ss220-flesh-kudzu-damage-fix-end

        if (HasComp<DamagedByContactComponent>(otherUid))
            return;

        if (_whitelistSystem.IsWhitelistPass(component.IgnoreWhitelist, otherUid) ||
            _whitelistSystem.IsBlacklistFail(component.IgnoreBlacklist, otherUid)) //SS220 Add ignore blacklist
            return;

        var damagedByContact = EnsureComp<DamagedByContactComponent>(otherUid);
        damagedByContact.Damage = component.Damage;

        damagedByContact.IgnoreResistances = component.IgnoreResistances; //SS220 Add IgnoreResistances param
        //SS220 Add stand still time begin
        damagedByContact.StandStillTime = component.StandStillTime;
        Dirty(otherUid, damagedByContact);
        //SS220 Add stand still time end
    }

    //SS220 Add stand still time begin
    private void OnMove(Entity<DamagedByContactComponent> ent, ref MoveEvent args)
    {
        var (uid, component) = ent;
        component.LastMovement = _timing.CurTime;
        Dirty(uid, component);
    }
    //SS220 Add stand still time end
}

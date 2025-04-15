namespace Content.Shared.SS220.Attachables.Core;

public abstract partial class SharedAttachablesContainerSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SharedAttachablesContainerComponent, ComponentInit>(OnComponentInit);
    }
    public void OnComponentInit(Entity<SharedAttachablesContainerComponent> ent, ref ComponentInit args) { }
}

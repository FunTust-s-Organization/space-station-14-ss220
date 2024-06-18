using System.Linq;
using Content.Server.Beam;
using Content.Server.Beam.Components;
using Content.Server.Lightning.Components;
using Content.Shared.Body.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Lightning;
using Robust.Server.GameObjects;
using Robust.Shared.Random;
using Content.Shared.Mobs;
using Robust.Shared.Utility;
using Content.Server.Physics.Components;
using Content.Server.SS220.Cult.Nyarlathotep.Components;

namespace Content.Server.SS220.Cult.Nyarlathotep;

public sealed class NyarlathotepSystem : SharedLightningSystem
{
    [Dependency] private readonly BeamSystem _beam = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly EntityLookupSystem _entityLookupSystem = default!;
    [Dependency] private readonly TransformSystem _transform = default!;
    [Dependency] private readonly MobStateSystem _mobStateSystem = default!;


    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<NyarlathotepComponent, ComponentRemove>(OnRemove);
    }

    private void OnRemove(EntityUid uid, NyarlathotepComponent component, ComponentRemove args)
    {
        if (!TryComp<BeamComponent>(uid, out var lightningBeam) || !TryComp<BeamComponent>(lightningBeam.VirtualBeamController, out var beamController))
        {
            return;
        }

        beamController.CreatedBeams.Remove(uid);
    }

    public void ShootNyarlathotep(EntityUid user, EntityUid target, string lightningPrototype = "Lightning", bool triggerLightningEvents = true)
    {
        var spriteState = LightningRandomizer();
        //_beam.TryCreateBeam(user, target, lightningPrototype, spriteState);
        if (triggerLightningEvents)
        {
            var ev = new HitByNyarlathotepEvent(user, target);
            RaiseLocalEvent(target, ref ev);
        }
    }

    public void ShootNearNyarlathotep(EntityUid user, float range, int boltCount, string lightningPrototype = "Lightning", int arcDepth = 0, bool triggerLightningEvents = true)
    {
        foreach (var target in _entityLookupSystem.GetComponentsInRange<MobStateComponent>(_transform.GetMapCoordinates(user), range))
        {
            ShootNyarlathotep(user, target.Owner, lightningPrototype, triggerLightningEvents);
            if(!TryComp(target.Owner, out NyarlathotepTargetComponent? test))
            {
                EntityManager.AddComponent(target.Owner, new NyarlathotepTargetComponent());
            }
        }
    }
}

[ByRefEvent]
public readonly record struct HitByNyarlathotepEvent(EntityUid Source, EntityUid Target);

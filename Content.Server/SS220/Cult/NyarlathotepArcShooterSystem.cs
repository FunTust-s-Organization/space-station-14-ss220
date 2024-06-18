using Content.Server.Lightning;
using Content.Server.SS220.Cult.Nyarlathotep.Components;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.SS220.Cult.Nyarlathotep;

/// <summary>
/// Fires electric arcs at surrounding objects.
/// </summary>
public sealed class NyarlathotepArcShooterSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly NyarlathotepSystem _nyarlathotep = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<NyarlathotepArcShooterComponent, MapInitEvent>(OnShooterMapInit);
    }

    private void OnShooterMapInit(EntityUid uid, NyarlathotepArcShooterComponent component, ref MapInitEvent args)
    {
        component.NextShootTime = _gameTiming.CurTime + TimeSpan.FromSeconds(component.ShootMaxInterval);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<NyarlathotepArcShooterComponent>();
        while (query.MoveNext(out var uid, out var arcShooter))
        {
            if (arcShooter.NextShootTime > _gameTiming.CurTime)
                continue;

            ArcShoot(uid, arcShooter);
            var delay = TimeSpan.FromSeconds(_random.NextFloat(arcShooter.ShootMinInterval, arcShooter.ShootMaxInterval));
            arcShooter.NextShootTime += delay;
        }
    }

    private void ArcShoot(EntityUid uid, NyarlathotepArcShooterComponent component)
    {
        var arcs = _random.Next(1, component.MaxLightningArc);
        _nyarlathotep.ShootNearNyarlathotep(uid, component.ShootRange, arcs, component.LightningPrototype, component.ArcDepth);
    }
}

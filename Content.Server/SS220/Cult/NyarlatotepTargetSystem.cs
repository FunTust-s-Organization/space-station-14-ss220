using Content.Server.Explosion.EntitySystems;
using Content.Server.Lightning;
using Content.Server.Lightning.Components;
using Content.Shared.Damage;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;
using Content.Shared.Explosion;
using Content.Server.SS220.Cult.Nyarlathotep.Components;

namespace Content.Server.SS220.Cult.Nyarlathotep;

public sealed class NyarlathotepTargetSystem : EntitySystem
{
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly ExplosionSystem _explosionSystem = default!;
    [Dependency] private readonly TransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<NyarlathotepTargetComponent, HitByNyarlathotepEvent>(OnHitByNyarlathotep);
    }

    private void OnHitByNyarlathotep(Entity<NyarlathotepTargetComponent> uid, ref HitByNyarlathotepEvent args)
    {
        DamageSpecifier damage = new();
        var t = uid.Comp.DamageFromLightning;
        Log.Debug(t.ToString());
        damage.DamageDict.Add("Structural", uid.Comp.DamageFromLightning);
        _damageable.TryChangeDamage(uid, damage, true);
        /*_explosionSystem.QueueExplosion(
            _transform.GetMapCoordinates(uid),
            "Default",
            uid.Comp.TotalIntensity, uid.Comp.Dropoff,
            uid.Comp.MaxTileIntensity,
            canCreateVacuum: false);*/
    }
}

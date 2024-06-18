using Content.Server.Tesla.EntitySystems;
using Content.Shared.Explosion;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Server.SS220.Cult.Nyarlathotep.Components;

/// <summary>
/// This component allows the lightning system to select a given entity as the target of a lightning strike.
/// It also determines the priority of selecting this target, and the behavior of the explosion. Used for tesla.
/// </summary>
[RegisterComponent, Access(typeof(NyarlathotepSystem), typeof(LightningTargetSystem))]
public sealed partial class NyarlathotepTargetComponent : Component
{
    /// <summary>
    /// Lightning has a number of bounces into neighboring targets.
    /// This number controls how many bounces the lightning bolt has left after hitting that target.
    /// At high values, the lightning will not travel farther than that entity.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int LightningResistance = 1; //by default, reduces the number of bounces from this target by 1

    /// <summary>
    /// The total amount of intensity an explosion can achieve
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float TotalIntensity = 25f;

    /// <summary>
    /// How quickly does the explosion's power slope? Higher = smaller area and more concentrated damage, lower = larger area and more spread out damage
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Dropoff = 2f;

    /// <summary>
    /// How much intensity can be applied per tile?
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxTileIntensity = 5f;

    /// <summary>
    /// how much structural damage the object takes from a lightning strike
    /// </summary>
    [DataField]
    public FixedPoint2 DamageFromLightning = 1;
}

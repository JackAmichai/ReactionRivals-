using UnityEngine;

/// <summary>
/// Defines an ability that elements can cast when reaching full electron shell (Octet Rule)
/// </summary>
[CreateAssetMenu(fileName = "New Ability", menuName = "Reaction Rivals/Ability Data")]
public class AbilityData : ScriptableObject
{
    [Header("Identity")]
    public string AbilityName;
    
    [TextArea(2, 4)]
    public string Description;

    [Header("Targeting")]
    [Tooltip("How this ability selects targets")]
    public AbilityTargetType TargetType = AbilityTargetType.SingleEnemy;
    
    [Tooltip("Range in hex cells (0 = self only)")]
    [Range(0, 10)]
    public int Range = 3;
    
    [Tooltip("Area of effect radius for AoE abilities")]
    [Range(0, 5)]
    public int AoERadius = 0;

    [Header("Effects")]
    [Tooltip("Base damage dealt")]
    public float Damage = 50f;
    
    [Tooltip("Healing amount (negative for damage)")]
    public float Healing = 0f;
    
    [Tooltip("Status effect to apply")]
    public StatusEffect AppliedEffect = StatusEffect.None;
    
    [Tooltip("Duration of status effect in seconds")]
    public float EffectDuration = 3f;

    [Header("Chemistry Flavor")]
    [Tooltip("Chemical reaction type for visual/audio cues")]
    public ReactionType ReactionType = ReactionType.None;
    
    [Tooltip("Does this ability consume/sacrifice the caster?")]
    public bool SacrificeOnCast = false;

    [Header("Visuals")]
    public GameObject CastVFX;
    public GameObject ImpactVFX;
    public AudioClip CastSFX;
}

public enum AbilityTargetType
{
    Self,
    SingleEnemy,
    SingleAlly,
    AllEnemies,
    AllAllies,
    AoEAroundSelf,
    AoEAroundTarget,
    Line
}

public enum StatusEffect
{
    None,
    
    [Tooltip("Damage over time")]
    Burn,
    
    [Tooltip("Damage over time, spreads on death")]
    Poison,
    
    [Tooltip("Reduced attack speed")]
    Slow,
    
    [Tooltip("Cannot move or act")]
    Stun,
    
    [Tooltip("Reduced armor")]
    Corroded,
    
    [Tooltip("Cannot cast abilities")]
    Silenced,
    
    [Tooltip("Increased damage taken")]
    Vulnerable,
    
    [Tooltip("Damage reflection")]
    Thorns,
    
    [Tooltip("Healing over time")]
    Regeneration
}

public enum ReactionType
{
    None,
    Combustion,     // Fire + Oxygen
    Neutralization, // Acid + Base
    Oxidation,      // Rust/Corrosion
    Reduction,      // Electron gain
    Synthesis,      // Building compounds
    Decomposition,  // Breaking compounds
    Precipitation,  // Forming solids
    Explosion       // Alkali + Water
}

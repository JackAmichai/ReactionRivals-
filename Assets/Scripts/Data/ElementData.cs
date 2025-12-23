using UnityEngine;

/// <summary>
/// ScriptableObject defining a Chemical Element's properties.
/// Stats are derived from real periodic table data for educational accuracy.
/// </summary>
[CreateAssetMenu(fileName = "New Element", menuName = "Reaction Rivals/Element Data")]
public class ElementData : ScriptableObject
{
    [Header("Identity")]
    [Tooltip("Full element name (e.g., 'Hydrogen')")]
    public string ElementName;
    
    [Tooltip("Chemical symbol (e.g., 'H')")]
    public string Symbol;
    
    [Tooltip("Atomic number from periodic table")]
    [Range(1, 118)]
    public int AtomicNumber;

    [Header("Combat Stats")]
    [Tooltip("Health Points - derived from Atomic Mass")]
    public float BaseHP = 100f;
    
    [Tooltip("Attack damage per hit")]
    public float BaseDamage = 10f;
    
    [Tooltip("Attack speed (attacks per second)")]
    public float AttackSpeed = 1f;
    
    [Tooltip("Attack range in hex cells - derived from Atomic Radius")]
    [Range(1, 5)]
    public int AttackRange = 1;

    [Header("Chemistry Properties")]
    [Tooltip("Number of valence electrons (used for Octet Rule ultimate)")]
    [Range(1, 8)]
    public int ValenceElectrons = 1;
    
    [Tooltip("Electronegativity affects electron stealing mechanics")]
    [Range(0f, 4f)]
    public float Electronegativity = 2f;
    
    [Tooltip("Element family for trait synergies")]
    public ElementFamily Family;

    [Header("Economy")]
    [Tooltip("Shop cost in Energy (ATP)")]
    [Range(1, 5)]
    public int Cost = 1;
    
    [Tooltip("Rarity tier affects shop appearance rate")]
    public ElementRarity Rarity = ElementRarity.Common;

    [Header("Visuals")]
    [Tooltip("Color representing this element")]
    public Color ElementColor = Color.white;
    
    [Tooltip("Prefab to spawn for this element")]
    public GameObject UnitPrefab;
    
    [Tooltip("Icon for shop and UI")]
    public Sprite Icon;

    [Header("Abilities")]
    [Tooltip("Ultimate ability triggered at full electron shell")]
    public AbilityData UltimateAbility;

    /// <summary>
    /// Electrons needed to fill the outer shell.
    /// - Noble Gases: Already full (0 needed)
    /// - Hydrogen: Follows Duet Rule (needs 2 total, so 2 - 1 = 1)
    /// - Others: Follow Octet Rule (needs 8 total)
    /// </summary>
    public int ElectronsToOctet
    {
        get
        {
            // Noble gases have full shells
            if (Family == ElementFamily.NobleGas) return 0;
            
            // Hydrogen follows the Duet Rule (2 electrons for full shell)
            if (Family == ElementFamily.Hydrogen) return 2 - ValenceElectrons; // = 1
            
            // All others follow the Octet Rule (8 electrons for full shell)
            return 8 - ValenceElectrons;
        }
    }
    
    /// <summary>
    /// The target electrons for a full shell (2 for H/He, 8 for others)
    /// </summary>
    public int FullShellElectrons => (Family == ElementFamily.Hydrogen || AtomicNumber <= 2) ? 2 : 8;

    /// <summary>
    /// Check if this element can bond with another (simplified covalent bonding)
    /// </summary>
    public bool CanBondWith(ElementData other)
    {
        // Noble gases don't bond
        if (Family == ElementFamily.NobleGas || other.Family == ElementFamily.NobleGas)
            return false;
        
        // Check if combined valence electrons can form stable bonds
        return true; // Simplified - actual bonding logic in BondingManager
    }
}

/// <summary>
/// Periodic table family groupings for trait synergies
/// </summary>
public enum ElementFamily
{
    [Tooltip("Li, Na, K - Explosive when contacting water")]
    Alkali,
    
    [Tooltip("Be, Mg, Ca - Defensive, forms strong structures")]
    AlkalineEarth,
    
    [Tooltip("Fe, Cu, Au, Ag - Metallic bonding, damage sharing")]
    TransitionMetal,
    
    [Tooltip("C, N, O, P, S - Core building blocks of life")]
    NonMetal,
    
    [Tooltip("F, Cl, Br, I - High electronegativity, electron thieves")]
    Halogen,
    
    [Tooltip("He, Ne, Ar - Inert, spell immune tanks")]
    NobleGas,
    
    [Tooltip("H - Special case, most versatile bonding")]
    Hydrogen
}

/// <summary>
/// Rarity tiers affecting shop odds
/// </summary>
public enum ElementRarity
{
    Common,     // 1-cost: H, C, N, O
    Uncommon,   // 2-cost: Na, Cl, S, P
    Rare,       // 3-cost: Fe, Cu, Ca
    Epic,       // 4-cost: Ag, K, Br
    Legendary   // 5-cost: Au, U, Pt
}

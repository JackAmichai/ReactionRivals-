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
    public string elementName;
    
    [Tooltip("Chemical symbol (e.g., 'H')")]
    public string symbol;
    
    [Tooltip("Atomic number from periodic table")]
    [Range(1, 118)]
    public int atomicNumber;

    [Header("Combat Stats")]
    [Tooltip("Health Points - derived from Atomic Mass")]
    public float baseHP = 100f;
    
    [Tooltip("Attack damage per hit")]
    public float baseAttack = 10f;
    
    [Tooltip("Attack speed (attacks per second)")]
    public float attackSpeed = 1f;
    
    [Tooltip("Attack range in hex cells - derived from Atomic Radius")]
    [Range(1, 5)]
    public int attackRange = 1;

    [Header("Chemistry Properties")]
    [Tooltip("Number of valence electrons (used for Octet Rule ultimate)")]
    [Range(1, 8)]
    public int valenceElectrons = 1;
    
    [Tooltip("Electronegativity affects electron stealing mechanics")]
    [Range(0f, 4f)]
    public float electronegativity = 2f;
    
    [Tooltip("Element family for trait synergies")]
    public ElementFamily family;

    [Header("Economy")]
    [Tooltip("Shop cost in Energy (ATP)")]
    [Range(1, 5)]
    public int cost = 1;
    
    [Tooltip("Rarity tier affects shop appearance rate")]
    public ElementRarity rarity = ElementRarity.Common;

    [Header("Visuals")]
    [Tooltip("Color representing this element")]
    public Color elementColor = Color.white;
    
    [Tooltip("Prefab to spawn for this element")]
    public GameObject unitPrefab;
    
    [Tooltip("Icon for shop and UI")]
    public Sprite icon;

    [Header("Abilities")]
    [Tooltip("Ultimate ability triggered at full electron shell")]
    public AbilityData ultimateAbility;
    
    // Property accessors for backwards compatibility
    public string ElementName => elementName;
    public string Symbol => symbol;
    public int AtomicNumber => atomicNumber;
    public float BaseHP => baseHP;
    public float BaseDamage => baseAttack;
    public float AttackSpeed => attackSpeed;
    public int AttackRange => attackRange;
    public int ValenceElectrons => valenceElectrons;
    public float Electronegativity => electronegativity;
    public ElementFamily Family => family;
    public int Cost => cost;
    public ElementRarity Rarity => rarity;
    public Color ElementColor => elementColor;
    public GameObject UnitPrefab => unitPrefab;
    public Sprite Icon => icon;
    public AbilityData UltimateAbility => ultimateAbility;

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
            if (family == ElementFamily.NobleGas) return 0;
            
            // Hydrogen follows the Duet Rule (2 electrons for full shell)
            if (family == ElementFamily.Hydrogen) return 2 - valenceElectrons; // = 1
            
            // All others follow the Octet Rule (8 electrons for full shell)
            return 8 - valenceElectrons;
        }
    }
    
    /// <summary>
    /// The target electrons for a full shell (2 for H/He, 8 for others)
    /// </summary>
    public int FullShellElectrons => (family == ElementFamily.Hydrogen || atomicNumber <= 2) ? 2 : 8;

    /// <summary>
    /// Check if this element can bond with another (simplified covalent bonding)
    /// </summary>
    public bool CanBondWith(ElementData other)
    {
        // Noble gases don't bond
        if (family == ElementFamily.NobleGas || other.family == ElementFamily.NobleGas)
            return false;
        
        // Check if combined valence electrons can form stable bonds
        return true; // Simplified - actual bonding logic in BondingManager
    }
}

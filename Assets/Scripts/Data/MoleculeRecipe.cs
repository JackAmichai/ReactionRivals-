using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Defines a molecule that can be formed by combining elements on the board.
/// When elements are positioned correctly, they merge into a powerful compound.
/// </summary>
[CreateAssetMenu(fileName = "New Molecule", menuName = "Reaction Rivals/Molecule Recipe")]
public class MoleculeRecipe : ScriptableObject
{
    [Header("Identity")]
    [Tooltip("Display name (e.g., 'Water')")]
    public string MoleculeName;
    
    [Tooltip("Chemical formula (e.g., 'H₂O')")]
    public string Formula;
    
    [Tooltip("Description of the molecule's effect")]
    [TextArea(2, 4)]
    public string Description;

    [Header("Recipe Requirements")]
    [Tooltip("The central/core element of this molecule")]
    public ElementData CoreElement;
    
    [Tooltip("Required surrounding elements to complete the molecule")]
    public List<ElementRequirement> RequiredElements = new List<ElementRequirement>();
    
    [Tooltip("If true, elements must be in specific hex positions")]
    public bool RequiresSpecificShape = false;
    
    [Tooltip("Specific shape pattern if required (relative hex coordinates)")]
    public List<Vector2Int> ShapePattern = new List<Vector2Int>();

    [Header("Molecule Stats")]
    [Tooltip("Bond type determines the special mechanic")]
    public BondType BondType = BondType.Covalent;
    
    [Tooltip("Stat multiplier applied to the fused unit")]
    public float HPMultiplier = 1.5f;
    
    [Tooltip("Damage multiplier applied to the fused unit")]
    public float DamageMultiplier = 1.5f;

    [Header("Special Effects")]
    [Tooltip("Special ability granted to the molecule")]
    public MoleculeAbility SpecialAbility;
    
    [Tooltip("Particle effect when molecule forms")]
    public GameObject FormationVFX;
    
    [Tooltip("Sound effect when molecule forms")]
    public AudioClip FormationSFX;

    [Header("Visuals")]
    [Tooltip("Color of the formed molecule")]
    public Color MoleculeColor = Color.cyan;
    
    [Tooltip("Prefab for the combined molecule unit")]
    public GameObject MoleculePrefab;
    
    [Tooltip("Icon for UI display")]
    public Sprite Icon;

    /// <summary>
    /// Check if a set of elements matches this recipe
    /// </summary>
    public bool MatchesRecipe(ElementData core, List<ElementData> neighbors)
    {
        // Check if core element matches
        if (CoreElement != null && core.Symbol != CoreElement.Symbol)
            return false;

        // Count required elements
        Dictionary<string, int> required = new Dictionary<string, int>();
        foreach (var req in RequiredElements)
        {
            if (!required.ContainsKey(req.Element.Symbol))
                required[req.Element.Symbol] = 0;
            required[req.Element.Symbol] += req.Count;
        }

        // Count available neighbors
        Dictionary<string, int> available = new Dictionary<string, int>();
        foreach (var neighbor in neighbors)
        {
            if (!available.ContainsKey(neighbor.Symbol))
                available[neighbor.Symbol] = 0;
            available[neighbor.Symbol]++;
        }

        // Check if we have enough of each required element
        foreach (var req in required)
        {
            if (!available.ContainsKey(req.Key) || available[req.Key] < req.Value)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Get the total number of units consumed by this molecule
    /// </summary>
    public int GetTotalUnitCount()
    {
        int count = 1; // Core element
        foreach (var req in RequiredElements)
        {
            count += req.Count;
        }
        return count;
    }
}

/// <summary>
/// Defines a required element and its count for a recipe
/// </summary>
[System.Serializable]
public class ElementRequirement
{
    [Tooltip("The element required")]
    public ElementData Element;
    
    [Tooltip("How many of this element are needed")]
    [Range(1, 6)]
    public int Count = 1;
}

/// <summary>
/// Types of chemical bonds with different game mechanics
/// </summary>
public enum BondType
{
    [Tooltip("Units merge into one powerful compound (Megazord)")]
    Covalent,
    
    [Tooltip("Units gain defensive buff, reflect damage (Crystal Armor)")]
    Ionic,
    
    [Tooltip("Metal units share damage pool (Electron Sea)")]
    Metallic
}

/// <summary>
/// Special abilities that molecules can have
/// </summary>
public enum MoleculeAbility
{
    None,
    
    [Tooltip("H₂O - Heals nearby allies")]
    Healing,
    
    [Tooltip("CH₄ - AoE poison damage")]
    PoisonCloud,
    
    [Tooltip("CO₂ - Slows enemy attack speed")]
    Suffocate,
    
    [Tooltip("NH₃ - Cleanses debuffs")]
    Cleanse,
    
    [Tooltip("NaCl - Reflects damage")]
    CrystalArmor,
    
    [Tooltip("H₂SO₄ - Armor reduction")]
    Corrosion,
    
    [Tooltip("C₆H₁₂O₆ - Mana regeneration")]
    EnergyBoost
}

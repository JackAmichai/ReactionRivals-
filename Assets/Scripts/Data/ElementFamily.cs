using UnityEngine;

/// <summary>
/// Element family classifications for the periodic table
/// </summary>
public enum ElementFamily
{
    [Tooltip("H - Special case, most versatile bonding")]
    Hydrogen,
    
    [Tooltip("Li, Na, K - Explosive when contacting water")]
    Alkali,              // Group 1 (except H)
    
    [Tooltip("Be, Mg, Ca - Defensive, forms strong structures")]
    AlkalineEarth,       // Group 2
    
    [Tooltip("Fe, Cu, Au, Ag - Metallic bonding, damage sharing")]
    TransitionMetal,     // Groups 3-12
    
    PostTransitionMetal, // Al, Ga, In, Sn, Tl, Pb, Bi, Nh, Fl, Mc, Lv
    
    Metalloid,           // B, Si, Ge, As, Sb, Te, Po
    
    [Tooltip("C, N, O, P, S - Core building blocks of life")]
    NonMetal,            // C, N, O, P, S, Se
    
    [Tooltip("F, Cl, Br, I - High electronegativity, electron thieves")]
    Halogen,             // Group 17
    
    [Tooltip("He, Ne, Ar - Inert, spell immune tanks")]
    NobleGas,            // Group 18
    
    Lanthanide,          // 57-71
    Actinide             // 89-103
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

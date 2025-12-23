using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The Bonding Engine - detects and forms molecules when elements are positioned correctly.
/// This is the core unique mechanic that differentiates Reaction Rivals from other auto-battlers.
/// </summary>
public class BondingManager : MonoBehaviour
{
    public static BondingManager Instance { get; private set; }

    [Header("Recipes")]
    [Tooltip("All available molecule recipes")]
    public List<MoleculeRecipe> AllRecipes = new List<MoleculeRecipe>();

    [Header("Active Molecules")]
    [Tooltip("Currently formed molecules on the board")]
    public List<Molecule> ActiveMolecules = new List<Molecule>();

    [Header("Settings")]
    [Tooltip("Check for bonds automatically when units are placed")]
    public bool AutoCheckOnPlacement = true;
    
    [Tooltip("Allow partial bonds (buff without merge)")]
    public bool AllowPartialBonds = true;

    [Header("Prefabs")]
    [SerializeField] private GameObject moleculePrefab;
    [SerializeField] private GameObject bondFormationVFX;

    // Events
    public System.Action<Molecule> OnMoleculeFormed;
    public System.Action<Molecule> OnMoleculeBroken;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Check for all possible bonds on the entire board
    /// Called at the start of combat phase
    /// </summary>
    public void CheckAllBonds()
    {
        // Clear previous molecules
        BreakAllMolecules();

        // Get all units on the board
        var allUnits = HexGrid.Instance?.GetAllBoardUnits(0);
        if (allUnits == null || allUnits.Count == 0) return;

        // Track which units have been used in molecules
        HashSet<Unit> usedUnits = new HashSet<Unit>();

        // Check each recipe
        foreach (var recipe in AllRecipes)
        {
            CheckRecipeOnBoard(recipe, allUnits, usedUnits);
        }
    }

    /// <summary>
    /// Check bonds for a specific unit (called when unit is placed)
    /// </summary>
    public void CheckBondsForUnit(Unit unit)
    {
        if (!AutoCheckOnPlacement) return;
        if (unit == null || unit.Data == null) return;

        // Check if this unit can be a core element for any recipe
        foreach (var recipe in AllRecipes)
        {
            if (recipe.CoreElement != null && recipe.CoreElement.Symbol == unit.Data.Symbol)
            {
                TryFormMolecule(unit, recipe);
            }
        }

        // Check if this unit completes any existing partial molecules
        var neighbors = HexGrid.Instance?.GetNeighbors(unit);
        foreach (var neighbor in neighbors)
        {
            foreach (var recipe in AllRecipes)
            {
                if (recipe.CoreElement != null && neighbor.Data.Symbol == recipe.CoreElement.Symbol)
                {
                    TryFormMolecule(neighbor, recipe);
                }
            }
        }

        // Highlight potential bonds for visual feedback
        HighlightPotentialBonds(unit);
    }

    /// <summary>
    /// Check if a specific recipe can be formed on the board
    /// </summary>
    private void CheckRecipeOnBoard(MoleculeRecipe recipe, List<Unit> allUnits, HashSet<Unit> usedUnits)
    {
        // Find all potential core elements
        var coreUnits = allUnits.Where(u => 
            !usedUnits.Contains(u) && 
            u.Data.Symbol == recipe.CoreElement.Symbol
        ).ToList();

        foreach (var coreUnit in coreUnits)
        {
            if (TryFormMolecule(coreUnit, recipe, usedUnits))
            {
                // Mark units as used
                usedUnits.Add(coreUnit);
            }
        }
    }

    /// <summary>
    /// Attempt to form a molecule with the given core unit
    /// </summary>
    private bool TryFormMolecule(Unit coreUnit, MoleculeRecipe recipe, HashSet<Unit> excludeUnits = null)
    {
        if (coreUnit == null || coreUnit.IsInMolecule) return false;

        // Get neighbors
        var neighbors = HexGrid.Instance?.GetNeighbors(coreUnit);
        if (neighbors == null) return false;

        // Filter out excluded units
        if (excludeUnits != null)
        {
            neighbors = neighbors.Where(n => !excludeUnits.Contains(n) && !n.IsInMolecule).ToList();
        }

        // Get neighbor element data
        var neighborData = neighbors.Select(n => n.Data).ToList();

        // Check if recipe matches
        if (!recipe.MatchesRecipe(coreUnit.Data, neighborData))
        {
            // Check for partial bonds
            if (AllowPartialBonds)
            {
                ApplyPartialBondBuffs(coreUnit, neighbors, recipe);
            }
            return false;
        }

        // Find the specific units to use
        List<Unit> componentUnits = FindComponentUnits(neighbors, recipe);
        if (componentUnits == null) return false;

        // Form the molecule!
        FormMolecule(recipe, coreUnit, componentUnits.ToArray());
        
        // Mark component units
        if (excludeUnits != null)
        {
            foreach (var comp in componentUnits)
            {
                excludeUnits.Add(comp);
            }
        }

        return true;
    }

    /// <summary>
    /// Find the specific units needed to complete a recipe
    /// </summary>
    private List<Unit> FindComponentUnits(List<Unit> availableUnits, MoleculeRecipe recipe)
    {
        List<Unit> components = new List<Unit>();
        Dictionary<string, int> needed = new Dictionary<string, int>();

        // Build requirements dictionary
        foreach (var req in recipe.RequiredElements)
        {
            if (!needed.ContainsKey(req.Element.Symbol))
                needed[req.Element.Symbol] = 0;
            needed[req.Element.Symbol] += req.Count;
        }

        // Find units to fulfill requirements
        foreach (var unit in availableUnits)
        {
            if (unit.IsInMolecule) continue;

            string symbol = unit.Data.Symbol;
            if (needed.ContainsKey(symbol) && needed[symbol] > 0)
            {
                components.Add(unit);
                needed[symbol]--;
            }
        }

        // Check if all requirements met
        foreach (var count in needed.Values)
        {
            if (count > 0) return null;
        }

        return components;
    }

    /// <summary>
    /// Form a molecule from component units
    /// </summary>
    public void FormMolecule(MoleculeRecipe recipe, Unit coreUnit, params Unit[] componentUnits)
    {
        Debug.Log($"🧪 Forming {recipe.MoleculeName} ({recipe.Formula})!");

        // Create molecule object
        GameObject moleculeObj = moleculePrefab != null 
            ? Instantiate(moleculePrefab, coreUnit.transform.position, Quaternion.identity)
            : new GameObject($"Molecule_{recipe.MoleculeName}");

        Molecule molecule = moleculeObj.GetComponent<Molecule>();
        if (molecule == null)
            molecule = moleculeObj.AddComponent<Molecule>();

        // Initialize molecule
        molecule.Initialize(recipe, coreUnit, componentUnits);

        // Handle different bond types
        switch (recipe.BondType)
        {
            case BondType.Covalent:
                // Merge units into one powerful compound
                MergeCovalentBond(molecule, coreUnit, componentUnits);
                break;

            case BondType.Ionic:
                // Units stay separate but get defensive buffs
                ApplyIonicBond(molecule, coreUnit, componentUnits);
                break;

            case BondType.Metallic:
                // Create damage-sharing pool
                ApplyMetallicBond(molecule, coreUnit, componentUnits);
                break;
        }

        ActiveMolecules.Add(molecule);
        OnMoleculeFormed?.Invoke(molecule);

        // Play formation effect
        if (bondFormationVFX != null)
        {
            Instantiate(bondFormationVFX, coreUnit.transform.position, Quaternion.identity);
        }
        if (recipe.FormationVFX != null)
        {
            Instantiate(recipe.FormationVFX, coreUnit.transform.position, Quaternion.identity);
        }
    }

    #region Bond Type Implementations

    /// <summary>
    /// Covalent Bond: Units merge into one powerful compound (Megazord style)
    /// Example: C + 4H → CH₄ (Methane)
    /// </summary>
    private void MergeCovalentBond(Molecule molecule, Unit coreUnit, Unit[] components)
    {
        // Calculate combined stats
        float totalHP = coreUnit.MaxHP * molecule.Recipe.HPMultiplier;
        float totalDamage = coreUnit.Damage * molecule.Recipe.DamageMultiplier;

        foreach (var comp in components)
        {
            totalHP += comp.MaxHP * 0.5f; // Components contribute 50% of their HP
            totalDamage += comp.Damage * 0.3f; // Components contribute 30% damage
        }

        // Apply stats to core unit (which becomes the molecule)
        coreUnit.MaxHP = totalHP;
        coreUnit.CurrentHP = totalHP;
        coreUnit.Damage = totalDamage;
        coreUnit.IsInMolecule = true;
        coreUnit.ParentMolecule = molecule;

        // Hide component units (they're "fused" into the core)
        foreach (var comp in components)
        {
            comp.IsInMolecule = true;
            comp.ParentMolecule = molecule;
            comp.gameObject.SetActive(false);
        }

        // Update visuals
        var visuals = coreUnit.GetComponent<UnitVisuals>();
        visuals?.ShowBondLines(components);

        Debug.Log($"⚛️ Covalent merge complete! {molecule.Recipe.MoleculeName}: HP={totalHP}, DMG={totalDamage}");
    }

    /// <summary>
    /// Ionic Bond: Units gain defensive buffs and damage reflection (Crystal Armor)
    /// Example: Na + Cl → NaCl (Salt)
    /// </summary>
    private void ApplyIonicBond(Molecule molecule, Unit coreUnit, Unit[] components)
    {
        // All units in ionic bond get Crystal Armor
        float armorBonus = 0.3f; // 30% damage reduction
        float reflectPercent = 0.2f; // 20% damage reflection

        void ApplyIonicBuff(Unit unit)
        {
            unit.IsInMolecule = true;
            unit.ParentMolecule = molecule;
            // Store buff data on the unit
            // In a full implementation, you'd have a buff system
        }

        ApplyIonicBuff(coreUnit);
        foreach (var comp in components)
        {
            ApplyIonicBuff(comp);
        }

        Debug.Log($"💎 Ionic bond formed! {molecule.Recipe.MoleculeName}: All units gain Crystal Armor");
    }

    /// <summary>
    /// Metallic Bond: Metal units share damage across the group (Electron Sea)
    /// Example: Fe + Fe + Fe → Metal cluster
    /// </summary>
    private void ApplyMetallicBond(Molecule molecule, Unit coreUnit, Unit[] components)
    {
        // Calculate total HP pool
        float totalPool = coreUnit.CurrentHP;
        foreach (var comp in components)
        {
            totalPool += comp.CurrentHP;
        }

        // Store pool on molecule
        molecule.SharedDamagePool = totalPool;

        // Mark all units
        coreUnit.IsInMolecule = true;
        coreUnit.ParentMolecule = molecule;
        foreach (var comp in components)
        {
            comp.IsInMolecule = true;
            comp.ParentMolecule = molecule;
        }

        Debug.Log($"🔗 Metallic bond formed! {molecule.Recipe.MoleculeName}: Shared HP pool = {totalPool}");
    }

    #endregion

    #region Partial Bonds

    /// <summary>
    /// Apply smaller buffs when only part of a molecule is formed
    /// </summary>
    private void ApplyPartialBondBuffs(Unit coreUnit, List<Unit> neighbors, MoleculeRecipe targetRecipe)
    {
        // Count how many required elements are present
        int totalRequired = targetRecipe.RequiredElements.Sum(r => r.Count);
        int present = 0;

        foreach (var req in targetRecipe.RequiredElements)
        {
            int found = neighbors.Count(n => n.Data.Symbol == req.Element.Symbol);
            present += Mathf.Min(found, req.Count);
        }

        if (present == 0) return;

        float completionRatio = (float)present / totalRequired;
        
        // Apply partial buff (scaled by completion)
        float partialHPBuff = (targetRecipe.HPMultiplier - 1f) * completionRatio * 0.5f;
        float partialDamageBuff = (targetRecipe.DamageMultiplier - 1f) * completionRatio * 0.5f;

        // Apply to core unit
        coreUnit.MaxHP *= (1f + partialHPBuff);
        coreUnit.CurrentHP = coreUnit.MaxHP;
        coreUnit.Damage *= (1f + partialDamageBuff);

        Debug.Log($"📊 Partial bond for {targetRecipe.MoleculeName}: {completionRatio:P0} complete");
    }

    #endregion

    #region Molecule Breaking

    /// <summary>
    /// Break a specific molecule
    /// </summary>
    public void BreakMolecule(Molecule molecule)
    {
        if (molecule == null) return;

        // Restore component units
        if (molecule.Recipe.BondType == BondType.Covalent)
        {
            foreach (var comp in molecule.ComponentUnits)
            {
                if (comp != null)
                {
                    comp.gameObject.SetActive(true);
                    comp.IsInMolecule = false;
                    comp.ParentMolecule = null;
                    comp.CalculateStats(); // Reset to base stats
                }
            }
        }

        // Clear molecule state from core
        if (molecule.CoreUnit != null)
        {
            molecule.CoreUnit.IsInMolecule = false;
            molecule.CoreUnit.ParentMolecule = null;
            molecule.CoreUnit.CalculateStats();
        }

        ActiveMolecules.Remove(molecule);
        OnMoleculeBroken?.Invoke(molecule);

        Destroy(molecule.gameObject);
    }

    /// <summary>
    /// Break all molecules on the board
    /// </summary>
    public void BreakAllMolecules()
    {
        foreach (var molecule in ActiveMolecules.ToList())
        {
            BreakMolecule(molecule);
        }
        ActiveMolecules.Clear();
    }

    #endregion

    #region Visual Helpers

    /// <summary>
    /// Highlight cells that would complete a molecule when placing a unit
    /// </summary>
    private void HighlightPotentialBonds(Unit unit)
    {
        if (unit?.Data == null) return;

        var neighbors = HexGrid.Instance?.GetNeighbors(unit);
        if (neighbors == null) return;

        foreach (var neighbor in neighbors)
        {
            // Check if this neighbor could form a molecule with the placed unit
            foreach (var recipe in AllRecipes)
            {
                if (CouldFormMolecule(unit, neighbor, recipe))
                {
                    neighbor.CurrentCell?.SetBondHighlight();
                }
            }
        }
    }

    /// <summary>
    /// Check if two units could potentially form a molecule together
    /// </summary>
    private bool CouldFormMolecule(Unit unit1, Unit unit2, MoleculeRecipe recipe)
    {
        // Simple check - one is core, other is component
        bool unit1IsCore = recipe.CoreElement?.Symbol == unit1.Data?.Symbol;
        bool unit2IsCore = recipe.CoreElement?.Symbol == unit2.Data?.Symbol;

        if (!unit1IsCore && !unit2IsCore) return false;

        Unit core = unit1IsCore ? unit1 : unit2;
        Unit potential = unit1IsCore ? unit2 : unit1;

        // Check if potential component is needed
        return recipe.RequiredElements.Any(r => r.Element.Symbol == potential.Data?.Symbol);
    }

    #endregion

    #region Specific Molecule Checks (for MVP)

    /// <summary>
    /// Check specifically for Water (H₂O) - MVP molecule
    /// </summary>
    public bool CheckForWater()
    {
        var allUnits = HexGrid.Instance?.GetAllBoardUnits(0);
        if (allUnits == null) return false;

        foreach (var unit in allUnits.Where(u => u.Data?.Symbol == "O"))
        {
            var neighbors = HexGrid.Instance.GetNeighbors(unit);
            var hydrogens = neighbors.Where(n => n.Data?.Symbol == "H" && !n.IsInMolecule).ToList();

            if (hydrogens.Count >= 2)
            {
                var waterRecipe = AllRecipes.FirstOrDefault(r => r.MoleculeName == "Water");
                if (waterRecipe != null)
                {
                    FormMolecule(waterRecipe, unit, hydrogens[0], hydrogens[1]);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Check specifically for Methane (CH₄) - MVP molecule
    /// </summary>
    public bool CheckForMethane()
    {
        var allUnits = HexGrid.Instance?.GetAllBoardUnits(0);
        if (allUnits == null) return false;

        foreach (var unit in allUnits.Where(u => u.Data?.Symbol == "C"))
        {
            var neighbors = HexGrid.Instance.GetNeighbors(unit);
            var hydrogens = neighbors.Where(n => n.Data?.Symbol == "H" && !n.IsInMolecule).ToList();

            if (hydrogens.Count >= 4)
            {
                var methaneRecipe = AllRecipes.FirstOrDefault(r => r.MoleculeName == "Methane");
                if (methaneRecipe != null)
                {
                    FormMolecule(methaneRecipe, unit, hydrogens.Take(4).ToArray());
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Check specifically for Carbon Dioxide (CO₂) - MVP molecule
    /// </summary>
    public bool CheckForCO2()
    {
        var allUnits = HexGrid.Instance?.GetAllBoardUnits(0);
        if (allUnits == null) return false;

        foreach (var unit in allUnits.Where(u => u.Data?.Symbol == "C"))
        {
            var neighbors = HexGrid.Instance.GetNeighbors(unit);
            var oxygens = neighbors.Where(n => n.Data?.Symbol == "O" && !n.IsInMolecule).ToList();

            if (oxygens.Count >= 2)
            {
                var co2Recipe = AllRecipes.FirstOrDefault(r => r.MoleculeName == "Carbon Dioxide");
                if (co2Recipe != null)
                {
                    FormMolecule(co2Recipe, unit, oxygens[0], oxygens[1]);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Check specifically for Ammonia (NH₃) - MVP molecule
    /// </summary>
    public bool CheckForAmmonia()
    {
        var allUnits = HexGrid.Instance?.GetAllBoardUnits(0);
        if (allUnits == null) return false;

        foreach (var unit in allUnits.Where(u => u.Data?.Symbol == "N"))
        {
            var neighbors = HexGrid.Instance.GetNeighbors(unit);
            var hydrogens = neighbors.Where(n => n.Data?.Symbol == "H" && !n.IsInMolecule).ToList();

            if (hydrogens.Count >= 3)
            {
                var ammoniaRecipe = AllRecipes.FirstOrDefault(r => r.MoleculeName == "Ammonia");
                if (ammoniaRecipe != null)
                {
                    FormMolecule(ammoniaRecipe, unit, hydrogens.Take(3).ToArray());
                    return true;
                }
            }
        }
        return false;
    }

    #endregion
}

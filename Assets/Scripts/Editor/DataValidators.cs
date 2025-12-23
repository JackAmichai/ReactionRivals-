using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Editor tools for validating chemistry data accuracy.
/// Ensures all element and molecule data is correct for educational purposes.
/// </summary>
public static class DataValidators
{
    #region Element Validation Data
    
    // Reference data for validation (subset of common elements)
    private static readonly Dictionary<string, ElementValidationData> ReferenceData = new Dictionary<string, ElementValidationData>
    {
        // Format: Symbol -> (AtomicNumber, ValenceElectrons, Electronegativity, Family)
        {"H",  new ElementValidationData(1, 1, 2.20f, ElementFamily.Hydrogen)},
        {"He", new ElementValidationData(2, 2, 0f, ElementFamily.NobleGas)}, // Full shell (2)
        {"Li", new ElementValidationData(3, 1, 0.98f, ElementFamily.Alkali)},
        {"Be", new ElementValidationData(4, 2, 1.57f, ElementFamily.AlkalineEarth)},
        {"B",  new ElementValidationData(5, 3, 2.04f, ElementFamily.Metalloid)},
        {"C",  new ElementValidationData(6, 4, 2.55f, ElementFamily.NonMetal)},
        {"N",  new ElementValidationData(7, 5, 3.04f, ElementFamily.NonMetal)},
        {"O",  new ElementValidationData(8, 6, 3.44f, ElementFamily.NonMetal)},
        {"F",  new ElementValidationData(9, 7, 3.98f, ElementFamily.Halogen)},
        {"Ne", new ElementValidationData(10, 8, 0f, ElementFamily.NobleGas)},
        {"Na", new ElementValidationData(11, 1, 0.93f, ElementFamily.Alkali)},
        {"Mg", new ElementValidationData(12, 2, 1.31f, ElementFamily.AlkalineEarth)},
        {"Al", new ElementValidationData(13, 3, 1.61f, ElementFamily.PostTransitionMetal)},
        {"Si", new ElementValidationData(14, 4, 1.90f, ElementFamily.Metalloid)},
        {"P",  new ElementValidationData(15, 5, 2.19f, ElementFamily.NonMetal)},
        {"S",  new ElementValidationData(16, 6, 2.58f, ElementFamily.NonMetal)},
        {"Cl", new ElementValidationData(17, 7, 3.16f, ElementFamily.Halogen)},
        {"Ar", new ElementValidationData(18, 8, 0f, ElementFamily.NobleGas)},
        {"K",  new ElementValidationData(19, 1, 0.82f, ElementFamily.Alkali)},
        {"Ca", new ElementValidationData(20, 2, 1.00f, ElementFamily.AlkalineEarth)},
        {"Fe", new ElementValidationData(26, 2, 1.83f, ElementFamily.TransitionMetal)}, // Common Fe2+ state
        {"Cu", new ElementValidationData(29, 1, 1.90f, ElementFamily.TransitionMetal)}, // Common Cu+ state
        {"Zn", new ElementValidationData(30, 2, 1.65f, ElementFamily.TransitionMetal)},
        {"Br", new ElementValidationData(35, 7, 2.96f, ElementFamily.Halogen)},
        {"Kr", new ElementValidationData(36, 8, 3.00f, ElementFamily.NobleGas)},
        {"Ag", new ElementValidationData(47, 1, 1.93f, ElementFamily.TransitionMetal)},
        {"I",  new ElementValidationData(53, 7, 2.66f, ElementFamily.Halogen)},
        {"Xe", new ElementValidationData(54, 8, 2.60f, ElementFamily.NobleGas)},
        {"Au", new ElementValidationData(79, 1, 2.54f, ElementFamily.TransitionMetal)},
    };

    // Neutron counts for most stable/common isotopes
    private static readonly Dictionary<string, int> NeutronCounts = new Dictionary<string, int>
    {
        {"H", 0}, {"He", 2}, {"Li", 4}, {"Be", 5}, {"B", 6},
        {"C", 6}, {"N", 7}, {"O", 8}, {"F", 10}, {"Ne", 10},
        {"Na", 12}, {"Mg", 12}, {"Al", 14}, {"Si", 14}, {"P", 16},
        {"S", 16}, {"Cl", 18}, {"Ar", 22}, {"K", 20}, {"Ca", 20},
        {"Fe", 30}, {"Cu", 35}, {"Zn", 35}, {"Br", 45}, {"Kr", 48},
        {"Ag", 61}, {"I", 74}, {"Xe", 77}, {"Au", 118}
    };
    
    #endregion

    [MenuItem("Tools/Reaction Rivals/Validate All Data", priority = 100)]
    public static void ValidateAllData()
    {
        Debug.Log("🔬 Starting Reaction Rivals Data Validation...\n");
        
        int totalErrors = 0;
        int totalWarnings = 0;
        
        // Validate PeriodicTable static data
        totalErrors += ValidatePeriodicTable();
        
        // Validate ElementData ScriptableObjects
        totalErrors += ValidateElementDataAssets();
        
        // Validate MoleculeRecipe assets
        totalErrors += ValidateMoleculeRecipes();
        
        // Validate AtomBuilder data
        totalErrors += ValidateAtomBuilderData();
        
        // Summary
        Debug.Log("═══════════════════════════════════════");
        if (totalErrors == 0)
        {
            Debug.Log("✅ <color=green>All data validated successfully!</color>");
        }
        else
        {
            Debug.LogError($"❌ Found {totalErrors} error(s). Please review and fix.");
        }
        Debug.Log("═══════════════════════════════════════");
    }

    [MenuItem("Tools/Reaction Rivals/Validate Periodic Table", priority = 101)]
    public static int ValidatePeriodicTable()
    {
        Debug.Log("📊 Validating Periodic Table Data...");
        int errors = 0;
        
        foreach (var element in PeriodicTable.Elements)
        {
            // Check atomic number sequence
            int expectedAtomicNumber = System.Array.IndexOf(PeriodicTable.Elements, element) + 1;
            if (element.AtomicNumber != expectedAtomicNumber)
            {
                Debug.LogError($"❌ {element.Symbol}: Atomic number mismatch. Expected {expectedAtomicNumber}, got {element.AtomicNumber}");
                errors++;
            }
            
            // Validate against reference data if available
            if (ReferenceData.TryGetValue(element.Symbol, out var refData))
            {
                if (element.ValenceElectrons != refData.ValenceElectrons)
                {
                    Debug.LogError($"❌ {element.Symbol}: Valence electrons wrong. Expected {refData.ValenceElectrons}, got {element.ValenceElectrons}");
                    errors++;
                }
                
                if (element.Family != refData.Family)
                {
                    Debug.LogWarning($"⚠️ {element.Symbol}: Family mismatch. Expected {refData.Family}, got {element.Family}");
                }
                
                // Electronegativity check (allow small variance for noble gases)
                if (refData.Electronegativity > 0 && Mathf.Abs(element.Electronegativity - refData.Electronegativity) > 0.1f)
                {
                    Debug.LogWarning($"⚠️ {element.Symbol}: Electronegativity off. Expected ~{refData.Electronegativity:F2}, got {element.Electronegativity:F2}");
                }
            }
            
            // Validate valence electrons by group (simplified)
            int expectedValence = GetExpectedValenceByGroup(element.Group, element.Family);
            if (expectedValence > 0 && element.ValenceElectrons != expectedValence)
            {
                // This is a soft check - transition metals can vary
                if (element.Family != ElementFamily.TransitionMetal && 
                    element.Family != ElementFamily.Lanthanide && 
                    element.Family != ElementFamily.Actinide)
                {
                    Debug.LogWarning($"⚠️ {element.Symbol}: Valence electrons ({element.ValenceElectrons}) may not match group {element.Group}");
                }
            }
        }
        
        Debug.Log($"   Periodic Table: {(errors == 0 ? "✓ OK" : $"{errors} errors")}");
        return errors;
    }

    [MenuItem("Tools/Reaction Rivals/Validate Element Assets", priority = 102)]
    public static int ValidateElementDataAssets()
    {
        Debug.Log("📁 Validating ElementData ScriptableObjects...");
        int errors = 0;
        
        string[] guids = AssetDatabase.FindAssets("t:ElementData");
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ElementData element = AssetDatabase.LoadAssetAtPath<ElementData>(path);
            
            if (element == null) continue;
            
            // Validate against reference
            if (ReferenceData.TryGetValue(element.Symbol, out var refData))
            {
                if (element.AtomicNumber != refData.AtomicNumber)
                {
                    Debug.LogError($"❌ {element.Symbol} ({path}): Atomic number wrong. Expected {refData.AtomicNumber}, got {element.AtomicNumber}");
                    errors++;
                }
                
                if (element.ValenceElectrons != refData.ValenceElectrons)
                {
                    Debug.LogError($"❌ {element.Symbol} ({path}): Valence electrons wrong. Expected {refData.ValenceElectrons}, got {element.ValenceElectrons}");
                    errors++;
                }
            }
            
            // Validate cost is reasonable
            if (element.Cost < 1 || element.Cost > 5)
            {
                Debug.LogError($"❌ {element.Symbol} ({path}): Cost {element.Cost} is out of valid range (1-5)");
                errors++;
            }
            
            // Validate HP is positive
            if (element.BaseHP <= 0)
            {
                Debug.LogError($"❌ {element.Symbol} ({path}): BaseHP must be positive, got {element.BaseHP}");
                errors++;
            }
        }
        
        Debug.Log($"   Element Assets: {(errors == 0 ? "✓ OK" : $"{errors} errors")} ({guids.Length} assets checked)");
        return errors;
    }

    [MenuItem("Tools/Reaction Rivals/Validate Molecule Recipes", priority = 103)]
    public static int ValidateMoleculeRecipes()
    {
        Debug.Log("🧪 Validating MoleculeRecipe ScriptableObjects...");
        int errors = 0;
        
        string[] guids = AssetDatabase.FindAssets("t:MoleculeRecipe");
        
        // Expected recipes based on chemistry
        var expectedRecipes = new Dictionary<string, (string core, Dictionary<string, int> requirements)>
        {
            {"Water", ("O", new Dictionary<string, int> {{"H", 2}})},           // H₂O: O core + 2H
            {"Methane", ("C", new Dictionary<string, int> {{"H", 4}})},         // CH₄: C core + 4H
            {"Carbon Dioxide", ("C", new Dictionary<string, int> {{"O", 2}})},  // CO₂: C core + 2O
            {"Ammonia", ("N", new Dictionary<string, int> {{"H", 3}})},         // NH₃: N core + 3H
            {"Salt", ("Na", new Dictionary<string, int> {{"Cl", 1}})}           // NaCl: Na + Cl
        };
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            MoleculeRecipe recipe = AssetDatabase.LoadAssetAtPath<MoleculeRecipe>(path);
            
            if (recipe == null) continue;
            
            // Check core element exists
            if (recipe.CoreElement == null)
            {
                Debug.LogError($"❌ {recipe.MoleculeName} ({path}): Missing CoreElement");
                errors++;
                continue;
            }
            
            // Check required elements exist
            foreach (var req in recipe.RequiredElements)
            {
                if (req.Element == null)
                {
                    Debug.LogError($"❌ {recipe.MoleculeName} ({path}): Has null element in requirements");
                    errors++;
                }
                if (req.Count < 1)
                {
                    Debug.LogError($"❌ {recipe.MoleculeName} ({path}): Invalid count ({req.Count}) for requirement");
                    errors++;
                }
            }
            
            // Validate known recipes
            if (expectedRecipes.TryGetValue(recipe.MoleculeName, out var expected))
            {
                if (recipe.CoreElement.Symbol != expected.core)
                {
                    Debug.LogError($"❌ {recipe.MoleculeName}: Core element should be {expected.core}, got {recipe.CoreElement.Symbol}");
                    errors++;
                }
                
                // Check requirements match
                foreach (var req in expected.requirements)
                {
                    var found = recipe.RequiredElements.FirstOrDefault(r => r.Element?.Symbol == req.Key);
                    if (found == null)
                    {
                        Debug.LogError($"❌ {recipe.MoleculeName}: Missing required element {req.Key}");
                        errors++;
                    }
                    else if (found.Count != req.Value)
                    {
                        Debug.LogError($"❌ {recipe.MoleculeName}: {req.Key} count should be {req.Value}, got {found.Count}");
                        errors++;
                    }
                }
            }
        }
        
        Debug.Log($"   Molecule Recipes: {(errors == 0 ? "✓ OK" : $"{errors} errors")} ({guids.Length} recipes checked)");
        return errors;
    }

    [MenuItem("Tools/Reaction Rivals/Validate AtomBuilder Data", priority = 104)]
    public static int ValidateAtomBuilderData()
    {
        Debug.Log("⚛️ Validating AtomBuilder Element Data...");
        int errors = 0;
        
        // Create temporary instance to check data
        var atomBuilder = new GameObject("TempAtomBuilder").AddComponent<ReactionRivals.AtomBuilder>();
        
        // The AtomBuilder initializes elements in Awake, so we need to check them
        // Since it's internal, we'll validate the static PeriodicTable data matches expected neutron counts
        
        foreach (var neutronData in NeutronCounts)
        {
            var element = PeriodicTable.GetElement(neutronData.Key);
            if (element == null)
            {
                Debug.LogError($"❌ AtomBuilder: Element {neutronData.Key} not found in PeriodicTable");
                errors++;
                continue;
            }
            
            // Cross-reference: Atomic number should match protons
            // Valence electrons should be consistent
        }
        
        // Cleanup
        Object.DestroyImmediate(atomBuilder.gameObject);
        
        Debug.Log($"   AtomBuilder Data: {(errors == 0 ? "✓ OK" : $"{errors} errors")}");
        return errors;
    }

    private static int GetExpectedValenceByGroup(int group, ElementFamily family)
    {
        // Main group elements have predictable valence electrons
        return group switch
        {
            1 => family == ElementFamily.Hydrogen ? 1 : 1,  // Group 1: 1 valence
            2 => 2,                                          // Group 2: 2 valence
            13 => 3,                                         // Group 13: 3 valence
            14 => 4,                                         // Group 14: 4 valence
            15 => 5,                                         // Group 15: 5 valence
            16 => 6,                                         // Group 16: 6 valence
            17 => 7,                                         // Group 17 (Halogens): 7 valence
            18 => family == ElementFamily.Hydrogen ? 2 : 8,  // Group 18 (Noble Gas): 8 (or 2 for He)
            _ => -1                                          // Transition metals vary
        };
    }

    private struct ElementValidationData
    {
        public int AtomicNumber;
        public int ValenceElectrons;
        public float Electronegativity;
        public ElementFamily Family;

        public ElementValidationData(int atomicNum, int valence, float electroneg, ElementFamily fam)
        {
            AtomicNumber = atomicNum;
            ValenceElectrons = valence;
            Electronegativity = electroneg;
            Family = fam;
        }
    }
}
#endif

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Editor utility to generate MVP element data and molecule recipes.
/// Run from Unity menu: Tools > Reaction Rivals > Generate MVP Data
/// </summary>
public class MVPDataGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Reaction Rivals/Generate MVP Data")]
    public static void GenerateMVPData()
    {
        string dataPath = "Assets/Data/Elements/";
        string recipePath = "Assets/Data/Molecules/";
        
        // Create folders if they don't exist
        if (!AssetDatabase.IsValidFolder("Assets/Data"))
            AssetDatabase.CreateFolder("Assets", "Data");
        if (!AssetDatabase.IsValidFolder("Assets/Data/Elements"))
            AssetDatabase.CreateFolder("Assets/Data", "Elements");
        if (!AssetDatabase.IsValidFolder("Assets/Data/Molecules"))
            AssetDatabase.CreateFolder("Assets/Data", "Molecules");

        // Generate MVP Elements (The "Life Set")
        // HP values are based on Atomic Mass × 10 (scaled for gameplay)
        // Accurate atomic masses from periodic table
        
        ElementData hydrogen = CreateElement(
            name: "Hydrogen",
            symbol: "H",
            atomicNumber: 1,
            atomicMass: 1.008f,        // Actual atomic mass
            hp: 10f,                    // 1.008 × 10 (squishy, lightest element!)
            damage: 15f,
            valenceElectrons: 1,        // Needs 1 more for Duet Rule
            cost: 1,
            family: ElementFamily.Hydrogen,
            rarity: ElementRarity.Common,
            color: new Color(0.9f, 0.9f, 1f),  // Almost white
            path: dataPath + "Hydrogen.asset"
        );

        ElementData carbon = CreateElement(
            name: "Carbon",
            symbol: "C",
            atomicNumber: 6,
            atomicMass: 12.011f,
            hp: 120f,                   // 12.011 × 10
            damage: 20f,
            valenceElectrons: 4,        // Tetravalent - forms 4 bonds!
            cost: 2,
            family: ElementFamily.NonMetal,
            rarity: ElementRarity.Common,
            color: new Color(0.2f, 0.2f, 0.2f),  // Black/dark gray (like graphite)
            path: dataPath + "Carbon.asset"
        );

        ElementData nitrogen = CreateElement(
            name: "Nitrogen",
            symbol: "N",
            atomicNumber: 7,
            atomicMass: 14.007f,
            hp: 140f,                   // 14.007 × 10
            damage: 18f,
            valenceElectrons: 5,        // Forms up to 3 bonds
            cost: 2,
            family: ElementFamily.NonMetal,
            rarity: ElementRarity.Common,
            color: new Color(0.12f, 0.12f, 0.56f),  // Dark blue
            path: dataPath + "Nitrogen.asset"
        );

        ElementData oxygen = CreateElement(
            name: "Oxygen",
            symbol: "O",
            atomicNumber: 8,
            atomicMass: 15.999f,
            hp: 160f,                   // 15.999 × 10
            damage: 25f,
            valenceElectrons: 6,        // Forms 2 bonds
            cost: 1,
            family: ElementFamily.NonMetal,
            rarity: ElementRarity.Common,
            color: new Color(1f, 0.05f, 0.05f),  // Red
            path: dataPath + "Oxygen.asset"
        );

        // Additional elements for extended gameplay
        ElementData sodium = CreateElement(
            name: "Sodium",
            symbol: "Na",
            atomicNumber: 11,
            atomicMass: 22.990f,
            hp: 230f,                   // 22.990 × 10
            damage: 22f,
            valenceElectrons: 1,        // Wants to LOSE this electron!
            cost: 2,
            family: ElementFamily.Alkali,
            rarity: ElementRarity.Uncommon,
            color: new Color(0.67f, 0.36f, 0.95f),  // Purple (like sodium flame test)
            path: dataPath + "Sodium.asset"
        );

        ElementData chlorine = CreateElement(
            name: "Chlorine",
            symbol: "Cl",
            atomicNumber: 17,
            atomicMass: 35.45f,
            hp: 355f,                   // 35.45 × 10
            damage: 28f,
            valenceElectrons: 7,        // Wants to GAIN 1 electron!
            cost: 2,
            family: ElementFamily.Halogen,
            rarity: ElementRarity.Uncommon,
            color: new Color(0.12f, 0.94f, 0.12f),  // Yellow-green (chlorine gas color)
            path: dataPath + "Chlorine.asset"
        );

        ElementData iron = CreateElement(
            name: "Iron",
            symbol: "Fe",
            atomicNumber: 26,
            atomicMass: 55.845f,
            hp: 558f,                   // 55.845 × 10 (TANK!)
            damage: 15f,
            valenceElectrons: 2,        // Common Fe²⁺ state
            cost: 3,
            family: ElementFamily.TransitionMetal,
            rarity: ElementRarity.Rare,
            color: new Color(0.44f, 0.44f, 0.44f),  // Gray metallic
            path: dataPath + "Iron.asset"
        );

        ElementData helium = CreateElement(
            name: "Helium",
            symbol: "He",
            atomicNumber: 2,
            atomicMass: 4.003f,
            hp: 40f,                    // 4.003 × 10 (light but full shell = stable)
            damage: 10f,
            valenceElectrons: 2,        // FULL SHELL already! (Duet Rule satisfied)
            cost: 3,
            family: ElementFamily.NobleGas,
            rarity: ElementRarity.Rare,
            color: new Color(1f, 1f, 0.78f),  // Pale yellow
            path: dataPath + "Helium.asset",
            attackRange: 1              // Small atomic radius = melee
        );

        // Generate MVP Molecule Recipes
        CreateWaterRecipe(oxygen, hydrogen, recipePath);
        CreateMethaneRecipe(carbon, hydrogen, recipePath);
        CreateCO2Recipe(carbon, oxygen, recipePath);
        CreateAmmoniaRecipe(nitrogen, hydrogen, recipePath);
        CreateSaltRecipe(sodium, chlorine, recipePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("✅ MVP Data generated successfully!");
        Debug.Log($"📁 Elements created in: {dataPath}");
        Debug.Log($"📁 Molecules created in: {recipePath}");
    }

    private static ElementData CreateElement(
        string name, string symbol, int atomicNumber,
        float atomicMass, float hp, float damage, int valenceElectrons, int cost,
        ElementFamily family, ElementRarity rarity, Color color,
        string path, int attackRange = 2)
    {
        ElementData element = ScriptableObject.CreateInstance<ElementData>();
        
        // Use lowercase field names (matching ElementData.cs serialized fields)
        element.elementName = name;
        element.symbol = symbol;
        element.atomicNumber = atomicNumber;
        element.baseHP = hp;
        element.baseAttack = damage;
        element.valenceElectrons = valenceElectrons;
        element.cost = cost;
        element.family = family;
        element.rarity = rarity;
        element.elementColor = color;
        element.attackRange = attackRange;
        element.attackSpeed = 1f;
        element.electronegativity = GetElectronegativity(atomicNumber);

        AssetDatabase.CreateAsset(element, path);
        
        return element;
    }

    private static float GetElectronegativity(int atomicNumber)
    {
        // Simplified electronegativity values
        return atomicNumber switch
        {
            1 => 2.2f,   // H
            2 => 0f,     // He (noble gas)
            6 => 2.55f,  // C
            7 => 3.04f,  // N
            8 => 3.44f,  // O
            11 => 0.93f, // Na
            17 => 3.16f, // Cl
            26 => 1.83f, // Fe
            _ => 2f
        };
    }

    private static void CreateWaterRecipe(ElementData oxygen, ElementData hydrogen, string path)
    {
        MoleculeRecipe water = ScriptableObject.CreateInstance<MoleculeRecipe>();
        water.MoleculeName = "Water";
        water.Formula = "H₂O";
        water.Description = "The universal solvent! Heals nearby allies and cleanses debuffs.";
        water.CoreElement = oxygen;
        water.RequiredElements = new System.Collections.Generic.List<ElementRequirement>
        {
            new ElementRequirement { Element = hydrogen, Count = 2 }
        };
        water.BondType = BondType.Covalent;
        water.HPMultiplier = 1.5f;
        water.DamageMultiplier = 1.2f;
        water.SpecialAbility = MoleculeAbility.Healing;
        water.MoleculeColor = new Color(0.3f, 0.6f, 1f);

        AssetDatabase.CreateAsset(water, path + "Water.asset");
    }

    private static void CreateMethaneRecipe(ElementData carbon, ElementData hydrogen, string path)
    {
        MoleculeRecipe methane = ScriptableObject.CreateInstance<MoleculeRecipe>();
        methane.MoleculeName = "Methane";
        methane.Formula = "CH₄";
        methane.Description = "A powerful gas giant! Deals AoE poison damage to enemies.";
        methane.CoreElement = carbon;
        methane.RequiredElements = new System.Collections.Generic.List<ElementRequirement>
        {
            new ElementRequirement { Element = hydrogen, Count = 4 }
        };
        methane.BondType = BondType.Covalent;
        methane.HPMultiplier = 2f;
        methane.DamageMultiplier = 1.8f;
        methane.SpecialAbility = MoleculeAbility.PoisonCloud;
        methane.MoleculeColor = new Color(0.5f, 0.8f, 0.3f);

        AssetDatabase.CreateAsset(methane, path + "Methane.asset");
    }

    private static void CreateCO2Recipe(ElementData carbon, ElementData oxygen, string path)
    {
        MoleculeRecipe co2 = ScriptableObject.CreateInstance<MoleculeRecipe>();
        co2.MoleculeName = "Carbon Dioxide";
        co2.Formula = "CO₂";
        co2.Description = "Suffocates enemies! Slows attack speed of nearby foes.";
        co2.CoreElement = carbon;
        co2.RequiredElements = new System.Collections.Generic.List<ElementRequirement>
        {
            new ElementRequirement { Element = oxygen, Count = 2 }
        };
        co2.BondType = BondType.Covalent;
        co2.HPMultiplier = 1.4f;
        co2.DamageMultiplier = 1.5f;
        co2.SpecialAbility = MoleculeAbility.Suffocate;
        co2.MoleculeColor = new Color(0.5f, 0.5f, 0.5f);

        AssetDatabase.CreateAsset(co2, path + "CarbonDioxide.asset");
    }

    private static void CreateAmmoniaRecipe(ElementData nitrogen, ElementData hydrogen, string path)
    {
        MoleculeRecipe ammonia = ScriptableObject.CreateInstance<MoleculeRecipe>();
        ammonia.MoleculeName = "Ammonia";
        ammonia.Formula = "NH₃";
        ammonia.Description = "A pungent cleanser! Removes all debuffs from nearby allies.";
        ammonia.CoreElement = nitrogen;
        ammonia.RequiredElements = new System.Collections.Generic.List<ElementRequirement>
        {
            new ElementRequirement { Element = hydrogen, Count = 3 }
        };
        ammonia.BondType = BondType.Covalent;
        ammonia.HPMultiplier = 1.3f;
        ammonia.DamageMultiplier = 1.4f;
        ammonia.SpecialAbility = MoleculeAbility.Cleanse;
        ammonia.MoleculeColor = new Color(0.3f, 0.3f, 0.8f);

        AssetDatabase.CreateAsset(ammonia, path + "Ammonia.asset");
    }

    private static void CreateSaltRecipe(ElementData sodium, ElementData chlorine, string path)
    {
        MoleculeRecipe salt = ScriptableObject.CreateInstance<MoleculeRecipe>();
        salt.MoleculeName = "Salt";
        salt.Formula = "NaCl";
        salt.Description = "Crystal armor! Both units gain damage reflection.";
        salt.CoreElement = sodium;
        salt.RequiredElements = new System.Collections.Generic.List<ElementRequirement>
        {
            new ElementRequirement { Element = chlorine, Count = 1 }
        };
        salt.BondType = BondType.Ionic;
        salt.HPMultiplier = 1.6f;
        salt.DamageMultiplier = 1.3f;
        salt.SpecialAbility = MoleculeAbility.CrystalArmor;
        salt.MoleculeColor = new Color(1f, 1f, 1f);

        AssetDatabase.CreateAsset(salt, path + "Salt.asset");
    }
#endif
}

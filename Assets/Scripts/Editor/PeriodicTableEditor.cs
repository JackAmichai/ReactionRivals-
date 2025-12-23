using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor utilities for periodic table system
/// </summary>
public class PeriodicTableEditor : EditorWindow
{
    [MenuItem("ReactionRivals/Create Level Progression Asset")]
    public static void CreateLevelProgressionAsset()
    {
        LevelElementProgression asset = ScriptableObject.CreateInstance<LevelElementProgression>();
        asset.GenerateDefaultProgression();

        string path = "Assets/ScriptableObjects/Data";
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Data");
        }

        AssetDatabase.CreateAsset(asset, path + "/LevelElementProgression.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        Debug.Log("Created LevelElementProgression asset with default progression for all 118 elements!");
    }

    [MenuItem("ReactionRivals/Create Periodic Table Prefab")]
    public static void CreatePeriodicTablePrefab()
    {
        // Create root object
        GameObject root = new GameObject("PeriodicTable");
        
        // Add Canvas if creating standalone
        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        root.AddComponent<UnityEngine.UI.CanvasScaler>();
        root.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // Create table container
        GameObject tableContainer = new GameObject("TableContainer");
        tableContainer.transform.SetParent(root.transform);
        RectTransform containerRect = tableContainer.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0.5f);
        containerRect.anchorMax = new Vector2(0.5f, 0.5f);
        containerRect.pivot = new Vector2(0, 1);
        containerRect.anchoredPosition = new Vector2(-450, 200);
        containerRect.sizeDelta = new Vector2(1000, 600);

        // Add PeriodicTableUI component
        PeriodicTableUI tableUI = root.AddComponent<PeriodicTableUI>();

        // Create element cell prefab
        GameObject cellPrefab = CreateElementCellPrefab();

        // Create tooltip
        GameObject tooltip = new GameObject("ElementTooltip");
        tooltip.transform.SetParent(root.transform);
        RectTransform tooltipRect = tooltip.AddComponent<RectTransform>();
        tooltipRect.sizeDelta = new Vector2(220, 220);
        UnityEngine.UI.Image tooltipBg = tooltip.AddComponent<UnityEngine.UI.Image>();
        tooltipBg.color = new Color(0.1f, 0.1f, 0.15f, 0.95f);
        tooltip.AddComponent<ElementTooltip>();

        Debug.Log("Periodic Table prefab structure created! Configure the PeriodicTableUI component with the cell prefab and level progression asset.");
    }

    private static GameObject CreateElementCellPrefab()
    {
        GameObject cell = new GameObject("ElementCell");
        RectTransform rect = cell.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(50, 60);

        UnityEngine.UI.Image bg = cell.AddComponent<UnityEngine.UI.Image>();
        bg.color = new Color(0.3f, 0.3f, 0.4f);

        cell.AddComponent<PeriodicTableCell>();

        // Save as prefab
        string prefabPath = "Assets/Prefabs";
        if (!AssetDatabase.IsValidFolder(prefabPath))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        string fullPath = prefabPath + "/ElementCell.prefab";
        
        // Check if prefab already exists
        GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(fullPath);
        if (existingPrefab != null)
        {
            PrefabUtility.SaveAsPrefabAsset(cell, fullPath);
        }
        else
        {
            PrefabUtility.SaveAsPrefabAsset(cell, fullPath);
        }

        Object.DestroyImmediate(cell);

        return AssetDatabase.LoadAssetAtPath<GameObject>(fullPath);
    }

    [MenuItem("ReactionRivals/Generate All Element ScriptableObjects")]
    public static void GenerateAllElementScriptableObjects()
    {
        string path = "Assets/ScriptableObjects/Elements";
        
        // Create folders if needed
        if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects"))
        {
            AssetDatabase.CreateFolder("Assets", "ScriptableObjects");
        }
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Elements");
        }

        int created = 0;
        foreach (var element in PeriodicTable.Elements)
        {
            ElementData data = ScriptableObject.CreateInstance<ElementData>();
            
            data.elementName = element.Name;
            data.symbol = element.Symbol;
            data.atomicNumber = element.AtomicNumber;
            data.valenceElectrons = element.ValenceElectrons;
            data.electronegativity = element.Electronegativity;
            data.baseHP = element.GetGameHP();
            data.baseAttack = CalculateAttack(element);
            data.attackSpeed = CalculateAttackSpeed(element);
            data.elementColor = element.ElementColor;
            data.rarity = CalculateRarity(element);

            string assetPath = $"{path}/{element.Symbol}_{element.Name}.asset";
            AssetDatabase.CreateAsset(data, assetPath);
            created++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Created {created} Element ScriptableObjects!");
    }

    private static float CalculateAttack(PeriodicElementInfo element)
    {
        // Base attack on valence electrons and electronegativity
        float baseAttack = 10f + (element.ValenceElectrons * 2f);
        if (element.Electronegativity > 0)
        {
            baseAttack += element.Electronegativity * 3f;
        }
        return Mathf.Round(baseAttack);
    }

    private static float CalculateAttackSpeed(PeriodicElementInfo element)
    {
        // Lighter elements attack faster
        float speed = 1.5f - (element.AtomicMass / 300f);
        return Mathf.Clamp(speed, 0.5f, 1.5f);
    }

    private static ElementRarity CalculateRarity(PeriodicElementInfo element)
    {
        // Rarity based on element family and atomic number
        if (element.Family == ElementFamily.Actinide || element.AtomicNumber > 103)
            return ElementRarity.Legendary;
        
        if (element.Family == ElementFamily.Lanthanide || element.AtomicNumber > 86)
            return ElementRarity.Epic;
        
        if (element.Family == ElementFamily.TransitionMetal && element.AtomicNumber > 70)
            return ElementRarity.Rare;
        
        if (element.Family == ElementFamily.NobleGas || element.Family == ElementFamily.TransitionMetal)
            return ElementRarity.Rare;
        
        if (element.AtomicNumber <= 10)
            return ElementRarity.Common;
        
        if (element.AtomicNumber <= 20)
            return ElementRarity.Uncommon;
        
        return ElementRarity.Uncommon;
    }

    [MenuItem("ReactionRivals/Print Periodic Table Stats")]
    public static void PrintPeriodicTableStats()
    {
        Debug.Log("=== PERIODIC TABLE STATISTICS ===");
        Debug.Log($"Total Elements: {PeriodicTable.Elements.Length}");
        
        // Count by family
        System.Collections.Generic.Dictionary<ElementFamily, int> familyCounts = 
            new System.Collections.Generic.Dictionary<ElementFamily, int>();
        
        foreach (var element in PeriodicTable.Elements)
        {
            if (!familyCounts.ContainsKey(element.Family))
                familyCounts[element.Family] = 0;
            familyCounts[element.Family]++;
        }

        Debug.Log("\n=== Elements by Family ===");
        foreach (var kvp in familyCounts)
        {
            Debug.Log($"{kvp.Key}: {kvp.Value}");
        }

        // Period counts
        Debug.Log("\n=== Elements by Period ===");
        for (int p = 1; p <= 7; p++)
        {
            int count = 0;
            foreach (var e in PeriodicTable.Elements)
            {
                if (e.Period == p) count++;
            }
            Debug.Log($"Period {p}: {count}");
        }

        // Verify first few elements
        Debug.Log("\n=== Sample Element Data ===");
        string[] samples = { "H", "C", "O", "Na", "Fe", "Au", "U" };
        foreach (string sym in samples)
        {
            var e = PeriodicTable.GetElement(sym);
            if (e != null)
            {
                Debug.Log($"{e.Symbol} ({e.Name}): Z={e.AtomicNumber}, Mass={e.AtomicMass}, " +
                         $"ValE={e.ValenceElectrons}, EN={e.Electronegativity}");
            }
        }
    }
}

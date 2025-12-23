using UnityEngine;

/// <summary>
/// Bootstrap component to set up the game scene.
/// Attach to a GameObject in your main scene.
/// </summary>
public class GameBootstrap : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject hexCellPrefab;
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private GameObject moleculePrefab;

    [Header("Element Data")]
    [SerializeField] private ElementData[] startingElements;

    [Header("Molecule Recipes")]
    [SerializeField] private MoleculeRecipe[] allRecipes;

    [Header("Shop Configuration")]
    [SerializeField] private RarityOdds[] rarityOddsByLevel;

    private void Awake()
    {
        SetupManagers();
    }

    private void SetupManagers()
    {
        // Find or create GameManager
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            GameObject gmObj = new GameObject("GameManager");
            gameManager = gmObj.AddComponent<GameManager>();
        }

        // Find or create HexGrid
        HexGrid hexGrid = FindObjectOfType<HexGrid>();
        if (hexGrid == null)
        {
            GameObject gridObj = new GameObject("HexGrid");
            hexGrid = gridObj.AddComponent<HexGrid>();
        }

        // Find or create BondingManager
        BondingManager bondingManager = FindObjectOfType<BondingManager>();
        if (bondingManager == null)
        {
            GameObject bondObj = new GameObject("BondingManager");
            bondingManager = bondObj.AddComponent<BondingManager>();
        }
        if (allRecipes != null && allRecipes.Length > 0)
        {
            bondingManager.AllRecipes = new System.Collections.Generic.List<MoleculeRecipe>(allRecipes);
        }

        // Find or create ShopManager
        ShopManager shopManager = FindObjectOfType<ShopManager>();
        if (shopManager == null)
        {
            GameObject shopObj = new GameObject("ShopManager");
            shopManager = shopObj.AddComponent<ShopManager>();
        }
        if (startingElements != null && startingElements.Length > 0)
        {
            shopManager.ElementPool = new System.Collections.Generic.List<ElementData>(startingElements);
        }
        if (rarityOddsByLevel != null && rarityOddsByLevel.Length > 0)
        {
            shopManager.OddsByLevel = rarityOddsByLevel;
        }

        // Find or create UIManager
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            GameObject uiObj = new GameObject("UIManager");
            uiManager = uiObj.AddComponent<UIManager>();
        }

        Debug.Log("🎮 Game Bootstrap complete! All managers initialized.");
    }

    /// <summary>
    /// Quick test: Spawn a unit for debugging
    /// </summary>
    [ContextMenu("Spawn Test Hydrogen")]
    public void SpawnTestHydrogen()
    {
        if (startingElements == null || startingElements.Length == 0)
        {
            Debug.LogError("No starting elements configured!");
            return;
        }

        ElementData hydrogen = System.Array.Find(startingElements, e => e.Symbol == "H");
        if (hydrogen == null)
        {
            hydrogen = startingElements[0];
        }

        SpawnTestUnit(hydrogen, Vector3.zero);
    }

    /// <summary>
    /// Quick test: Spawn a water molecule setup
    /// </summary>
    [ContextMenu("Spawn Water Setup (1O + 2H)")]
    public void SpawnWaterSetup()
    {
        if (startingElements == null || startingElements.Length == 0)
        {
            Debug.LogError("No starting elements configured!");
            return;
        }

        ElementData oxygen = System.Array.Find(startingElements, e => e.Symbol == "O");
        ElementData hydrogen = System.Array.Find(startingElements, e => e.Symbol == "H");

        if (oxygen == null || hydrogen == null)
        {
            Debug.LogError("Need Oxygen and Hydrogen in starting elements!");
            return;
        }

        // Spawn in hex positions that are adjacent
        SpawnTestUnit(oxygen, Vector3.zero);
        SpawnTestUnit(hydrogen, new Vector3(HexCell.HexWidth, 0, 0));
        SpawnTestUnit(hydrogen, new Vector3(HexCell.HexWidth * 0.5f, HexCell.HexVertSpacing, 0));

        Debug.Log("💧 Water setup spawned! Check BondingManager.CheckAllBonds()");
    }

    private Unit SpawnTestUnit(ElementData element, Vector3 position)
    {
        GameObject unitObj = unitPrefab != null 
            ? Instantiate(unitPrefab, position, Quaternion.identity)
            : new GameObject($"Unit_{element.Symbol}");

        unitObj.transform.position = position;

        // Add required components if missing
        if (unitObj.GetComponent<SpriteRenderer>() == null)
        {
            SpriteRenderer sr = unitObj.AddComponent<SpriteRenderer>();
            sr.color = element.ElementColor;
        }
        if (unitObj.GetComponent<Collider2D>() == null)
        {
            unitObj.AddComponent<CircleCollider2D>();
        }

        Unit unit = unitObj.GetComponent<Unit>();
        if (unit == null)
            unit = unitObj.AddComponent<Unit>();

        unit.Initialize(element);

        Debug.Log($"⚛️ Spawned {element.Symbol} at {position}");
        return unit;
    }
}

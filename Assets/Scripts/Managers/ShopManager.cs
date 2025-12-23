using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the element shop where players buy atoms using Energy (ATP).
/// Handles shop refreshing, element pool, and rarity odds.
/// </summary>
public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("Shop Configuration")]
    [Tooltip("Number of elements shown in shop")]
    public int ShopSize = 5;
    
    [Tooltip("Cost to refresh the shop")]
    public int RefreshCost = 2;

    [Header("Element Pool")]
    [Tooltip("All available elements to appear in shop")]
    public List<ElementData> ElementPool = new List<ElementData>();
    
    [Tooltip("Current elements displayed in shop")]
    public List<ElementData> CurrentShop = new List<ElementData>();

    [Header("Rarity Odds (by Player Level)")]
    [Tooltip("Odds for each rarity tier at each level")]
    public RarityOdds[] OddsByLevel;

    [Header("Pool Quantities")]
    [Tooltip("How many of each element exist in the shared pool")]
    public int CommonPoolSize = 29;
    public int UncommonPoolSize = 22;
    public int RarePoolSize = 18;
    public int EpicPoolSize = 12;
    public int LegendaryPoolSize = 10;

    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform shopUIParent;
    [SerializeField] private GameObject shopSlotPrefab;

    // Pool tracking (how many of each element remain)
    private Dictionary<ElementData, int> remainingPool = new Dictionary<ElementData, int>();
    
    // Shop UI slots
    private List<ShopSlot> shopSlots = new List<ShopSlot>();

    // Events
    public System.Action OnShopRefreshed;
    public System.Action<int, ElementData> OnElementPurchased;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        InitializePool();
        CreateShopUI();
        RefreshShop();
    }

    /// <summary>
    /// Initialize the element pool with quantities based on rarity
    /// </summary>
    private void InitializePool()
    {
        remainingPool.Clear();

        foreach (var element in ElementPool)
        {
            int poolSize = element.Rarity switch
            {
                ElementRarity.Common => CommonPoolSize,
                ElementRarity.Uncommon => UncommonPoolSize,
                ElementRarity.Rare => RarePoolSize,
                ElementRarity.Epic => EpicPoolSize,
                ElementRarity.Legendary => LegendaryPoolSize,
                _ => CommonPoolSize
            };
            
            remainingPool[element] = poolSize;
        }
    }

    /// <summary>
    /// Create the shop UI slots
    /// </summary>
    private void CreateShopUI()
    {
        shopSlots.Clear();
        
        for (int i = 0; i < ShopSize; i++)
        {
            if (shopSlotPrefab != null && shopUIParent != null)
            {
                GameObject slotObj = Instantiate(shopSlotPrefab, shopUIParent);
                ShopSlot slot = slotObj.GetComponent<ShopSlot>();
                if (slot == null)
                    slot = slotObj.AddComponent<ShopSlot>();
                
                slot.Initialize(i, this);
                shopSlots.Add(slot);
            }
        }
    }

    /// <summary>
    /// Refresh the shop with new random elements
    /// </summary>
    public void RefreshShop(bool free = false)
    {
        // Check cost
        if (!free && gameManager != null)
        {
            if (!gameManager.SpendEnergy(RefreshCost))
            {
                Debug.Log("Not enough energy to refresh shop!");
                return;
            }
        }

        // Return current shop elements to pool
        foreach (var element in CurrentShop)
        {
            if (element != null)
                ReturnToPool(element);
        }

        CurrentShop.Clear();

        // Get player level for odds
        int level = gameManager?.CurrentLevel ?? 1;
        RarityOdds odds = GetOddsForLevel(level);

        // Roll new shop
        for (int i = 0; i < ShopSize; i++)
        {
            ElementData element = RollElement(odds);
            CurrentShop.Add(element);
            
            // Remove from pool
            if (element != null && remainingPool.ContainsKey(element))
            {
                remainingPool[element]--;
            }
        }

        // Update UI
        UpdateShopUI();
        OnShopRefreshed?.Invoke();
    }

    /// <summary>
    /// Roll a random element based on rarity odds
    /// </summary>
    private ElementData RollElement(RarityOdds odds)
    {
        // Roll for rarity
        float roll = Random.value * 100f;
        ElementRarity targetRarity;

        if (roll < odds.LegendaryChance)
            targetRarity = ElementRarity.Legendary;
        else if (roll < odds.LegendaryChance + odds.EpicChance)
            targetRarity = ElementRarity.Epic;
        else if (roll < odds.LegendaryChance + odds.EpicChance + odds.RareChance)
            targetRarity = ElementRarity.Rare;
        else if (roll < odds.LegendaryChance + odds.EpicChance + odds.RareChance + odds.UncommonChance)
            targetRarity = ElementRarity.Uncommon;
        else
            targetRarity = ElementRarity.Common;

        // Get available elements of this rarity
        var available = ElementPool.Where(e => 
            e.Rarity == targetRarity && 
            remainingPool.ContainsKey(e) && 
            remainingPool[e] > 0
        ).ToList();

        // Fallback if no elements available at rolled rarity
        if (available.Count == 0)
        {
            available = ElementPool.Where(e => 
                remainingPool.ContainsKey(e) && 
                remainingPool[e] > 0
            ).ToList();
        }

        if (available.Count == 0)
        {
            Debug.LogWarning("Element pool depleted!");
            return null;
        }

        return available[Random.Range(0, available.Count)];
    }

    /// <summary>
    /// Get rarity odds for a player level
    /// </summary>
    private RarityOdds GetOddsForLevel(int level)
    {
        if (OddsByLevel == null || OddsByLevel.Length == 0)
        {
            // Default odds if not configured
            return new RarityOdds
            {
                CommonChance = 70f,
                UncommonChance = 25f,
                RareChance = 5f,
                EpicChance = 0f,
                LegendaryChance = 0f
            };
        }

        int index = Mathf.Clamp(level - 1, 0, OddsByLevel.Length - 1);
        return OddsByLevel[index];
    }

    /// <summary>
    /// Purchase an element from the shop
    /// </summary>
    public bool PurchaseElement(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= CurrentShop.Count)
            return false;

        ElementData element = CurrentShop[slotIndex];
        if (element == null) return false;

        // Check cost
        if (gameManager != null && !gameManager.SpendEnergy(element.Cost))
        {
            Debug.Log($"Not enough energy to buy {element.Symbol}!");
            return false;
        }

        // Try to place on bench
        HexCell benchCell = HexGrid.Instance?.GetEmptyBenchCell();
        if (benchCell == null)
        {
            Debug.Log("Bench is full!");
            // Refund
            gameManager?.AddEnergy(element.Cost);
            return false;
        }

        // Spawn unit
        SpawnUnit(element, benchCell);
        
        // Remove from shop
        CurrentShop[slotIndex] = null;
        
        // Update UI
        if (slotIndex < shopSlots.Count)
        {
            shopSlots[slotIndex].SetEmpty();
        }

        OnElementPurchased?.Invoke(slotIndex, element);
        return true;
    }

    /// <summary>
    /// Spawn a unit from element data
    /// </summary>
    private Unit SpawnUnit(ElementData element, HexCell cell)
    {
        GameObject unitObj;
        
        if (element.UnitPrefab != null)
        {
            unitObj = Instantiate(element.UnitPrefab, cell.WorldPosition, Quaternion.identity);
        }
        else
        {
            // Create default unit
            unitObj = new GameObject($"Unit_{element.Symbol}");
            unitObj.transform.position = cell.WorldPosition;
            
            // Add required components
            SpriteRenderer sr = unitObj.AddComponent<SpriteRenderer>();
            sr.sprite = element.Icon;
            sr.color = element.ElementColor;
            
            unitObj.AddComponent<CircleCollider2D>();
        }

        Unit unit = unitObj.GetComponent<Unit>();
        if (unit == null)
            unit = unitObj.AddComponent<Unit>();

        unit.Initialize(element);
        cell.PlaceUnit(unit);

        // Check for auto-combine
        gameManager?.CheckForUpgrades(unit);

        Debug.Log($"⚛️ Purchased {element.Symbol} for {element.Cost} ATP");
        return unit;
    }

    /// <summary>
    /// Return an element to the pool (when selling or refreshing)
    /// </summary>
    public void ReturnToPool(ElementData element)
    {
        if (element == null) return;
        
        if (remainingPool.ContainsKey(element))
        {
            remainingPool[element]++;
        }
        else
        {
            int maxPool = element.Rarity switch
            {
                ElementRarity.Common => CommonPoolSize,
                ElementRarity.Uncommon => UncommonPoolSize,
                ElementRarity.Rare => RarePoolSize,
                ElementRarity.Epic => EpicPoolSize,
                ElementRarity.Legendary => LegendaryPoolSize,
                _ => CommonPoolSize
            };
            remainingPool[element] = Mathf.Min(remainingPool.GetValueOrDefault(element, 0) + 1, maxPool);
        }
    }

    /// <summary>
    /// Sell a unit back to the shop
    /// </summary>
    public void SellUnit(Unit unit)
    {
        if (unit == null || unit.Data == null) return;

        // Calculate sell value (cost - 1, minimum 1)
        int sellValue = Mathf.Max(1, unit.Data.Cost - 1);
        
        // Bonus for upgraded units
        if (unit.StarLevel == 2) sellValue *= 3;
        if (unit.StarLevel == 3) sellValue *= 9;

        gameManager?.AddEnergy(sellValue);

        // Return to pool
        ReturnToPool(unit.Data);
        
        // Remove unit
        if (unit.CurrentCell != null)
            unit.CurrentCell.RemoveUnit();
        Destroy(unit.gameObject);

        Debug.Log($"💰 Sold {unit.Data.Symbol} for {sellValue} ATP");
    }

    /// <summary>
    /// Update the shop UI
    /// </summary>
    private void UpdateShopUI()
    {
        for (int i = 0; i < shopSlots.Count; i++)
        {
            if (i < CurrentShop.Count && CurrentShop[i] != null)
            {
                shopSlots[i].SetElement(CurrentShop[i]);
            }
            else
            {
                shopSlots[i].SetEmpty();
            }
        }
    }
}

/// <summary>
/// Rarity odds configuration for shop rolls
/// </summary>
[System.Serializable]
public class RarityOdds
{
    [Range(0, 100)] public float CommonChance = 70f;
    [Range(0, 100)] public float UncommonChance = 25f;
    [Range(0, 100)] public float RareChance = 5f;
    [Range(0, 100)] public float EpicChance = 0f;
    [Range(0, 100)] public float LegendaryChance = 0f;
}

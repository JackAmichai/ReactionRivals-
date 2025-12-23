using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Central game manager handling game state, phases, and round flow.
/// Follows the Auto-Battler structure: Shop → Prep → Combat → Repeat
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public GamePhase CurrentPhase = GamePhase.Prep;
    public int CurrentRound = 1;
    public int PlayerHP = 100;
    public int EnemyHP = 100;

    [Header("Economy")]
    [Tooltip("Currency for buying elements (ATP = Adenosine Triphosphate)")]
    public int Energy = 10;
    public int MaxEnergy = 10;
    public int InterestCap = 5;
    public int BaseIncomePerRound = 5;

    [Header("Board Limits")]
    public int MaxUnitsOnBoard = 5;
    public int CurrentLevel = 1;

    [Header("Phase Timing")]
    public float PrepPhaseDuration = 30f;
    public float CombatPhaseDuration = 60f;
    private float phaseTimer;

    [Header("Drag State (for UI)")]
    public bool IsDragging = false;
    public Unit DraggedUnit = null;
    public bool CanMoveUnits => CurrentPhase == GamePhase.Prep || CurrentPhase == GamePhase.Shop;

    [Header("References")]
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private BondingManager bondingManager;
    [SerializeField] private HexGrid hexGrid;
    [SerializeField] private UIManager uiManager;

    // Events
    public System.Action<GamePhase> OnPhaseChanged;
    public System.Action<int> OnRoundChanged;
    public System.Action<int> OnEnergyChanged;
    public System.Action<int, int> OnPlayerHPChanged;
    public System.Action OnGameOver;

    private Coroutine combatCoroutine;

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
        InitializeGame();
    }

    private void Update()
    {
        UpdatePhaseTimer();
    }

    /// <summary>
    /// Initialize a new game
    /// </summary>
    public void InitializeGame()
    {
        CurrentRound = 1;
        PlayerHP = 100;
        EnemyHP = 100;
        Energy = 10;
        CurrentLevel = 1;
        MaxUnitsOnBoard = GetUnitCapForLevel(CurrentLevel);

        // Start in prep phase
        SetPhase(GamePhase.Prep);
        
        // Generate initial shop
        shopManager?.RefreshShop();

        Debug.Log("⚗️ Reaction Rivals - Game Started!");
    }

    /// <summary>
    /// Update the phase timer
    /// </summary>
    private void UpdatePhaseTimer()
    {
        if (CurrentPhase == GamePhase.Prep || CurrentPhase == GamePhase.Shop)
        {
            phaseTimer -= Time.deltaTime;
            
            if (phaseTimer <= 0)
            {
                StartCombat();
            }
        }
    }

    #region Phase Management

    /// <summary>
    /// Set the current game phase
    /// </summary>
    public void SetPhase(GamePhase newPhase)
    {
        CurrentPhase = newPhase;
        
        switch (newPhase)
        {
            case GamePhase.Shop:
            case GamePhase.Prep:
                phaseTimer = PrepPhaseDuration;
                break;
                
            case GamePhase.Combat:
                phaseTimer = CombatPhaseDuration;
                break;
        }

        OnPhaseChanged?.Invoke(newPhase);
        Debug.Log($"📍 Phase changed to: {newPhase}");
    }

    /// <summary>
    /// Start the combat phase
    /// </summary>
    public void StartCombat()
    {
        SetPhase(GamePhase.Combat);
        
        // Check for bonds before combat
        bondingManager?.CheckAllBonds();
        
        // Start combat for all units
        var playerUnits = hexGrid?.GetAllBoardUnits(0);
        var enemyUnits = hexGrid?.GetAllBoardUnits(1);

        foreach (var unit in playerUnits)
        {
            unit.GetComponent<UnitCombat>()?.EnterCombat();
        }
        foreach (var unit in enemyUnits)
        {
            unit.GetComponent<UnitCombat>()?.EnterCombat();
        }

        // Start combat monitoring
        combatCoroutine = StartCoroutine(CombatLoop());
    }

    /// <summary>
    /// Combat loop - monitors for round end
    /// </summary>
    private IEnumerator CombatLoop()
    {
        while (CurrentPhase == GamePhase.Combat)
        {
            var playerUnits = hexGrid?.GetAllBoardUnits(0);
            var enemyUnits = hexGrid?.GetAllBoardUnits(1);

            // Check for round end conditions
            bool playerWon = enemyUnits == null || enemyUnits.Count == 0;
            bool enemyWon = playerUnits == null || playerUnits.Count == 0;

            if (playerWon || enemyWon)
            {
                EndCombat(playerWon);
                yield break;
            }

            // Timeout check
            if (phaseTimer <= 0)
            {
                // Compare remaining HP
                float playerTotalHP = 0, enemyTotalHP = 0;
                foreach (var u in playerUnits) playerTotalHP += u.CurrentHP;
                foreach (var u in enemyUnits) enemyTotalHP += u.CurrentHP;
                
                EndCombat(playerTotalHP > enemyTotalHP);
                yield break;
            }

            phaseTimer -= Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// End the combat phase
    /// </summary>
    public void EndCombat(bool playerWon)
    {
        if (combatCoroutine != null)
        {
            StopCoroutine(combatCoroutine);
            combatCoroutine = null;
        }

        // Exit combat for all units
        foreach (var unit in hexGrid?.GetAllBoardUnits(0) ?? new List<Unit>())
        {
            unit.GetComponent<UnitCombat>()?.ExitCombat();
        }

        // Calculate damage
        if (!playerWon)
        {
            var remainingEnemies = hexGrid?.GetAllBoardUnits(1);
            int damage = CalculateDamage(remainingEnemies);
            TakeDamage(damage);
        }

        // Break molecules
        bondingManager?.BreakAllMolecules();

        // Check for game over
        if (PlayerHP <= 0)
        {
            GameOver(false);
            return;
        }

        // Start next round
        NextRound();
    }

    /// <summary>
    /// Calculate damage based on surviving enemy units
    /// </summary>
    private int CalculateDamage(List<Unit> remainingUnits)
    {
        if (remainingUnits == null) return 0;
        
        int damage = CurrentRound; // Base damage scales with round
        
        foreach (var unit in remainingUnits)
        {
            damage += unit.StarLevel; // Each star level adds damage
        }
        
        return damage;
    }

    /// <summary>
    /// Player takes damage
    /// </summary>
    public void TakeDamage(int damage)
    {
        int oldHP = PlayerHP;
        PlayerHP = Mathf.Max(0, PlayerHP - damage);
        OnPlayerHPChanged?.Invoke(PlayerHP, oldHP - PlayerHP);
        
        Debug.Log($"💔 Player took {damage} damage! HP: {PlayerHP}");
    }

    /// <summary>
    /// Start the next round
    /// </summary>
    public void NextRound()
    {
        CurrentRound++;
        OnRoundChanged?.Invoke(CurrentRound);

        // Income calculation
        int income = CalculateIncome();
        AddEnergy(income);

        // Refresh shop
        shopManager?.RefreshShop();

        // Reset unit positions/HP
        ResetUnitsForNewRound();

        // Spawn enemy units for new round
        SpawnEnemyUnits();

        SetPhase(GamePhase.Prep);
        
        Debug.Log($"🔄 Round {CurrentRound} started! Income: {income} ATP");
    }

    /// <summary>
    /// Calculate income for the round
    /// </summary>
    private int CalculateIncome()
    {
        int income = BaseIncomePerRound;
        
        // Interest (1 per 10 energy, capped)
        int interest = Mathf.Min(Energy / 10, InterestCap);
        income += interest;

        // Win/loss streaks would add more here
        
        return income;
    }

    /// <summary>
    /// Reset units for a new round
    /// </summary>
    private void ResetUnitsForNewRound()
    {
        foreach (var unit in hexGrid?.GetAllBoardUnits(0) ?? new List<Unit>())
        {
            unit.CurrentHP = unit.MaxHP;
            unit.IsInMolecule = false;
            unit.ParentMolecule = null;
            
            var combat = unit.GetComponent<UnitCombat>();
            if (combat != null)
            {
                combat.CurrentElectrons = combat.ValenceElectrons;
            }
        }
    }

    /// <summary>
    /// Spawn enemy units for the round (simple AI for now)
    /// </summary>
    private void SpawnEnemyUnits()
    {
        // TODO: Implement enemy spawning based on round number
        // For MVP: Mirror player's units or spawn preset combinations
    }

    /// <summary>
    /// Handle game over
    /// </summary>
    public void GameOver(bool victory)
    {
        CurrentPhase = GamePhase.GameOver;
        OnGameOver?.Invoke();
        
        Debug.Log(victory ? "🏆 Victory!" : "💀 Defeat!");
    }

    #endregion

    #region Economy

    /// <summary>
    /// Add energy (ATP)
    /// </summary>
    public void AddEnergy(int amount)
    {
        Energy = Mathf.Min(Energy + amount, MaxEnergy);
        OnEnergyChanged?.Invoke(Energy);
    }

    /// <summary>
    /// Spend energy
    /// </summary>
    public bool SpendEnergy(int amount)
    {
        if (Energy < amount) return false;
        
        Energy -= amount;
        OnEnergyChanged?.Invoke(Energy);
        return true;
    }

    /// <summary>
    /// Buy XP to level up (increases unit cap)
    /// </summary>
    public bool BuyXP()
    {
        if (!SpendEnergy(4)) return false;
        
        // Add XP logic here
        // For simplicity, just level up every purchase for MVP
        CurrentLevel = Mathf.Min(CurrentLevel + 1, 9);
        MaxUnitsOnBoard = GetUnitCapForLevel(CurrentLevel);
        
        Debug.Log($"⬆️ Leveled up to {CurrentLevel}! Unit cap: {MaxUnitsOnBoard}");
        return true;
    }

    /// <summary>
    /// Get unit capacity for a level
    /// </summary>
    private int GetUnitCapForLevel(int level)
    {
        return level switch
        {
            1 => 1,
            2 => 2,
            3 => 3,
            4 => 4,
            5 => 5,
            6 => 6,
            7 => 7,
            8 => 8,
            9 => 9,
            _ => 5
        };
    }

    #endregion

    #region Unit Management

    /// <summary>
    /// Check if player can add another unit to the board
    /// </summary>
    public bool CanAddUnitToBoard()
    {
        int currentUnits = hexGrid?.GetAllBoardUnits(0)?.Count ?? 0;
        return currentUnits < MaxUnitsOnBoard;
    }

    /// <summary>
    /// Check for and combine duplicate units (3 copies → upgrade)
    /// </summary>
    public void CheckForUpgrades(Unit newUnit)
    {
        if (newUnit == null || newUnit.StarLevel >= 3) return;

        var allUnits = new List<Unit>();
        allUnits.AddRange(hexGrid?.GetAllBoardUnits(0) ?? new List<Unit>());
        allUnits.AddRange(hexGrid?.GetAllBenchUnits() ?? new List<Unit>());

        // Find matching units
        var matches = allUnits.FindAll(u => 
            u != newUnit &&
            u.Data == newUnit.Data &&
            u.StarLevel == newUnit.StarLevel
        );

        if (matches.Count >= 2)
        {
            // Upgrade!
            newUnit.TryUpgrade();
            
            // Destroy the used copies
            for (int i = 0; i < 2; i++)
            {
                if (matches[i].CurrentCell != null)
                    matches[i].CurrentCell.RemoveUnit();
                Destroy(matches[i].gameObject);
            }
            
            // Check for further upgrades (2★ → 3★)
            CheckForUpgrades(newUnit);
        }
    }

    #endregion
}

/// <summary>
/// Game phases following auto-battler structure
/// </summary>
public enum GamePhase
{
    Shop,       // Buying elements from the shop
    Prep,       // Arranging units on the board
    Combat,     // Automated battle
    GameOver    // Game ended
}

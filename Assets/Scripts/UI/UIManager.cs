using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Manages all UI elements including shop, unit info, phase indicators, and player stats.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Main Panels")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject unitInfoPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject moleculePreviewPanel;

    [Header("Top Bar - Player Stats")]
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private Slider hpSlider;

    [Header("Phase Display")]
    [SerializeField] private TextMeshProUGUI phaseText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image phaseIcon;

    [Header("Shop UI")]
    [SerializeField] private Transform shopSlotsContainer;
    [SerializeField] private Button refreshButton;
    [SerializeField] private TextMeshProUGUI refreshCostText;
    [SerializeField] private Button levelUpButton;
    [SerializeField] private TextMeshProUGUI levelUpCostText;

    [Header("Unit Info Panel")]
    [SerializeField] private Image unitIcon;
    [SerializeField] private TextMeshProUGUI unitNameText;
    [SerializeField] private TextMeshProUGUI unitSymbolText;
    [SerializeField] private TextMeshProUGUI unitStatsText;
    [SerializeField] private TextMeshProUGUI unitFamilyText;
    [SerializeField] private TextMeshProUGUI unitAbilityText;
    [SerializeField] private Button sellButton;

    [Header("Molecule Preview")]
    [SerializeField] private TextMeshProUGUI moleculeNameText;
    [SerializeField] private TextMeshProUGUI moleculeFormulaText;
    [SerializeField] private TextMeshProUGUI moleculeEffectText;
    [SerializeField] private Image[] ingredientIcons;

    [Header("Game Over")]
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI finalRoundText;
    [SerializeField] private Button restartButton;

    [Header("Periodic Table")]
    [SerializeField] private PeriodicTableUI periodicTableUI;
    [SerializeField] private Button toggleTableButton;

    [Header("Colors")]
    [SerializeField] private Color prepPhaseColor = new Color(0.2f, 0.6f, 1f);
    [SerializeField] private Color combatPhaseColor = new Color(1f, 0.3f, 0.3f);
    [SerializeField] private Color energyColor = new Color(0f, 1f, 0.5f);

    private Unit selectedUnit;

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
        SetupButtonListeners();
        SubscribeToEvents();
        
        // Hide panels initially
        if (unitInfoPanel != null) unitInfoPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (moleculePreviewPanel != null) moleculePreviewPanel.SetActive(false);
    }

    private void SetupButtonListeners()
    {
        if (refreshButton != null)
            refreshButton.onClick.AddListener(OnRefreshClicked);
        
        if (levelUpButton != null)
            levelUpButton.onClick.AddListener(OnLevelUpClicked);
        
        if (sellButton != null)
            sellButton.onClick.AddListener(OnSellClicked);
        
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);

        if (toggleTableButton != null)
            toggleTableButton.onClick.AddListener(TogglePeriodicTable);
    }

    private void SubscribeToEvents()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPhaseChanged += UpdatePhaseDisplay;
            GameManager.Instance.OnRoundChanged += UpdateRoundDisplay;
            GameManager.Instance.OnEnergyChanged += UpdateEnergyDisplay;
            GameManager.Instance.OnPlayerHPChanged += UpdateHPDisplay;
            GameManager.Instance.OnGameOver += ShowGameOver;
        }

        if (BondingManager.Instance != null)
        {
            BondingManager.Instance.OnMoleculeFormed += ShowMoleculeFormed;
        }
    }

    private void Update()
    {
        UpdateTimer();
    }

    #region Top Bar Updates

    public void UpdateEnergyDisplay(int energy)
    {
        if (energyText != null)
        {
            energyText.text = $"⚡ {energy} ATP";
            energyText.color = energy > 0 ? energyColor : Color.red;
        }
    }

    public void UpdateHPDisplay(int currentHP, int damage)
    {
        if (hpText != null)
            hpText.text = $"❤️ {currentHP}";
        
        if (hpSlider != null)
        {
            hpSlider.maxValue = 100;
            hpSlider.value = currentHP;
        }
    }

    public void UpdateRoundDisplay(int round)
    {
        if (roundText != null)
            roundText.text = $"Round {round}";
    }

    public void UpdateLevelDisplay(int level, int unitCap)
    {
        if (levelText != null)
            levelText.text = $"Lv.{level} ({unitCap} units)";
    }

    #endregion

    #region Phase Display

    public void UpdatePhaseDisplay(GamePhase phase)
    {
        if (phaseText != null)
        {
            phaseText.text = phase switch
            {
                GamePhase.Shop => "SHOP PHASE",
                GamePhase.Prep => "PREP PHASE",
                GamePhase.Combat => "⚔️ COMBAT!",
                GamePhase.GameOver => "GAME OVER",
                _ => ""
            };

            phaseText.color = phase == GamePhase.Combat ? combatPhaseColor : prepPhaseColor;
        }

        // Show/hide shop during combat
        if (shopPanel != null)
            shopPanel.SetActive(phase != GamePhase.Combat && phase != GamePhase.GameOver);
    }

    private void UpdateTimer()
    {
        if (timerText == null || GameManager.Instance == null) return;

        // Access timer through reflection or add public property
        // For now, just show phase-appropriate text
        if (GameManager.Instance.CurrentPhase == GamePhase.Combat)
        {
            timerText.text = "Fighting...";
        }
        else
        {
            timerText.text = "Prepare!";
        }
    }

    #endregion

    #region Unit Info Panel

    /// <summary>
    /// Show information about a selected unit
    /// </summary>
    public void ShowUnitInfo(Unit unit)
    {
        if (unit == null || unit.Data == null)
        {
            HideUnitInfo();
            return;
        }

        selectedUnit = unit;
        
        if (unitInfoPanel != null)
            unitInfoPanel.SetActive(true);

        // Basic info
        if (unitNameText != null)
            unitNameText.text = unit.Data.ElementName;
        
        if (unitSymbolText != null)
            unitSymbolText.text = unit.Data.Symbol;
        
        if (unitIcon != null && unit.Data.Icon != null)
            unitIcon.sprite = unit.Data.Icon;

        // Stats
        if (unitStatsText != null)
        {
            unitStatsText.text = $"HP: {unit.CurrentHP:F0}/{unit.MaxHP:F0}\n" +
                                 $"DMG: {unit.Damage:F0}\n" +
                                 $"Range: {unit.AttackRange}\n" +
                                 $"Valence: {unit.Data.ValenceElectrons}e⁻\n" +
                                 $"Star: {new string('★', unit.StarLevel)}";
        }

        // Family trait
        if (unitFamilyText != null)
        {
            string familyDesc = GetFamilyDescription(unit.Data.Family);
            unitFamilyText.text = $"{unit.Data.Family}: {familyDesc}";
        }

        // Ability
        if (unitAbilityText != null)
        {
            string abilityDesc = unit.Data.UltimateAbility != null 
                ? unit.Data.UltimateAbility.Description 
                : GetDefaultAbilityDescription(unit.Data.Family);
            unitAbilityText.text = $"Ultimate: {abilityDesc}";
        }
    }

    public void HideUnitInfo()
    {
        if (unitInfoPanel != null)
            unitInfoPanel.SetActive(false);
        selectedUnit = null;
    }

    private string GetFamilyDescription(ElementFamily family)
    {
        return family switch
        {
            ElementFamily.Alkali => "Explodes on death if near water",
            ElementFamily.AlkalineEarth => "Extra armor",
            ElementFamily.TransitionMetal => "Damage sharing in metallic bonds",
            ElementFamily.NonMetal => "Core building blocks",
            ElementFamily.Halogen => "Steals electrons from enemies",
            ElementFamily.NobleGas => "Immune to abilities",
            ElementFamily.Hydrogen => "Versatile bonding",
            _ => ""
        };
    }

    private string GetDefaultAbilityDescription(ElementFamily family)
    {
        return family switch
        {
            ElementFamily.Alkali => "Massive AoE explosion",
            ElementFamily.Halogen => "Steal all enemy electrons",
            ElementFamily.NobleGas => "Shield nearby allies",
            _ => "Enhanced attack"
        };
    }

    #endregion

    #region Molecule Preview

    /// <summary>
    /// Show molecule formation notification
    /// </summary>
    public void ShowMoleculeFormed(Molecule molecule)
    {
        if (molecule?.Recipe == null) return;

        if (moleculePreviewPanel != null)
        {
            moleculePreviewPanel.SetActive(true);
            
            if (moleculeNameText != null)
                moleculeNameText.text = molecule.Recipe.MoleculeName;
            
            if (moleculeFormulaText != null)
                moleculeFormulaText.text = molecule.Recipe.Formula;
            
            if (moleculeEffectText != null)
                moleculeEffectText.text = molecule.Recipe.Description;

            // Auto-hide after delay
            Invoke(nameof(HideMoleculePreview), 3f);
        }
    }

    public void HideMoleculePreview()
    {
        if (moleculePreviewPanel != null)
            moleculePreviewPanel.SetActive(false);
    }

    /// <summary>
    /// Show potential molecule when hovering/placing units
    /// </summary>
    public void ShowPotentialMolecule(MoleculeRecipe recipe, int completionPercent)
    {
        if (recipe == null) return;

        if (moleculePreviewPanel != null)
        {
            moleculePreviewPanel.SetActive(true);
            
            if (moleculeNameText != null)
                moleculeNameText.text = $"{recipe.MoleculeName} ({completionPercent}%)";
            
            if (moleculeFormulaText != null)
                moleculeFormulaText.text = recipe.Formula;
        }
    }

    #endregion

    #region Game Over

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        bool victory = GameManager.Instance?.PlayerHP > 0;
        
        if (resultText != null)
            resultText.text = victory ? "VICTORY!" : "DEFEAT";
        
        if (finalRoundText != null)
            finalRoundText.text = $"Final Round: {GameManager.Instance?.CurrentRound ?? 0}";
    }

    #endregion

    #region Button Handlers

    private void OnRefreshClicked()
    {
        ShopManager.Instance?.RefreshShop();
    }

    private void OnLevelUpClicked()
    {
        GameManager.Instance?.BuyXP();
    }

    private void OnSellClicked()
    {
        if (selectedUnit != null)
        {
            ShopManager.Instance?.SellUnit(selectedUnit);
            HideUnitInfo();
        }
    }

    private void OnRestartClicked()
    {
        // Reload scene or reinitialize game
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    #endregion

    #region Periodic Table

    /// <summary>
    /// Toggle periodic table visibility
    /// </summary>
    public void TogglePeriodicTable()
    {
        if (periodicTableUI != null)
        {
            periodicTableUI.ToggleVisibility();
        }
    }

    /// <summary>
    /// Update periodic table to highlight owned elements
    /// </summary>
    public void UpdatePeriodicTableWithOwnedElements(List<Unit> ownedUnits)
    {
        if (periodicTableUI == null) return;

        List<int> atomicNumbers = new List<int>();
        foreach (var unit in ownedUnits)
        {
            if (unit?.Data != null)
            {
                atomicNumbers.Add(unit.Data.AtomicNumber);
            }
        }
        periodicTableUI.SetOwnedElements(atomicNumbers);
    }

    /// <summary>
    /// Update periodic table for current player level
    /// </summary>
    public void UpdatePeriodicTableLevel(int level)
    {
        if (periodicTableUI != null)
        {
            periodicTableUI.SetPlayerLevel(level);
        }
    }

    /// <summary>
    /// Highlight elements that are part of active molecules
    /// </summary>
    public void HighlightMoleculeElements(List<Molecule> activeMolecules)
    {
        if (periodicTableUI == null) return;

        List<int> moleculeElements = new List<int>();
        foreach (var molecule in activeMolecules)
        {
            foreach (var unit in molecule.BondedUnits)
            {
                if (unit?.Data != null)
                {
                    moleculeElements.Add(unit.Data.AtomicNumber);
                }
            }
        }
        periodicTableUI.SetElementsInMolecules(moleculeElements);
    }

    #endregion

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPhaseChanged -= UpdatePhaseDisplay;
            GameManager.Instance.OnRoundChanged -= UpdateRoundDisplay;
            GameManager.Instance.OnEnergyChanged -= UpdateEnergyDisplay;
            GameManager.Instance.OnPlayerHPChanged -= UpdateHPDisplay;
            GameManager.Instance.OnGameOver -= ShowGameOver;
        }
    }
}

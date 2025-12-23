using UnityEngine;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// Represents a chemical element unit on the board.
/// Handles drag-and-drop, visual representation, and links to combat/data systems.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Unit : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Data")]
    [SerializeField] private ElementData elementData;
    public ElementData Data => elementData;

    [Header("State")]
    [SerializeField] private HexCell currentCell;
    public HexCell CurrentCell
    {
        get => currentCell;
        set => currentCell = value;
    }

    [Tooltip("Current star level (1-3). Combine 3 units to upgrade.")]
    [Range(1, 3)]
    public int StarLevel = 1;

    [Tooltip("Is this unit currently part of a molecule?")]
    public bool IsInMolecule = false;

    [Tooltip("Reference to parent molecule if bonded")]
    public Molecule ParentMolecule;

    [Header("Combat Stats (Runtime)")]
    public float CurrentHP;
    public float MaxHP;
    public float Damage;
    public int AttackRange;

    [Header("Components")]
    [SerializeField] private UnitCombat combat;
    [SerializeField] private UnitVisuals visuals;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D unitCollider;

    // Drag state
    private bool isDragging = false;
    private Vector3 dragOffset;
    private HexCell originalCell;
    private Vector3 originalPosition;
    private int originalSortingOrder;

    // Events
    public event Action<Unit> OnUnitPlaced;
    public event Action<Unit> OnUnitDeath;
    public event Action<Unit, int> OnStarLevelChanged;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        combat = GetComponent<UnitCombat>();
        visuals = GetComponent<UnitVisuals>();
        unitCollider = GetComponent<Collider2D>();

        if (combat == null)
            combat = gameObject.AddComponent<UnitCombat>();
        if (visuals == null)
            visuals = gameObject.AddComponent<UnitVisuals>();
    }

    /// <summary>
    /// Initialize the unit with element data
    /// </summary>
    public void Initialize(ElementData data, int starLevel = 1)
    {
        elementData = data;
        StarLevel = starLevel;
        
        // Calculate stats based on data and star level
        CalculateStats();
        
        // Set up visuals
        if (visuals != null)
            visuals.SetupVisuals(data, starLevel);
        
        // Initialize combat
        if (combat != null)
            combat.Initialize(this);
        
        // Update name for debugging
        gameObject.name = $"{data.Symbol}_{StarLevel}★";
    }

    /// <summary>
    /// Calculate stats based on element data and star level
    /// </summary>
    public void CalculateStats()
    {
        if (elementData == null) return;

        // Star level multipliers: 1★ = 1x, 2★ = 1.8x, 3★ = 3.2x
        float starMultiplier = StarLevel switch
        {
            1 => 1f,
            2 => 1.8f,
            3 => 3.2f,
            _ => 1f
        };

        MaxHP = elementData.BaseHP * starMultiplier;
        CurrentHP = MaxHP;
        Damage = elementData.BaseDamage * starMultiplier;
        AttackRange = elementData.AttackRange;
    }

    /// <summary>
    /// Upgrade this unit by combining with copies
    /// </summary>
    public bool TryUpgrade()
    {
        if (StarLevel >= 3) return false;
        
        StarLevel++;
        CalculateStats();
        
        if (visuals != null)
            visuals.PlayUpgradeEffect(StarLevel);
        
        OnStarLevelChanged?.Invoke(this, StarLevel);
        
        Debug.Log($"{elementData.Symbol} upgraded to {StarLevel}★!");
        return true;
    }

    /// <summary>
    /// Take damage from combat
    /// </summary>
    public void TakeDamage(float damage, Unit source = null)
    {
        CurrentHP -= damage;
        
        if (visuals != null)
            visuals.PlayHitEffect();
        
        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handle unit death
    /// </summary>
    private void Die()
    {
        if (visuals != null)
            visuals.PlayDeathEffect();
        
        // Special death effects based on element family
        if (elementData != null)
        {
            HandleDeathEffect();
        }
        
        // Remove from cell
        if (currentCell != null)
            currentCell.RemoveUnit();
        
        OnUnitDeath?.Invoke(this);
        
        // Delay destruction for death animation
        Destroy(gameObject, 0.5f);
    }

    /// <summary>
    /// Handle special death effects (e.g., Alkali metals exploding in water)
    /// </summary>
    private void HandleDeathEffect()
    {
        switch (elementData.Family)
        {
            case ElementFamily.Alkali:
                // Check if near water/moisture for explosion
                if (CheckNearWater())
                {
                    TriggerExplosion();
                }
                break;
        }
    }

    private bool CheckNearWater()
    {
        // Check neighboring units for water molecules
        var neighbors = HexGrid.Instance?.GetNeighbors(this);
        if (neighbors == null) return false;
        
        foreach (var neighbor in neighbors)
        {
            if (neighbor.IsInMolecule && neighbor.ParentMolecule?.Recipe?.MoleculeName == "Water")
                return true;
        }
        return false;
    }

    private void TriggerExplosion()
    {
        float explosionDamage = Damage * 3f;
        var nearbyUnits = HexGrid.Instance?.GetUnitsInRange(currentCell, 2);
        
        foreach (var unit in nearbyUnits)
        {
            unit.TakeDamage(explosionDamage, this);
        }
        
        Debug.Log($"{elementData.Symbol} exploded on contact with water!");
    }

    #region Drag and Drop

    public void OnPointerDown(PointerEventData eventData)
    {
        // Prepare for potential drag
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Handle click (not drag)
        if (!isDragging)
        {
            // Show unit info panel
            UIManager.Instance?.ShowUnitInfo(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Only allow dragging during prep phase
        if (GameManager.Instance != null && !GameManager.Instance.CanMoveUnits)
            return;
        
        isDragging = true;
        originalCell = currentCell;
        originalPosition = transform.position;
        originalSortingOrder = spriteRenderer.sortingOrder;
        
        // Visual feedback
        spriteRenderer.sortingOrder = 100; // Bring to front
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.7f);
        
        // Notify game manager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IsDragging = true;
            GameManager.Instance.DraggedUnit = this;
        }
        
        // Calculate offset for smooth dragging
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = 0;
        dragOffset = transform.position - mouseWorldPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = 0;
        transform.position = mouseWorldPos + dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        
        isDragging = false;
        spriteRenderer.sortingOrder = originalSortingOrder;
        spriteRenderer.color = Color.white;
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IsDragging = false;
            GameManager.Instance.DraggedUnit = null;
        }
        
        // Find target cell
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = 0;
        HexCell targetCell = HexGrid.Instance?.GetCellAtPosition(mouseWorldPos);
        
        if (targetCell != null && targetCell.CanPlaceUnit(this))
        {
            // Check for unit swap
            if (targetCell.OccupiedUnit != null && targetCell.OccupiedUnit != this)
            {
                SwapUnits(targetCell.OccupiedUnit);
            }
            else
            {
                // Place on new cell
                if (originalCell != null)
                    originalCell.RemoveUnit();
                
                targetCell.PlaceUnit(this);
                OnUnitPlaced?.Invoke(this);
                
                // Check for bonding opportunities
                BondingManager.Instance?.CheckBondsForUnit(this);
            }
        }
        else
        {
            // Return to original position
            transform.position = originalPosition;
            if (originalCell != null)
                originalCell.PlaceUnit(this);
        }
        
        HexGrid.Instance?.ResetAllHighlights();
    }

    /// <summary>
    /// Swap positions with another unit
    /// </summary>
    private void SwapUnits(Unit other)
    {
        HexCell otherOriginalCell = other.CurrentCell;
        
        // Remove both from cells
        if (originalCell != null)
            originalCell.RemoveUnit();
        otherOriginalCell.RemoveUnit();
        
        // Swap
        originalCell?.PlaceUnit(other);
        otherOriginalCell.PlaceUnit(this);
        
        OnUnitPlaced?.Invoke(this);
        OnUnitPlaced?.Invoke(other);
        
        // Check bonds for both
        BondingManager.Instance?.CheckBondsForUnit(this);
        BondingManager.Instance?.CheckBondsForUnit(other);
    }

    #endregion

    /// <summary>
    /// Check if this unit can bond with another (basic check)
    /// </summary>
    public bool CanBondWith(Unit other)
    {
        if (other == null || elementData == null || other.Data == null)
            return false;
        
        return elementData.CanBondWith(other.Data);
    }

    private void OnDestroy()
    {
        if (currentCell != null)
            currentCell.RemoveUnit();
    }
}

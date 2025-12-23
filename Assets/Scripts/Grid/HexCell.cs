using UnityEngine;

/// <summary>
/// Represents a single cell in the hexagonal grid.
/// Uses axial coordinates (q, r) for efficient neighbor calculations.
/// </summary>
public class HexCell : MonoBehaviour
{
    [Header("Grid Position")]
    [Tooltip("Axial coordinates (q, r) of this cell")]
    [SerializeField] private Vector2Int coordinates;
    
    public Vector2Int Coordinates
    {
        get => coordinates;
        set => coordinates = value;
    }

    [Header("State")]
    [Tooltip("The unit currently occupying this cell")]
    [SerializeField] private Unit occupiedUnit;
    
    public Unit OccupiedUnit
    {
        get => occupiedUnit;
        set => occupiedUnit = value;
    }

    [Tooltip("Is this cell part of the player's board (true) or bench (false)?")]
    public bool IsBoard = true;

    [Tooltip("Which player owns this cell (0 = player, 1 = enemy)")]
    public int OwnerID = 0;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer cellRenderer;
    [SerializeField] private Color defaultColor = new Color(0.2f, 0.3f, 0.4f, 0.5f);
    [SerializeField] private Color highlightColor = new Color(0.3f, 0.6f, 0.8f, 0.8f);
    [SerializeField] private Color invalidColor = new Color(0.8f, 0.2f, 0.2f, 0.8f);
    [SerializeField] private Color bondHighlightColor = new Color(0.2f, 0.8f, 0.4f, 0.8f);

    // Cached world position for performance
    private Vector3 worldPosition;
    public Vector3 WorldPosition => worldPosition;

    // Constants for hex geometry (pointy-topped)
    public static readonly float HexWidth = 1.732f;  // sqrt(3)
    public static readonly float HexHeight = 2f;
    public static readonly float HexVertSpacing = 1.5f;

    private void Awake()
    {
        if (cellRenderer == null)
            cellRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Initialize this cell with grid coordinates
    /// </summary>
    public void Initialize(int q, int r, bool isBoard = true, int ownerID = 0)
    {
        coordinates = new Vector2Int(q, r);
        IsBoard = isBoard;
        OwnerID = ownerID;
        
        // Calculate world position from axial coordinates
        worldPosition = AxialToWorld(coordinates);
        transform.position = worldPosition;
        
        // Set visual state
        SetDefaultVisual();
    }

    /// <summary>
    /// Convert axial coordinates to world position (pointy-topped hex)
    /// </summary>
    public static Vector3 AxialToWorld(Vector2Int axial)
    {
        float x = HexWidth * (axial.x + axial.y / 2f);
        float y = HexVertSpacing * axial.y;
        return new Vector3(x, y, 0);
    }

    /// <summary>
    /// Convert world position to axial coordinates
    /// </summary>
    public static Vector2Int WorldToAxial(Vector3 worldPos)
    {
        float q = (worldPos.x * 2f / 3f);
        float r = (-worldPos.x / 3f + Mathf.Sqrt(3f) / 3f * worldPos.y);
        return AxialRound(q, r);
    }

    /// <summary>
    /// Round floating point axial coordinates to nearest hex
    /// </summary>
    private static Vector2Int AxialRound(float q, float r)
    {
        float s = -q - r;
        
        int rq = Mathf.RoundToInt(q);
        int rr = Mathf.RoundToInt(r);
        int rs = Mathf.RoundToInt(s);
        
        float qDiff = Mathf.Abs(rq - q);
        float rDiff = Mathf.Abs(rr - r);
        float sDiff = Mathf.Abs(rs - s);
        
        if (qDiff > rDiff && qDiff > sDiff)
            rq = -rr - rs;
        else if (rDiff > sDiff)
            rr = -rq - rs;
        
        return new Vector2Int(rq, rr);
    }

    /// <summary>
    /// Check if this cell can accept a unit
    /// </summary>
    public bool CanPlaceUnit(Unit unit)
    {
        return occupiedUnit == null || occupiedUnit == unit;
    }

    /// <summary>
    /// Place a unit on this cell
    /// </summary>
    public bool PlaceUnit(Unit unit)
    {
        if (!CanPlaceUnit(unit))
            return false;
        
        // Remove from previous cell
        if (unit.CurrentCell != null && unit.CurrentCell != this)
        {
            unit.CurrentCell.RemoveUnit();
        }
        
        occupiedUnit = unit;
        unit.CurrentCell = this;
        unit.transform.position = worldPosition;
        
        return true;
    }

    /// <summary>
    /// Remove the current unit from this cell
    /// </summary>
    public void RemoveUnit()
    {
        if (occupiedUnit != null)
        {
            occupiedUnit.CurrentCell = null;
            occupiedUnit = null;
        }
    }

    #region Visual Feedback

    public void SetDefaultVisual()
    {
        if (cellRenderer != null)
            cellRenderer.color = defaultColor;
    }

    public void SetHighlight(bool valid = true)
    {
        if (cellRenderer != null)
            cellRenderer.color = valid ? highlightColor : invalidColor;
    }

    public void SetBondHighlight()
    {
        if (cellRenderer != null)
            cellRenderer.color = bondHighlightColor;
    }

    #endregion

    /// <summary>
    /// Calculate distance to another cell in hex grid steps
    /// </summary>
    public int DistanceTo(HexCell other)
    {
        return HexDistance(coordinates, other.Coordinates);
    }

    /// <summary>
    /// Calculate hex distance between two axial coordinates
    /// </summary>
    public static int HexDistance(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x) 
              + Mathf.Abs(a.x + a.y - b.x - b.y)
              + Mathf.Abs(a.y - b.y)) / 2;
    }

    private void OnMouseEnter()
    {
        // Visual feedback when hovering (for drag-drop)
        if (GameManager.Instance != null && GameManager.Instance.IsDragging)
        {
            SetHighlight(CanPlaceUnit(GameManager.Instance.DraggedUnit));
        }
    }

    private void OnMouseExit()
    {
        SetDefaultVisual();
    }
}

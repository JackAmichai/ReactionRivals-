using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the hexagonal grid system for unit placement and adjacency detection.
/// Uses pointy-topped axial coordinate system (q, r).
/// </summary>
public class HexGrid : MonoBehaviour
{
    public static HexGrid Instance { get; private set; }

    [Header("Grid Configuration")]
    [Tooltip("Number of columns in the grid")]
    [SerializeField] private int gridWidth = 7;
    
    [Tooltip("Number of rows for player's board")]
    [SerializeField] private int boardRows = 4;
    
    [Tooltip("Number of cells on the bench")]
    [SerializeField] private int benchSize = 9;

    [Header("Prefabs")]
    [SerializeField] private GameObject hexCellPrefab;
    [SerializeField] private GameObject benchCellPrefab;

    [Header("Layout")]
    [SerializeField] private Transform boardParent;
    [SerializeField] private Transform benchParent;
    [SerializeField] private Vector3 boardOffset = Vector3.zero;
    [SerializeField] private Vector3 benchOffset = new Vector3(0, -5f, 0);

    // Grid storage using axial coordinates
    public Dictionary<Vector2Int, HexCell> BoardGrid { get; private set; } = new Dictionary<Vector2Int, HexCell>();
    public Dictionary<Vector2Int, HexCell> EnemyGrid { get; private set; } = new Dictionary<Vector2Int, HexCell>();
    public List<HexCell> BenchCells { get; private set; } = new List<HexCell>();
    public List<HexCell> AllCells { get; private set; } = new List<HexCell>();

    // Directions for pointy-topped hex neighbors (axial coordinates)
    private readonly Vector2Int[] neighborDirections = new Vector2Int[]
    {
        new Vector2Int(1, 0),   // East
        new Vector2Int(1, -1),  // Northeast
        new Vector2Int(0, -1),  // Northwest
        new Vector2Int(-1, 0),  // West
        new Vector2Int(-1, 1),  // Southwest
        new Vector2Int(0, 1)    // Southeast
    };

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GenerateGrid();
    }

    /// <summary>
    /// Generate the complete game grid including player board, enemy board, and bench
    /// </summary>
    public void GenerateGrid()
    {
        ClearGrid();

        // Create board parent if needed
        if (boardParent == null)
        {
            boardParent = new GameObject("Board").transform;
            boardParent.SetParent(transform);
        }

        if (benchParent == null)
        {
            benchParent = new GameObject("Bench").transform;
            benchParent.SetParent(transform);
        }

        // Generate player's board (bottom half)
        GenerateBoard(BoardGrid, 0, boardRows, 0, boardOffset);
        
        // Generate enemy's board (top half, mirrored)
        Vector3 enemyOffset = boardOffset + new Vector3(0, boardRows * HexCell.HexVertSpacing + 2f, 0);
        GenerateBoard(EnemyGrid, boardRows, boardRows * 2, 1, enemyOffset);

        // Generate bench
        GenerateBench();

        Debug.Log($"Grid generated: {BoardGrid.Count} player cells, {EnemyGrid.Count} enemy cells, {BenchCells.Count} bench cells");
    }

    /// <summary>
    /// Generate a rectangular hex board section
    /// </summary>
    private void GenerateBoard(Dictionary<Vector2Int, HexCell> grid, int startRow, int endRow, int ownerID, Vector3 offset)
    {
        for (int r = startRow; r < endRow; r++)
        {
            // Offset for odd rows (hex grid stagger)
            int rOffset = r / 2;
            
            for (int q = -rOffset; q < gridWidth - rOffset; q++)
            {
                Vector2Int coords = new Vector2Int(q, r - startRow);
                
                GameObject cellObj = Instantiate(hexCellPrefab, boardParent);
                cellObj.name = $"Cell ({q}, {r - startRow})";
                
                HexCell cell = cellObj.GetComponent<HexCell>();
                if (cell == null)
                    cell = cellObj.AddComponent<HexCell>();
                
                cell.Initialize(q, r - startRow, true, ownerID);
                cellObj.transform.position += offset;
                
                grid[coords] = cell;
                AllCells.Add(cell);
            }
        }
    }

    /// <summary>
    /// Generate the bench for storing unused units
    /// </summary>
    private void GenerateBench()
    {
        for (int i = 0; i < benchSize; i++)
        {
            Vector2Int coords = new Vector2Int(i, -100); // Special y-coord for bench
            
            GameObject cellObj = benchCellPrefab != null 
                ? Instantiate(benchCellPrefab, benchParent)
                : Instantiate(hexCellPrefab, benchParent);
            
            cellObj.name = $"Bench ({i})";
            
            HexCell cell = cellObj.GetComponent<HexCell>();
            if (cell == null)
                cell = cellObj.AddComponent<HexCell>();
            
            // Position bench cells in a row
            Vector3 benchPos = benchOffset + new Vector3(i * HexCell.HexWidth, 0, 0);
            cell.Initialize(i, -100, false, 0);
            cellObj.transform.position = benchPos;
            
            BenchCells.Add(cell);
            AllCells.Add(cell);
        }
    }

    /// <summary>
    /// Clear all cells from the grid
    /// </summary>
    public void ClearGrid()
    {
        foreach (var cell in AllCells)
        {
            if (cell != null)
                Destroy(cell.gameObject);
        }
        
        BoardGrid.Clear();
        EnemyGrid.Clear();
        BenchCells.Clear();
        AllCells.Clear();
    }

    #region Neighbor & Adjacency Methods

    /// <summary>
    /// Get all neighboring cells of a given cell
    /// </summary>
    public List<HexCell> GetNeighborCells(HexCell cell)
    {
        List<HexCell> neighbors = new List<HexCell>();
        
        if (cell == null) return neighbors;
        
        // Check if it's a board cell
        Dictionary<Vector2Int, HexCell> grid = cell.OwnerID == 0 ? BoardGrid : EnemyGrid;
        
        foreach (var dir in neighborDirections)
        {
            Vector2Int neighborCoord = cell.Coordinates + dir;
            if (grid.TryGetValue(neighborCoord, out HexCell neighbor))
            {
                neighbors.Add(neighbor);
            }
        }
        
        return neighbors;
    }

    /// <summary>
    /// Get all units neighboring a given unit
    /// </summary>
    public List<Unit> GetNeighbors(Unit unit)
    {
        List<Unit> neighbors = new List<Unit>();
        
        if (unit == null || unit.CurrentCell == null)
            return neighbors;
        
        foreach (var neighborCell in GetNeighborCells(unit.CurrentCell))
        {
            if (neighborCell.OccupiedUnit != null)
            {
                neighbors.Add(neighborCell.OccupiedUnit);
            }
        }
        
        return neighbors;
    }

    /// <summary>
    /// Get all units within a certain hex range of a position
    /// </summary>
    public List<Unit> GetUnitsInRange(HexCell center, int range, bool includeCenter = false)
    {
        List<Unit> units = new List<Unit>();
        Dictionary<Vector2Int, HexCell> grid = center.OwnerID == 0 ? BoardGrid : EnemyGrid;
        
        foreach (var kvp in grid)
        {
            if (HexCell.HexDistance(center.Coordinates, kvp.Key) <= range)
            {
                if (!includeCenter && kvp.Value == center)
                    continue;
                
                if (kvp.Value.OccupiedUnit != null)
                    units.Add(kvp.Value.OccupiedUnit);
            }
        }
        
        return units;
    }

    /// <summary>
    /// Check if two units are adjacent (for bonding)
    /// </summary>
    public bool AreAdjacent(Unit unitA, Unit unitB)
    {
        if (unitA?.CurrentCell == null || unitB?.CurrentCell == null)
            return false;
        
        return HexCell.HexDistance(unitA.CurrentCell.Coordinates, unitB.CurrentCell.Coordinates) == 1;
    }

    #endregion

    #region Cell Lookup Methods

    /// <summary>
    /// Get cell at world position
    /// </summary>
    public HexCell GetCellAtPosition(Vector3 worldPos)
    {
        // Check board cells
        float minDist = float.MaxValue;
        HexCell closestCell = null;
        
        foreach (var cell in AllCells)
        {
            float dist = Vector3.Distance(worldPos, cell.WorldPosition);
            if (dist < minDist && dist < HexCell.HexWidth)
            {
                minDist = dist;
                closestCell = cell;
            }
        }
        
        return closestCell;
    }

    /// <summary>
    /// Get cell by axial coordinates
    /// </summary>
    public HexCell GetCell(Vector2Int coords, int ownerID = 0)
    {
        var grid = ownerID == 0 ? BoardGrid : EnemyGrid;
        return grid.TryGetValue(coords, out HexCell cell) ? cell : null;
    }

    /// <summary>
    /// Get first available bench cell
    /// </summary>
    public HexCell GetEmptyBenchCell()
    {
        return BenchCells.FirstOrDefault(c => c.OccupiedUnit == null);
    }

    /// <summary>
    /// Get all units on the player's board
    /// </summary>
    public List<Unit> GetAllBoardUnits(int ownerID = 0)
    {
        var grid = ownerID == 0 ? BoardGrid : EnemyGrid;
        return grid.Values
            .Where(c => c.OccupiedUnit != null)
            .Select(c => c.OccupiedUnit)
            .ToList();
    }

    /// <summary>
    /// Get all units on the bench
    /// </summary>
    public List<Unit> GetAllBenchUnits()
    {
        return BenchCells
            .Where(c => c.OccupiedUnit != null)
            .Select(c => c.OccupiedUnit)
            .ToList();
    }

    #endregion

    #region Visual Helpers

    /// <summary>
    /// Highlight cells that would complete a bond with the given unit
    /// </summary>
    public void HighlightBondingCells(Unit unit)
    {
        if (unit?.Data == null) return;
        
        // Get all valid bonding positions based on recipes
        var bondingManager = BondingManager.Instance;
        if (bondingManager == null) return;
        
        foreach (var cell in BoardGrid.Values)
        {
            if (cell.OccupiedUnit != null)
            {
                // Check if placing here would complete a molecule
                var neighbors = GetNeighborCells(cell);
                // Logic to highlight potential bond formations
            }
        }
    }

    /// <summary>
    /// Reset all cell visuals to default
    /// </summary>
    public void ResetAllHighlights()
    {
        foreach (var cell in AllCells)
        {
            cell.SetDefaultVisual();
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        // Draw grid in editor for debugging
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.yellow;
        foreach (var cell in AllCells)
        {
            Gizmos.DrawWireSphere(cell.WorldPosition, 0.3f);
        }
    }
}

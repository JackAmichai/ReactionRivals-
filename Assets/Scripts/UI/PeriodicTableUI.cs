using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// UI Component that displays the full periodic table with level-based highlighting.
/// Elements are highlighted based on:
/// - Unlocked at current level (available in shop)
/// - Currently owned by player
/// - Part of active molecules
/// </summary>
public class PeriodicTableUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform tableContainer;
    [SerializeField] private GameObject elementCellPrefab;
    [SerializeField] private LevelElementProgression levelProgression;

    [Header("Display Settings")]
    [SerializeField] private float cellWidth = 50f;
    [SerializeField] private float cellHeight = 60f;
    [SerializeField] private float cellSpacing = 2f;
    [SerializeField] private float lanthanideOffset = 30f;

    [Header("Colors")]
    [SerializeField] private Color lockedColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    [SerializeField] private Color unlockedColor = new Color(0.4f, 0.4f, 0.5f, 1f);
    [SerializeField] private Color ownedColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    [SerializeField] private Color inMoleculeColor = new Color(1f, 0.84f, 0f, 1f);
    [SerializeField] private Color highlightBorderColor = new Color(1f, 1f, 1f, 1f);

    [Header("State")]
    [SerializeField] private int currentPlayerLevel = 1;

    // Cell references for updating
    private Dictionary<int, PeriodicTableCell> cellsByAtomicNumber = new Dictionary<int, PeriodicTableCell>();
    
    // Current state
    private HashSet<int> ownedElements = new HashSet<int>();
    private HashSet<int> elementsInMolecules = new HashSet<int>();

    private void Start()
    {
        GeneratePeriodicTable();
        UpdateHighlighting();
    }

    /// <summary>
    /// Generate the full periodic table UI
    /// </summary>
    public void GeneratePeriodicTable()
    {
        // Clear existing
        foreach (Transform child in tableContainer)
        {
            Destroy(child.gameObject);
        }
        cellsByAtomicNumber.Clear();

        // Standard periodic table positions
        // Main table: Columns 1-18, Rows 1-7
        // Lanthanides: Row 9 (offset below row 6)
        // Actinides: Row 10 (offset below row 7)

        foreach (var element in PeriodicTable.Elements)
        {
            CreateElementCell(element);
        }

        // Add labels for Lanthanides and Actinides
        CreateSeriesLabel("Lanthanides (57-71)", 3, 9);
        CreateSeriesLabel("Actinides (89-103)", 3, 10);
    }

    private void CreateElementCell(PeriodicElementInfo element)
    {
        GameObject cellObj = Instantiate(elementCellPrefab, tableContainer);
        PeriodicTableCell cell = cellObj.GetComponent<PeriodicTableCell>();
        
        if (cell == null)
        {
            cell = cellObj.AddComponent<PeriodicTableCell>();
        }

        // Calculate position based on standard periodic table layout
        Vector2 position = GetCellPosition(element);
        
        RectTransform rectTransform = cellObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(cellWidth, cellHeight);

        // Initialize cell with element data
        cell.Initialize(element);
        cellsByAtomicNumber[element.AtomicNumber] = cell;
    }

    private Vector2 GetCellPosition(PeriodicElementInfo element)
    {
        int col = element.Group;
        int row = element.Period;

        // Handle Lanthanides (57-71) - they go in a separate row
        if (element.Family == ElementFamily.Lanthanide)
        {
            row = 9;
            col = 4 + (element.AtomicNumber - 57); // Start after La placeholder
        }
        // Handle Actinides (89-103)
        else if (element.Family == ElementFamily.Actinide)
        {
            row = 10;
            col = 4 + (element.AtomicNumber - 89); // Start after Ac placeholder
        }
        // Handle transition metal position adjustments for Period 6 and 7
        else if ((element.Period == 6 || element.Period == 7) && element.Group >= 4 && element.Group <= 17)
        {
            // These elements have correct positions already
        }

        float x = (col - 1) * (cellWidth + cellSpacing);
        float y = -(row - 1) * (cellHeight + cellSpacing);

        // Add extra offset for lanthanide/actinide rows
        if (row >= 9)
        {
            y -= lanthanideOffset;
        }

        return new Vector2(x, y);
    }

    private void CreateSeriesLabel(string text, int col, int row)
    {
        GameObject labelObj = new GameObject("Label_" + text);
        labelObj.transform.SetParent(tableContainer);

        RectTransform rectTransform = labelObj.AddComponent<RectTransform>();
        float x = (col - 1) * (cellWidth + cellSpacing) - cellWidth;
        float y = -(row - 1) * (cellHeight + cellSpacing) - lanthanideOffset;
        rectTransform.anchoredPosition = new Vector2(x, y);
        rectTransform.sizeDelta = new Vector2(cellWidth * 2, cellHeight);

        TextMeshProUGUI tmp = labelObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 10;
        tmp.alignment = TextAlignmentOptions.MidlineRight;
        tmp.color = Color.white;
    }

    /// <summary>
    /// Update highlighting based on current game state
    /// </summary>
    public void UpdateHighlighting()
    {
        HashSet<int> unlockedElements = new HashSet<int>();
        
        if (levelProgression != null)
        {
            unlockedElements = levelProgression.GetUnlockedElements(currentPlayerLevel);
        }

        foreach (var kvp in cellsByAtomicNumber)
        {
            int atomicNumber = kvp.Key;
            PeriodicTableCell cell = kvp.Value;

            HighlightState state = HighlightState.Locked;

            if (elementsInMolecules.Contains(atomicNumber))
            {
                state = HighlightState.InMolecule;
            }
            else if (ownedElements.Contains(atomicNumber))
            {
                state = HighlightState.Owned;
            }
            else if (unlockedElements.Contains(atomicNumber))
            {
                state = HighlightState.Unlocked;
            }

            cell.SetHighlightState(state, GetColorForState(state));
        }
    }

    private Color GetColorForState(HighlightState state)
    {
        switch (state)
        {
            case HighlightState.Locked:
                return lockedColor;
            case HighlightState.Unlocked:
                return unlockedColor;
            case HighlightState.Owned:
                return ownedColor;
            case HighlightState.InMolecule:
                return inMoleculeColor;
            default:
                return lockedColor;
        }
    }

    /// <summary>
    /// Update the player level and refresh highlighting
    /// </summary>
    public void SetPlayerLevel(int level)
    {
        currentPlayerLevel = level;
        UpdateHighlighting();
    }

    /// <summary>
    /// Add an owned element
    /// </summary>
    public void AddOwnedElement(int atomicNumber)
    {
        ownedElements.Add(atomicNumber);
        UpdateHighlighting();
    }

    /// <summary>
    /// Remove an owned element
    /// </summary>
    public void RemoveOwnedElement(int atomicNumber)
    {
        ownedElements.Remove(atomicNumber);
        UpdateHighlighting();
    }

    /// <summary>
    /// Set all owned elements at once
    /// </summary>
    public void SetOwnedElements(IEnumerable<int> atomicNumbers)
    {
        ownedElements.Clear();
        foreach (int num in atomicNumbers)
        {
            ownedElements.Add(num);
        }
        UpdateHighlighting();
    }

    /// <summary>
    /// Mark elements as being part of an active molecule
    /// </summary>
    public void SetElementsInMolecules(IEnumerable<int> atomicNumbers)
    {
        elementsInMolecules.Clear();
        foreach (int num in atomicNumbers)
        {
            elementsInMolecules.Add(num);
        }
        UpdateHighlighting();
    }

    /// <summary>
    /// Clear molecule highlighting
    /// </summary>
    public void ClearMoleculeHighlighting()
    {
        elementsInMolecules.Clear();
        UpdateHighlighting();
    }

    /// <summary>
    /// Get all unlocked elements at current level
    /// </summary>
    public List<PeriodicElementInfo> GetUnlockedElements()
    {
        List<PeriodicElementInfo> result = new List<PeriodicElementInfo>();
        
        if (levelProgression != null)
        {
            HashSet<int> unlocked = levelProgression.GetUnlockedElements(currentPlayerLevel);
            foreach (int atomicNumber in unlocked)
            {
                result.Add(PeriodicTable.GetElement(atomicNumber));
            }
        }

        return result;
    }

    /// <summary>
    /// Show/hide the periodic table panel
    /// </summary>
    public void ToggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

/// <summary>
/// Highlight states for periodic table cells
/// </summary>
public enum HighlightState
{
    Locked,     // Not yet available at this level
    Unlocked,   // Available in shop but not owned
    Owned,      // Currently owned by player
    InMolecule  // Part of an active molecule
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Individual cell in the periodic table UI.
/// Displays element info and handles highlighting.
/// </summary>
public class PeriodicTableCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("UI Elements")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image borderImage;
    [SerializeField] private TextMeshProUGUI symbolText;
    [SerializeField] private TextMeshProUGUI atomicNumberText;
    [SerializeField] private TextMeshProUGUI nameText;

    [Header("State")]
    private PeriodicElementInfo elementInfo;
    private HighlightState currentState = HighlightState.Locked;
    private Color baseColor;
    private bool isHovered = false;

    // Event for when cell is clicked
    public System.Action<PeriodicElementInfo> OnCellClicked;
    
    // Static event for global tooltip
    public static System.Action<PeriodicElementInfo, Vector3> OnShowTooltip;
    public static System.Action OnHideTooltip;

    private void Awake()
    {
        // Create UI elements if they don't exist
        if (backgroundImage == null)
        {
            backgroundImage = GetComponent<Image>();
            if (backgroundImage == null)
            {
                backgroundImage = gameObject.AddComponent<Image>();
            }
        }

        CreateUIElements();
    }

    private void CreateUIElements()
    {
        // Create border
        if (borderImage == null)
        {
            GameObject borderObj = new GameObject("Border");
            borderObj.transform.SetParent(transform);
            borderImage = borderObj.AddComponent<Image>();
            RectTransform borderRect = borderObj.GetComponent<RectTransform>();
            borderRect.anchorMin = Vector2.zero;
            borderRect.anchorMax = Vector2.one;
            borderRect.offsetMin = Vector2.zero;
            borderRect.offsetMax = Vector2.zero;
            borderImage.color = Color.clear;
            borderImage.raycastTarget = false;
        }

        // Create atomic number text (top left)
        if (atomicNumberText == null)
        {
            GameObject numObj = new GameObject("AtomicNumber");
            numObj.transform.SetParent(transform);
            atomicNumberText = numObj.AddComponent<TextMeshProUGUI>();
            RectTransform numRect = numObj.GetComponent<RectTransform>();
            numRect.anchorMin = new Vector2(0, 1);
            numRect.anchorMax = new Vector2(1, 1);
            numRect.pivot = new Vector2(0, 1);
            numRect.anchoredPosition = new Vector2(2, -2);
            numRect.sizeDelta = new Vector2(40, 12);
            atomicNumberText.fontSize = 8;
            atomicNumberText.alignment = TextAlignmentOptions.TopLeft;
            atomicNumberText.color = Color.white;
            atomicNumberText.raycastTarget = false;
        }

        // Create symbol text (center)
        if (symbolText == null)
        {
            GameObject symObj = new GameObject("Symbol");
            symObj.transform.SetParent(transform);
            symbolText = symObj.AddComponent<TextMeshProUGUI>();
            RectTransform symRect = symObj.GetComponent<RectTransform>();
            symRect.anchorMin = Vector2.zero;
            symRect.anchorMax = Vector2.one;
            symRect.offsetMin = new Vector2(0, 10);
            symRect.offsetMax = new Vector2(0, -10);
            symbolText.fontSize = 16;
            symbolText.fontStyle = FontStyles.Bold;
            symbolText.alignment = TextAlignmentOptions.Center;
            symbolText.color = Color.white;
            symbolText.raycastTarget = false;
        }

        // Create name text (bottom)
        if (nameText == null)
        {
            GameObject nameObj = new GameObject("Name");
            nameObj.transform.SetParent(transform);
            nameText = nameObj.AddComponent<TextMeshProUGUI>();
            RectTransform nameRect = nameObj.GetComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0, 0);
            nameRect.anchorMax = new Vector2(1, 0);
            nameRect.pivot = new Vector2(0.5f, 0);
            nameRect.anchoredPosition = new Vector2(0, 2);
            nameRect.sizeDelta = new Vector2(50, 12);
            nameText.fontSize = 6;
            nameText.alignment = TextAlignmentOptions.Bottom;
            nameText.color = Color.white;
            nameText.raycastTarget = false;
            nameText.enableWordWrapping = false;
            nameText.overflowMode = TextOverflowModes.Ellipsis;
        }
    }

    /// <summary>
    /// Initialize the cell with element data
    /// </summary>
    public void Initialize(PeriodicElementInfo element)
    {
        elementInfo = element;
        gameObject.name = $"Element_{element.Symbol}";

        atomicNumberText.text = element.AtomicNumber.ToString();
        symbolText.text = element.Symbol;
        nameText.text = element.Name;

        // Set base color from element family
        baseColor = GetFamilyColor(element.Family);
        backgroundImage.color = baseColor;
    }

    /// <summary>
    /// Set the highlight state of this cell
    /// </summary>
    public void SetHighlightState(HighlightState state, Color stateColor)
    {
        currentState = state;

        switch (state)
        {
            case HighlightState.Locked:
                backgroundImage.color = stateColor;
                borderImage.color = Color.clear;
                symbolText.color = new Color(1, 1, 1, 0.3f);
                atomicNumberText.color = new Color(1, 1, 1, 0.3f);
                nameText.color = new Color(1, 1, 1, 0.3f);
                break;

            case HighlightState.Unlocked:
                backgroundImage.color = Color.Lerp(baseColor, stateColor, 0.5f);
                borderImage.color = Color.clear;
                symbolText.color = Color.white;
                atomicNumberText.color = Color.white;
                nameText.color = Color.white;
                break;

            case HighlightState.Owned:
                backgroundImage.color = baseColor;
                borderImage.color = stateColor;
                SetBorderWidth(3);
                symbolText.color = Color.white;
                atomicNumberText.color = Color.white;
                nameText.color = Color.white;
                break;

            case HighlightState.InMolecule:
                backgroundImage.color = baseColor;
                borderImage.color = stateColor;
                SetBorderWidth(4);
                symbolText.color = Color.white;
                atomicNumberText.color = Color.white;
                nameText.color = Color.white;
                break;
        }
    }

    private void SetBorderWidth(float width)
    {
        // Use outline or adjust border rect
        RectTransform borderRect = borderImage.GetComponent<RectTransform>();
        borderRect.offsetMin = new Vector2(-width, -width);
        borderRect.offsetMax = new Vector2(width, width);
        borderImage.type = Image.Type.Sliced;
    }

    private Color GetFamilyColor(ElementFamily family)
    {
        switch (family)
        {
            case ElementFamily.Hydrogen:
                return new Color(0.6f, 0.8f, 1f);
            case ElementFamily.Alkali:
                return new Color(1f, 0.6f, 0.6f);
            case ElementFamily.AlkalineEarth:
                return new Color(1f, 0.85f, 0.6f);
            case ElementFamily.TransitionMetal:
                return new Color(1f, 0.75f, 0.8f);
            case ElementFamily.PostTransitionMetal:
                return new Color(0.8f, 0.8f, 0.8f);
            case ElementFamily.Metalloid:
                return new Color(0.6f, 0.9f, 0.8f);
            case ElementFamily.NonMetal:
                return new Color(0.6f, 1f, 0.6f);
            case ElementFamily.Halogen:
                return new Color(1f, 1f, 0.6f);
            case ElementFamily.NobleGas:
                return new Color(0.7f, 0.9f, 1f);
            case ElementFamily.Lanthanide:
                return new Color(1f, 0.75f, 0.9f);
            case ElementFamily.Actinide:
                return new Color(1f, 0.6f, 0.85f);
            default:
                return Color.gray;
        }
    }

    // Hover effects
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        transform.localScale = Vector3.one * 1.1f;
        
        // Show tooltip
        OnShowTooltip?.Invoke(elementInfo, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        transform.localScale = Vector3.one;
        
        // Hide tooltip
        OnHideTooltip?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCellClicked?.Invoke(elementInfo);
    }

    /// <summary>
    /// Get the element info for this cell
    /// </summary>
    public PeriodicElementInfo GetElementInfo()
    {
        return elementInfo;
    }

    /// <summary>
    /// Get the current highlight state
    /// </summary>
    public HighlightState GetHighlightState()
    {
        return currentState;
    }
}

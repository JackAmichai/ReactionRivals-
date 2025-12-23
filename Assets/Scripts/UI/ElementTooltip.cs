using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Tooltip that shows detailed element information when hovering over periodic table cells
/// </summary>
public class ElementTooltip : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform tooltipRect;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI symbolText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI detailsText;

    [Header("Settings")]
    [SerializeField] private Vector2 offset = new Vector2(10, 10);
    [SerializeField] private float fadeSpeed = 10f;

    private bool isVisible = false;
    private float targetAlpha = 0f;

    private void Start()
    {
        // Subscribe to tooltip events
        PeriodicTableCell.OnShowTooltip += ShowTooltip;
        PeriodicTableCell.OnHideTooltip += HideTooltip;

        CreateUIIfNeeded();
        canvasGroup.alpha = 0f;
    }

    private void OnDestroy()
    {
        PeriodicTableCell.OnShowTooltip -= ShowTooltip;
        PeriodicTableCell.OnHideTooltip -= HideTooltip;
    }

    private void CreateUIIfNeeded()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (tooltipRect == null)
        {
            tooltipRect = GetComponent<RectTransform>();
        }

        if (backgroundImage == null)
        {
            backgroundImage = GetComponent<Image>();
            if (backgroundImage == null)
            {
                backgroundImage = gameObject.AddComponent<Image>();
                backgroundImage.color = new Color(0.1f, 0.1f, 0.15f, 0.95f);
            }
        }

        // Create text elements if they don't exist
        if (symbolText == null)
        {
            GameObject symObj = new GameObject("Symbol");
            symObj.transform.SetParent(transform);
            symbolText = symObj.AddComponent<TextMeshProUGUI>();
            RectTransform rect = symbolText.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(10, -10);
            rect.sizeDelta = new Vector2(200, 40);
            symbolText.fontSize = 28;
            symbolText.fontStyle = FontStyles.Bold;
            symbolText.color = Color.white;
        }

        if (nameText == null)
        {
            GameObject nameObj = new GameObject("Name");
            nameObj.transform.SetParent(transform);
            nameText = nameObj.AddComponent<TextMeshProUGUI>();
            RectTransform rect = nameText.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(10, -50);
            rect.sizeDelta = new Vector2(200, 25);
            nameText.fontSize = 16;
            nameText.color = new Color(0.8f, 0.8f, 0.8f);
        }

        if (detailsText == null)
        {
            GameObject detailsObj = new GameObject("Details");
            detailsObj.transform.SetParent(transform);
            detailsText = detailsObj.AddComponent<TextMeshProUGUI>();
            RectTransform rect = detailsText.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(10, -80);
            rect.sizeDelta = new Vector2(200, 120);
            detailsText.fontSize = 12;
            detailsText.color = Color.white;
            detailsText.lineSpacing = 5;
        }
    }

    private void Update()
    {
        // Fade animation
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
        canvasGroup.blocksRaycasts = isVisible;
    }

    public void ShowTooltip(PeriodicElementInfo element, Vector3 position)
    {
        if (element == null) return;

        isVisible = true;
        targetAlpha = 1f;

        // Update content
        symbolText.text = $"{element.Symbol} <size=14>({element.AtomicNumber})</size>";
        nameText.text = element.Name;
        
        string details = $"<b>Atomic Mass:</b> {element.AtomicMass:F3} u\n";
        details += $"<b>Valence Electrons:</b> {element.ValenceElectrons}\n";
        details += $"<b>Electronegativity:</b> {(element.Electronegativity > 0 ? element.Electronegativity.ToString("F2") : "N/A")}\n";
        details += $"<b>Family:</b> {FormatFamilyName(element.Family)}\n";
        details += $"<b>Period:</b> {element.Period} | <b>Group:</b> {element.Group}\n";
        details += $"\n<color=#88ff88><b>Game Stats</b></color>\n";
        details += $"HP: {element.GetGameHP():F0}\n";
        details += $"Electrons to Full Shell: {element.GetElectronsToFullShell()}";
        
        detailsText.text = details;

        // Position tooltip near the cursor but keep on screen
        Vector2 screenPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            position,
            null,
            out screenPos
        );

        tooltipRect.anchoredPosition = screenPos + offset;

        // Keep tooltip on screen
        ClampToScreen();
    }

    public void HideTooltip()
    {
        isVisible = false;
        targetAlpha = 0f;
    }

    private void ClampToScreen()
    {
        if (transform.parent == null) return;

        RectTransform parentRect = transform.parent as RectTransform;
        if (parentRect == null) return;

        Vector3[] corners = new Vector3[4];
        tooltipRect.GetWorldCorners(corners);

        Vector3[] parentCorners = new Vector3[4];
        parentRect.GetWorldCorners(parentCorners);

        Vector2 pos = tooltipRect.anchoredPosition;

        // Clamp right edge
        if (corners[2].x > parentCorners[2].x)
        {
            pos.x -= (corners[2].x - parentCorners[2].x);
        }

        // Clamp bottom edge
        if (corners[0].y < parentCorners[0].y)
        {
            pos.y += (parentCorners[0].y - corners[0].y);
        }

        tooltipRect.anchoredPosition = pos;
    }

    private string FormatFamilyName(ElementFamily family)
    {
        switch (family)
        {
            case ElementFamily.Hydrogen: return "Hydrogen";
            case ElementFamily.Alkali: return "Alkali Metal";
            case ElementFamily.AlkalineEarth: return "Alkaline Earth Metal";
            case ElementFamily.TransitionMetal: return "Transition Metal";
            case ElementFamily.PostTransitionMetal: return "Post-Transition Metal";
            case ElementFamily.Metalloid: return "Metalloid";
            case ElementFamily.NonMetal: return "Nonmetal";
            case ElementFamily.Halogen: return "Halogen";
            case ElementFamily.NobleGas: return "Noble Gas";
            case ElementFamily.Lanthanide: return "Lanthanide";
            case ElementFamily.Actinide: return "Actinide";
            default: return family.ToString();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Individual shop slot displaying an element that can be purchased.
/// Handles click-to-buy and displays element information.
/// </summary>
public class ShopSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI Elements")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image elementIcon;
    [SerializeField] private TextMeshProUGUI symbolText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image familyBorder;
    [SerializeField] private GameObject soldOutOverlay;
    [SerializeField] private GameObject hoverHighlight;

    [Header("Colors")]
    [SerializeField] private Color commonColor = new Color(0.6f, 0.6f, 0.6f);
    [SerializeField] private Color uncommonColor = new Color(0.2f, 0.8f, 0.2f);
    [SerializeField] private Color rareColor = new Color(0.2f, 0.5f, 1f);
    [SerializeField] private Color epicColor = new Color(0.6f, 0.2f, 0.8f);
    [SerializeField] private Color legendaryColor = new Color(1f, 0.8f, 0.2f);

    private int slotIndex;
    private ElementData currentElement;
    private ShopManager shopManager;
    private bool isEmpty = true;

    /// <summary>
    /// Initialize the shop slot
    /// </summary>
    public void Initialize(int index, ShopManager manager)
    {
        slotIndex = index;
        shopManager = manager;
        SetEmpty();
    }

    /// <summary>
    /// Set the element displayed in this slot
    /// </summary>
    public void SetElement(ElementData element)
    {
        currentElement = element;
        isEmpty = element == null;

        if (element == null)
        {
            SetEmpty();
            return;
        }

        // Update visuals
        if (elementIcon != null)
        {
            elementIcon.gameObject.SetActive(true);
            elementIcon.sprite = element.Icon;
            elementIcon.color = element.ElementColor;
        }

        if (symbolText != null)
        {
            symbolText.gameObject.SetActive(true);
            symbolText.text = element.Symbol;
            symbolText.color = GetContrastColor(element.ElementColor);
        }

        if (costText != null)
        {
            costText.gameObject.SetActive(true);
            costText.text = $"{element.Cost}⚡";
        }

        if (nameText != null)
        {
            nameText.gameObject.SetActive(true);
            nameText.text = element.ElementName;
        }

        // Set rarity color
        if (backgroundImage != null)
        {
            backgroundImage.color = GetRarityColor(element.Rarity);
        }

        // Set family border
        if (familyBorder != null)
        {
            familyBorder.color = GetFamilyColor(element.Family);
        }

        if (soldOutOverlay != null)
            soldOutOverlay.SetActive(false);
    }

    /// <summary>
    /// Set slot to empty/sold state
    /// </summary>
    public void SetEmpty()
    {
        isEmpty = true;
        currentElement = null;

        if (elementIcon != null)
            elementIcon.gameObject.SetActive(false);

        if (symbolText != null)
            symbolText.gameObject.SetActive(false);

        if (costText != null)
            costText.gameObject.SetActive(false);

        if (nameText != null)
            nameText.gameObject.SetActive(false);

        if (soldOutOverlay != null)
            soldOutOverlay.SetActive(true);

        if (backgroundImage != null)
            backgroundImage.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    }

    #region Pointer Events

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isEmpty || currentElement == null)
            return;

        // Try to purchase
        if (shopManager != null)
        {
            bool success = shopManager.PurchaseElement(slotIndex);
            if (success)
            {
                SetEmpty();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isEmpty) return;

        // Show hover highlight
        if (hoverHighlight != null)
            hoverHighlight.SetActive(true);

        // Show detailed tooltip
        UIManager.Instance?.ShowPotentialMolecule(null, 0); // Could show recipes involving this element
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverHighlight != null)
            hoverHighlight.SetActive(false);

        UIManager.Instance?.HideMoleculePreview();
    }

    #endregion

    #region Color Helpers

    private Color GetRarityColor(ElementRarity rarity)
    {
        return rarity switch
        {
            ElementRarity.Common => commonColor,
            ElementRarity.Uncommon => uncommonColor,
            ElementRarity.Rare => rareColor,
            ElementRarity.Epic => epicColor,
            ElementRarity.Legendary => legendaryColor,
            _ => commonColor
        };
    }

    private Color GetFamilyColor(ElementFamily family)
    {
        return family switch
        {
            ElementFamily.Alkali => new Color(1f, 0.5f, 0f),        // Orange
            ElementFamily.AlkalineEarth => new Color(0.8f, 0.8f, 0f), // Yellow
            ElementFamily.TransitionMetal => new Color(0.7f, 0.7f, 0.8f), // Silver
            ElementFamily.NonMetal => new Color(0.2f, 0.6f, 1f),    // Blue
            ElementFamily.Halogen => new Color(0.5f, 0f, 0.5f),     // Purple
            ElementFamily.NobleGas => new Color(0f, 1f, 1f),        // Cyan
            ElementFamily.Hydrogen => Color.white,
            _ => Color.gray
        };
    }

    private Color GetContrastColor(Color background)
    {
        float luminance = 0.299f * background.r + 0.587f * background.g + 0.114f * background.b;
        return luminance > 0.5f ? Color.black : Color.white;
    }

    #endregion
}

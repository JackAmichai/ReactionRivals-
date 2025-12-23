using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// UI panel that displays educational content when the player levels up.
/// Shows information about newly unlocked elements and their significance.
/// </summary>
public class LevelUpEducationUI : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject educationPanel;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Level Info")]
    [SerializeField] private TextMeshProUGUI levelTitleText;
    [SerializeField] private TextMeshProUGUI levelSubtitleText;
    [SerializeField] private TextMeshProUGUI themeText;
    [SerializeField] private TextMeshProUGUI discoveryPeriodText;

    [Header("Main Content")]
    [SerializeField] private TextMeshProUGUI mainDescriptionText;
    [SerializeField] private Transform didYouKnowContainer;
    [SerializeField] private GameObject factPrefab;

    [Header("Unlocked Elements")]
    [SerializeField] private Transform elementsContainer;
    [SerializeField] private GameObject elementCardPrefab;

    [Header("Element Detail View")]
    [SerializeField] private GameObject elementDetailPanel;
    [SerializeField] private TextMeshProUGUI elementNameText;
    [SerializeField] private TextMeshProUGUI elementSymbolText;
    [SerializeField] private TextMeshProUGUI discoveryText;
    [SerializeField] private TextMeshProUGUI historicalText;
    [SerializeField] private TextMeshProUGUI modernUseText;
    [SerializeField] private Transform funFactsContainer;

    [Header("Buttons")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button nextElementButton;
    [SerializeField] private Button prevElementButton;

    [Header("Animation")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float elementRevealDelay = 0.2f;

    private LevelElementProgression levelProgression;
    private int currentLevel;
    private List<int> unlockedAtomicNumbers = new List<int>();
    private int currentElementIndex = 0;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);
        
        if (backButton != null)
            backButton.onClick.AddListener(OnBackClicked);
        
        if (nextElementButton != null)
            nextElementButton.onClick.AddListener(OnNextElement);
        
        if (prevElementButton != null)
            prevElementButton.onClick.AddListener(OnPrevElement);

        Hide();
    }

    /// <summary>
    /// Show the level up education screen
    /// </summary>
    public void ShowLevelUp(int level, LevelElementProgression progression)
    {
        currentLevel = level;
        levelProgression = progression;
        
        // Get elements unlocked at this specific level
        unlockedAtomicNumbers.Clear();
        foreach (var unlock in progression.LevelUnlocks)
        {
            if (unlock.Level == level)
            {
                unlockedAtomicNumbers.AddRange(unlock.UnlockedAtomicNumbers);
                break;
            }
        }

        PopulateLevelContent();
        PopulateUnlockedElements();
        
        educationPanel.SetActive(true);
        if (elementDetailPanel != null)
            elementDetailPanel.SetActive(false);
        
        // Fade in
        StartCoroutine(FadeIn());
    }

    private void PopulateLevelContent()
    {
        var content = LevelEducation.GetLevelContent(currentLevel);
        
        if (content != null)
        {
            if (levelTitleText != null)
                levelTitleText.text = $"Level {currentLevel}: {content.Title}";
            
            if (levelSubtitleText != null)
                levelSubtitleText.text = content.Subtitle;
            
            if (themeText != null)
                themeText.text = $"<b>Theme:</b> {content.Theme}";
            
            if (discoveryPeriodText != null)
                discoveryPeriodText.text = $"<b>Discovery Era:</b> {content.DiscoveryPeriod}";
            
            if (mainDescriptionText != null)
                mainDescriptionText.text = content.MainDescription;

            // Populate "Did You Know?" facts
            PopulateDidYouKnow(content.DidYouKnow);
        }
        else
        {
            // Fallback for levels without custom content
            if (levelTitleText != null)
                levelTitleText.text = $"Level {currentLevel}";
            
            if (levelSubtitleText != null)
                levelSubtitleText.text = "New elements unlocked!";
        }
    }

    private void PopulateDidYouKnow(string[] facts)
    {
        if (didYouKnowContainer == null) return;

        // Clear existing
        foreach (Transform child in didYouKnowContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (string fact in facts)
        {
            CreateFactItem(didYouKnowContainer, $"💡 {fact}");
        }
    }

    private void CreateFactItem(Transform container, string text)
    {
        GameObject factObj = new GameObject("Fact");
        factObj.transform.SetParent(container);

        RectTransform rect = factObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(400, 30);

        TextMeshProUGUI tmp = factObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 14;
        tmp.color = new Color(0.9f, 0.9f, 0.7f);
        tmp.alignment = TextAlignmentOptions.Left;
    }

    private void PopulateUnlockedElements()
    {
        if (elementsContainer == null) return;

        // Clear existing
        foreach (Transform child in elementsContainer)
        {
            Destroy(child.gameObject);
        }

        int index = 0;
        foreach (int atomicNumber in unlockedAtomicNumbers)
        {
            var element = PeriodicTable.GetElement(atomicNumber);
            if (element != null)
            {
                CreateElementCard(element, index);
                index++;
            }
        }
    }

    private void CreateElementCard(PeriodicElementInfo element, int index)
    {
        GameObject cardObj = new GameObject($"Card_{element.Symbol}");
        cardObj.transform.SetParent(elementsContainer);

        RectTransform rect = cardObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(80, 100);

        // Background
        Image bg = cardObj.AddComponent<Image>();
        bg.color = GetFamilyColor(element.Family);

        // Make clickable
        Button button = cardObj.AddComponent<Button>();
        int capturedIndex = index;
        button.onClick.AddListener(() => ShowElementDetail(capturedIndex));

        // Add hover effect
        ColorBlock colors = button.colors;
        colors.highlightedColor = Color.white;
        colors.pressedColor = new Color(0.8f, 0.8f, 0.8f);
        button.colors = colors;

        // Symbol text
        GameObject symObj = new GameObject("Symbol");
        symObj.transform.SetParent(cardObj.transform);
        TextMeshProUGUI symText = symObj.AddComponent<TextMeshProUGUI>();
        symText.text = element.Symbol;
        symText.fontSize = 28;
        symText.fontStyle = FontStyles.Bold;
        symText.alignment = TextAlignmentOptions.Center;
        symText.color = Color.white;
        RectTransform symRect = symObj.GetComponent<RectTransform>();
        symRect.anchorMin = Vector2.zero;
        symRect.anchorMax = Vector2.one;
        symRect.offsetMin = new Vector2(5, 25);
        symRect.offsetMax = new Vector2(-5, -15);

        // Name text
        GameObject nameObj = new GameObject("Name");
        nameObj.transform.SetParent(cardObj.transform);
        TextMeshProUGUI nameText = nameObj.AddComponent<TextMeshProUGUI>();
        nameText.text = element.Name;
        nameText.fontSize = 10;
        nameText.alignment = TextAlignmentOptions.Center;
        nameText.color = Color.white;
        RectTransform nameRect = nameObj.GetComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0, 0);
        nameRect.anchorMax = new Vector2(1, 0);
        nameRect.pivot = new Vector2(0.5f, 0);
        nameRect.anchoredPosition = new Vector2(0, 5);
        nameRect.sizeDelta = new Vector2(80, 20);

        // Atomic number
        GameObject numObj = new GameObject("AtomicNum");
        numObj.transform.SetParent(cardObj.transform);
        TextMeshProUGUI numText = numObj.AddComponent<TextMeshProUGUI>();
        numText.text = element.AtomicNumber.ToString();
        numText.fontSize = 10;
        numText.alignment = TextAlignmentOptions.TopLeft;
        numText.color = Color.white;
        RectTransform numRect = numObj.GetComponent<RectTransform>();
        numRect.anchorMin = new Vector2(0, 1);
        numRect.anchorMax = new Vector2(0, 1);
        numRect.pivot = new Vector2(0, 1);
        numRect.anchoredPosition = new Vector2(5, -5);
        numRect.sizeDelta = new Vector2(30, 15);

        // "Click to learn more" tooltip
        GameObject tipObj = new GameObject("Tip");
        tipObj.transform.SetParent(cardObj.transform);
        TextMeshProUGUI tipText = tipObj.AddComponent<TextMeshProUGUI>();
        tipText.text = "Click to learn more";
        tipText.fontSize = 8;
        tipText.alignment = TextAlignmentOptions.Center;
        tipText.color = new Color(1, 1, 1, 0.5f);
        RectTransform tipRect = tipObj.GetComponent<RectTransform>();
        tipRect.anchorMin = new Vector2(0, 0);
        tipRect.anchorMax = new Vector2(1, 0);
        tipRect.pivot = new Vector2(0.5f, 1);
        tipRect.anchoredPosition = new Vector2(0, 0);
        tipRect.sizeDelta = new Vector2(80, 12);
    }

    private void ShowElementDetail(int index)
    {
        if (index < 0 || index >= unlockedAtomicNumbers.Count) return;
        
        currentElementIndex = index;
        int atomicNumber = unlockedAtomicNumbers[index];
        
        var element = PeriodicTable.GetElement(atomicNumber);
        var history = ElementHistory.GetInfo(atomicNumber);
        
        if (element == null) return;

        // Show detail panel
        if (elementDetailPanel != null)
            elementDetailPanel.SetActive(true);

        // Populate data
        if (elementNameText != null)
            elementNameText.text = element.Name;
        
        if (elementSymbolText != null)
            elementSymbolText.text = $"{element.Symbol} ({element.AtomicNumber})";

        if (history != null)
        {
            if (discoveryText != null)
            {
                string discoveryInfo = $"<b>Discovered:</b> {history.GetFormattedYear()}\n";
                discoveryInfo += $"<b>By:</b> {history.Discoverer}\n";
                discoveryInfo += $"<b>Location:</b> {history.DiscoveryLocation}";
                discoveryText.text = discoveryInfo;
            }

            if (historicalText != null)
                historicalText.text = $"<b>History:</b>\n{history.HistoricalFact}";

            if (modernUseText != null)
                modernUseText.text = $"<b>Modern Uses:</b>\n{history.ModernUse}";

            // Fun facts
            PopulateFunFacts(history.FunFacts);
        }
        else
        {
            // Basic info for elements without detailed history
            if (discoveryText != null)
                discoveryText.text = "Discovery information not yet documented.";
            if (historicalText != null)
                historicalText.text = $"Atomic Mass: {element.AtomicMass:F3}";
            if (modernUseText != null)
                modernUseText.text = $"Family: {element.Family}";
        }

        // Update navigation buttons
        UpdateNavigationButtons();
    }

    private void PopulateFunFacts(string[] facts)
    {
        if (funFactsContainer == null) return;

        foreach (Transform child in funFactsContainer)
        {
            Destroy(child.gameObject);
        }

        if (facts == null) return;

        foreach (string fact in facts)
        {
            CreateFactItem(funFactsContainer, $"⭐ {fact}");
        }
    }

    private void UpdateNavigationButtons()
    {
        if (prevElementButton != null)
            prevElementButton.interactable = currentElementIndex > 0;
        
        if (nextElementButton != null)
            nextElementButton.interactable = currentElementIndex < unlockedAtomicNumbers.Count - 1;
    }

    private void OnNextElement()
    {
        if (currentElementIndex < unlockedAtomicNumbers.Count - 1)
        {
            ShowElementDetail(currentElementIndex + 1);
        }
    }

    private void OnPrevElement()
    {
        if (currentElementIndex > 0)
        {
            ShowElementDetail(currentElementIndex - 1);
        }
    }

    private void OnBackClicked()
    {
        if (elementDetailPanel != null)
            elementDetailPanel.SetActive(false);
    }

    private void OnContinueClicked()
    {
        Hide();
    }

    public void Hide()
    {
        if (educationPanel != null)
            educationPanel.SetActive(false);
        if (elementDetailPanel != null)
            elementDetailPanel.SetActive(false);
    }

    private System.Collections.IEnumerator FadeIn()
    {
        if (canvasGroup == null) yield break;

        canvasGroup.alpha = 0f;
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private Color GetFamilyColor(ElementFamily family)
    {
        switch (family)
        {
            case ElementFamily.Hydrogen: return new Color(0.4f, 0.6f, 0.9f);
            case ElementFamily.Alkali: return new Color(0.9f, 0.4f, 0.4f);
            case ElementFamily.AlkalineEarth: return new Color(0.9f, 0.7f, 0.3f);
            case ElementFamily.TransitionMetal: return new Color(0.9f, 0.5f, 0.6f);
            case ElementFamily.PostTransitionMetal: return new Color(0.6f, 0.6f, 0.6f);
            case ElementFamily.Metalloid: return new Color(0.4f, 0.8f, 0.7f);
            case ElementFamily.NonMetal: return new Color(0.4f, 0.9f, 0.4f);
            case ElementFamily.Halogen: return new Color(0.9f, 0.9f, 0.4f);
            case ElementFamily.NobleGas: return new Color(0.5f, 0.8f, 0.9f);
            case ElementFamily.Lanthanide: return new Color(0.9f, 0.6f, 0.8f);
            case ElementFamily.Actinide: return new Color(0.9f, 0.4f, 0.7f);
            default: return Color.gray;
        }
    }
}

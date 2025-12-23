using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// UI panel displaying the chemistry glossary.
/// Terms unlock as the player progresses through levels.
/// </summary>
public class GlossaryUI : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject glossaryPanel;
    [SerializeField] private Transform categoryContainer;
    [SerializeField] private Transform termsContainer;
    [SerializeField] private GameObject termDetailPanel;

    [Header("Term Detail")]
    [SerializeField] private TextMeshProUGUI termTitleText;
    [SerializeField] private TextMeshProUGUI definitionText;
    [SerializeField] private TextMeshProUGUI funFactText;
    [SerializeField] private TextMeshProUGUI categoryBadge;

    [Header("Templates")]
    [SerializeField] private GameObject categoryButtonPrefab;
    [SerializeField] private GameObject termButtonPrefab;

    [Header("Buttons")]
    [SerializeField] private Button closeButton;

    [Header("State")]
    [SerializeField] private int currentPlayerLevel = 1;

    private string selectedCategory = "";
    private List<Button> categoryButtons = new List<Button>();
    private List<Button> termButtons = new List<Button>();

    private void Awake()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        Hide();
    }

    /// <summary>
    /// Show the glossary panel
    /// </summary>
    public void Show()
    {
        glossaryPanel.SetActive(true);
        PopulateCategories();
        
        // Show first category by default
        var categories = ChemistryGlossary.GetAllCategories();
        if (categories.Count > 0)
        {
            SelectCategory(categories[0]);
        }

        if (termDetailPanel != null)
            termDetailPanel.SetActive(false);
    }

    /// <summary>
    /// Hide the glossary panel
    /// </summary>
    public void Hide()
    {
        if (glossaryPanel != null)
            glossaryPanel.SetActive(false);
    }

    /// <summary>
    /// Toggle glossary visibility
    /// </summary>
    public void Toggle()
    {
        if (glossaryPanel.activeSelf)
            Hide();
        else
            Show();
    }

    /// <summary>
    /// Update player level to unlock more terms
    /// </summary>
    public void SetPlayerLevel(int level)
    {
        currentPlayerLevel = level;
        if (glossaryPanel.activeSelf)
        {
            PopulateTerms(selectedCategory);
        }
    }

    private void PopulateCategories()
    {
        // Clear existing
        foreach (var btn in categoryButtons)
        {
            if (btn != null)
                Destroy(btn.gameObject);
        }
        categoryButtons.Clear();

        foreach (Transform child in categoryContainer)
        {
            Destroy(child.gameObject);
        }

        var categories = ChemistryGlossary.GetAllCategories();
        categories.Sort();

        foreach (string category in categories)
        {
            CreateCategoryButton(category);
        }
    }

    private void CreateCategoryButton(string category)
    {
        GameObject btnObj = new GameObject($"Category_{category}");
        btnObj.transform.SetParent(categoryContainer);

        RectTransform rect = btnObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(150, 35);

        Image bg = btnObj.AddComponent<Image>();
        bg.color = new Color(0.2f, 0.3f, 0.4f);

        Button btn = btnObj.AddComponent<Button>();
        string capturedCategory = category;
        btn.onClick.AddListener(() => SelectCategory(capturedCategory));

        ColorBlock colors = btn.colors;
        colors.highlightedColor = new Color(0.3f, 0.5f, 0.6f);
        colors.selectedColor = new Color(0.4f, 0.6f, 0.7f);
        btn.colors = colors;

        // Category text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform);
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = $"📁 {category}";
        tmp.fontSize = 14;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        // Count available terms
        var terms = ChemistryGlossary.GetTermsByCategory(category);
        int available = 0;
        foreach (var term in terms)
        {
            if (term.UnlockLevel <= currentPlayerLevel)
                available++;
        }

        // Add count badge
        GameObject countObj = new GameObject("Count");
        countObj.transform.SetParent(btnObj.transform);
        TextMeshProUGUI countText = countObj.AddComponent<TextMeshProUGUI>();
        countText.text = $"{available}/{terms.Count}";
        countText.fontSize = 10;
        countText.alignment = TextAlignmentOptions.Right;
        countText.color = new Color(0.7f, 0.7f, 0.7f);
        
        RectTransform countRect = countObj.GetComponent<RectTransform>();
        countRect.anchorMin = new Vector2(1, 0.5f);
        countRect.anchorMax = new Vector2(1, 0.5f);
        countRect.pivot = new Vector2(1, 0.5f);
        countRect.anchoredPosition = new Vector2(-5, 0);
        countRect.sizeDelta = new Vector2(40, 20);

        categoryButtons.Add(btn);
    }

    private void SelectCategory(string category)
    {
        selectedCategory = category;
        PopulateTerms(category);

        // Update button visuals
        foreach (var btn in categoryButtons)
        {
            Image bg = btn.GetComponent<Image>();
            TextMeshProUGUI text = btn.GetComponentInChildren<TextMeshProUGUI>();
            
            bool isSelected = text != null && text.text.Contains(category);
            bg.color = isSelected ? new Color(0.4f, 0.6f, 0.7f) : new Color(0.2f, 0.3f, 0.4f);
        }
    }

    private void PopulateTerms(string category)
    {
        // Clear existing
        foreach (var btn in termButtons)
        {
            if (btn != null)
                Destroy(btn.gameObject);
        }
        termButtons.Clear();

        foreach (Transform child in termsContainer)
        {
            Destroy(child.gameObject);
        }

        var terms = ChemistryGlossary.GetTermsByCategory(category);
        terms.Sort((a, b) => a.UnlockLevel.CompareTo(b.UnlockLevel));

        foreach (var term in terms)
        {
            CreateTermButton(term);
        }
    }

    private void CreateTermButton(GlossaryEntry entry)
    {
        bool isUnlocked = entry.UnlockLevel <= currentPlayerLevel;

        GameObject btnObj = new GameObject($"Term_{entry.Term}");
        btnObj.transform.SetParent(termsContainer);

        RectTransform rect = btnObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 40);

        Image bg = btnObj.AddComponent<Image>();
        bg.color = isUnlocked ? new Color(0.25f, 0.35f, 0.45f) : new Color(0.15f, 0.15f, 0.2f);

        Button btn = btnObj.AddComponent<Button>();
        btn.interactable = isUnlocked;
        
        GlossaryEntry capturedEntry = entry;
        btn.onClick.AddListener(() => ShowTermDetail(capturedEntry));

        // Term text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform);
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        
        if (isUnlocked)
        {
            tmp.text = $"📖 {entry.Term}";
            tmp.color = Color.white;
        }
        else
        {
            tmp.text = $"🔒 {entry.Term}";
            tmp.color = new Color(0.5f, 0.5f, 0.5f);
        }
        
        tmp.fontSize = 14;
        tmp.alignment = TextAlignmentOptions.Left;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(10, 0);
        textRect.offsetMax = new Vector2(-10, 0);

        // Unlock level badge for locked terms
        if (!isUnlocked)
        {
            GameObject levelObj = new GameObject("Level");
            levelObj.transform.SetParent(btnObj.transform);
            TextMeshProUGUI levelText = levelObj.AddComponent<TextMeshProUGUI>();
            levelText.text = $"Lv.{entry.UnlockLevel}";
            levelText.fontSize = 10;
            levelText.alignment = TextAlignmentOptions.Right;
            levelText.color = new Color(0.4f, 0.4f, 0.4f);
            
            RectTransform levelRect = levelObj.GetComponent<RectTransform>();
            levelRect.anchorMin = new Vector2(1, 0.5f);
            levelRect.anchorMax = new Vector2(1, 0.5f);
            levelRect.pivot = new Vector2(1, 0.5f);
            levelRect.anchoredPosition = new Vector2(-5, 0);
            levelRect.sizeDelta = new Vector2(40, 20);
        }

        termButtons.Add(btn);
    }

    private void ShowTermDetail(GlossaryEntry entry)
    {
        if (termDetailPanel != null)
            termDetailPanel.SetActive(true);

        if (termTitleText != null)
            termTitleText.text = entry.Term;

        if (definitionText != null)
            definitionText.text = entry.Definition;

        if (funFactText != null)
            funFactText.text = $"💡 <i>{entry.FunFact}</i>";

        if (categoryBadge != null)
            categoryBadge.text = entry.Category;
    }
}

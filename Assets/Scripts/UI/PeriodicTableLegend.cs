using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Legend panel showing color coding for element families and highlight states
/// </summary>
public class PeriodicTableLegend : MonoBehaviour
{
    [Header("Layout")]
    [SerializeField] private Transform familyLegendContainer;
    [SerializeField] private Transform stateLegendContainer;
    [SerializeField] private GameObject legendItemPrefab;

    [Header("Colors")]
    [SerializeField] private Color lockedColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    [SerializeField] private Color unlockedColor = new Color(0.4f, 0.4f, 0.5f, 1f);
    [SerializeField] private Color ownedColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    [SerializeField] private Color inMoleculeColor = new Color(1f, 0.84f, 0f, 1f);

    private void Start()
    {
        GenerateLegend();
    }

    public void GenerateLegend()
    {
        // Family colors
        CreateLegendSection("Element Families", familyLegendContainer, new (string, Color)[]
        {
            ("Hydrogen", new Color(0.6f, 0.8f, 1f)),
            ("Alkali Metals", new Color(1f, 0.6f, 0.6f)),
            ("Alkaline Earth", new Color(1f, 0.85f, 0.6f)),
            ("Transition Metals", new Color(1f, 0.75f, 0.8f)),
            ("Post-Transition", new Color(0.8f, 0.8f, 0.8f)),
            ("Metalloids", new Color(0.6f, 0.9f, 0.8f)),
            ("Nonmetals", new Color(0.6f, 1f, 0.6f)),
            ("Halogens", new Color(1f, 1f, 0.6f)),
            ("Noble Gases", new Color(0.7f, 0.9f, 1f)),
            ("Lanthanides", new Color(1f, 0.75f, 0.9f)),
            ("Actinides", new Color(1f, 0.6f, 0.85f))
        });

        // Highlight states
        CreateLegendSection("Status", stateLegendContainer, new (string, Color)[]
        {
            ("Locked", lockedColor),
            ("Unlocked", unlockedColor),
            ("Owned", ownedColor),
            ("In Molecule", inMoleculeColor)
        });
    }

    private void CreateLegendSection(string title, Transform container, (string name, Color color)[] items)
    {
        if (container == null) return;

        // Clear existing
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // Create title
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(container);
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = title;
        titleText.fontSize = 14;
        titleText.fontStyle = FontStyles.Bold;
        titleText.color = Color.white;
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(150, 20);

        // Create items
        foreach (var item in items)
        {
            CreateLegendItem(container, item.name, item.color);
        }
    }

    private void CreateLegendItem(Transform parent, string label, Color color)
    {
        GameObject itemObj = new GameObject("Legend_" + label);
        itemObj.transform.SetParent(parent);

        HorizontalLayoutGroup layout = itemObj.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 5;
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        RectTransform itemRect = itemObj.GetComponent<RectTransform>();
        itemRect.sizeDelta = new Vector2(150, 18);

        // Color swatch
        GameObject swatchObj = new GameObject("Swatch");
        swatchObj.transform.SetParent(itemObj.transform);
        Image swatchImage = swatchObj.AddComponent<Image>();
        swatchImage.color = color;
        RectTransform swatchRect = swatchObj.GetComponent<RectTransform>();
        swatchRect.sizeDelta = new Vector2(16, 16);
        LayoutElement swatchLayout = swatchObj.AddComponent<LayoutElement>();
        swatchLayout.preferredWidth = 16;
        swatchLayout.preferredHeight = 16;

        // Label text
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(itemObj.transform);
        TextMeshProUGUI labelText = labelObj.AddComponent<TextMeshProUGUI>();
        labelText.text = label;
        labelText.fontSize = 11;
        labelText.color = Color.white;
        RectTransform labelRect = labelObj.GetComponent<RectTransform>();
        labelRect.sizeDelta = new Vector2(120, 18);
        LayoutElement labelLayout = labelObj.AddComponent<LayoutElement>();
        labelLayout.preferredWidth = 120;
        labelLayout.preferredHeight = 18;
    }
}

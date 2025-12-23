using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Handles visual representation of units including sprites, effects, and UI elements.
/// </summary>
public class UnitVisuals : MonoBehaviour
{
    [Header("Core Components")]
    [SerializeField] private SpriteRenderer mainSprite;
    [SerializeField] private SpriteRenderer glowSprite;
    [SerializeField] private SpriteRenderer shadowSprite;
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshPro symbolText;
    [SerializeField] private SpriteRenderer[] starIndicators;
    [SerializeField] private Transform healthBarContainer;
    [SerializeField] private SpriteRenderer healthBarFill;
    
    [Header("Effect Transforms")]
    [SerializeField] private Transform effectsParent;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject deathEffectPrefab;
    [SerializeField] private GameObject upgradeEffectPrefab;
    [SerializeField] private GameObject bondLinesPrefab;

    [Header("Colors")]
    [SerializeField] private Color healthyColor = Color.green;
    [SerializeField] private Color damagedColor = Color.yellow;
    [SerializeField] private Color criticalColor = Color.red;

    private Unit unit;
    private ElementData elementData;
    private Material glowMaterial;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        
        // Get or create components
        if (mainSprite == null)
            mainSprite = GetComponent<SpriteRenderer>();
        
        // Create glow sprite if needed
        if (glowSprite == null)
        {
            CreateGlowSprite();
        }
        
        // Create symbol text if needed
        if (symbolText == null)
        {
            CreateSymbolText();
        }
    }

    private void CreateGlowSprite()
    {
        GameObject glowObj = new GameObject("Glow");
        glowObj.transform.SetParent(transform);
        glowObj.transform.localPosition = Vector3.zero;
        glowObj.transform.localScale = Vector3.one * 1.2f;
        
        glowSprite = glowObj.AddComponent<SpriteRenderer>();
        glowSprite.sortingOrder = mainSprite.sortingOrder - 1;
    }

    private void CreateSymbolText()
    {
        GameObject textObj = new GameObject("Symbol");
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = Vector3.zero;
        
        symbolText = textObj.AddComponent<TextMeshPro>();
        symbolText.alignment = TextAlignmentOptions.Center;
        symbolText.fontSize = 3;
        symbolText.sortingOrder = mainSprite.sortingOrder + 1;
    }

    /// <summary>
    /// Set up visuals based on element data
    /// </summary>
    public void SetupVisuals(ElementData data, int starLevel)
    {
        elementData = data;
        
        // Set main sprite color
        if (mainSprite != null)
        {
            mainSprite.color = data.ElementColor;
            
            if (data.Icon != null)
                mainSprite.sprite = data.Icon;
        }
        
        // Set glow based on family
        if (glowSprite != null)
        {
            glowSprite.color = GetFamilyGlowColor(data.Family);
        }
        
        // Set symbol text
        if (symbolText != null)
        {
            symbolText.text = data.Symbol;
            symbolText.color = GetContrastColor(data.ElementColor);
        }
        
        // Update star indicators
        UpdateStarIndicators(starLevel);
    }

    /// <summary>
    /// Get glow color based on element family
    /// </summary>
    private Color GetFamilyGlowColor(ElementFamily family)
    {
        return family switch
        {
            ElementFamily.Alkali => new Color(1f, 0.5f, 0f, 0.5f),        // Orange
            ElementFamily.AlkalineEarth => new Color(0.8f, 0.8f, 0f, 0.5f), // Yellow
            ElementFamily.TransitionMetal => new Color(0.7f, 0.7f, 0.8f, 0.5f), // Silver
            ElementFamily.NonMetal => new Color(0.2f, 0.6f, 1f, 0.5f),    // Blue
            ElementFamily.Halogen => new Color(0.5f, 0f, 0.5f, 0.5f),     // Purple
            ElementFamily.NobleGas => new Color(0f, 1f, 1f, 0.5f),        // Cyan
            ElementFamily.Hydrogen => new Color(1f, 1f, 1f, 0.5f),        // White
            _ => new Color(0.5f, 0.5f, 0.5f, 0.5f)
        };
    }

    /// <summary>
    /// Get contrasting text color for readability
    /// </summary>
    private Color GetContrastColor(Color background)
    {
        float luminance = 0.299f * background.r + 0.587f * background.g + 0.114f * background.b;
        return luminance > 0.5f ? Color.black : Color.white;
    }

    /// <summary>
    /// Update star level indicators
    /// </summary>
    private void UpdateStarIndicators(int starLevel)
    {
        if (starIndicators == null) return;
        
        for (int i = 0; i < starIndicators.Length; i++)
        {
            if (starIndicators[i] != null)
            {
                starIndicators[i].enabled = i < starLevel;
                starIndicators[i].color = starLevel == 3 ? Color.yellow : Color.white;
            }
        }
    }

    /// <summary>
    /// Update health bar display
    /// </summary>
    public void UpdateHealthBar()
    {
        if (healthBarFill == null || unit == null) return;
        
        float healthPercent = unit.CurrentHP / unit.MaxHP;
        healthBarFill.transform.localScale = new Vector3(healthPercent, 1f, 1f);
        
        // Color based on health
        if (healthPercent > 0.6f)
            healthBarFill.color = healthyColor;
        else if (healthPercent > 0.3f)
            healthBarFill.color = damagedColor;
        else
            healthBarFill.color = criticalColor;
    }

    #region Effect Methods

    /// <summary>
    /// Play hit effect when taking damage
    /// </summary>
    public void PlayHitEffect()
    {
        StartCoroutine(HitFlash());
        
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }
        
        UpdateHealthBar();
    }

    private IEnumerator HitFlash()
    {
        if (mainSprite == null) yield break;
        
        Color originalColor = mainSprite.color;
        mainSprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        mainSprite.color = originalColor;
    }

    /// <summary>
    /// Play death effect
    /// </summary>
    public void PlayDeathEffect()
    {
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
        
        // Fade out
        StartCoroutine(DeathFade());
    }

    private IEnumerator DeathFade()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        
        Color startColor = mainSprite.color;
        Vector3 startScale = transform.localScale;
        
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            
            if (mainSprite != null)
            {
                mainSprite.color = new Color(startColor.r, startColor.g, startColor.b, 1f - t);
            }
            
            transform.localScale = startScale * (1f - t * 0.5f);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Play upgrade effect when combining units
    /// </summary>
    public void PlayUpgradeEffect(int newStarLevel)
    {
        UpdateStarIndicators(newStarLevel);
        
        if (upgradeEffectPrefab != null)
        {
            Instantiate(upgradeEffectPrefab, transform.position, Quaternion.identity);
        }
        
        StartCoroutine(UpgradeAnimation(newStarLevel));
    }

    private IEnumerator UpgradeAnimation(int starLevel)
    {
        // Scale pulse
        Vector3 originalScale = transform.localScale;
        Vector3 largeScale = originalScale * 1.5f;
        float duration = 0.3f;
        
        // Grow
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, largeScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // Update size based on star level
        float sizeMultiplier = 1f + (starLevel - 1) * 0.2f;
        Vector3 newScale = originalScale * sizeMultiplier;
        
        // Shrink to new size
        elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(largeScale, newScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.localScale = newScale;
    }

    /// <summary>
    /// Show bond lines to connected units
    /// </summary>
    public void ShowBondLines(Unit[] bondedUnits)
    {
        // Clear existing bond lines
        HideBondLines();
        
        foreach (var bondedUnit in bondedUnits)
        {
            if (bondedUnit == null) continue;
            
            // Create line between units
            GameObject lineObj = new GameObject("BondLine");
            lineObj.transform.SetParent(effectsParent != null ? effectsParent : transform);
            
            LineRenderer line = lineObj.AddComponent<LineRenderer>();
            line.positionCount = 2;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, bondedUnit.transform.position);
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = Color.cyan;
            line.endColor = Color.cyan;
            line.sortingOrder = mainSprite.sortingOrder - 2;
        }
    }

    /// <summary>
    /// Hide all bond lines
    /// </summary>
    public void HideBondLines()
    {
        Transform parent = effectsParent != null ? effectsParent : transform;
        
        foreach (Transform child in parent)
        {
            if (child.name == "BondLine")
            {
                Destroy(child.gameObject);
            }
        }
    }

    /// <summary>
    /// Pulse glow for ability cast
    /// </summary>
    public void PlayAbilityCastEffect()
    {
        StartCoroutine(AbilityGlowPulse());
    }

    private IEnumerator AbilityGlowPulse()
    {
        if (glowSprite == null) yield break;
        
        Color originalColor = glowSprite.color;
        Color brightColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        
        float duration = 0.2f;
        
        // Pulse up
        for (int i = 0; i < 3; i++)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                glowSprite.color = Color.Lerp(originalColor, brightColor, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            elapsed = 0f;
            while (elapsed < duration)
            {
                glowSprite.color = Color.Lerp(brightColor, originalColor, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        
        glowSprite.color = originalColor;
    }

    #endregion

    private void LateUpdate()
    {
        // Keep health bar facing camera (for 3D scenarios)
        if (healthBarContainer != null && Camera.main != null)
        {
            healthBarContainer.rotation = Camera.main.transform.rotation;
        }
    }
}

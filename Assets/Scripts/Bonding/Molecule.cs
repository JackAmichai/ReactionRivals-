using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represents a formed molecule on the board.
/// Tracks the component units and provides molecule-specific abilities.
/// </summary>
public class Molecule : MonoBehaviour
{
    [Header("Recipe")]
    public MoleculeRecipe Recipe;

    [Header("Component Units")]
    public Unit CoreUnit;
    public List<Unit> ComponentUnits = new List<Unit>();

    [Header("State")]
    [Tooltip("For metallic bonds - shared damage pool")]
    public float SharedDamagePool;
    
    [Tooltip("Is this molecule currently active")]
    public bool IsActive = true;

    [Header("Visuals")]
    [SerializeField] private LineRenderer[] bondLines;
    [SerializeField] private ParticleSystem moleculeAura;

    /// <summary>
    /// Initialize the molecule with its components
    /// </summary>
    public void Initialize(MoleculeRecipe recipe, Unit core, Unit[] components)
    {
        Recipe = recipe;
        CoreUnit = core;
        ComponentUnits = new List<Unit>(components);
        IsActive = true;

        gameObject.name = $"Molecule_{recipe.MoleculeName}";

        // Subscribe to unit death events
        if (core != null)
        {
            core.OnUnitDeath += OnComponentDeath;
        }
        foreach (var comp in components)
        {
            if (comp != null)
            {
                comp.OnUnitDeath += OnComponentDeath;
            }
        }

        // Set up visuals
        SetupVisuals();
    }

    /// <summary>
    /// Handle damage to the molecule (for ionic/metallic bonds)
    /// </summary>
    public void TakeDamage(float damage, Unit target)
    {
        if (Recipe == null) return;

        switch (Recipe.BondType)
        {
            case BondType.Ionic:
                // Reflect damage back
                HandleIonicDamage(damage, target);
                break;

            case BondType.Metallic:
                // Distribute damage across pool
                HandleMetallicDamage(damage, target);
                break;
        }
    }

    /// <summary>
    /// Handle ionic bond damage reflection
    /// </summary>
    private void HandleIonicDamage(float damage, Unit target)
    {
        // Reduce incoming damage by 30%
        float reducedDamage = damage * 0.7f;
        
        // Reflect 20% back to attacker
        // Note: This would need attacker reference from combat system
        
        // Apply reduced damage
        target.CurrentHP -= reducedDamage;
    }

    /// <summary>
    /// Handle metallic bond damage sharing
    /// </summary>
    private void HandleMetallicDamage(float damage, Unit target)
    {
        // Distribute damage across all units in the molecule
        int unitCount = ComponentUnits.Count + 1; // +1 for core
        float distributedDamage = damage / unitCount;

        // Apply to pool
        SharedDamagePool -= damage;

        // Check if pool is depleted
        if (SharedDamagePool <= 0)
        {
            // All units in molecule die
            Break();
        }
        else
        {
            // Update individual unit HP visuals
            float hpPerUnit = SharedDamagePool / unitCount;
            
            if (CoreUnit != null)
            {
                CoreUnit.CurrentHP = Mathf.Min(hpPerUnit, CoreUnit.MaxHP);
            }
            foreach (var comp in ComponentUnits)
            {
                if (comp != null && comp.gameObject.activeSelf)
                {
                    comp.CurrentHP = Mathf.Min(hpPerUnit, comp.MaxHP);
                }
            }
        }
    }

    /// <summary>
    /// Handle when a component unit dies
    /// </summary>
    private void OnComponentDeath(Unit deadUnit)
    {
        // Remove from components
        ComponentUnits.Remove(deadUnit);

        // Check if molecule should break
        if (deadUnit == CoreUnit || ShouldMoleculeBreak())
        {
            Break();
        }
    }

    /// <summary>
    /// Check if the molecule should break based on remaining components
    /// </summary>
    private bool ShouldMoleculeBreak()
    {
        if (Recipe == null) return true;

        // Count remaining required elements
        foreach (var req in Recipe.RequiredElements)
        {
            int remaining = ComponentUnits.Count(u => u != null && u.Data?.Symbol == req.Element.Symbol);
            if (remaining < req.Count)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Break the molecule, releasing components
    /// </summary>
    public void Break()
    {
        IsActive = false;
        BondingManager.Instance?.BreakMolecule(this);
    }

    /// <summary>
    /// Execute the molecule's special ability
    /// </summary>
    public void TriggerSpecialAbility()
    {
        if (Recipe == null || CoreUnit == null) return;

        switch (Recipe.SpecialAbility)
        {
            case MoleculeAbility.Healing:
                HealNearbyAllies();
                break;

            case MoleculeAbility.PoisonCloud:
                EmitPoisonCloud();
                break;

            case MoleculeAbility.Suffocate:
                SlowNearbyEnemies();
                break;

            case MoleculeAbility.Cleanse:
                CleanseNearbyAllies();
                break;

            case MoleculeAbility.CrystalArmor:
                // Passive - handled in TakeDamage
                break;

            case MoleculeAbility.Corrosion:
                ApplyCorrosion();
                break;

            case MoleculeAbility.EnergyBoost:
                BoostAllyMana();
                break;
        }
    }

    #region Special Abilities

    private void HealNearbyAllies()
    {
        if (CoreUnit?.CurrentCell == null) return;

        var allies = HexGrid.Instance?.GetUnitsInRange(CoreUnit.CurrentCell, 2, true)
            .Where(u => u.CurrentCell.OwnerID == CoreUnit.CurrentCell.OwnerID);

        float healAmount = CoreUnit.MaxHP * 0.2f; // Heal 20% of max HP

        foreach (var ally in allies)
        {
            ally.CurrentHP = Mathf.Min(ally.CurrentHP + healAmount, ally.MaxHP);
        }

        Debug.Log($"💧 {Recipe.MoleculeName} heals nearby allies for {healAmount}!");
    }

    private void EmitPoisonCloud()
    {
        if (CoreUnit?.CurrentCell == null) return;

        var enemies = HexGrid.Instance?.GetUnitsInRange(CoreUnit.CurrentCell, 2, false)
            .Where(u => u.CurrentCell.OwnerID != CoreUnit.CurrentCell.OwnerID);

        foreach (var enemy in enemies)
        {
            var combat = enemy.GetComponent<UnitCombat>();
            combat?.ApplyStatusEffect(enemy, StatusEffect.Poison, 5f);
        }

        Debug.Log($"☠️ {Recipe.MoleculeName} emits poison cloud!");
    }

    private void SlowNearbyEnemies()
    {
        if (CoreUnit?.CurrentCell == null) return;

        var enemies = HexGrid.Instance?.GetUnitsInRange(CoreUnit.CurrentCell, 3, false)
            .Where(u => u.CurrentCell.OwnerID != CoreUnit.CurrentCell.OwnerID);

        foreach (var enemy in enemies)
        {
            var combat = enemy.GetComponent<UnitCombat>();
            combat?.ApplyStatusEffect(enemy, StatusEffect.Slow, 4f);
        }

        Debug.Log($"💨 {Recipe.MoleculeName} suffocates enemies, slowing them!");
    }

    private void CleanseNearbyAllies()
    {
        if (CoreUnit?.CurrentCell == null) return;

        var allies = HexGrid.Instance?.GetUnitsInRange(CoreUnit.CurrentCell, 2, true)
            .Where(u => u.CurrentCell.OwnerID == CoreUnit.CurrentCell.OwnerID);

        foreach (var ally in allies)
        {
            var combat = ally.GetComponent<UnitCombat>();
            if (combat != null)
            {
                combat.IsBurning = false;
                combat.IsPoisoned = false;
                combat.IsStunned = false;
            }
        }

        Debug.Log($"✨ {Recipe.MoleculeName} cleanses allies!");
    }

    private void ApplyCorrosion()
    {
        if (CoreUnit?.CurrentCell == null) return;

        var enemies = HexGrid.Instance?.GetUnitsInRange(CoreUnit.CurrentCell, 2, false)
            .Where(u => u.CurrentCell.OwnerID != CoreUnit.CurrentCell.OwnerID);

        foreach (var enemy in enemies)
        {
            var combat = enemy.GetComponent<UnitCombat>();
            combat?.ApplyStatusEffect(enemy, StatusEffect.Corroded, 6f);
        }

        Debug.Log($"🧪 {Recipe.MoleculeName} corrodes enemy armor!");
    }

    private void BoostAllyMana()
    {
        if (CoreUnit?.CurrentCell == null) return;

        var allies = HexGrid.Instance?.GetUnitsInRange(CoreUnit.CurrentCell, 2, true)
            .Where(u => u.CurrentCell.OwnerID == CoreUnit.CurrentCell.OwnerID);

        foreach (var ally in allies)
        {
            var combat = ally.GetComponent<UnitCombat>();
            if (combat != null)
            {
                combat.GainElectron(2);
            }
        }

        Debug.Log($"⚡ {Recipe.MoleculeName} boosts ally electrons!");
    }

    #endregion

    #region Visuals

    private void SetupVisuals()
    {
        if (Recipe == null) return;

        // Create bond lines between units
        CreateBondLines();

        // Set molecule color
        if (moleculeAura != null)
        {
            var main = moleculeAura.main;
            main.startColor = Recipe.MoleculeColor;
        }
    }

    private void CreateBondLines()
    {
        // Create visual bond lines connecting all units
        List<LineRenderer> lines = new List<LineRenderer>();

        foreach (var comp in ComponentUnits)
        {
            if (comp == null || CoreUnit == null) continue;

            GameObject lineObj = new GameObject($"Bond_{comp.Data?.Symbol}");
            lineObj.transform.SetParent(transform);

            LineRenderer line = lineObj.AddComponent<LineRenderer>();
            line.positionCount = 2;
            line.SetPosition(0, CoreUnit.transform.position);
            line.SetPosition(1, comp.transform.position);
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            
            // Bond color based on type
            Color bondColor = Recipe.BondType switch
            {
                BondType.Covalent => Color.cyan,
                BondType.Ionic => Color.yellow,
                BondType.Metallic => new Color(0.7f, 0.7f, 0.8f),
                _ => Color.white
            };
            
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = bondColor;
            line.endColor = bondColor;

            lines.Add(line);
        }

        bondLines = lines.ToArray();
    }

    private void Update()
    {
        // Update bond line positions
        if (bondLines != null && CoreUnit != null)
        {
            for (int i = 0; i < bondLines.Length && i < ComponentUnits.Count; i++)
            {
                if (bondLines[i] != null && ComponentUnits[i] != null)
                {
                    bondLines[i].SetPosition(0, CoreUnit.transform.position);
                    bondLines[i].SetPosition(1, ComponentUnits[i].transform.position);
                }
            }
        }
    }

    #endregion

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (CoreUnit != null)
        {
            CoreUnit.OnUnitDeath -= OnComponentDeath;
        }
        foreach (var comp in ComponentUnits)
        {
            if (comp != null)
            {
                comp.OnUnitDeath -= OnComponentDeath;
            }
        }
    }

    // LINQ helper
    private int Count(System.Func<Unit, bool> predicate)
    {
        int count = 0;
        foreach (var unit in ComponentUnits)
        {
            if (predicate(unit)) count++;
        }
        return count;
    }
}

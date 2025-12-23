using UnityEngine;
using System.Collections;

/// <summary>
/// Handles combat logic for units including attacking, abilities, and the Octet Rule mana system.
/// Units gain electrons on attack and cast their ultimate when reaching 8 electrons (Octet Rule).
/// </summary>
public class UnitCombat : MonoBehaviour
{
    [Header("References")]
    private Unit unit;
    
    [Header("Electron System (Mana)")]
    [Tooltip("Current electrons gained from combat")]
    public int CurrentElectrons;
    
    [Tooltip("Base valence electrons from element data")]
    public int ValenceElectrons;
    
    [Tooltip("Electrons needed for full shell (8 for Octet Rule, 2 for Hydrogen's Duet Rule)")]
    public int MaxElectrons = 8;

    [Header("Combat State")]
    public bool IsInCombat = false;
    public Unit CurrentTarget;
    public float AttackCooldown = 0f;

    [Header("Status Effects")]
    public bool IsStunned = false;
    public bool IsBurning = false;
    public bool IsPoisoned = false;
    public float BurnDamagePerTick = 5f;
    public float PoisonDamagePerTick = 3f;

    // Combat timing
    private float timeSinceLastAttack = 0f;
    private float statusTickTimer = 0f;
    private const float STATUS_TICK_INTERVAL = 1f;

    public void Initialize(Unit parentUnit)
    {
        unit = parentUnit;
        
        if (unit.Data != null)
        {
            ValenceElectrons = unit.Data.ValenceElectrons;
            CurrentElectrons = ValenceElectrons;
            
            // Set max electrons based on element type:
            // Hydrogen/Helium follow Duet Rule (2), others follow Octet Rule (8)
            MaxElectrons = unit.Data.FullShellElectrons;
        }
        
        AttackCooldown = unit.Data != null ? 1f / unit.Data.AttackSpeed : 1f;
    }

    private void Update()
    {
        if (!IsInCombat) return;
        
        timeSinceLastAttack += Time.deltaTime;
        
        // Process status effects
        ProcessStatusEffects();
        
        // Combat logic
        if (!IsStunned)
        {
            if (CurrentTarget == null || !IsValidTarget(CurrentTarget))
            {
                FindNewTarget();
            }
            
            if (CurrentTarget != null && timeSinceLastAttack >= AttackCooldown)
            {
                PerformAttack();
            }
        }
    }

    /// <summary>
    /// Start combat mode
    /// </summary>
    public void EnterCombat()
    {
        IsInCombat = true;
        CurrentTarget = null;
        timeSinceLastAttack = 0f;
        FindNewTarget();
    }

    /// <summary>
    /// Exit combat mode
    /// </summary>
    public void ExitCombat()
    {
        IsInCombat = false;
        CurrentTarget = null;
        ClearStatusEffects();
    }

    /// <summary>
    /// Find a new target to attack
    /// </summary>
    private void FindNewTarget()
    {
        if (unit.CurrentCell == null) return;
        
        // Get all enemy units
        var enemyUnits = HexGrid.Instance?.GetAllBoardUnits(unit.CurrentCell.OwnerID == 0 ? 1 : 0);
        if (enemyUnits == null || enemyUnits.Count == 0)
        {
            CurrentTarget = null;
            return;
        }

        // Find closest enemy in range
        Unit closestEnemy = null;
        int closestDistance = int.MaxValue;
        
        foreach (var enemy in enemyUnits)
        {
            if (!IsValidTarget(enemy)) continue;
            
            int distance = unit.CurrentCell.DistanceTo(enemy.CurrentCell);
            
            if (distance <= unit.AttackRange && distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        CurrentTarget = closestEnemy;
        
        // If no target in range, move towards closest enemy (future: pathfinding)
        if (CurrentTarget == null && enemyUnits.Count > 0)
        {
            // TODO: Implement movement towards enemies
        }
    }

    /// <summary>
    /// Check if a unit is a valid attack target
    /// </summary>
    private bool IsValidTarget(Unit target)
    {
        if (target == null) return false;
        if (target.CurrentHP <= 0) return false;
        if (target.CurrentCell == null) return false;
        
        // Noble gases are spell immune (but can still be auto-attacked)
        return true;
    }

    /// <summary>
    /// Perform an attack on the current target
    /// </summary>
    private void PerformAttack()
    {
        if (CurrentTarget == null) return;
        
        timeSinceLastAttack = 0f;
        
        // Deal damage
        float damage = unit.Damage;
        
        // Apply Halogen electron steal (high electronegativity)
        if (unit.Data.Family == ElementFamily.Halogen)
        {
            StealElectrons(CurrentTarget);
        }
        
        CurrentTarget.TakeDamage(damage, unit);
        
        // Gain electron on attack
        GainElectron();
        
        // Visual feedback
        StartCoroutine(AttackAnimation());
        
        Debug.Log($"{unit.Data.Symbol} attacks {CurrentTarget.Data.Symbol} for {damage} damage!");
    }

    /// <summary>
    /// Gain an electron (mana) towards ultimate
    /// </summary>
    public void GainElectron(int amount = 1)
    {
        CurrentElectrons += amount;
        
        // Check for Octet Rule (full outer shell)
        if (CurrentElectrons >= MaxElectrons)
        {
            CastUltimate();
        }
    }

    /// <summary>
    /// Cast the unit's ultimate ability (Octet Rule triggered)
    /// </summary>
    private void CastUltimate()
    {
        Debug.Log($"{unit.Data.Symbol} reached Octet! Casting Ultimate!");
        
        // Reset electrons to base valence
        CurrentElectrons = ValenceElectrons;
        
        AbilityData ability = unit.Data.UltimateAbility;
        if (ability == null)
        {
            // Default ultimate based on element family
            CastDefaultUltimate();
            return;
        }
        
        // Execute the ability
        ExecuteAbility(ability);
    }

    /// <summary>
    /// Cast a default ultimate based on element family
    /// </summary>
    private void CastDefaultUltimate()
    {
        switch (unit.Data.Family)
        {
            case ElementFamily.Alkali:
                // Explosion - AoE damage
                CastExplosion();
                break;
            
            case ElementFamily.Halogen:
                // Mass electron steal
                CastMassSteal();
                break;
            
            case ElementFamily.NobleGas:
                // Shield self and allies
                CastInertShield();
                break;
            
            case ElementFamily.NonMetal:
                // Standard damage ability
                if (CurrentTarget != null)
                    CurrentTarget.TakeDamage(unit.Damage * 2f, unit);
                break;
            
            default:
                // Generic damage
                if (CurrentTarget != null)
                    CurrentTarget.TakeDamage(unit.Damage * 1.5f, unit);
                break;
        }
    }

    /// <summary>
    /// Execute a configured ability
    /// </summary>
    private void ExecuteAbility(AbilityData ability)
    {
        switch (ability.TargetType)
        {
            case AbilityTargetType.SingleEnemy:
                if (CurrentTarget != null)
                {
                    CurrentTarget.TakeDamage(ability.Damage, unit);
                    ApplyStatusEffect(CurrentTarget, ability.AppliedEffect, ability.EffectDuration);
                }
                break;
            
            case AbilityTargetType.AoEAroundSelf:
                var nearbyUnits = HexGrid.Instance?.GetUnitsInRange(unit.CurrentCell, ability.AoERadius, false);
                foreach (var target in nearbyUnits)
                {
                    if (target.CurrentCell.OwnerID != unit.CurrentCell.OwnerID)
                    {
                        target.TakeDamage(ability.Damage, unit);
                        ApplyStatusEffect(target, ability.AppliedEffect, ability.EffectDuration);
                    }
                }
                break;
            
            case AbilityTargetType.AllAllies:
                var allies = HexGrid.Instance?.GetAllBoardUnits(unit.CurrentCell.OwnerID);
                foreach (var ally in allies)
                {
                    if (ability.Healing > 0)
                    {
                        ally.CurrentHP = Mathf.Min(ally.CurrentHP + ability.Healing, ally.MaxHP);
                    }
                }
                break;
        }
        
        // Check for sacrifice
        if (ability.SacrificeOnCast)
        {
            unit.TakeDamage(unit.CurrentHP, unit);
        }
        
        // Spawn VFX
        if (ability.CastVFX != null)
        {
            Instantiate(ability.CastVFX, transform.position, Quaternion.identity);
        }
    }

    #region Element-Specific Abilities

    private void CastExplosion()
    {
        float explosionDamage = unit.Damage * 2.5f;
        var nearbyUnits = HexGrid.Instance?.GetUnitsInRange(unit.CurrentCell, 2, false);
        
        foreach (var target in nearbyUnits)
        {
            target.TakeDamage(explosionDamage, unit);
        }
        
        Debug.Log($"{unit.Data.Symbol} EXPLODES!");
    }

    private void CastMassSteal()
    {
        var enemies = HexGrid.Instance?.GetAllBoardUnits(unit.CurrentCell.OwnerID == 0 ? 1 : 0);
        
        foreach (var enemy in enemies)
        {
            StealElectrons(enemy, 2);
        }
        
        Debug.Log($"{unit.Data.Symbol} steals electrons from all enemies!");
    }

    private void CastInertShield()
    {
        var allies = HexGrid.Instance?.GetUnitsInRange(unit.CurrentCell, 2, true);
        
        foreach (var ally in allies)
        {
            // Apply immunity effect
            var combat = ally.GetComponent<UnitCombat>();
            if (combat != null)
            {
                combat.ApplyShield(3f); // 3 second immunity
            }
        }
        
        Debug.Log($"{unit.Data.Symbol} shields nearby allies!");
    }

    #endregion

    #region Halogen Electron Stealing

    private void StealElectrons(Unit target, int amount = 1)
    {
        var targetCombat = target.GetComponent<UnitCombat>();
        if (targetCombat == null) return;
        
        // Steal based on electronegativity difference
        float stealChance = unit.Data.Electronegativity / 4f;
        if (Random.value < stealChance)
        {
            int stolen = Mathf.Min(amount, targetCombat.CurrentElectrons);
            targetCombat.CurrentElectrons -= stolen;
            GainElectron(stolen);
            
            Debug.Log($"{unit.Data.Symbol} stole {stolen} electron(s) from {target.Data.Symbol}!");
        }
    }

    #endregion

    #region Status Effects

    private void ProcessStatusEffects()
    {
        statusTickTimer += Time.deltaTime;
        
        if (statusTickTimer >= STATUS_TICK_INTERVAL)
        {
            statusTickTimer = 0f;
            
            if (IsBurning)
            {
                unit.TakeDamage(BurnDamagePerTick);
            }
            
            if (IsPoisoned)
            {
                unit.TakeDamage(PoisonDamagePerTick);
            }
        }
    }

    public void ApplyStatusEffect(Unit target, StatusEffect effect, float duration)
    {
        var combat = target.GetComponent<UnitCombat>();
        if (combat == null) return;
        
        switch (effect)
        {
            case StatusEffect.Burn:
                combat.StartCoroutine(combat.BurnEffect(duration));
                break;
            case StatusEffect.Poison:
                combat.StartCoroutine(combat.PoisonEffect(duration));
                break;
            case StatusEffect.Stun:
                combat.StartCoroutine(combat.StunEffect(duration));
                break;
            case StatusEffect.Slow:
                combat.StartCoroutine(combat.SlowEffect(duration));
                break;
        }
    }

    public void ApplyShield(float duration)
    {
        StartCoroutine(ShieldEffect(duration));
    }

    private IEnumerator BurnEffect(float duration)
    {
        IsBurning = true;
        yield return new WaitForSeconds(duration);
        IsBurning = false;
    }

    private IEnumerator PoisonEffect(float duration)
    {
        IsPoisoned = true;
        yield return new WaitForSeconds(duration);
        IsPoisoned = false;
    }

    private IEnumerator StunEffect(float duration)
    {
        IsStunned = true;
        yield return new WaitForSeconds(duration);
        IsStunned = false;
    }

    private IEnumerator SlowEffect(float duration)
    {
        float originalCooldown = AttackCooldown;
        AttackCooldown *= 2f;
        yield return new WaitForSeconds(duration);
        AttackCooldown = originalCooldown;
    }

    private IEnumerator ShieldEffect(float duration)
    {
        // Store original layer and set to ignore raycast
        // This is a simplified shield - real implementation would prevent damage
        yield return new WaitForSeconds(duration);
    }

    private void ClearStatusEffects()
    {
        IsBurning = false;
        IsPoisoned = false;
        IsStunned = false;
        StopAllCoroutines();
    }

    #endregion

    #region Animations

    private IEnumerator AttackAnimation()
    {
        if (CurrentTarget == null) yield break;
        
        Vector3 startPos = transform.position;
        Vector3 targetPos = CurrentTarget.transform.position;
        Vector3 attackPos = Vector3.Lerp(startPos, targetPos, 0.3f);
        
        // Lunge forward
        float duration = 0.1f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, attackPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // Return
        elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(attackPos, startPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = startPos;
    }

    #endregion
}

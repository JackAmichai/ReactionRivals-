using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Defines which elements unlock at each player level
/// </summary>
[CreateAssetMenu(fileName = "LevelElementProgression", menuName = "ReactionRivals/LevelElementProgression")]
public class LevelElementProgression : ScriptableObject
{
    [System.Serializable]
    public class LevelUnlock
    {
        public int Level;
        [Tooltip("Atomic numbers of elements that unlock at this level")]
        public int[] UnlockedAtomicNumbers;
    }

    [Header("Element Unlocks by Level")]
    public LevelUnlock[] LevelUnlocks;

    /// <summary>
    /// Get all elements unlocked at or below the given level
    /// </summary>
    public HashSet<int> GetUnlockedElements(int playerLevel)
    {
        HashSet<int> unlocked = new HashSet<int>();
        
        foreach (var unlock in LevelUnlocks)
        {
            if (unlock.Level <= playerLevel)
            {
                foreach (var atomicNumber in unlock.UnlockedAtomicNumbers)
                {
                    unlocked.Add(atomicNumber);
                }
            }
        }
        
        return unlocked;
    }

    /// <summary>
    /// Check if a specific element is unlocked at the given level
    /// </summary>
    public bool IsElementUnlocked(int atomicNumber, int playerLevel)
    {
        foreach (var unlock in LevelUnlocks)
        {
            if (unlock.Level <= playerLevel)
            {
                foreach (var num in unlock.UnlockedAtomicNumbers)
                {
                    if (num == atomicNumber) return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Get the level at which an element unlocks
    /// </summary>
    public int GetUnlockLevel(int atomicNumber)
    {
        foreach (var unlock in LevelUnlocks)
        {
            foreach (var num in unlock.UnlockedAtomicNumbers)
            {
                if (num == atomicNumber) return unlock.Level;
            }
        }
        return -1; // Not unlockable
    }

    /// <summary>
    /// Create default progression with all 118 elements
    /// </summary>
    public void GenerateDefaultProgression()
    {
        List<LevelUnlock> unlocks = new List<LevelUnlock>();

        // Level 1: Basic elements - H, C, N, O (foundation of organic chemistry)
        unlocks.Add(new LevelUnlock
        {
            Level = 1,
            UnlockedAtomicNumbers = new int[] { 1, 6, 7, 8 } // H, C, N, O
        });

        // Level 2: Common elements - He, Na, Cl, Fe
        unlocks.Add(new LevelUnlock
        {
            Level = 2,
            UnlockedAtomicNumbers = new int[] { 2, 11, 17, 26 } // He, Na, Cl, Fe
        });

        // Level 3: Period 2 completion + common metals
        unlocks.Add(new LevelUnlock
        {
            Level = 3,
            UnlockedAtomicNumbers = new int[] { 3, 4, 5, 9, 10, 12 } // Li, Be, B, F, Ne, Mg
        });

        // Level 4: Period 3 completion
        unlocks.Add(new LevelUnlock
        {
            Level = 4,
            UnlockedAtomicNumbers = new int[] { 13, 14, 15, 16, 18 } // Al, Si, P, S, Ar
        });

        // Level 5: Period 4 alkali/alkaline + halogens
        unlocks.Add(new LevelUnlock
        {
            Level = 5,
            UnlockedAtomicNumbers = new int[] { 19, 20, 35, 36 } // K, Ca, Br, Kr
        });

        // Level 6: Period 4 transition metals part 1
        unlocks.Add(new LevelUnlock
        {
            Level = 6,
            UnlockedAtomicNumbers = new int[] { 21, 22, 23, 24, 25 } // Sc, Ti, V, Cr, Mn
        });

        // Level 7: Period 4 transition metals part 2
        unlocks.Add(new LevelUnlock
        {
            Level = 7,
            UnlockedAtomicNumbers = new int[] { 27, 28, 29, 30 } // Co, Ni, Cu, Zn
        });

        // Level 8: Period 4 p-block
        unlocks.Add(new LevelUnlock
        {
            Level = 8,
            UnlockedAtomicNumbers = new int[] { 31, 32, 33, 34 } // Ga, Ge, As, Se
        });

        // Level 9: Period 5 s-block + noble gas
        unlocks.Add(new LevelUnlock
        {
            Level = 9,
            UnlockedAtomicNumbers = new int[] { 37, 38, 53, 54 } // Rb, Sr, I, Xe
        });

        // Level 10: Period 5 early transition metals
        unlocks.Add(new LevelUnlock
        {
            Level = 10,
            UnlockedAtomicNumbers = new int[] { 39, 40, 41, 42, 43 } // Y, Zr, Nb, Mo, Tc
        });

        // Level 11: Period 5 precious metals
        unlocks.Add(new LevelUnlock
        {
            Level = 11,
            UnlockedAtomicNumbers = new int[] { 44, 45, 46, 47, 48 } // Ru, Rh, Pd, Ag, Cd
        });

        // Level 12: Period 5 p-block
        unlocks.Add(new LevelUnlock
        {
            Level = 12,
            UnlockedAtomicNumbers = new int[] { 49, 50, 51, 52 } // In, Sn, Sb, Te
        });

        // Level 13: Period 6 s-block + noble gas
        unlocks.Add(new LevelUnlock
        {
            Level = 13,
            UnlockedAtomicNumbers = new int[] { 55, 56, 85, 86 } // Cs, Ba, At, Rn
        });

        // Level 14: Period 6 precious metals
        unlocks.Add(new LevelUnlock
        {
            Level = 14,
            UnlockedAtomicNumbers = new int[] { 78, 79, 80 } // Pt, Au, Hg
        });

        // Level 15: Period 6 other transition metals
        unlocks.Add(new LevelUnlock
        {
            Level = 15,
            UnlockedAtomicNumbers = new int[] { 72, 73, 74, 75, 76, 77 } // Hf, Ta, W, Re, Os, Ir
        });

        // Level 16: Period 6 p-block
        unlocks.Add(new LevelUnlock
        {
            Level = 16,
            UnlockedAtomicNumbers = new int[] { 81, 82, 83, 84 } // Tl, Pb, Bi, Po
        });

        // Level 17: Lanthanides part 1
        unlocks.Add(new LevelUnlock
        {
            Level = 17,
            UnlockedAtomicNumbers = new int[] { 57, 58, 59, 60, 61, 62, 63, 64 } // La-Gd
        });

        // Level 18: Lanthanides part 2
        unlocks.Add(new LevelUnlock
        {
            Level = 18,
            UnlockedAtomicNumbers = new int[] { 65, 66, 67, 68, 69, 70, 71 } // Tb-Lu
        });

        // Level 19: Period 7 s-block + noble gas
        unlocks.Add(new LevelUnlock
        {
            Level = 19,
            UnlockedAtomicNumbers = new int[] { 87, 88, 117, 118 } // Fr, Ra, Ts, Og
        });

        // Level 20: Actinides common (Th, U, Pu)
        unlocks.Add(new LevelUnlock
        {
            Level = 20,
            UnlockedAtomicNumbers = new int[] { 89, 90, 91, 92, 93, 94 } // Ac-Pu
        });

        // Level 21: Actinides rare
        unlocks.Add(new LevelUnlock
        {
            Level = 21,
            UnlockedAtomicNumbers = new int[] { 95, 96, 97, 98, 99, 100, 101, 102, 103 } // Am-Lr
        });

        // Level 22: Period 7 superheavy elements
        unlocks.Add(new LevelUnlock
        {
            Level = 22,
            UnlockedAtomicNumbers = new int[] { 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116 } // Rf-Lv
        });

        LevelUnlocks = unlocks.ToArray();
    }
}

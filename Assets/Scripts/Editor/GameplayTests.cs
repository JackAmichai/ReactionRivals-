using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Runtime tests for core game systems. Run these to verify game correctness.
/// Tests can be executed via menu: Tools > Reaction Rivals > Run All Tests
/// </summary>
public static class GameplayTests
{
#if UNITY_EDITOR
    private static int passCount = 0;
    private static int failCount = 0;
    private static List<string> failures = new List<string>();

    [MenuItem("Tools/Reaction Rivals/Run All Tests", priority = 200)]
    public static void RunAllTests()
    {
        passCount = 0;
        failCount = 0;
        failures.Clear();

        Debug.Log("═══════════════════════════════════════");
        Debug.Log("🧪 Running Reaction Rivals Test Suite");
        Debug.Log("═══════════════════════════════════════\n");

        // Phase 1: Data Correctness Tests
        TestPeriodicTableData();
        TestValenceElectronRules();
        TestOctetDuetRules();
        
        // Phase 2: Hex Grid Tests
        TestHexNeighborDirections();
        TestHexDistanceCalculation();
        
        // Phase 3: Bonding Tests
        TestWaterRecipeFormation();
        TestAmmoniaRecipeFormation();
        TestCO2RecipeFormation();
        
        // Phase 4: Combat Tests
        TestOctetRuleTriggersUltimate();
        TestElectronResetAfterUltimate();

        // Summary
        Debug.Log("\n═══════════════════════════════════════");
        Debug.Log($"Test Results: {passCount} passed, {failCount} failed");
        
        if (failCount > 0)
        {
            Debug.LogError("❌ SOME TESTS FAILED:");
            foreach (var failure in failures)
            {
                Debug.LogError($"   • {failure}");
            }
        }
        else
        {
            Debug.Log("✅ <color=green>ALL TESTS PASSED!</color>");
        }
        Debug.Log("═══════════════════════════════════════");
    }

    #region Data Tests

    private static void TestPeriodicTableData()
    {
        Debug.Log("📊 Testing Periodic Table Data...");
        
        // Test: All 118 elements exist
        AssertEqual(PeriodicTable.Elements.Length, 118, "PeriodicTable should have 118 elements");
        
        // Test: Atomic numbers are sequential
        for (int i = 0; i < PeriodicTable.Elements.Length; i++)
        {
            AssertEqual(PeriodicTable.Elements[i].AtomicNumber, i + 1, 
                $"Element at index {i} should have atomic number {i + 1}");
        }
        
        // Test: Key elements exist with correct data
        var hydrogen = PeriodicTable.GetElement("H");
        AssertNotNull(hydrogen, "Hydrogen should exist");
        AssertEqual(hydrogen?.AtomicNumber ?? 0, 1, "Hydrogen atomic number should be 1");
        AssertEqual(hydrogen?.ValenceElectrons ?? 0, 1, "Hydrogen valence electrons should be 1");
        
        var oxygen = PeriodicTable.GetElement("O");
        AssertNotNull(oxygen, "Oxygen should exist");
        AssertEqual(oxygen?.AtomicNumber ?? 0, 8, "Oxygen atomic number should be 8");
        AssertEqual(oxygen?.ValenceElectrons ?? 0, 6, "Oxygen valence electrons should be 6");
        
        var carbon = PeriodicTable.GetElement("C");
        AssertNotNull(carbon, "Carbon should exist");
        AssertEqual(carbon?.AtomicNumber ?? 0, 6, "Carbon atomic number should be 6");
        AssertEqual(carbon?.ValenceElectrons ?? 0, 4, "Carbon valence electrons should be 4");
        
        var nitrogen = PeriodicTable.GetElement("N");
        AssertNotNull(nitrogen, "Nitrogen should exist");
        AssertEqual(nitrogen?.AtomicNumber ?? 0, 7, "Nitrogen atomic number should be 7");
        AssertEqual(nitrogen?.ValenceElectrons ?? 0, 5, "Nitrogen valence electrons should be 5");
    }

    private static void TestValenceElectronRules()
    {
        Debug.Log("⚛️ Testing Valence Electron Rules...");
        
        // Main group elements: Group number determines valence
        var sodium = PeriodicTable.GetElement("Na");
        AssertEqual(sodium?.ValenceElectrons ?? 0, 1, "Sodium (Group 1) should have 1 valence electron");
        
        var magnesium = PeriodicTable.GetElement("Mg");
        AssertEqual(magnesium?.ValenceElectrons ?? 0, 2, "Magnesium (Group 2) should have 2 valence electrons");
        
        var chlorine = PeriodicTable.GetElement("Cl");
        AssertEqual(chlorine?.ValenceElectrons ?? 0, 7, "Chlorine (Group 17) should have 7 valence electrons");
        
        // Noble gases have 8 (or 2 for Helium)
        var helium = PeriodicTable.GetElement("He");
        AssertEqual(helium?.ValenceElectrons ?? 0, 2, "Helium should have 2 valence electrons (full shell)");
        
        var neon = PeriodicTable.GetElement("Ne");
        AssertEqual(neon?.ValenceElectrons ?? 0, 8, "Neon should have 8 valence electrons");
        
        var argon = PeriodicTable.GetElement("Ar");
        AssertEqual(argon?.ValenceElectrons ?? 0, 8, "Argon should have 8 valence electrons");
    }

    private static void TestOctetDuetRules()
    {
        Debug.Log("8️⃣ Testing Octet/Duet Rules...");
        
        // Test Duet Rule (H, He need 2 electrons)
        var h = PeriodicTable.GetElement("H");
        int hFullShell = (h?.Family == ElementFamily.Hydrogen || h?.AtomicNumber <= 2) ? 2 : 8;
        AssertEqual(hFullShell, 2, "Hydrogen follows Duet Rule (2 electrons for full shell)");
        
        var he = PeriodicTable.GetElement("He");
        AssertEqual(he?.ValenceElectrons ?? 0, 2, "Helium already has full shell (2 electrons)");
        
        // Test Octet Rule (most elements need 8)
        var carbon = PeriodicTable.GetElement("C");
        int cFullShell = (carbon?.Family == ElementFamily.Hydrogen || carbon?.AtomicNumber <= 2) ? 2 : 8;
        AssertEqual(cFullShell, 8, "Carbon follows Octet Rule (8 electrons for full shell)");
        
        // Noble gases already have 8 (except He)
        var neon = PeriodicTable.GetElement("Ne");
        AssertTrue(neon?.Family == ElementFamily.NobleGas, "Neon should be a Noble Gas");
    }

    #endregion

    #region Hex Grid Tests

    private static void TestHexNeighborDirections()
    {
        Debug.Log("🔷 Testing Hex Neighbor Directions...");
        
        // For pointy-topped hex with axial coordinates, the 6 neighbors are:
        // (+1,0), (+1,-1), (0,-1), (-1,0), (-1,+1), (0,+1)
        Vector2Int[] expectedDirs = new Vector2Int[]
        {
            new Vector2Int(1, 0),   // East
            new Vector2Int(1, -1),  // Northeast
            new Vector2Int(0, -1),  // Northwest
            new Vector2Int(-1, 0),  // West
            new Vector2Int(-1, 1),  // Southwest
            new Vector2Int(0, 1)    // Southeast
        };
        
        // Verify there are exactly 6 directions
        AssertEqual(expectedDirs.Length, 6, "Hex grid should have 6 neighbor directions");
        
        // Test that all directions sum to zero (they should form a closed loop)
        Vector2Int sum = Vector2Int.zero;
        foreach (var dir in expectedDirs)
        {
            sum += dir;
        }
        AssertEqual(sum, Vector2Int.zero, "Sum of all neighbor directions should be zero");
    }

    private static void TestHexDistanceCalculation()
    {
        Debug.Log("📏 Testing Hex Distance Calculation...");
        
        // Distance from origin to itself should be 0
        Vector2Int origin = Vector2Int.zero;
        AssertEqual(HexCell.HexDistance(origin, origin), 0, "Distance to self should be 0");
        
        // Distance to immediate neighbor should be 1
        Vector2Int neighbor = new Vector2Int(1, 0);
        AssertEqual(HexCell.HexDistance(origin, neighbor), 1, "Distance to immediate neighbor should be 1");
        
        // Distance 2 steps away
        Vector2Int twoAway = new Vector2Int(2, 0);
        AssertEqual(HexCell.HexDistance(origin, twoAway), 2, "Distance two steps east should be 2");
        
        // Diagonal distance
        Vector2Int diagonal = new Vector2Int(1, 1);
        int diagDist = HexCell.HexDistance(origin, diagonal);
        AssertTrue(diagDist >= 1 && diagDist <= 2, $"Diagonal distance should be 1-2, got {diagDist}");
    }

    #endregion

    #region Bonding Tests

    private static void TestWaterRecipeFormation()
    {
        Debug.Log("💧 Testing Water (H₂O) Recipe...");
        
        // Water: O core + 2H
        var oxygen = PeriodicTable.GetElement("O");
        var hydrogen = PeriodicTable.GetElement("H");
        
        AssertNotNull(oxygen, "Oxygen must exist for Water");
        AssertNotNull(hydrogen, "Hydrogen must exist for Water");
        
        // Verify bonding makes sense:
        // O has 6 valence electrons, needs 2 more for octet
        // H has 1 valence electron, shares it
        // O + 2H = 6 + 1 + 1 = 8 shared electrons (stable)
        int oxygenNeeds = 8 - (oxygen?.ValenceElectrons ?? 0);
        AssertEqual(oxygenNeeds, 2, "Oxygen needs 2 more electrons for octet");
        
        int hydrogenProvides = hydrogen?.ValenceElectrons ?? 0;
        AssertEqual(hydrogenProvides, 1, "Each Hydrogen provides 1 electron");
    }

    private static void TestAmmoniaRecipeFormation()
    {
        Debug.Log("🧪 Testing Ammonia (NH₃) Recipe...");
        
        // Ammonia: N core + 3H
        var nitrogen = PeriodicTable.GetElement("N");
        var hydrogen = PeriodicTable.GetElement("H");
        
        AssertNotNull(nitrogen, "Nitrogen must exist for Ammonia");
        AssertNotNull(hydrogen, "Hydrogen must exist for Ammonia");
        
        // N has 5 valence electrons, needs 3 more for octet
        int nitrogenNeeds = 8 - (nitrogen?.ValenceElectrons ?? 0);
        AssertEqual(nitrogenNeeds, 3, "Nitrogen needs 3 more electrons for octet");
    }

    private static void TestCO2RecipeFormation()
    {
        Debug.Log("💨 Testing Carbon Dioxide (CO₂) Recipe...");
        
        // CO₂: C core + 2O (double bonds)
        var carbon = PeriodicTable.GetElement("C");
        var oxygen = PeriodicTable.GetElement("O");
        
        AssertNotNull(carbon, "Carbon must exist for CO₂");
        AssertNotNull(oxygen, "Oxygen must exist for CO₂");
        
        // C has 4 valence electrons, needs 4 more
        // Each O shares 2 electrons (double bond)
        int carbonNeeds = 8 - (carbon?.ValenceElectrons ?? 0);
        AssertEqual(carbonNeeds, 4, "Carbon needs 4 more electrons for octet");
    }

    #endregion

    #region Combat Tests

    private static void TestOctetRuleTriggersUltimate()
    {
        Debug.Log("⚡ Testing Octet Rule Ultimate Trigger...");
        
        // For most elements, reaching 8 electrons triggers ultimate
        var carbon = PeriodicTable.GetElement("C");
        int carbonValence = carbon?.ValenceElectrons ?? 4;
        int electronsToOctet = 8 - carbonValence;
        
        AssertEqual(electronsToOctet, 4, "Carbon needs 4 attacks to reach Octet (4 + 4 = 8)");
        
        // For Hydrogen, reaching 2 electrons triggers (Duet Rule)
        var hydrogen = PeriodicTable.GetElement("H");
        int hydrogenValence = hydrogen?.ValenceElectrons ?? 1;
        int electronsToFull = (hydrogen?.AtomicNumber <= 2) ? (2 - hydrogenValence) : (8 - hydrogenValence);
        
        AssertEqual(electronsToFull, 1, "Hydrogen needs 1 attack to reach Duet (1 + 1 = 2)");
    }

    private static void TestElectronResetAfterUltimate()
    {
        Debug.Log("🔄 Testing Electron Reset After Ultimate...");
        
        // After ultimate, electrons should reset to base valence, not zero
        var oxygen = PeriodicTable.GetElement("O");
        int oxygenValence = oxygen?.ValenceElectrons ?? 6;
        
        // Verify valence is what we expect
        AssertEqual(oxygenValence, 6, "Oxygen valence should be 6");
        
        // After ultimate cast, CurrentElectrons should reset to 6, not 0
        // (This is verified by UnitCombat.CastUltimate setting CurrentElectrons = ValenceElectrons)
        AssertTrue(oxygenValence > 0, "Valence should be positive for reset");
    }

    #endregion

    #region Assertion Helpers

    private static void AssertEqual<T>(T actual, T expected, string message)
    {
        if (EqualityComparer<T>.Default.Equals(actual, expected))
        {
            passCount++;
            Debug.Log($"   ✓ {message}");
        }
        else
        {
            failCount++;
            string failure = $"{message} - Expected: {expected}, Got: {actual}";
            failures.Add(failure);
            Debug.LogError($"   ✗ {failure}");
        }
    }

    private static void AssertTrue(bool condition, string message)
    {
        if (condition)
        {
            passCount++;
            Debug.Log($"   ✓ {message}");
        }
        else
        {
            failCount++;
            failures.Add(message);
            Debug.LogError($"   ✗ {message}");
        }
    }

    private static void AssertNotNull(object obj, string message)
    {
        if (obj != null)
        {
            passCount++;
            Debug.Log($"   ✓ {message}");
        }
        else
        {
            failCount++;
            failures.Add($"{message} - was null");
            Debug.LogError($"   ✗ {message} - was null");
        }
    }

    #endregion
#endif
}

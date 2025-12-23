using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Chemistry glossary with definitions of key terms.
/// Terms are unlocked and reinforced through gameplay.
/// </summary>
public static class ChemistryGlossary
{
    public static readonly Dictionary<string, GlossaryEntry> Terms = new Dictionary<string, GlossaryEntry>
    {
        // Atomic Structure
        {"Atom", new GlossaryEntry(
            "Atom",
            "The smallest unit of an element that maintains its chemical properties.",
            "Everything is made of atoms! They're so small that millions could fit on the period at the end of this sentence.",
            "Atomic Structure", 1
        )},
        
        {"Proton", new GlossaryEntry(
            "Proton",
            "A positively charged particle in the nucleus of an atom. The number of protons defines which element it is.",
            "The number of protons = the atomic number. Hydrogen has 1, Carbon has 6, Gold has 79!",
            "Atomic Structure", 1
        )},
        
        {"Neutron", new GlossaryEntry(
            "Neutron",
            "A neutral particle in the nucleus. Different numbers of neutrons create isotopes of the same element.",
            "Carbon-12 has 6 neutrons, Carbon-14 has 8. Carbon-14 is used to date ancient artifacts!",
            "Atomic Structure", 1
        )},
        
        {"Electron", new GlossaryEntry(
            "Electron",
            "A negatively charged particle that orbits the nucleus. Electrons determine chemical bonding.",
            "Electrons are 1,836 times lighter than protons! Chemistry is basically the study of electron behavior.",
            "Atomic Structure", 1
        )},
        
        {"Valence Electron", new GlossaryEntry(
            "Valence Electron",
            "An electron in the outermost shell of an atom that can participate in bonding.",
            "In the game, valence electrons determine how elements bond. Carbon has 4, so it can make 4 bonds!",
            "Atomic Structure", 2
        )},
        
        {"Electron Shell", new GlossaryEntry(
            "Electron Shell",
            "Energy levels around the nucleus where electrons are found. Shells fill from inside to outside.",
            "First shell holds 2 electrons, second holds 8, third holds 8 (for most atoms). That's the Octet Rule!",
            "Atomic Structure", 2
        )},
        
        // Bonding
        {"Chemical Bond", new GlossaryEntry(
            "Chemical Bond",
            "A lasting attraction between atoms that enables the formation of molecules.",
            "There are 3 main types: covalent (sharing), ionic (transferring), and metallic (pooling electrons).",
            "Chemical Bonding", 2
        )},
        
        {"Covalent Bond", new GlossaryEntry(
            "Covalent Bond",
            "A bond where two atoms share electrons. Common between nonmetals.",
            "Water (H₂O) has covalent bonds. The oxygen shares electrons with two hydrogens.",
            "Chemical Bonding", 3
        )},
        
        {"Ionic Bond", new GlossaryEntry(
            "Ionic Bond",
            "A bond where one atom transfers electrons to another, creating charged ions that attract.",
            "Table salt (NaCl) is ionic. Sodium gives its electron to chlorine, making Na⁺ and Cl⁻ ions.",
            "Chemical Bonding", 3
        )},
        
        {"Metallic Bond", new GlossaryEntry(
            "Metallic Bond",
            "A bond in metals where electrons flow freely between atoms in an 'electron sea'.",
            "This is why metals conduct electricity! The free electrons can carry current.",
            "Chemical Bonding", 6
        )},
        
        {"Octet Rule", new GlossaryEntry(
            "Octet Rule",
            "Atoms tend to gain, lose, or share electrons to have 8 electrons in their outer shell.",
            "In the game, reaching 8 electrons (full shell) triggers your element's ultimate ability!",
            "Chemical Bonding", 2
        )},
        
        {"Duet Rule", new GlossaryEntry(
            "Duet Rule",
            "Hydrogen and Helium only need 2 electrons to fill their outer shell (first shell holds max 2).",
            "That's why hydrogen only makes one bond, and helium doesn't bond at all!",
            "Chemical Bonding", 2
        )},
        
        // Periodic Table
        {"Periodic Table", new GlossaryEntry(
            "Periodic Table",
            "An arrangement of elements by atomic number, where elements in the same column have similar properties.",
            "Dmitri Mendeleev created it in 1869 and even predicted elements that hadn't been discovered yet!",
            "Periodic Table", 1
        )},
        
        {"Period", new GlossaryEntry(
            "Period",
            "A horizontal row in the periodic table. Elements in the same period have the same number of electron shells.",
            "Period 1 has 2 elements, Period 2 has 8, and so on. The game unlocks new periods as you level up!",
            "Periodic Table", 3
        )},
        
        {"Group", new GlossaryEntry(
            "Group",
            "A vertical column in the periodic table. Elements in the same group have similar chemical properties.",
            "Group 1 = Alkali metals (reactive!), Group 18 = Noble gases (unreactive). Same column = similar behavior.",
            "Periodic Table", 3
        )},
        
        {"Atomic Number", new GlossaryEntry(
            "Atomic Number",
            "The number of protons in an atom's nucleus. This number defines the element.",
            "Change the atomic number, change the element! Gold is 79, add a proton and you get Mercury (80).",
            "Periodic Table", 1
        )},
        
        {"Atomic Mass", new GlossaryEntry(
            "Atomic Mass",
            "The average mass of an element's atoms, measured in atomic mass units (u).",
            "In the game, atomic mass determines HP! Heavier elements have more health.",
            "Periodic Table", 4
        )},
        
        // Element Families
        {"Alkali Metal", new GlossaryEntry(
            "Alkali Metal",
            "Group 1 elements (except H). Highly reactive metals that explode in water!",
            "Na, K, Li... these are soft metals you can cut with a knife. Drop them in water for fireworks!",
            "Element Families", 2
        )},
        
        {"Alkaline Earth Metal", new GlossaryEntry(
            "Alkaline Earth Metal",
            "Group 2 elements. Reactive but less so than alkali metals. Includes calcium in your bones!",
            "Magnesium burns bright white (used in fireworks), Calcium builds your bones and teeth.",
            "Element Families", 3
        )},
        
        {"Transition Metal", new GlossaryEntry(
            "Transition Metal",
            "Elements in the middle of the periodic table (Groups 3-12). Often colorful and good conductors.",
            "Iron, copper, gold, silver... most metals you interact with daily are transition metals!",
            "Element Families", 6
        )},
        
        {"Halogen", new GlossaryEntry(
            "Halogen",
            "Group 17 elements. Very reactive nonmetals that readily form salts with metals.",
            "Fluorine (toothpaste), Chlorine (pools), Iodine (medicine). They all want one more electron!",
            "Element Families", 4
        )},
        
        {"Noble Gas", new GlossaryEntry(
            "Noble Gas",
            "Group 18 elements. Extremely unreactive because their outer shells are already full.",
            "Helium, Neon, Argon... they're so stable they were called 'inert gases'. In the game, they're immune to abilities!",
            "Element Families", 2
        )},
        
        {"Lanthanide", new GlossaryEntry(
            "Lanthanide",
            "Elements 57-71, also called 'rare earth elements' (though not actually rare).",
            "Essential for smartphones, electric cars, and wind turbines. China produces most of the world's supply.",
            "Element Families", 17
        )},
        
        {"Actinide", new GlossaryEntry(
            "Actinide",
            "Elements 89-103, including radioactive elements like Uranium and Plutonium.",
            "All actinides are radioactive. Uranium powers nuclear plants, Plutonium powered the Voyager spacecraft!",
            "Element Families", 20
        )},
        
        // Molecules & Compounds
        {"Molecule", new GlossaryEntry(
            "Molecule",
            "Two or more atoms bonded together. Can be the same element (O₂) or different (H₂O).",
            "In the game, forming molecules gives powerful bonuses! Water heals, Methane damages.",
            "Molecules", 1
        )},
        
        {"Compound", new GlossaryEntry(
            "Compound",
            "A substance made of two or more different elements chemically bonded.",
            "Water (H₂O), salt (NaCl), sugar (C₆H₁₂O₆)... most substances around you are compounds!",
            "Molecules", 2
        )},
        
        {"Chemical Formula", new GlossaryEntry(
            "Chemical Formula",
            "A notation showing which elements and how many atoms are in a molecule.",
            "H₂O = 2 hydrogens + 1 oxygen. CO₂ = 1 carbon + 2 oxygens. The subscript shows the count!",
            "Molecules", 1
        )},
        
        // Reactivity
        {"Electronegativity", new GlossaryEntry(
            "Electronegativity",
            "A measure of how strongly an atom attracts electrons in a bond.",
            "Fluorine is the most electronegative (3.98). In the game, high electronegativity = electron stealing!",
            "Reactivity", 4
        )},
        
        {"Ion", new GlossaryEntry(
            "Ion",
            "An atom that has gained or lost electrons, giving it a positive or negative charge.",
            "Na loses an electron → Na⁺ (positive). Cl gains one → Cl⁻ (negative). Opposites attract!",
            "Reactivity", 3
        )},
        
        {"Oxidation", new GlossaryEntry(
            "Oxidation",
            "When an atom loses electrons (or gains oxygen). Rusting is oxidation of iron.",
            "Remember: OIL RIG - Oxidation Is Loss, Reduction Is Gain (of electrons).",
            "Reactivity", 5
        )},
        
        {"Reduction", new GlossaryEntry(
            "Reduction",
            "When an atom gains electrons (or loses oxygen). The opposite of oxidation.",
            "Metals are extracted from ores by reduction - removing the oxygen to get pure metal.",
            "Reactivity", 5
        )},
        
        // States of Matter
        {"Allotrope", new GlossaryEntry(
            "Allotrope",
            "Different structural forms of the same element. Diamond and graphite are both pure carbon!",
            "Carbon has several: diamond (hardest natural substance), graphite (soft, used in pencils), graphene (super strong).",
            "States of Matter", 6
        )},
        
        {"Isotope", new GlossaryEntry(
            "Isotope",
            "Atoms of the same element with different numbers of neutrons.",
            "Carbon-12 and Carbon-14 are isotopes. C-14 is radioactive and used to date ancient objects!",
            "Nuclear Chemistry", 8
        )},
        
        {"Radioactivity", new GlossaryEntry(
            "Radioactivity",
            "When unstable atoms release energy and particles to become more stable.",
            "Some elements are naturally radioactive. Uranium decays over billions of years - used to date Earth's age!",
            "Nuclear Chemistry", 20
        )},
    };

    /// <summary>
    /// Get terms available at a given level
    /// </summary>
    public static List<GlossaryEntry> GetTermsForLevel(int level)
    {
        var result = new List<GlossaryEntry>();
        foreach (var entry in Terms.Values)
        {
            if (entry.UnlockLevel <= level)
                result.Add(entry);
        }
        return result;
    }

    /// <summary>
    /// Get terms by category
    /// </summary>
    public static List<GlossaryEntry> GetTermsByCategory(string category)
    {
        var result = new List<GlossaryEntry>();
        foreach (var entry in Terms.Values)
        {
            if (entry.Category == category)
                result.Add(entry);
        }
        return result;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    public static List<string> GetAllCategories()
    {
        var categories = new HashSet<string>();
        foreach (var entry in Terms.Values)
        {
            categories.Add(entry.Category);
        }
        return new List<string>(categories);
    }
}

/// <summary>
/// A glossary entry for a chemistry term
/// </summary>
[System.Serializable]
public class GlossaryEntry
{
    public string Term;
    public string Definition;
    public string FunFact;
    public string Category;
    public int UnlockLevel;

    public GlossaryEntry(string term, string definition, string funFact, string category, int unlockLevel)
    {
        Term = term;
        Definition = definition;
        FunFact = funFact;
        Category = category;
        UnlockLevel = unlockLevel;
    }
}

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Educational data about element discovery and significance.
/// All data is historically accurate for educational purposes.
/// </summary>
public static class ElementHistory
{
    /// <summary>
    /// Historical and educational data for each element
    /// </summary>
    public static readonly Dictionary<int, ElementEducationalInfo> Data = new Dictionary<int, ElementEducationalInfo>
    {
        // Period 1
        {1, new ElementEducationalInfo(
            "Hydrogen",
            1766, "Henry Cavendish", "England",
            "The most abundant element in the universe, making up about 75% of all matter. Its name means 'water-former' in Greek.",
            "Powers the Sun through nuclear fusion. Used in rocket fuel, fuel cells, and producing ammonia for fertilizers.",
            new string[] { "Lightest element", "Most common element in universe", "Water is H₂O" }
        )},
        
        {2, new ElementEducationalInfo(
            "Helium",
            1868, "Pierre Janssen & Joseph Lockyer", "France/England",
            "First discovered in the Sun's spectrum during a solar eclipse, before being found on Earth. Named after Helios, Greek god of the Sun.",
            "Used in MRI machines, party balloons, deep-sea diving tanks, and cooling superconducting magnets.",
            new string[] { "Second lightest element", "Won't freeze at normal pressure", "Discovered in the Sun first" }
        )},
        
        // Period 2
        {3, new ElementEducationalInfo(
            "Lithium",
            1817, "Johan August Arfwedson", "Sweden",
            "The lightest metal and least dense solid element. Named from Greek 'lithos' meaning stone.",
            "Powers smartphones and electric vehicles through lithium-ion batteries. Also used to treat bipolar disorder.",
            new string[] { "Lightest metal", "Floats on water", "Powers your phone" }
        )},
        
        {4, new ElementEducationalInfo(
            "Beryllium",
            1798, "Louis Nicolas Vauquelin", "France",
            "A rare element that's surprisingly strong and light. Named after the mineral beryl (which includes emeralds).",
            "Used in aerospace, X-ray windows, and nuclear reactors. Emeralds and aquamarines contain beryllium.",
            new string[] { "Emeralds contain beryllium", "Very toxic", "X-ray transparent" }
        )},
        
        {5, new ElementEducationalInfo(
            "Boron",
            1808, "Humphry Davy, Gay-Lussac & Thénard", "England/France",
            "A metalloid essential for plant growth. Named from Arabic 'buraq' (borax).",
            "Used in fiberglass, detergents (borax), and high-strength materials. Essential nutrient for plants.",
            new string[] { "Plants need it to grow", "Makes glass heat-resistant", "Used in slime recipes" }
        )},
        
        {6, new ElementEducationalInfo(
            "Carbon",
            -3750, "Ancient civilizations", "Prehistoric",
            "The basis of all known life. Can form more compounds than any other element. Known since antiquity as charcoal and soot.",
            "Forms the backbone of DNA, proteins, and all organic molecules. Diamond and graphite are pure carbon.",
            new string[] { "Basis of all life", "Diamond is pure carbon", "In every living thing" }
        )},
        
        {7, new ElementEducationalInfo(
            "Nitrogen",
            1772, "Daniel Rutherford", "Scotland",
            "Makes up 78% of Earth's atmosphere. Named from Greek 'nitron genes' meaning 'niter-forming'.",
            "Essential for proteins and DNA. Used in fertilizers, explosives, and food preservation. Liquid nitrogen freezes things instantly.",
            new string[] { "78% of air you breathe", "In every protein", "Makes fertilizers" }
        )},
        
        {8, new ElementEducationalInfo(
            "Oxygen",
            1774, "Joseph Priestley & Carl Wilhelm Scheele", "England/Sweden",
            "Essential for almost all life on Earth. Makes up 21% of atmosphere and 65% of human body mass.",
            "Required for respiration and combustion. Third most abundant element in the universe. Water is 89% oxygen by mass.",
            new string[] { "You breathe it to live", "Fire needs it to burn", "Most of your body weight" }
        )},
        
        {9, new ElementEducationalInfo(
            "Fluorine",
            1886, "Henri Moissan", "France",
            "The most reactive and electronegative element. Moissan won the Nobel Prize for isolating it.",
            "In toothpaste to prevent cavities. Used in Teflon (non-stick pans) and refrigerants.",
            new string[] { "In your toothpaste", "Most reactive element", "Makes Teflon" }
        )},
        
        {10, new ElementEducationalInfo(
            "Neon",
            1898, "William Ramsay & Morris Travers", "England",
            "A noble gas that glows bright red-orange when electrified. Name means 'new' in Greek.",
            "Famous for neon signs. Also used in lasers, cryogenics, and high-voltage indicators.",
            new string[] { "Neon signs glow orange-red", "Won't react with anything", "Used in lasers" }
        )},
        
        // Period 3
        {11, new ElementEducationalInfo(
            "Sodium",
            1807, "Humphry Davy", "England",
            "A soft, silvery metal that reacts explosively with water. Named from 'soda'.",
            "Essential for nerve function. Table salt is sodium chloride. Used in street lights (yellow glow).",
            new string[] { "In table salt (NaCl)", "Explodes in water", "Your nerves need it" }
        )},
        
        {12, new ElementEducationalInfo(
            "Magnesium",
            1755, "Joseph Black", "Scotland",
            "Named after Magnesia, a district in Greece. Burns with an intensely bright white light.",
            "Essential for hundreds of enzyme reactions in your body. Used in fireworks and lightweight alloys.",
            new string[] { "Burns brilliant white", "In chlorophyll (plants)", "Lightweight but strong" }
        )},
        
        {13, new ElementEducationalInfo(
            "Aluminum",
            1825, "Hans Christian Ørsted", "Denmark",
            "Once more valuable than gold! Napoleon III served honored guests with aluminum cutlery.",
            "Most abundant metal in Earth's crust. Used in cans, foil, aircraft, and smartphones.",
            new string[] { "Once worth more than gold", "Most common metal in crust", "In soda cans" }
        )},
        
        {14, new ElementEducationalInfo(
            "Silicon",
            1824, "Jöns Jacob Berzelius", "Sweden",
            "The second most abundant element in Earth's crust after oxygen. Named from Latin 'silex' (flint).",
            "Powers the computer age - all microchips are made from silicon. Also in glass and concrete.",
            new string[] { "Computer chips are silicon", "In sand and glass", "Powers all electronics" }
        )},
        
        {15, new ElementEducationalInfo(
            "Phosphorus",
            1669, "Hennig Brand", "Germany",
            "Discovered while searching for the Philosopher's Stone in urine! Glows in the dark.",
            "Essential for DNA and ATP (energy currency of cells). Used in fertilizers and matches.",
            new string[] { "Found in DNA", "Discovered from urine", "Makes matches light" }
        )},
        
        {16, new ElementEducationalInfo(
            "Sulfur",
            -2000, "Ancient civilizations", "Prehistoric",
            "Known since ancient times as 'brimstone'. Has a distinctive rotten egg smell.",
            "Used in gunpowder, rubber vulcanization, and making sulfuric acid (most produced chemical).",
            new string[] { "Called 'brimstone' in Bible", "Rotten egg smell", "In gunpowder" }
        )},
        
        {17, new ElementEducationalInfo(
            "Chlorine",
            1774, "Carl Wilhelm Scheele", "Sweden",
            "A toxic yellow-green gas with a suffocating smell. Name means 'greenish-yellow' in Greek.",
            "Purifies drinking water worldwide. In table salt and PVC plastic. Used in WWI as chemical weapon.",
            new string[] { "Cleans drinking water", "In swimming pools", "In table salt" }
        )},
        
        {18, new ElementEducationalInfo(
            "Argon",
            1894, "Lord Rayleigh & William Ramsay", "England",
            "Makes up nearly 1% of Earth's atmosphere. Name means 'lazy' in Greek (it's very unreactive).",
            "Used in light bulbs and welding to prevent oxidation. Third most abundant gas in atmosphere.",
            new string[] { "Name means 'lazy'", "In light bulbs", "1% of the air" }
        )},
        
        // Period 4
        {19, new ElementEducationalInfo(
            "Potassium",
            1807, "Humphry Davy", "England",
            "First metal isolated by electrolysis. Reacts violently with water, even more than sodium.",
            "Essential for heart function and nerve signals. Bananas are rich in potassium.",
            new string[] { "Bananas have lots of it", "Your heart needs it", "Burns purple" }
        )},
        
        {20, new ElementEducationalInfo(
            "Calcium",
            1808, "Humphry Davy", "England",
            "Most abundant metal in the human body. Name comes from Latin 'calx' (lime).",
            "Makes bones and teeth strong. In limestone, marble, and chalk. Needed for muscle contraction.",
            new string[] { "Makes bones strong", "In milk and cheese", "In limestone/marble" }
        )},
        
        {21, new ElementEducationalInfo(
            "Scandium",
            1879, "Lars Fredrik Nilson", "Sweden",
            "Named after Scandinavia. One of the rare earth elements, though not actually that rare.",
            "Used in aerospace alloys and sports equipment (baseball bats, bicycle frames). In metal halide lights.",
            new string[] { "Named after Scandinavia", "In sports equipment", "Makes lights brighter" }
        )},
        
        {22, new ElementEducationalInfo(
            "Titanium",
            1791, "William Gregor", "England",
            "Named after the Titans of Greek mythology. As strong as steel but 45% lighter.",
            "Used in aircraft, spacecraft, and medical implants. Won't corrode and is biocompatible.",
            new string[] { "Strong as steel, half the weight", "In hip replacements", "Won't corrode" }
        )},
        
        {23, new ElementEducationalInfo(
            "Vanadium",
            1801, "Andrés Manuel del Río", "Mexico",
            "Named after Vanadis, Norse goddess of beauty, because of its beautiful multicolored compounds.",
            "Strengthens steel. Used in tools, springs, and jet engines. Some organisms use it instead of iron in blood.",
            new string[] { "Makes steel stronger", "Beautiful colored compounds", "Some animals have it in blood" }
        )},
        
        {24, new ElementEducationalInfo(
            "Chromium",
            1797, "Louis Nicolas Vauquelin", "France",
            "Named from Greek 'chroma' (color) for its brightly colored compounds. Makes rubies red.",
            "Chrome plating prevents rust. Stainless steel contains chromium. Makes emeralds green, rubies red.",
            new string[] { "Makes things shiny", "In stainless steel", "Colors rubies and emeralds" }
        )},
        
        {25, new ElementEducationalInfo(
            "Manganese",
            1774, "Johan Gottlieb Gahn", "Sweden",
            "Often confused with magnesium historically. Essential trace element for all life.",
            "Used in steel production and batteries. Your body needs it for bone formation and metabolism.",
            new string[] { "In most batteries", "Strengthens steel", "Your body needs traces" }
        )},
        
        {26, new ElementEducationalInfo(
            "Iron",
            -3000, "Ancient civilizations", "Prehistoric",
            "The Iron Age began around 1200 BCE. Most common element on Earth by mass. Core of Earth is mostly iron.",
            "In hemoglobin carrying oxygen in blood. Used in steel, the most important structural metal.",
            new string[] { "In your blood (hemoglobin)", "Earth's core is iron", "Most used metal" }
        )},
        
        {27, new ElementEducationalInfo(
            "Cobalt",
            1735, "Georg Brandt", "Sweden",
            "First metal discovered in modern times. Name comes from 'kobold' (German for goblin).",
            "Used in blue pigments, batteries, and jet engines. Essential for vitamin B12.",
            new string[] { "Makes blue pigments", "In rechargeable batteries", "In vitamin B12" }
        )},
        
        {28, new ElementEducationalInfo(
            "Nickel",
            1751, "Axel Fredrik Cronstedt", "Sweden",
            "Named after 'Kupfernickel' (devil's copper) because miners couldn't extract copper from its ore.",
            "Used in coins, stainless steel, and batteries. Nickel allergy is common.",
            new string[] { "In many coins", "In stainless steel", "Common allergy" }
        )},
        
        {29, new ElementEducationalInfo(
            "Copper",
            -9000, "Ancient civilizations", "Prehistoric",
            "One of the first metals used by humans. The Bronze Age began when people alloyed it with tin.",
            "Excellent conductor of electricity. Used in wiring, plumbing, and coins. Statue of Liberty is copper.",
            new string[] { "Used for 10,000 years", "Best electrical conductor", "Turns green over time" }
        )},
        
        {30, new ElementEducationalInfo(
            "Zinc",
            1746, "Andreas Sigismund Marggraf", "Germany",
            "Used in brass since ancient times but not isolated until 1746. Name might come from German 'zinke' (tooth).",
            "Protects steel from rust (galvanization). In sunscreen, batteries, and supplements for immune system.",
            new string[] { "Prevents rust", "In sunscreen", "Boosts immune system" }
        )},
        
        {31, new ElementEducationalInfo(
            "Gallium",
            1875, "Paul Emile Lecoq de Boisbaudran", "France",
            "Melts at just 29.76°C - it will melt in your hand! Named after Gallia (France).",
            "Used in semiconductors, LEDs, and solar panels. Gallium arsenide is in smartphones.",
            new string[] { "Melts in your hand", "In LEDs", "In smartphones" }
        )},
        
        {32, new ElementEducationalInfo(
            "Germanium",
            1886, "Clemens Winkler", "Germany",
            "Predicted by Mendeleev 15 years before discovery. Named after Germany.",
            "Used in fiber optics, infrared optics, and early transistors. Now mostly replaced by silicon.",
            new string[] { "Predicted before found", "In fiber optics", "In infrared cameras" }
        )},
        
        {33, new ElementEducationalInfo(
            "Arsenic",
            1250, "Albertus Magnus", "Germany",
            "The 'king of poisons' - used historically for assassination due to being hard to detect.",
            "Used in semiconductors and wood preservatives. Some organisms can use arsenic instead of phosphorus.",
            new string[] { "Famous poison in history", "In semiconductors", "Some life uses it" }
        )},
        
        {34, new ElementEducationalInfo(
            "Selenium",
            1817, "Jöns Jacob Berzelius", "Sweden",
            "Named after Selene, Greek goddess of the Moon (to pair with tellurium, named after Earth).",
            "Essential nutrient in small amounts. Used in electronics, glass-making, and anti-dandruff shampoos.",
            new string[] { "Named after the Moon", "In anti-dandruff shampoo", "Essential nutrient" }
        )},
        
        {35, new ElementEducationalInfo(
            "Bromine",
            1826, "Antoine Jérôme Balard", "France",
            "One of only two elements liquid at room temperature (with mercury). Name means 'stench' in Greek.",
            "Used in flame retardants and photography. Historically used as a sedative.",
            new string[] { "Liquid at room temp", "Name means 'stench'", "In flame retardants" }
        )},
        
        {36, new ElementEducationalInfo(
            "Krypton",
            1898, "William Ramsay & Morris Travers", "England",
            "Name means 'hidden one' in Greek. Superman's home planet was named after this element!",
            "Used in high-performance light bulbs, lasers, and the definition of the meter until 1983.",
            new string[] { "Superman's planet name", "In camera flashes", "Used to define the meter" }
        )},
        
        // Period 5 (selected elements)
        {37, new ElementEducationalInfo(
            "Rubidium",
            1861, "Robert Bunsen & Gustav Kirchhoff", "Germany",
            "Named from Latin 'rubidus' (deep red) for its red spectral lines. Discovered using spectroscopy.",
            "Used in atomic clocks (GPS satellites). Extremely reactive with water.",
            new string[] { "In GPS atomic clocks", "Named for red color", "Very reactive" }
        )},
        
        {38, new ElementEducationalInfo(
            "Strontium",
            1790, "Adair Crawford", "Scotland",
            "Named after Strontian, a village in Scotland where it was found. Makes red fireworks.",
            "Creates brilliant red color in fireworks. Used in CRT televisions and glow-in-the-dark paints.",
            new string[] { "Red fireworks", "In glow-in-dark paint", "Named after Scottish village" }
        )},
        
        {47, new ElementEducationalInfo(
            "Silver",
            -5000, "Ancient civilizations", "Prehistoric",
            "Known since ancient times. Symbol Ag from Latin 'argentum'. Highest electrical conductivity of any element.",
            "Used in jewelry, coins, photography, and electronics. Has antibacterial properties.",
            new string[] { "Best electrical conductor", "Antibacterial", "In photography" }
        )},
        
        {53, new ElementEducationalInfo(
            "Iodine",
            1811, "Bernard Courtois", "France",
            "Discovered accidentally while making saltpeter. Essential for thyroid hormones.",
            "Added to table salt to prevent goiter. Used in X-ray contrast dyes and wound disinfection.",
            new string[] { "In iodized salt", "Your thyroid needs it", "Turns starch purple" }
        )},
        
        // Period 6 (selected elements)
        {79, new ElementEducationalInfo(
            "Gold",
            -6000, "Ancient civilizations", "Prehistoric",
            "Has been treasured since antiquity. One of the few elements found in pure form. Symbol Au from Latin 'aurum'.",
            "Used in jewelry, electronics (it never corrodes), and as currency backing. All gold mined would fit in 3.5 Olympic pools.",
            new string[] { "Never rusts or tarnishes", "In every smartphone", "All gold fits in 3 pools" }
        )},
        
        {80, new ElementEducationalInfo(
            "Mercury",
            -2000, "Ancient civilizations", "Prehistoric",
            "Only metal liquid at room temperature. Named after the planet. Ancient symbol ☿.",
            "Used in thermometers and fluorescent lights. Very toxic - causes 'mad hatter' disease.",
            new string[] { "Only liquid metal", "Very toxic", "In old thermometers" }
        )},
        
        {82, new ElementEducationalInfo(
            "Lead",
            -7000, "Ancient civilizations", "Prehistoric",
            "One of the first metals smelted by humans. Romans used it for plumbing (word 'plumbing' comes from Latin 'plumbum').",
            "Used in car batteries and radiation shielding. Toxic - was removed from gasoline and paint.",
            new string[] { "Very dense and soft", "Blocks radiation", "Was in gasoline" }
        )},
        
        // Actinides
        {92, new ElementEducationalInfo(
            "Uranium",
            1789, "Martin Heinrich Klaproth", "Germany",
            "Named after the planet Uranus. Its radioactivity was discovered by Henri Becquerel in 1896.",
            "Powers nuclear reactors generating 10% of world's electricity. Can be enriched for nuclear weapons.",
            new string[] { "Powers nuclear plants", "Very radioactive", "Named after Uranus" }
        )},
        
        {94, new ElementEducationalInfo(
            "Plutonium",
            1940, "Glenn Seaborg", "USA",
            "First synthesized element. Named after Pluto. Extremely toxic and radioactive.",
            "Used in nuclear weapons (Fat Man bomb) and spacecraft (Voyager missions use plutonium batteries).",
            new string[] { "Made by humans first", "Powers spacecraft", "In nuclear weapons" }
        )},
    };

    /// <summary>
    /// Get educational info for an element, or create basic info if not in database
    /// </summary>
    public static ElementEducationalInfo GetInfo(int atomicNumber)
    {
        if (Data.TryGetValue(atomicNumber, out var info))
        {
            return info;
        }
        
        // Return basic info for elements not yet detailed
        var element = PeriodicTable.GetElement(atomicNumber);
        if (element != null)
        {
            return new ElementEducationalInfo(
                element.Name,
                0, "Various scientists", "Unknown",
                $"A {element.Family.ToString().ToLower()} element with atomic number {atomicNumber}.",
                "Research ongoing for potential applications.",
                new string[] { $"Atomic mass: {element.AtomicMass:F2}", $"Family: {element.Family}" }
            );
        }
        
        return null;
    }
}

/// <summary>
/// Educational information about a chemical element
/// </summary>
[System.Serializable]
public class ElementEducationalInfo
{
    public string ElementName;
    public int DiscoveryYear;        // Negative for BCE
    public string Discoverer;
    public string DiscoveryLocation;
    public string HistoricalFact;
    public string ModernUse;
    public string[] FunFacts;

    public ElementEducationalInfo(string name, int year, string discoverer, string location,
        string historical, string modern, string[] facts)
    {
        ElementName = name;
        DiscoveryYear = year;
        Discoverer = discoverer;
        DiscoveryLocation = location;
        HistoricalFact = historical;
        ModernUse = modern;
        FunFacts = facts;
    }

    /// <summary>
    /// Get formatted discovery year (handles BCE)
    /// </summary>
    public string GetFormattedYear()
    {
        if (DiscoveryYear == 0) return "Unknown";
        if (DiscoveryYear < 0) return $"{-DiscoveryYear} BCE (Ancient)";
        return $"{DiscoveryYear} CE";
    }
}

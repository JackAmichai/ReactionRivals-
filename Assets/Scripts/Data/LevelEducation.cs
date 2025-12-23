using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Educational descriptions for each level explaining what elements unlock and why they're significant.
/// </summary>
public static class LevelEducation
{
    /// <summary>
    /// Educational content for each level
    /// </summary>
    public static readonly Dictionary<int, LevelEducationalContent> LevelContent = new Dictionary<int, LevelEducationalContent>
    {
        {1, new LevelEducationalContent(
            "The Building Blocks of Life",
            "Welcome, young chemist! You start with the four most essential elements for life on Earth.",
            "These four elements - Hydrogen, Carbon, Nitrogen, and Oxygen - make up 96% of the mass of all living organisms. " +
            "Carbon forms the backbone of every organic molecule, while Hydrogen and Oxygen combine to form water. " +
            "Nitrogen is essential for proteins and DNA.",
            new string[] { "CHNOPS = 6 elements that make all life", "Your body is mostly these 4 elements", "Carbon can form 4 bonds - very versatile!" },
            "1766-1774",
            "Life Chemistry"
        )},

        {2, new LevelEducationalContent(
            "Noble Beginnings & Salt of the Earth",
            "Discover the most stable element and the components of common table salt!",
            "Helium is a noble gas - so stable it won't react with anything. Meanwhile, Sodium and Chlorine are opposites: " +
            "sodium is highly reactive, chlorine is toxic gas, but together they form harmless table salt (NaCl). " +
            "Iron is the metal that powers your blood and built civilizations.",
            new string[] { "NaCl is table salt!", "Noble gases have full electron shells", "Iron Age began ~1200 BCE" },
            "Ancient - 1868",
            "Stability & Reactivity"
        )},

        {3, new LevelEducationalContent(
            "The Lightest Metals & First Noble Gas",
            "Meet the alkali metals and complete the first period of the periodic table.",
            "Lithium and Beryllium are the lightest metals. Lithium powers your phone! Boron is a metalloid used in everything " +
            "from laundry detergent to bulletproof vests. Fluorine is the most reactive element - it attacks almost everything. " +
            "Neon gives us those iconic glowing signs.",
            new string[] { "Lithium-ion batteries changed the world", "Fluorine is in your toothpaste", "Neon signs were invented in 1910" },
            "1798-1898",
            "Light Elements"
        )},

        {4, new LevelEducationalContent(
            "The Silicon Age",
            "Welcome to the elements that power modern technology!",
            "Silicon is the second most abundant element in Earth's crust and the heart of every computer chip. " +
            "Aluminum was once more precious than gold - Napoleon served honored guests with aluminum cutlery! " +
            "Phosphorus was discovered from urine while searching for the Philosopher's Stone. Sulfur has been known since ancient times as 'brimstone'.",
            new string[] { "Silicon Valley is named after this element", "Phosphorus glows in the dark", "Sulfuric acid is most produced chemical" },
            "1669-1824",
            "Technology & Industry"
        )},

        {5, new LevelEducationalContent(
            "Alkali Expansion & Heavy Halogens",
            "More reactive metals and the elements of purification.",
            "Potassium is essential for your heart - bananas are famously rich in it. Calcium builds your bones and teeth. " +
            "Bromine is one of only two elements that's liquid at room temperature. Krypton inspired Superman's home planet name!",
            new string[] { "Eat bananas for potassium", "Bones are calcium phosphate", "Krypton means 'hidden one' in Greek" },
            "1807-1898",
            "Body Chemistry"
        )},

        {6, new LevelEducationalContent(
            "The Transition Begins",
            "Enter the colorful world of transition metals - Part 1.",
            "Transition metals give us beautiful colors and strong alloys. Scandium strengthens aluminum for aerospace. " +
            "Titanium is as strong as steel but 45% lighter - perfect for aircraft and medical implants. " +
            "Vanadium is named after a Norse goddess of beauty because its compounds are so colorful. " +
            "Chromium makes things shiny and is why rubies are red and emeralds are green!",
            new string[] { "Transition metals have multiple oxidation states", "Chrome plating prevents rust", "Your hip replacement might be titanium" },
            "1791-1830",
            "Transition Metals I"
        )},

        {7, new LevelEducationalContent(
            "Metals of Industry",
            "The workhorses of human civilization.",
            "Cobalt creates the brilliant blue in ceramics and is essential for vitamin B12. " +
            "Nickel is in your coins and gives stainless steel its corrosion resistance. " +
            "Copper has been used for 10,000 years - the Bronze Age began when humans mixed it with tin. " +
            "Zinc protects other metals from rust through galvanization.",
            new string[] { "Cobalt blue has been prized for centuries", "Bronze = Copper + Tin", "Galvanized steel is coated with zinc" },
            "1735-1751",
            "Transition Metals II"
        )},

        {8, new LevelEducationalContent(
            "The Semiconductor Revolution",
            "Elements that bridge metals and non-metals.",
            "Gallium has an amazing property: it melts in your hand! (29.76°C melting point). " +
            "Germanium was one of the elements Mendeleev predicted before it was discovered. " +
            "Arsenic was the 'king of poisons' used by assassins throughout history. " +
            "Selenium is named after the Moon goddess Selene.",
            new string[] { "Gallium is in your phone's chips", "Mendeleev predicted germanium's properties exactly", "Some shampoos contain selenium" },
            "1817-1886",
            "Metalloids"
        )},

        {9, new LevelEducationalContent(
            "Heavy Alkalis & Essential Iodine",
            "The most reactive metals and an essential nutrient.",
            "Rubidium is used in the atomic clocks that make GPS possible - accurate to 1 second in 300 million years! " +
            "Strontium creates the red colors in fireworks. " +
            "Iodine is essential for your thyroid gland - that's why it's added to table salt.",
            new string[] { "GPS needs atomic clocks with rubidium", "Red fireworks = strontium", "Iodine deficiency causes goiter" },
            "1811-1861",
            "Essential Traces"
        )},

        {10, new LevelEducationalContent(
            "The Rare Transition Metals",
            "Elements with specialized industrial applications.",
            "Yttrium is named after Ytterby, Sweden - a village that gave its name to 4 elements! " +
            "Zirconium doesn't react with water or acids - perfect for nuclear reactors. " +
            "Niobium makes MRI magnets possible. " +
            "Molybdenum is essential for all life - a tiny amount is in every cell.",
            new string[] { "4 elements named after one village", "Zirconia is fake diamond", "Molybdenum is in your enzymes" },
            "1791-1801",
            "Transition Metals III"
        )},

        {11, new LevelEducationalContent(
            "Precious Metals",
            "The most valuable elements on Earth.",
            "Ruthenium, Rhodium, and Palladium are 'platinum group metals' - rarer than gold. " +
            "Silver has the highest electrical conductivity of any element. " +
            "Cadmium is toxic but was used in nickel-cadmium batteries for decades.",
            new string[] { "Rhodium costs more than gold", "Silver is in solar panels", "Electric cars use precious metals" },
            "1735-1803",
            "Platinum Group"
        )},

        {12, new LevelEducationalContent(
            "Heavy Post-Transition Metals",
            "The elements at the edge of stability.",
            "Indium is so soft you can bite it! It's used in touchscreens. " +
            "Tin has been used since the Bronze Age (3300 BCE). " +
            "Antimony was used as eye makeup in ancient Egypt. " +
            "Tellurium is one of the rarest elements on Earth.",
            new string[] { "Your touchscreen has indium", "Bronze Age needed tin", "Tellurium smells like garlic" },
            "1250-1817",
            "Heavy Elements"
        )},

        {13, new LevelEducationalContent(
            "The Heaviest Stable Elements",
            "Approaching the edge of elemental stability.",
            "Cesium is the most reactive metal and is used in atomic clocks. " +
            "Barium is used in X-ray imaging - you might drink barium solution before an X-ray. " +
            "Astatine is so rare that at any moment there's only about 25 grams on Earth. " +
            "Radon is a radioactive gas that seeps from rocks - a health hazard in some homes.",
            new string[] { "Only 25g of astatine on Earth", "Radon is in some basements", "Cesium atomic clocks define the second" },
            "1860-1940",
            "Edge of Stability"
        )},

        {14, new LevelEducationalContent(
            "The Noble Metals",
            "Elements treasured throughout human history.",
            "Platinum is so rare that all ever mined would fit in a living room. " +
            "Gold has been treasured for 6,000 years and never tarnishes. " +
            "Mercury is the only metal that's liquid at room temperature - and is highly toxic.",
            new string[] { "All platinum fits in one room", "Gold never rusts", "Mercury causes 'mad hatter disease'" },
            "Ancient - 1735",
            "Precious Elements"
        )},

        {15, new LevelEducationalContent(
            "The Heavy Transition Metals",
            "The densest and most extreme elements.",
            "Hafnium was the last stable element to be discovered (1923). " +
            "Tantalum is used in your phone's capacitors. " +
            "Tungsten has the highest melting point of any element - 3422°C! " +
            "Iridium came to Earth in the asteroid that killed the dinosaurs. " +
            "Osmium is the densest naturally occurring element.",
            new string[] { "Tungsten = highest melting point", "Iridium marks dinosaur extinction", "Osmium is super dense" },
            "1783-1923",
            "Extreme Elements"
        )},

        {16, new LevelEducationalContent(
            "The Post-Transition Heavy Metals",
            "Elements with fascinating and dangerous properties.",
            "Thallium was used as a murder poison because it's tasteless and hard to detect. " +
            "Lead has been used for 7,000 years but we now know it's highly toxic. " +
            "Bismuth is the most naturally diamagnetic element - it repels magnets! " +
            "Polonium was discovered by Marie Curie and named after her homeland Poland.",
            new string[] { "Lead poisoning affected Rome", "Bismuth is in Pepto-Bismol", "Marie Curie discovered polonium" },
            "1753-1898",
            "Heavy Metals"
        )},

        {17, new LevelEducationalContent(
            "The Lanthanides Part 1",
            "The first rare earth elements - not actually rare!",
            "Despite being called 'rare earths,' lanthanides are fairly common. Cerium is more common than copper! " +
            "These elements are essential for modern technology: smartphones, electric vehicles, wind turbines. " +
            "Europium gives red color to TV screens and euro banknotes (named after Europe).",
            new string[] { "Not actually rare!", "In every smartphone", "Europium on euro banknotes" },
            "1839-1886",
            "Rare Earths I"
        )},

        {18, new LevelEducationalContent(
            "The Lanthanides Part 2",
            "More rare earth elements with amazing properties.",
            "Terbium is used in green phosphors for screens. " +
            "Holmium has the highest magnetic strength of any element. " +
            "Ytterbium is used in atomic clocks even more accurate than cesium clocks. " +
            "Lutetium, the last lanthanide, is named after Lutetia, the Roman name for Paris.",
            new string[] { "Holmium = strongest magnetism", "Paris's Roman name was Lutetia", "These power electric car motors" },
            "1843-1907",
            "Rare Earths II"
        )},

        {19, new LevelEducationalContent(
            "The Final Natural Elements",
            "The heaviest elements found in nature.",
            "Francium is the most unstable natural element - its longest-lived isotope lasts only 22 minutes. " +
            "Radium was once used in glow-in-the-dark watch dials, killing many 'Radium Girls' workers. " +
            "Tennessine was just confirmed in 2010 - one of the newest elements. " +
            "Oganesson, element 118, completes the periodic table (for now).",
            new string[] { "Francium: 22 minute half-life", "The 'Radium Girls' tragedy", "Periodic table now complete to 118" },
            "1898-2010",
            "Final Elements"
        )},

        {20, new LevelEducationalContent(
            "The Power of the Atom",
            "Elements that changed the world - nuclear power.",
            "Actinium is highly radioactive and glows blue in the dark. " +
            "Thorium could power next-generation nuclear reactors more safely. " +
            "Uranium powers nuclear plants generating 10% of world electricity. " +
            "Plutonium was used in the 'Fat Man' bomb and powers Voyager spacecraft.",
            new string[] { "Nuclear power = 10% of electricity", "Voyager uses plutonium", "Thorium reactors are being developed" },
            "1789-1940",
            "Nuclear Age"
        )},

        {21, new LevelEducationalContent(
            "The Synthetic Actinides",
            "Elements created in laboratories and nuclear reactors.",
            "These elements don't exist in nature - humans created them! " +
            "Americium is in your smoke detector. " +
            "Californium is used to detect gold and silver in prospecting. " +
            "Einsteinium was discovered in the fallout of the first hydrogen bomb test.",
            new string[] { "Smoke detectors have americium", "Named after scientists and places", "Made in nuclear reactors" },
            "1944-1961",
            "Synthetic Elements"
        )},

        {22, new LevelEducationalContent(
            "The Superheavy Elements",
            "The frontier of chemistry - elements that exist for fractions of a second.",
            "These elements are made by smashing atoms together in particle accelerators. " +
            "They exist for microseconds to milliseconds before decaying. " +
            "Scientists create just a few atoms at a time! " +
            "The 'Island of Stability' theory predicts some superheavy elements might be stable.",
            new string[] { "Made in particle accelerators", "Exist for milliseconds", "Only few atoms ever created" },
            "1964-2016",
            "The Final Frontier"
        )},
    };

    /// <summary>
    /// Get content for a specific level
    /// </summary>
    public static LevelEducationalContent GetLevelContent(int level)
    {
        if (LevelContent.TryGetValue(level, out var content))
        {
            return content;
        }
        return null;
    }
}

/// <summary>
/// Educational content for a game level
/// </summary>
[System.Serializable]
public class LevelEducationalContent
{
    public string Title;
    public string Subtitle;
    public string MainDescription;
    public string[] DidYouKnow;
    public string DiscoveryPeriod;
    public string Theme;

    public LevelEducationalContent(string title, string subtitle, string description, 
        string[] facts, string period, string theme)
    {
        Title = title;
        Subtitle = subtitle;
        MainDescription = description;
        DidYouKnow = facts;
        DiscoveryPeriod = period;
        Theme = theme;
    }
}

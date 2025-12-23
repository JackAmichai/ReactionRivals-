using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Complete Periodic Table data for all 118 elements.
/// Each element has accurate chemistry data from the real periodic table.
/// </summary>
public static class PeriodicTable
{
    /// <summary>
    /// All 118 elements organized by atomic number
    /// </summary>
    public static readonly PeriodicElementInfo[] Elements = new PeriodicElementInfo[]
    {
        // Period 1
        new PeriodicElementInfo(1, "H", "Hydrogen", 1.008f, 1, 2.20f, ElementFamily.Hydrogen, 1, 1, new Color(0.9f, 0.9f, 1f)),
        new PeriodicElementInfo(2, "He", "Helium", 4.003f, 2, 0f, ElementFamily.NobleGas, 1, 18, new Color(0.85f, 1f, 1f)),
        
        // Period 2
        new PeriodicElementInfo(3, "Li", "Lithium", 6.941f, 1, 0.98f, ElementFamily.Alkali, 2, 1, new Color(0.8f, 0.5f, 1f)),
        new PeriodicElementInfo(4, "Be", "Beryllium", 9.012f, 2, 1.57f, ElementFamily.AlkalineEarth, 2, 2, new Color(0.76f, 1f, 0f)),
        new PeriodicElementInfo(5, "B", "Boron", 10.81f, 3, 2.04f, ElementFamily.Metalloid, 2, 13, new Color(1f, 0.71f, 0.71f)),
        new PeriodicElementInfo(6, "C", "Carbon", 12.011f, 4, 2.55f, ElementFamily.NonMetal, 2, 14, new Color(0.2f, 0.2f, 0.2f)),
        new PeriodicElementInfo(7, "N", "Nitrogen", 14.007f, 5, 3.04f, ElementFamily.NonMetal, 2, 15, new Color(0.12f, 0.12f, 0.56f)),
        new PeriodicElementInfo(8, "O", "Oxygen", 15.999f, 6, 3.44f, ElementFamily.NonMetal, 2, 16, new Color(1f, 0.05f, 0.05f)),
        new PeriodicElementInfo(9, "F", "Fluorine", 18.998f, 7, 3.98f, ElementFamily.Halogen, 2, 17, new Color(0.56f, 0.88f, 0.31f)),
        new PeriodicElementInfo(10, "Ne", "Neon", 20.180f, 8, 0f, ElementFamily.NobleGas, 2, 18, new Color(0.7f, 0.89f, 0.96f)),
        
        // Period 3
        new PeriodicElementInfo(11, "Na", "Sodium", 22.990f, 1, 0.93f, ElementFamily.Alkali, 3, 1, new Color(0.67f, 0.36f, 0.95f)),
        new PeriodicElementInfo(12, "Mg", "Magnesium", 24.305f, 2, 1.31f, ElementFamily.AlkalineEarth, 3, 2, new Color(0.54f, 1f, 0f)),
        new PeriodicElementInfo(13, "Al", "Aluminum", 26.982f, 3, 1.61f, ElementFamily.PostTransitionMetal, 3, 13, new Color(0.75f, 0.65f, 0.65f)),
        new PeriodicElementInfo(14, "Si", "Silicon", 28.086f, 4, 1.90f, ElementFamily.Metalloid, 3, 14, new Color(0.94f, 0.78f, 0.63f)),
        new PeriodicElementInfo(15, "P", "Phosphorus", 30.974f, 5, 2.19f, ElementFamily.NonMetal, 3, 15, new Color(1f, 0.5f, 0f)),
        new PeriodicElementInfo(16, "S", "Sulfur", 32.065f, 6, 2.58f, ElementFamily.NonMetal, 3, 16, new Color(1f, 1f, 0.19f)),
        new PeriodicElementInfo(17, "Cl", "Chlorine", 35.453f, 7, 3.16f, ElementFamily.Halogen, 3, 17, new Color(0.12f, 0.94f, 0.12f)),
        new PeriodicElementInfo(18, "Ar", "Argon", 39.948f, 8, 0f, ElementFamily.NobleGas, 3, 18, new Color(0.5f, 0.82f, 0.89f)),
        
        // Period 4
        new PeriodicElementInfo(19, "K", "Potassium", 39.098f, 1, 0.82f, ElementFamily.Alkali, 4, 1, new Color(0.56f, 0.25f, 0.83f)),
        new PeriodicElementInfo(20, "Ca", "Calcium", 40.078f, 2, 1.00f, ElementFamily.AlkalineEarth, 4, 2, new Color(0.24f, 1f, 0f)),
        new PeriodicElementInfo(21, "Sc", "Scandium", 44.956f, 2, 1.36f, ElementFamily.TransitionMetal, 4, 3, new Color(0.9f, 0.9f, 0.9f)),
        new PeriodicElementInfo(22, "Ti", "Titanium", 47.867f, 2, 1.54f, ElementFamily.TransitionMetal, 4, 4, new Color(0.75f, 0.76f, 0.78f)),
        new PeriodicElementInfo(23, "V", "Vanadium", 50.942f, 2, 1.63f, ElementFamily.TransitionMetal, 4, 5, new Color(0.65f, 0.65f, 0.67f)),
        new PeriodicElementInfo(24, "Cr", "Chromium", 51.996f, 1, 1.66f, ElementFamily.TransitionMetal, 4, 6, new Color(0.54f, 0.6f, 0.78f)),
        new PeriodicElementInfo(25, "Mn", "Manganese", 54.938f, 2, 1.55f, ElementFamily.TransitionMetal, 4, 7, new Color(0.61f, 0.48f, 0.78f)),
        new PeriodicElementInfo(26, "Fe", "Iron", 55.845f, 2, 1.83f, ElementFamily.TransitionMetal, 4, 8, new Color(0.44f, 0.44f, 0.44f)),
        new PeriodicElementInfo(27, "Co", "Cobalt", 58.933f, 2, 1.88f, ElementFamily.TransitionMetal, 4, 9, new Color(0.94f, 0.56f, 0.63f)),
        new PeriodicElementInfo(28, "Ni", "Nickel", 58.693f, 2, 1.91f, ElementFamily.TransitionMetal, 4, 10, new Color(0.31f, 0.82f, 0.31f)),
        new PeriodicElementInfo(29, "Cu", "Copper", 63.546f, 1, 1.90f, ElementFamily.TransitionMetal, 4, 11, new Color(0.78f, 0.5f, 0.2f)),
        new PeriodicElementInfo(30, "Zn", "Zinc", 65.38f, 2, 1.65f, ElementFamily.TransitionMetal, 4, 12, new Color(0.49f, 0.5f, 0.69f)),
        new PeriodicElementInfo(31, "Ga", "Gallium", 69.723f, 3, 1.81f, ElementFamily.PostTransitionMetal, 4, 13, new Color(0.76f, 0.56f, 0.56f)),
        new PeriodicElementInfo(32, "Ge", "Germanium", 72.64f, 4, 2.01f, ElementFamily.Metalloid, 4, 14, new Color(0.4f, 0.56f, 0.56f)),
        new PeriodicElementInfo(33, "As", "Arsenic", 74.922f, 5, 2.18f, ElementFamily.Metalloid, 4, 15, new Color(0.74f, 0.5f, 0.89f)),
        new PeriodicElementInfo(34, "Se", "Selenium", 78.96f, 6, 2.55f, ElementFamily.NonMetal, 4, 16, new Color(1f, 0.63f, 0f)),
        new PeriodicElementInfo(35, "Br", "Bromine", 79.904f, 7, 2.96f, ElementFamily.Halogen, 4, 17, new Color(0.65f, 0.16f, 0.16f)),
        new PeriodicElementInfo(36, "Kr", "Krypton", 83.798f, 8, 3.00f, ElementFamily.NobleGas, 4, 18, new Color(0.36f, 0.72f, 0.82f)),
        
        // Period 5
        new PeriodicElementInfo(37, "Rb", "Rubidium", 85.468f, 1, 0.82f, ElementFamily.Alkali, 5, 1, new Color(0.44f, 0.18f, 0.69f)),
        new PeriodicElementInfo(38, "Sr", "Strontium", 87.62f, 2, 0.95f, ElementFamily.AlkalineEarth, 5, 2, new Color(0f, 1f, 0f)),
        new PeriodicElementInfo(39, "Y", "Yttrium", 88.906f, 2, 1.22f, ElementFamily.TransitionMetal, 5, 3, new Color(0.58f, 1f, 1f)),
        new PeriodicElementInfo(40, "Zr", "Zirconium", 91.224f, 2, 1.33f, ElementFamily.TransitionMetal, 5, 4, new Color(0.58f, 0.88f, 0.88f)),
        new PeriodicElementInfo(41, "Nb", "Niobium", 92.906f, 1, 1.6f, ElementFamily.TransitionMetal, 5, 5, new Color(0.45f, 0.76f, 0.79f)),
        new PeriodicElementInfo(42, "Mo", "Molybdenum", 95.96f, 1, 2.16f, ElementFamily.TransitionMetal, 5, 6, new Color(0.33f, 0.71f, 0.71f)),
        new PeriodicElementInfo(43, "Tc", "Technetium", 98f, 2, 1.9f, ElementFamily.TransitionMetal, 5, 7, new Color(0.23f, 0.62f, 0.62f)),
        new PeriodicElementInfo(44, "Ru", "Ruthenium", 101.07f, 1, 2.2f, ElementFamily.TransitionMetal, 5, 8, new Color(0.14f, 0.56f, 0.56f)),
        new PeriodicElementInfo(45, "Rh", "Rhodium", 102.91f, 1, 2.28f, ElementFamily.TransitionMetal, 5, 9, new Color(0.04f, 0.49f, 0.55f)),
        new PeriodicElementInfo(46, "Pd", "Palladium", 106.42f, 0, 2.20f, ElementFamily.TransitionMetal, 5, 10, new Color(0f, 0.41f, 0.52f)),
        new PeriodicElementInfo(47, "Ag", "Silver", 107.87f, 1, 1.93f, ElementFamily.TransitionMetal, 5, 11, new Color(0.75f, 0.75f, 0.75f)),
        new PeriodicElementInfo(48, "Cd", "Cadmium", 112.41f, 2, 1.69f, ElementFamily.TransitionMetal, 5, 12, new Color(1f, 0.85f, 0.56f)),
        new PeriodicElementInfo(49, "In", "Indium", 114.82f, 3, 1.78f, ElementFamily.PostTransitionMetal, 5, 13, new Color(0.65f, 0.46f, 0.45f)),
        new PeriodicElementInfo(50, "Sn", "Tin", 118.71f, 4, 1.96f, ElementFamily.PostTransitionMetal, 5, 14, new Color(0.4f, 0.5f, 0.5f)),
        new PeriodicElementInfo(51, "Sb", "Antimony", 121.76f, 5, 2.05f, ElementFamily.Metalloid, 5, 15, new Color(0.62f, 0.39f, 0.71f)),
        new PeriodicElementInfo(52, "Te", "Tellurium", 127.60f, 6, 2.1f, ElementFamily.Metalloid, 5, 16, new Color(0.83f, 0.48f, 0f)),
        new PeriodicElementInfo(53, "I", "Iodine", 126.90f, 7, 2.66f, ElementFamily.Halogen, 5, 17, new Color(0.58f, 0f, 0.58f)),
        new PeriodicElementInfo(54, "Xe", "Xenon", 131.29f, 8, 2.60f, ElementFamily.NobleGas, 5, 18, new Color(0.26f, 0.62f, 0.69f)),
        
        // Period 6
        new PeriodicElementInfo(55, "Cs", "Cesium", 132.91f, 1, 0.79f, ElementFamily.Alkali, 6, 1, new Color(0.34f, 0.09f, 0.56f)),
        new PeriodicElementInfo(56, "Ba", "Barium", 137.33f, 2, 0.89f, ElementFamily.AlkalineEarth, 6, 2, new Color(0f, 0.79f, 0f)),
        // Lanthanides (57-71)
        new PeriodicElementInfo(57, "La", "Lanthanum", 138.91f, 2, 1.1f, ElementFamily.Lanthanide, 6, 3, new Color(0.44f, 0.83f, 1f)),
        new PeriodicElementInfo(58, "Ce", "Cerium", 140.12f, 2, 1.12f, ElementFamily.Lanthanide, 6, 4, new Color(1f, 1f, 0.78f)),
        new PeriodicElementInfo(59, "Pr", "Praseodymium", 140.91f, 2, 1.13f, ElementFamily.Lanthanide, 6, 5, new Color(0.85f, 1f, 0.78f)),
        new PeriodicElementInfo(60, "Nd", "Neodymium", 144.24f, 2, 1.14f, ElementFamily.Lanthanide, 6, 6, new Color(0.78f, 1f, 0.78f)),
        new PeriodicElementInfo(61, "Pm", "Promethium", 145f, 2, 1.13f, ElementFamily.Lanthanide, 6, 7, new Color(0.64f, 1f, 0.78f)),
        new PeriodicElementInfo(62, "Sm", "Samarium", 150.36f, 2, 1.17f, ElementFamily.Lanthanide, 6, 8, new Color(0.56f, 1f, 0.78f)),
        new PeriodicElementInfo(63, "Eu", "Europium", 151.96f, 2, 1.2f, ElementFamily.Lanthanide, 6, 9, new Color(0.38f, 1f, 0.78f)),
        new PeriodicElementInfo(64, "Gd", "Gadolinium", 157.25f, 2, 1.2f, ElementFamily.Lanthanide, 6, 10, new Color(0.27f, 1f, 0.78f)),
        new PeriodicElementInfo(65, "Tb", "Terbium", 158.93f, 2, 1.1f, ElementFamily.Lanthanide, 6, 11, new Color(0.19f, 1f, 0.78f)),
        new PeriodicElementInfo(66, "Dy", "Dysprosium", 162.50f, 2, 1.22f, ElementFamily.Lanthanide, 6, 12, new Color(0.12f, 1f, 0.78f)),
        new PeriodicElementInfo(67, "Ho", "Holmium", 164.93f, 2, 1.23f, ElementFamily.Lanthanide, 6, 13, new Color(0f, 1f, 0.61f)),
        new PeriodicElementInfo(68, "Er", "Erbium", 167.26f, 2, 1.24f, ElementFamily.Lanthanide, 6, 14, new Color(0f, 0.9f, 0.46f)),
        new PeriodicElementInfo(69, "Tm", "Thulium", 168.93f, 2, 1.25f, ElementFamily.Lanthanide, 6, 15, new Color(0f, 0.83f, 0.32f)),
        new PeriodicElementInfo(70, "Yb", "Ytterbium", 173.05f, 2, 1.1f, ElementFamily.Lanthanide, 6, 16, new Color(0f, 0.75f, 0.22f)),
        new PeriodicElementInfo(71, "Lu", "Lutetium", 174.97f, 2, 1.27f, ElementFamily.Lanthanide, 6, 17, new Color(0f, 0.67f, 0.14f)),
        // Continue Period 6
        new PeriodicElementInfo(72, "Hf", "Hafnium", 178.49f, 2, 1.3f, ElementFamily.TransitionMetal, 6, 4, new Color(0.3f, 0.76f, 1f)),
        new PeriodicElementInfo(73, "Ta", "Tantalum", 180.95f, 2, 1.5f, ElementFamily.TransitionMetal, 6, 5, new Color(0.3f, 0.65f, 1f)),
        new PeriodicElementInfo(74, "W", "Tungsten", 183.84f, 2, 2.36f, ElementFamily.TransitionMetal, 6, 6, new Color(0.13f, 0.58f, 0.84f)),
        new PeriodicElementInfo(75, "Re", "Rhenium", 186.21f, 2, 1.9f, ElementFamily.TransitionMetal, 6, 7, new Color(0.15f, 0.49f, 0.67f)),
        new PeriodicElementInfo(76, "Os", "Osmium", 190.23f, 2, 2.2f, ElementFamily.TransitionMetal, 6, 8, new Color(0.15f, 0.4f, 0.59f)),
        new PeriodicElementInfo(77, "Ir", "Iridium", 192.22f, 2, 2.20f, ElementFamily.TransitionMetal, 6, 9, new Color(0.09f, 0.33f, 0.53f)),
        new PeriodicElementInfo(78, "Pt", "Platinum", 195.08f, 1, 2.28f, ElementFamily.TransitionMetal, 6, 10, new Color(0.82f, 0.82f, 0.88f)),
        new PeriodicElementInfo(79, "Au", "Gold", 196.97f, 1, 2.54f, ElementFamily.TransitionMetal, 6, 11, new Color(1f, 0.82f, 0f)),
        new PeriodicElementInfo(80, "Hg", "Mercury", 200.59f, 2, 2.00f, ElementFamily.TransitionMetal, 6, 12, new Color(0.72f, 0.72f, 0.82f)),
        new PeriodicElementInfo(81, "Tl", "Thallium", 204.38f, 3, 1.62f, ElementFamily.PostTransitionMetal, 6, 13, new Color(0.65f, 0.33f, 0.3f)),
        new PeriodicElementInfo(82, "Pb", "Lead", 207.2f, 4, 2.33f, ElementFamily.PostTransitionMetal, 6, 14, new Color(0.34f, 0.35f, 0.38f)),
        new PeriodicElementInfo(83, "Bi", "Bismuth", 208.98f, 5, 2.02f, ElementFamily.PostTransitionMetal, 6, 15, new Color(0.62f, 0.31f, 0.71f)),
        new PeriodicElementInfo(84, "Po", "Polonium", 209f, 6, 2.0f, ElementFamily.Metalloid, 6, 16, new Color(0.67f, 0.36f, 0f)),
        new PeriodicElementInfo(85, "At", "Astatine", 210f, 7, 2.2f, ElementFamily.Halogen, 6, 17, new Color(0.46f, 0.31f, 0.27f)),
        new PeriodicElementInfo(86, "Rn", "Radon", 222f, 8, 0f, ElementFamily.NobleGas, 6, 18, new Color(0.26f, 0.51f, 0.59f)),
        
        // Period 7
        new PeriodicElementInfo(87, "Fr", "Francium", 223f, 1, 0.7f, ElementFamily.Alkali, 7, 1, new Color(0.26f, 0f, 0.4f)),
        new PeriodicElementInfo(88, "Ra", "Radium", 226f, 2, 0.9f, ElementFamily.AlkalineEarth, 7, 2, new Color(0f, 0.49f, 0f)),
        // Actinides (89-103)
        new PeriodicElementInfo(89, "Ac", "Actinium", 227f, 2, 1.1f, ElementFamily.Actinide, 7, 3, new Color(0.44f, 0.67f, 0.98f)),
        new PeriodicElementInfo(90, "Th", "Thorium", 232.04f, 2, 1.3f, ElementFamily.Actinide, 7, 4, new Color(0f, 0.73f, 1f)),
        new PeriodicElementInfo(91, "Pa", "Protactinium", 231.04f, 2, 1.5f, ElementFamily.Actinide, 7, 5, new Color(0f, 0.63f, 1f)),
        new PeriodicElementInfo(92, "U", "Uranium", 238.03f, 2, 1.38f, ElementFamily.Actinide, 7, 6, new Color(0f, 0.56f, 1f)),
        new PeriodicElementInfo(93, "Np", "Neptunium", 237f, 2, 1.36f, ElementFamily.Actinide, 7, 7, new Color(0f, 0.5f, 1f)),
        new PeriodicElementInfo(94, "Pu", "Plutonium", 244f, 2, 1.28f, ElementFamily.Actinide, 7, 8, new Color(0f, 0.42f, 1f)),
        new PeriodicElementInfo(95, "Am", "Americium", 243f, 2, 1.3f, ElementFamily.Actinide, 7, 9, new Color(0.33f, 0.36f, 0.95f)),
        new PeriodicElementInfo(96, "Cm", "Curium", 247f, 2, 1.3f, ElementFamily.Actinide, 7, 10, new Color(0.47f, 0.36f, 0.89f)),
        new PeriodicElementInfo(97, "Bk", "Berkelium", 247f, 2, 1.3f, ElementFamily.Actinide, 7, 11, new Color(0.54f, 0.31f, 0.89f)),
        new PeriodicElementInfo(98, "Cf", "Californium", 251f, 2, 1.3f, ElementFamily.Actinide, 7, 12, new Color(0.63f, 0.21f, 0.83f)),
        new PeriodicElementInfo(99, "Es", "Einsteinium", 252f, 2, 1.3f, ElementFamily.Actinide, 7, 13, new Color(0.7f, 0.12f, 0.83f)),
        new PeriodicElementInfo(100, "Fm", "Fermium", 257f, 2, 1.3f, ElementFamily.Actinide, 7, 14, new Color(0.7f, 0.12f, 0.73f)),
        new PeriodicElementInfo(101, "Md", "Mendelevium", 258f, 2, 1.3f, ElementFamily.Actinide, 7, 15, new Color(0.7f, 0.05f, 0.65f)),
        new PeriodicElementInfo(102, "No", "Nobelium", 259f, 2, 1.3f, ElementFamily.Actinide, 7, 16, new Color(0.74f, 0.05f, 0.53f)),
        new PeriodicElementInfo(103, "Lr", "Lawrencium", 262f, 3, 1.3f, ElementFamily.Actinide, 7, 17, new Color(0.78f, 0f, 0.4f)),
        // Continue Period 7
        new PeriodicElementInfo(104, "Rf", "Rutherfordium", 267f, 2, 0f, ElementFamily.TransitionMetal, 7, 4, new Color(0.8f, 0f, 0.35f)),
        new PeriodicElementInfo(105, "Db", "Dubnium", 268f, 2, 0f, ElementFamily.TransitionMetal, 7, 5, new Color(0.82f, 0f, 0.31f)),
        new PeriodicElementInfo(106, "Sg", "Seaborgium", 271f, 2, 0f, ElementFamily.TransitionMetal, 7, 6, new Color(0.85f, 0f, 0.27f)),
        new PeriodicElementInfo(107, "Bh", "Bohrium", 270f, 2, 0f, ElementFamily.TransitionMetal, 7, 7, new Color(0.88f, 0f, 0.22f)),
        new PeriodicElementInfo(108, "Hs", "Hassium", 277f, 2, 0f, ElementFamily.TransitionMetal, 7, 8, new Color(0.9f, 0f, 0.18f)),
        new PeriodicElementInfo(109, "Mt", "Meitnerium", 276f, 2, 0f, ElementFamily.TransitionMetal, 7, 9, new Color(0.92f, 0f, 0.15f)),
        new PeriodicElementInfo(110, "Ds", "Darmstadtium", 281f, 2, 0f, ElementFamily.TransitionMetal, 7, 10, new Color(0.93f, 0f, 0.12f)),
        new PeriodicElementInfo(111, "Rg", "Roentgenium", 280f, 2, 0f, ElementFamily.TransitionMetal, 7, 11, new Color(0.94f, 0f, 0.09f)),
        new PeriodicElementInfo(112, "Cn", "Copernicium", 285f, 2, 0f, ElementFamily.TransitionMetal, 7, 12, new Color(0.95f, 0f, 0.06f)),
        new PeriodicElementInfo(113, "Nh", "Nihonium", 284f, 3, 0f, ElementFamily.PostTransitionMetal, 7, 13, new Color(0.96f, 0f, 0.03f)),
        new PeriodicElementInfo(114, "Fl", "Flerovium", 289f, 4, 0f, ElementFamily.PostTransitionMetal, 7, 14, new Color(0.97f, 0f, 0f)),
        new PeriodicElementInfo(115, "Mc", "Moscovium", 288f, 5, 0f, ElementFamily.PostTransitionMetal, 7, 15, new Color(0.98f, 0f, 0f)),
        new PeriodicElementInfo(116, "Lv", "Livermorium", 293f, 6, 0f, ElementFamily.PostTransitionMetal, 7, 16, new Color(0.99f, 0f, 0f)),
        new PeriodicElementInfo(117, "Ts", "Tennessine", 294f, 7, 0f, ElementFamily.Halogen, 7, 17, new Color(0.99f, 0f, 0f)),
        new PeriodicElementInfo(118, "Og", "Oganesson", 294f, 8, 0f, ElementFamily.NobleGas, 7, 18, new Color(0.26f, 0.42f, 0.49f)),
    };

    /// <summary>
    /// Get element by atomic number (1-118)
    /// </summary>
    public static PeriodicElementInfo GetElement(int atomicNumber)
    {
        if (atomicNumber < 1 || atomicNumber > Elements.Length)
            return null;
        return Elements[atomicNumber - 1];
    }

    /// <summary>
    /// Get element by symbol (e.g., "H", "He", "Li")
    /// </summary>
    public static PeriodicElementInfo GetElement(string symbol)
    {
        foreach (var element in Elements)
        {
            if (element.Symbol == symbol)
                return element;
        }
        return null;
    }
}

/// <summary>
/// Data structure for periodic element information
/// </summary>
[System.Serializable]
public class PeriodicElementInfo
{
    public int AtomicNumber;
    public string Symbol;
    public string Name;
    public float AtomicMass;
    public int ValenceElectrons;
    public float Electronegativity;
    public ElementFamily Family;
    public int Period;      // Row (1-7)
    public int Group;       // Column (1-18)
    public Color ElementColor;

    public PeriodicElementInfo(int atomicNumber, string symbol, string name, float atomicMass,
        int valenceElectrons, float electronegativity, ElementFamily family, int period, int group, Color color)
    {
        AtomicNumber = atomicNumber;
        Symbol = symbol;
        Name = name;
        AtomicMass = atomicMass;
        ValenceElectrons = valenceElectrons;
        Electronegativity = electronegativity;
        Family = family;
        Period = period;
        Group = group;
        ElementColor = color;
    }

    /// <summary>
    /// Calculate game HP from atomic mass
    /// </summary>
    public float GetGameHP(float multiplier = 10f)
    {
        return AtomicMass * multiplier;
    }

    /// <summary>
    /// Get electrons needed for full shell (Duet for H/He, Octet for others)
    /// </summary>
    public int GetElectronsToFullShell()
    {
        if (Family == ElementFamily.NobleGas) return 0;
        if (AtomicNumber <= 2) return 2 - ValenceElectrons; // Duet rule
        return 8 - ValenceElectrons; // Octet rule
    }
}

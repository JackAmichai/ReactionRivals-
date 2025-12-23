/*
 * CHEMISTRY REFERENCE DATA FOR REACTION RIVALS
 * =============================================
 * This file contains accurate periodic table data used in the game.
 * All values are based on real chemistry for educational accuracy.
 * 
 * HP = Atomic Mass (scaled for gameplay)
 * Mana = Valence Electrons (Octet/Duet Rule)
 * Range = Atomic Radius (larger = longer range)
 * 
 * PERIODIC TABLE DATA (MVP Elements)
 * ==================================
 * 
 * HYDROGEN (H)
 * - Atomic Number: 1
 * - Atomic Mass: 1.008 u
 * - Valence Electrons: 1
 * - Electronegativity: 2.20 (Pauling scale)
 * - Follows DUET RULE (needs 2 electrons, not 8)
 * - Most abundant element in universe
 * 
 * HELIUM (He)
 * - Atomic Number: 2
 * - Atomic Mass: 4.003 u
 * - Valence Electrons: 2 (FULL SHELL - Noble Gas)
 * - Electronegativity: N/A (Noble gases don't bond)
 * - Second most abundant element in universe
 * 
 * CARBON (C)
 * - Atomic Number: 6
 * - Atomic Mass: 12.011 u
 * - Valence Electrons: 4
 * - Electronegativity: 2.55
 * - Forms 4 bonds (tetravalent) - backbone of organic chemistry
 * 
 * NITROGEN (N)
 * - Atomic Number: 7
 * - Atomic Mass: 14.007 u
 * - Valence Electrons: 5
 * - Electronegativity: 3.04
 * - Forms up to 3 bonds typically
 * 
 * OXYGEN (O)
 * - Atomic Number: 8
 * - Atomic Mass: 15.999 u
 * - Valence Electrons: 6
 * - Electronegativity: 3.44 (second highest!)
 * - Forms 2 bonds typically
 * 
 * SODIUM (Na)
 * - Atomic Number: 11
 * - Atomic Mass: 22.990 u
 * - Valence Electrons: 1
 * - Electronegativity: 0.93 (very low - wants to lose electron)
 * - Alkali metal - EXPLODES in water!
 * 
 * CHLORINE (Cl)
 * - Atomic Number: 17
 * - Atomic Mass: 35.45 u
 * - Valence Electrons: 7
 * - Electronegativity: 3.16 (very high - wants to gain electron)
 * - Halogen - highly reactive
 * 
 * IRON (Fe)
 * - Atomic Number: 26
 * - Atomic Mass: 55.845 u
 * - Valence Electrons: 2 (in common Fe²⁺ state)
 * - Electronegativity: 1.83
 * - Transition metal - can have multiple oxidation states
 * 
 * MOLECULE FORMULAS
 * =================
 * 
 * WATER (H₂O)
 * - 2 Hydrogen + 1 Oxygen
 * - Polar covalent bonds
 * - Bond angle: 104.5°
 * - Universal solvent
 * 
 * METHANE (CH₄)
 * - 4 Hydrogen + 1 Carbon
 * - Tetrahedral shape
 * - Bond angle: 109.5°
 * - Simplest alkane, main component of natural gas
 * 
 * CARBON DIOXIDE (CO₂)
 * - 2 Oxygen + 1 Carbon
 * - Linear molecule
 * - Double bonds (O=C=O)
 * - Greenhouse gas
 * 
 * AMMONIA (NH₃)
 * - 3 Hydrogen + 1 Nitrogen
 * - Trigonal pyramidal shape
 * - Bond angle: 107°
 * - Pungent smell, common in cleaning products
 * 
 * SODIUM CHLORIDE (NaCl)
 * - 1 Sodium + 1 Chlorine
 * - IONIC bond (electron transfer, not sharing)
 * - Forms crystal lattice structure
 * - Table salt!
 * 
 * BOND TYPES
 * ==========
 * 
 * COVALENT BONDS
 * - Electrons are SHARED between atoms
 * - Forms molecules (H₂O, CH₄, CO₂, NH₃)
 * - Typically between nonmetals
 * - Game: Units MERGE into one powerful compound
 * 
 * IONIC BONDS
 * - Electrons are TRANSFERRED from one atom to another
 * - Forms crystal structures (NaCl)
 * - Between metals and nonmetals
 * - Game: Units gain CRYSTAL ARMOR buff
 * 
 * METALLIC BONDS
 * - Electrons form a "sea" shared by all metal atoms
 * - Allows electrical conductivity
 * - Game: Units SHARE DAMAGE across the pool
 * 
 * ELECTRON RULES
 * ==============
 * 
 * OCTET RULE
 * - Most atoms want 8 electrons in their outer shell
 * - Applies to: C, N, O, Na, Cl, etc.
 * - Exception: Noble gases already have 8
 * 
 * DUET RULE
 * - Hydrogen and Helium only need 2 electrons
 * - First electron shell only holds 2 electrons
 * - H has 1 valence electron, wants to gain 1 more
 * - He has 2 valence electrons, already full!
 * 
 * ELECTRONEGATIVITY
 * - Measure of how strongly an atom attracts electrons
 * - Fluorine is highest (3.98), Francium is lowest (0.7)
 * - High electronegativity = electron thief (Halogens)
 * - Low electronegativity = electron donor (Alkali metals)
 * 
 * ELEMENT FAMILIES (Groups)
 * =========================
 * 
 * ALKALI METALS (Group 1): Li, Na, K, Rb, Cs, Fr
 * - 1 valence electron
 * - Very reactive, especially with water
 * - Reactivity INCREASES down the group (K > Na > Li)
 * - Game trait: Explode on death near water
 * 
 * ALKALINE EARTH METALS (Group 2): Be, Mg, Ca, Sr, Ba, Ra
 * - 2 valence electrons
 * - Form strong structural compounds (bones = Ca)
 * - Game trait: Defensive, extra armor
 * 
 * HALOGENS (Group 17): F, Cl, Br, I, At
 * - 7 valence electrons (need 1 more for full shell)
 * - Highest electronegativity = electron thieves
 * - Game trait: Steal electrons from enemies
 * 
 * NOBLE GASES (Group 18): He, Ne, Ar, Kr, Xe, Rn
 * - Full outer shell (2 for He, 8 for others)
 * - Chemically inert (don't react)
 * - Game trait: Immune to abilities/spells
 * 
 * TRANSITION METALS (Groups 3-12): Fe, Cu, Zn, Ag, Au, etc.
 * - Can have multiple oxidation states
 * - Good conductors of electricity
 * - Game trait: Metallic bonding, damage sharing
 * 
 * NONMETALS: C, N, O, P, S, Se
 * - Form covalent bonds with each other
 * - Building blocks of life
 * - Game trait: Core elements for molecules
 */

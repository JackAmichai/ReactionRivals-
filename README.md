# Reaction Rivals - Chemistry Auto-Battler

A Unity-based **educational** Auto-Battler game where chemical elements are your units and chemical bonds are your synergies! Learn real chemistry while playing!

## 🎮 Game Overview

**Reaction Rivals** maps Auto-Battler mechanics onto Chemistry:
- **Draft Units** → Buy Elements from the periodic table
- **Combine Units** → Form Molecules through chemical bonding
- **Synergies** → Element families (Noble Gases, Halogens, etc.)
- **Learn** → Discover history and science behind each element!

## 🎓 Educational Features

### 📚 Level-Up Education System
Each time you level up, you'll learn about the newly unlocked elements:
- **Discovery History**: Who discovered it, when, and where
- **Historical Significance**: How it changed human history
- **Modern Applications**: Real-world uses today
- **Fun Facts**: Memorable tidbits to reinforce learning

### 🧠 Chemistry Quiz Mini-Game
Between rounds, test your knowledge:
- Multiple question types (symbols, atomic numbers, families, discoveries)
- Earn bonus ATP (energy) for correct answers!
- Streak bonuses reward consistent learning
- Questions adapt to your unlocked elements

### 📖 Interactive Glossary
Access definitions of chemistry terms:
- Terms unlock as you progress through levels
- Categories: Atomic Structure, Bonding, Periodic Table, Reactivity
- Real-world examples and fun facts for each term

### 🔬 Element History Database
Learn about every element's story:
- Discovery dates and discoverers (historically accurate)
- Significance in human history
- Modern applications and uses
- Fun facts that make chemistry memorable

## 🚀 Quick Start

### Prerequisites
- Unity 2022.3+ (LTS)
- TextMeshPro package (usually included)

### Setup Steps

1. **Create New Unity 2D Project**
   - File → New Project → 2D (Core)

2. **Import Scripts**
   - Copy all files from `Assets/Scripts/` to your Unity project

3. **Generate MVP Data**
   - In Unity: `Tools → Reaction Rivals → Generate MVP Data`
   - This creates the "Life Set" elements: H, C, N, O

4. **Create Game Scene**
   - Create empty GameObject named "Bootstrap"
   - Add `GameBootstrap` component
   - Assign generated Element and Molecule data

5. **Create Hex Cell Prefab**
   - Create 2D sprite (hexagon shape)
   - Add `HexCell` component
   - Save as prefab

6. **Play!**

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── Data/               # ScriptableObject definitions
│   │   ├── ElementData.cs
│   │   ├── ElementFamily.cs
│   │   ├── ElementHistory.cs     # Discovery & history data
│   │   ├── LevelEducation.cs     # Level-up educational content
│   │   ├── PeriodicTable.cs      # Full 118 elements
│   │   ├── LevelElementProgression.cs
│   │   ├── MoleculeRecipe.cs
│   │   └── AbilityData.cs
│   ├── Education/          # Educational systems
│   │   ├── ChemistryQuiz.cs      # Quiz question generator
│   │   └── ChemistryGlossary.cs  # Term definitions
│   ├── Grid/               # Hex grid system
│   │   ├── HexCell.cs
│   │   └── HexGrid.cs
│   ├── Units/              # Unit logic
│   │   ├── Unit.cs
│   │   ├── UnitCombat.cs
│   │   └── UnitVisuals.cs
│   ├── Bonding/            # Molecule formation
│   │   ├── BondingManager.cs
│   │   └── Molecule.cs
│   ├── Managers/           # Game systems
│   │   ├── GameManager.cs
│   │   └── ShopManager.cs
│   ├── UI/                 # User interface
│   │   ├── UIManager.cs
│   │   ├── ShopSlot.cs
│   │   ├── PeriodicTableUI.cs    # Full table display
│   │   ├── PeriodicTableCell.cs
│   │   ├── PeriodicTableLegend.cs
│   │   ├── ElementTooltip.cs
│   │   ├── LevelUpEducationUI.cs # Level-up learning screen
│   │   ├── ChemistryQuizUI.cs    # Quiz interface
│   │   └── GlossaryUI.cs         # Glossary browser
│   ├── Core/               # Bootstrap
│   │   └── GameBootstrap.cs
│   └── Editor/             # Editor tools
│       ├── MVPDataGenerator.cs
│       └── PeriodicTableEditor.cs
├── Data/                   # Generated ScriptableObjects
│   ├── Elements/
│   └── Molecules/
└── Prefabs/
    ├── HexCell.prefab
    ├── Unit.prefab
    └── ElementCell.prefab
```

## ⚗️ MVP Elements (The "Life Set")

| Element | Symbol | Cost | Family | HP | Damage | Valence |
|---------|--------|------|--------|-----|--------|---------|
| Hydrogen | H | 1 | Hydrogen | 50 | 15 | 1 |
| Carbon | C | 2 | NonMetal | 120 | 20 | 4 |
| Nitrogen | N | 2 | NonMetal | 100 | 18 | 5 |
| Oxygen | O | 1 | NonMetal | 80 | 25 | 6 |

## 🧪 MVP Molecules

| Molecule | Formula | Recipe | Effect |
|----------|---------|--------|--------|
| Water | H₂O | O + 2H | Heals allies |
| Methane | CH₄ | C + 4H | AoE poison |
| Carbon Dioxide | CO₂ | C + 2O | Slows enemies |
| Ammonia | NH₃ | N + 3H | Cleanses debuffs |

## 🎲 Core Mechanics

### Bonding System
Units must be **physically adjacent** on the hex grid to bond:
- **Covalent**: Units merge into one powerful compound
- **Ionic**: Units gain defensive buffs (Crystal Armor)
- **Metallic**: Metal units share a damage pool

### Octet Rule (Mana System)
- Units start with their valence electrons
- Each attack grants +1 electron
- At 8 electrons → Cast Ultimate ability
- Resets to base valence after casting

### Element Families (Traits)
- **Noble Gases**: Spell immune
- **Halogens**: Steal electrons from enemies
- **Alkali Metals**: Explode on death near water

## 🛠️ Extending the Game

### Adding New Elements
1. Create new `ElementData` ScriptableObject
2. Set properties based on periodic table
3. Add to `ShopManager.ElementPool`

### Adding New Molecules
1. Create new `MoleculeRecipe` ScriptableObject
2. Set core element and requirements
3. Add to `BondingManager.AllRecipes`

## 🧪 Full Periodic Table System

The game includes all 118 elements from the periodic table!

### Periodic Table UI
- **Press Tab** or click the **Periodic Table button** to view the full table
- Elements are color-coded by family (Alkali, Noble Gas, Halogen, etc.)
- **Highlighting shows your progress:**
  - 🔒 **Dark/Dim** - Locked (not available at your level)
  - ⬜ **Normal** - Unlocked (available in shop)
  - 🟢 **Green Border** - Owned (you have this element)
  - 🟡 **Gold Border** - In Molecule (part of an active compound)

### Level Progression
Elements unlock as you level up:

| Level | Elements Unlocked |
|-------|------------------|
| 1 | H, C, N, O (Basics of Life) |
| 2 | He, Na, Cl, Fe |
| 3 | Li, Be, B, F, Ne, Mg |
| 4 | Al, Si, P, S, Ar |
| 5+ | More elements per level... |
| 22 | All 118 elements! |

### Element Families
| Family | Elements | Game Effect |
|--------|----------|-------------|
| Hydrogen | H | Versatile bonding |
| Alkali | Li, Na, K, Rb, Cs, Fr | Explosive on death |
| Noble Gas | He, Ne, Ar, Kr, Xe, Rn, Og | Spell immune |
| Halogens | F, Cl, Br, I, At, Ts | Electron stealing |
| Transition Metals | Fe, Cu, Au, Ag, etc. | Metallic bonding |
| Lanthanides | La-Lu (57-71) | Rare earth powers |
| Actinides | Ac-Lr (89-103) | Radioactive abilities |

### Editor Tools
- **ReactionRivals → Create Level Progression Asset**: Generate default element unlock schedule
- **ReactionRivals → Generate All Element ScriptableObjects**: Create data for all 118 elements
- **ReactionRivals → Print Periodic Table Stats**: Debug info about elements

## 📚 Educational Features

### 🎓 Level-Up Learning
Every level-up shows educational content about newly unlocked elements:
- **Discovery History**: Who discovered each element and when
- **Historical Significance**: Why the element matters to science
- **Modern Uses**: Real-world applications
- **Fun Facts**: Engaging trivia to spark curiosity

Example Level-Up Content:
> **Level 1: The Building Blocks of Life**
> You've unlocked Hydrogen, Carbon, Nitrogen, and Oxygen!
> *Did you know? These four elements make up 96% of your body mass!*

### 📖 Element History Database
Over 40 elements have detailed historical information:

| Element | Discovered | Discoverer | Fun Fact |
|---------|------------|------------|----------|
| Hydrogen | 1766 | Henry Cavendish | Powers the Sun through nuclear fusion |
| Oxygen | 1774 | Joseph Priestley | Makes up 21% of Earth's atmosphere |
| Carbon | Ancient | Unknown | Can form more compounds than all other elements combined |
| Gold | ~6000 BCE | Unknown | All the gold ever mined would fit in a 21-meter cube |
| Uranium | 1789 | Martin Klaproth | Named after the planet Uranus |

### 🧠 Chemistry Quiz Mini-Game
Test your knowledge with 10 different question types:
- **Symbol to Name**: What element is "Fe"?
- **Name to Symbol**: What's the symbol for Gold?
- **Atomic Number**: Which element is #6?
- **Valence Electrons**: How many bonds can Oxygen form?
- **Element Family**: What family is Neon in?
- **Discovery Questions**: Who discovered Oxygen?
- **True/False**: "Helium is a Noble Gas" - True or False?
- **Bonding Types**: What bond forms between Na and Cl?
- **Molecule Formulas**: What's the formula for Water?
- **Real World Uses**: What is Helium commonly used for?

**Quiz Rewards:**
- Earn bonus ATP for correct answers
- Build streaks for multiplier bonuses
- Questions adapt to your unlocked elements

### 📚 Chemistry Glossary
Access 40+ chemistry terms with clear definitions:

**Categories:**
- Atomic Structure (Proton, Neutron, Electron, Nucleus, etc.)
- Chemical Bonding (Covalent, Ionic, Metallic, Valence)
- Periodic Table (Period, Group, Atomic Number)
- Element Families (Noble Gas, Halogen, Alkali Metal)
- Molecules & Compounds (Formula, Compound, Molecular)
- Reactivity (Electronegativity, Oxidation, Reduction)

**Example Entry:**
> **Octet Rule**
> *Category: Chemical Bonding*
> Most atoms want 8 electrons in their outer shell to be stable.
> *Fun Fact: Hydrogen only needs 2 electrons (duet rule) because it only has one shell!*

### 🎮 Natural Learning Through Gameplay
Players naturally learn:
- **Valency**: "Carbon needs 4 bonds"
- **Reactivity**: "Potassium explodes more than Lithium"
- **Formulas**: "Water is H₂O" through gameplay
- **Discovery History**: Element stories during level-ups
- **Scientific Method**: Experimenting with different combinations

## 🎯 Next Steps

1. **Add More Elements** - Expand beyond the Life Set
2. **Visual Polish** - Particle effects, animations
3. **Balance Tuning** - Adjust stats and costs
4. **Audio** - Sound effects and music
5. **Enemy AI** - Smart unit placement
6. **Multiplayer** - PvP mode

## 📝 License

MIT License - Use freely for educational purposes!

---

*"Reaction Rivals" - Where Chemistry becomes fun!*

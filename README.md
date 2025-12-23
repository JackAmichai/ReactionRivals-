# Reaction Rivals - Chemistry Auto-Battler

A Unity-based Auto-Battler game where chemical elements are your units and chemical bonds are your synergies!

## 🎮 Game Overview

**Reaction Rivals** maps Auto-Battler mechanics onto Chemistry:
- **Draft Units** → Buy Elements from the periodic table
- **Combine Units** → Form Molecules through chemical bonding
- **Synergies** → Element families (Noble Gases, Halogens, etc.)

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
│   │   ├── MoleculeRecipe.cs
│   │   └── AbilityData.cs
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
│   │   └── ShopSlot.cs
│   ├── Core/               # Bootstrap
│   │   └── GameBootstrap.cs
│   └── Editor/             # Editor tools
│       └── MVPDataGenerator.cs
├── Data/                   # Generated ScriptableObjects
│   ├── Elements/
│   └── Molecules/
└── Prefabs/
    ├── HexCell.prefab
    └── Unit.prefab
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

## 📚 Educational Value

Players naturally learn:
- **Valency**: "Carbon needs 4 bonds"
- **Reactivity**: "Potassium explodes more than Lithium"
- **Formulas**: "Water is H₂O" through gameplay

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

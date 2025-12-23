# 🧪 Jack's TODO List - Reaction Rivals Setup

## ✅ Completed
- [x] All 35 C# scripts created
- [x] Full periodic table (118 elements) implemented
- [x] Chemistry data validated (atomic numbers, valence electrons, electronegativity)
- [x] Data validators and gameplay tests created
- [x] Pushed to GitHub

---

## 🚀 Getting the App Running

### Step 1: Install Unity
- [ ] Download **Unity Hub** from [unity.com/download](https://unity.com/download)
- [ ] Install **Unity 2022.3 LTS** (or newer) through Unity Hub

---

### Step 2: Open the Project
- [ ] Open Unity Hub → **Open** → Browse to:
  ```
  c:\Users\yamichai\OneDrive - Deloitte (O365D)\Documents\General\temp\ReactionRivals
  ```
- [ ] Wait for Unity to import all assets and compile scripts

---

### Step 3: Generate Game Data
In Unity Editor:
- [ ] Go to menu: **Tools → Reaction Rivals → Generate MVP Data**
- [ ] Verify folders created:
  - `Assets/Data/Elements/` - 8 element ScriptableObjects (H, C, N, O, Na, Cl, Fe, He)
  - `Assets/Data/Molecules/` - 5 molecule recipes (Water, Methane, CO₂, Ammonia, Salt)

---

### Step 4: Run Data Validation
- [ ] Go to menu: **Tools → Reaction Rivals → Validate All Data**
- [ ] Go to menu: **Tools → Reaction Rivals → Run All Tests**
- [ ] Verify console shows: `✅ All data validated successfully!`

---

### Step 5: Create Prefabs

#### A) Hex Cell Prefab
- [ ] **GameObject → 2D Object → Sprites → Hexagon Flat-Top**
- [ ] Add component: `HexCell`
- [ ] Save as `Assets/Prefabs/HexCell.prefab`

#### B) Unit Prefab
- [ ] **GameObject → 2D Object → Sprites → Circle**
- [ ] Scale to `(0.5, 0.5, 1)`
- [ ] Add components:
  - `Unit`
  - `UnitCombat`
  - `UnitVisuals`
  - `BoxCollider2D`
- [ ] Save as `Assets/Prefabs/Unit.prefab`

---

### Step 6: Set Up the Scene
- [ ] Create empty GameObject named `Bootstrap`
- [ ] Add component: `GameBootstrap`
- [ ] In the Inspector, assign:

| Field | What to Assign |
|-------|----------------|
| **Hex Cell Prefab** | `Assets/Prefabs/HexCell.prefab` |
| **Unit Prefab** | `Assets/Prefabs/Unit.prefab` |
| **Starting Elements** | All 8 elements from `Assets/Data/Elements/` |
| **All Recipes** | All 5 molecules from `Assets/Data/Molecules/` |

---

### Step 7: Create UI Canvas
- [ ] **GameObject → UI → Canvas**
- [ ] Add component: `UIManager` to Canvas
- [ ] Create child panels:
  - `ShopPanel` (bottom)
  - `BenchPanel` (below grid)
  - `InfoPanel` (top right)

---

### Step 8: Play!
- [ ] Press **Play** ▶️
- [ ] Verify console shows: `🎮 Game Bootstrap complete!`

---

## 📁 Required File Structure

```
Assets/
├── Scripts/           ← ✅ Already done (35 files)
├── Data/
│   ├── Elements/      ← Generate via menu
│   └── Molecules/     ← Generate via menu
├── Prefabs/
│   ├── HexCell.prefab ← Create manually
│   └── Unit.prefab    ← Create manually
└── Scenes/
    └── Main.unity     ← Your scene
```

---

## 🧪 Quick Test Commands

After setup, right-click on `Bootstrap` in Hierarchy:
- **Spawn Test Hydrogen** - Creates a hydrogen unit
- **Spawn Water Setup (1O + 2H)** - Creates bonding test

---

## ⚠️ Troubleshooting

| Problem | Solution |
|---------|----------|
| "Missing script" errors | Make sure all `.cs` files compiled (check Console) |
| "ElementFamily not found" | Ensure `ElementFamily.cs` exists in Scripts/Data/ |
| Menu items don't appear | Wait for Unity to compile, check Console for errors |
| Prefabs missing references | Reassign prefabs in Bootstrap Inspector |

---

## 🔗 Resources

- **GitHub Repo**: https://github.com/JackAmichai/ReactionRivals-.git
- **Unity Download**: https://unity.com/download

---

## 📝 Notes

_Add your notes here as you work through the setup..._


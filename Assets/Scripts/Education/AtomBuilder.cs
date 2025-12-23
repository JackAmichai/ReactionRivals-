using System.Collections.Generic;
using UnityEngine;

namespace ReactionRivals
{
    /// <summary>
    /// Lightning Round: Build the Atom mini-game
    /// Players must place the correct number of protons, neutrons, and electrons
    /// to construct each element from the periodic table.
    /// </summary>
    public class AtomBuilder : MonoBehaviour
    {
        public static AtomBuilder Instance { get; private set; }

        [Header("Game State")]
        [SerializeField] private int currentElementIndex = 0;
        [SerializeField] private int totalElementsCompleted = 0;
        [SerializeField] private float timePerElement = 30f;
        [SerializeField] private float currentTime;
        [SerializeField] private bool isPlaying = false;

        [Header("Player Input")]
        [SerializeField] private int placedProtons = 0;
        [SerializeField] private int placedNeutrons = 0;
        [SerializeField] private int placedElectrons = 0;

        [Header("Scoring")]
        [SerializeField] private int score = 0;
        [SerializeField] private int streak = 0;
        [SerializeField] private int bestStreak = 0;
        [SerializeField] private float perfectTimeBonus = 10f;

        // All elements in order (atomic number 1-118)
        private List<ElementBuildData> allElements;

        // Current target element
        public ElementBuildData CurrentElement => 
            currentElementIndex < allElements.Count ? allElements[currentElementIndex] : null;

        public int CurrentLevel => currentElementIndex + 1;
        public int TotalLevels => allElements?.Count ?? 118;
        public int Score => score;
        public int Streak => streak;
        public int BestStreak => bestStreak;
        public float TimeRemaining => currentTime;
        public float TimeLimit => timePerElement;
        public bool IsPlaying => isPlaying;
        public int PlacedProtons => placedProtons;
        public int PlacedNeutrons => placedNeutrons;
        public int PlacedElectrons => placedElectrons;

        // Events
        public event System.Action<ElementBuildData> OnElementStarted;
        public event System.Action<bool, int> OnElementCompleted; // success, points earned
        public event System.Action OnGameCompleted;
        public event System.Action<string> OnFeedback;
        public event System.Action OnTimeUp;
        public event System.Action OnParticleAdded;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                InitializeElements();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (isPlaying)
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0)
                {
                    TimeUp();
                }
            }
        }

        private void InitializeElements()
        {
            allElements = new List<ElementBuildData>();

            // All 118 elements with their atomic data
            // Format: Symbol, Name, Atomic Number (protons), Neutrons (most common isotope), Electrons
            AddElement("H", "Hydrogen", 1, 0, 1);
            AddElement("He", "Helium", 2, 2, 2);
            AddElement("Li", "Lithium", 3, 4, 3);
            AddElement("Be", "Beryllium", 4, 5, 4);
            AddElement("B", "Boron", 5, 6, 5);
            AddElement("C", "Carbon", 6, 6, 6);
            AddElement("N", "Nitrogen", 7, 7, 7);
            AddElement("O", "Oxygen", 8, 8, 8);
            AddElement("F", "Fluorine", 9, 10, 9);
            AddElement("Ne", "Neon", 10, 10, 10);
            
            // Period 3
            AddElement("Na", "Sodium", 11, 12, 11);
            AddElement("Mg", "Magnesium", 12, 12, 12);
            AddElement("Al", "Aluminum", 13, 14, 13);
            AddElement("Si", "Silicon", 14, 14, 14);
            AddElement("P", "Phosphorus", 15, 16, 15);
            AddElement("S", "Sulfur", 16, 16, 16);
            AddElement("Cl", "Chlorine", 17, 18, 17);
            AddElement("Ar", "Argon", 18, 22, 18);
            
            // Period 4
            AddElement("K", "Potassium", 19, 20, 19);
            AddElement("Ca", "Calcium", 20, 20, 20);
            AddElement("Sc", "Scandium", 21, 24, 21);
            AddElement("Ti", "Titanium", 22, 26, 22);
            AddElement("V", "Vanadium", 23, 28, 23);
            AddElement("Cr", "Chromium", 24, 28, 24);
            AddElement("Mn", "Manganese", 25, 30, 25);
            AddElement("Fe", "Iron", 26, 30, 26);
            AddElement("Co", "Cobalt", 27, 32, 27);
            AddElement("Ni", "Nickel", 28, 31, 28);
            AddElement("Cu", "Copper", 29, 35, 29);
            AddElement("Zn", "Zinc", 30, 35, 30);
            AddElement("Ga", "Gallium", 31, 39, 31);
            AddElement("Ge", "Germanium", 32, 41, 32);
            AddElement("As", "Arsenic", 33, 42, 33);
            AddElement("Se", "Selenium", 34, 45, 34);
            AddElement("Br", "Bromine", 35, 45, 35);
            AddElement("Kr", "Krypton", 36, 48, 36);
            
            // Period 5
            AddElement("Rb", "Rubidium", 37, 48, 37);
            AddElement("Sr", "Strontium", 38, 50, 38);
            AddElement("Y", "Yttrium", 39, 50, 39);
            AddElement("Zr", "Zirconium", 40, 51, 40);
            AddElement("Nb", "Niobium", 41, 52, 41);
            AddElement("Mo", "Molybdenum", 42, 54, 42);
            AddElement("Tc", "Technetium", 43, 55, 43);
            AddElement("Ru", "Ruthenium", 44, 57, 44);
            AddElement("Rh", "Rhodium", 45, 58, 45);
            AddElement("Pd", "Palladium", 46, 60, 46);
            AddElement("Ag", "Silver", 47, 61, 47);
            AddElement("Cd", "Cadmium", 48, 64, 48);
            AddElement("In", "Indium", 49, 66, 49);
            AddElement("Sn", "Tin", 50, 69, 50);
            AddElement("Sb", "Antimony", 51, 71, 51);
            AddElement("Te", "Tellurium", 52, 76, 52);
            AddElement("I", "Iodine", 53, 74, 53);
            AddElement("Xe", "Xenon", 54, 77, 54);
            
            // Period 6
            AddElement("Cs", "Cesium", 55, 78, 55);
            AddElement("Ba", "Barium", 56, 81, 56);
            AddElement("La", "Lanthanum", 57, 82, 57);
            AddElement("Ce", "Cerium", 58, 82, 58);
            AddElement("Pr", "Praseodymium", 59, 82, 59);
            AddElement("Nd", "Neodymium", 60, 84, 60);
            AddElement("Pm", "Promethium", 61, 84, 61);
            AddElement("Sm", "Samarium", 62, 88, 62);
            AddElement("Eu", "Europium", 63, 89, 63);
            AddElement("Gd", "Gadolinium", 64, 93, 64);
            AddElement("Tb", "Terbium", 65, 94, 65);
            AddElement("Dy", "Dysprosium", 66, 97, 66);
            AddElement("Ho", "Holmium", 67, 98, 67);
            AddElement("Er", "Erbium", 68, 99, 68);
            AddElement("Tm", "Thulium", 69, 100, 69);
            AddElement("Yb", "Ytterbium", 70, 103, 70);
            AddElement("Lu", "Lutetium", 71, 104, 71);
            AddElement("Hf", "Hafnium", 72, 106, 72);
            AddElement("Ta", "Tantalum", 73, 108, 73);
            AddElement("W", "Tungsten", 74, 110, 74);
            AddElement("Re", "Rhenium", 75, 111, 75);
            AddElement("Os", "Osmium", 76, 114, 76);
            AddElement("Ir", "Iridium", 77, 115, 77);
            AddElement("Pt", "Platinum", 78, 117, 78);
            AddElement("Au", "Gold", 79, 118, 79);
            AddElement("Hg", "Mercury", 80, 121, 80);
            AddElement("Tl", "Thallium", 81, 123, 81);
            AddElement("Pb", "Lead", 82, 125, 82);
            AddElement("Bi", "Bismuth", 83, 126, 83);
            AddElement("Po", "Polonium", 84, 125, 84);
            AddElement("At", "Astatine", 85, 125, 85);
            AddElement("Rn", "Radon", 86, 136, 86);
            
            // Period 7
            AddElement("Fr", "Francium", 87, 136, 87);
            AddElement("Ra", "Radium", 88, 138, 88);
            AddElement("Ac", "Actinium", 89, 138, 89);
            AddElement("Th", "Thorium", 90, 142, 90);
            AddElement("Pa", "Protactinium", 91, 140, 91);
            AddElement("U", "Uranium", 92, 146, 92);
            AddElement("Np", "Neptunium", 93, 144, 93);
            AddElement("Pu", "Plutonium", 94, 150, 94);
            AddElement("Am", "Americium", 95, 148, 95);
            AddElement("Cm", "Curium", 96, 151, 96);
            AddElement("Bk", "Berkelium", 97, 150, 97);
            AddElement("Cf", "Californium", 98, 153, 98);
            AddElement("Es", "Einsteinium", 99, 153, 99);
            AddElement("Fm", "Fermium", 100, 157, 100);
            AddElement("Md", "Mendelevium", 101, 157, 101);
            AddElement("No", "Nobelium", 102, 157, 102);
            AddElement("Lr", "Lawrencium", 103, 159, 103);
            AddElement("Rf", "Rutherfordium", 104, 163, 104);
            AddElement("Db", "Dubnium", 105, 163, 105);
            AddElement("Sg", "Seaborgium", 106, 163, 106);
            AddElement("Bh", "Bohrium", 107, 163, 107);
            AddElement("Hs", "Hassium", 108, 169, 108);
            AddElement("Mt", "Meitnerium", 109, 169, 109);
            AddElement("Ds", "Darmstadtium", 110, 171, 110);
            AddElement("Rg", "Roentgenium", 111, 171, 111);
            AddElement("Cn", "Copernicium", 112, 173, 112);
            AddElement("Nh", "Nihonium", 113, 173, 113);
            AddElement("Fl", "Flerovium", 114, 175, 114);
            AddElement("Mc", "Moscovium", 115, 175, 115);
            AddElement("Lv", "Livermorium", 116, 177, 116);
            AddElement("Ts", "Tennessine", 117, 177, 117);
            AddElement("Og", "Oganesson", 118, 176, 118);
        }

        private void AddElement(string symbol, string name, int protons, int neutrons, int electrons)
        {
            allElements.Add(new ElementBuildData
            {
                Symbol = symbol,
                Name = name,
                AtomicNumber = protons,
                Protons = protons,
                Neutrons = neutrons,
                Electrons = electrons,
                ElectronShells = CalculateElectronShells(electrons)
            });
        }

        private int[] CalculateElectronShells(int totalElectrons)
        {
            // Simplified electron shell distribution (2, 8, 18, 32, 32, 18, 8)
            int[] maxPerShell = { 2, 8, 18, 32, 32, 18, 8 };
            List<int> shells = new List<int>();
            int remaining = totalElectrons;

            for (int i = 0; i < maxPerShell.Length && remaining > 0; i++)
            {
                int inThisShell = Mathf.Min(remaining, maxPerShell[i]);
                shells.Add(inThisShell);
                remaining -= inThisShell;
            }

            return shells.ToArray();
        }

        public void StartGame()
        {
            currentElementIndex = 0;
            totalElementsCompleted = 0;
            score = 0;
            streak = 0;
            bestStreak = 0;
            StartNextElement();
        }

        public void StartFromElement(int elementIndex)
        {
            currentElementIndex = Mathf.Clamp(elementIndex, 0, allElements.Count - 1);
            totalElementsCompleted = elementIndex;
            score = 0;
            streak = 0;
            StartNextElement();
        }

        private void StartNextElement()
        {
            if (currentElementIndex >= allElements.Count)
            {
                CompleteGame();
                return;
            }

            placedProtons = 0;
            placedNeutrons = 0;
            placedElectrons = 0;
            currentTime = timePerElement;
            isPlaying = true;

            OnElementStarted?.Invoke(CurrentElement);
        }

        public void AddProton()
        {
            if (!isPlaying) return;
            placedProtons++;
            OnParticleAdded?.Invoke();
            CheckAutoSubmit();
        }

        public void RemoveProton()
        {
            if (!isPlaying || placedProtons <= 0) return;
            placedProtons--;
            OnParticleAdded?.Invoke();
        }

        public void AddNeutron()
        {
            if (!isPlaying) return;
            placedNeutrons++;
            OnParticleAdded?.Invoke();
            CheckAutoSubmit();
        }

        public void RemoveNeutron()
        {
            if (!isPlaying || placedNeutrons <= 0) return;
            placedNeutrons--;
            OnParticleAdded?.Invoke();
        }

        public void AddElectron()
        {
            if (!isPlaying) return;
            placedElectrons++;
            OnParticleAdded?.Invoke();
            CheckAutoSubmit();
        }

        public void RemoveElectron()
        {
            if (!isPlaying || placedElectrons <= 0) return;
            placedElectrons--;
            OnParticleAdded?.Invoke();
        }

        private void CheckAutoSubmit()
        {
            // Auto-submit when all particles match (optional feature)
            if (placedProtons == CurrentElement.Protons &&
                placedNeutrons == CurrentElement.Neutrons &&
                placedElectrons == CurrentElement.Electrons)
            {
                // Small delay before auto-submit for visual feedback
                // Could be instant or have a confirm button instead
            }
        }

        public void SubmitAtom()
        {
            if (!isPlaying || CurrentElement == null) return;

            isPlaying = false;
            bool protonsCorrect = placedProtons == CurrentElement.Protons;
            bool neutronsCorrect = placedNeutrons == CurrentElement.Neutrons;
            bool electronsCorrect = placedElectrons == CurrentElement.Electrons;

            bool success = protonsCorrect && neutronsCorrect && electronsCorrect;
            int pointsEarned = 0;

            if (success)
            {
                streak++;
                if (streak > bestStreak) bestStreak = streak;

                // Base points + time bonus + streak bonus
                int basePoints = 100;
                int timeBonus = Mathf.RoundToInt(currentTime * perfectTimeBonus);
                int streakBonus = (streak - 1) * 25;
                pointsEarned = basePoints + timeBonus + streakBonus;
                score += pointsEarned;

                string feedback = $"✓ Correct! {CurrentElement.Name} built!";
                if (streak > 1) feedback += $" ({streak} streak!)";
                OnFeedback?.Invoke(feedback);
            }
            else
            {
                streak = 0;
                
                // Detailed feedback
                string feedback = "✗ Not quite! ";
                if (!protonsCorrect)
                    feedback += $"Protons: {placedProtons} (need {CurrentElement.Protons}). ";
                if (!neutronsCorrect)
                    feedback += $"Neutrons: {placedNeutrons} (need {CurrentElement.Neutrons}). ";
                if (!electronsCorrect)
                    feedback += $"Electrons: {placedElectrons} (need {CurrentElement.Electrons}).";
                
                OnFeedback?.Invoke(feedback);
            }

            OnElementCompleted?.Invoke(success, pointsEarned);

            if (success)
            {
                totalElementsCompleted++;
                currentElementIndex++;
            }
        }

        public void SkipElement()
        {
            if (!isPlaying) return;
            
            isPlaying = false;
            streak = 0;
            OnFeedback?.Invoke($"Skipped {CurrentElement.Name}. Keep trying!");
            OnElementCompleted?.Invoke(false, 0);
        }

        public void ContinueToNext()
        {
            StartNextElement();
        }

        public void RetryCurrentElement()
        {
            StartNextElement(); // Same element since index didn't change on failure
        }

        private void TimeUp()
        {
            isPlaying = false;
            streak = 0;
            OnTimeUp?.Invoke();
            OnFeedback?.Invoke($"⏱ Time's up! {CurrentElement.Name} needs: " +
                $"{CurrentElement.Protons}p, {CurrentElement.Neutrons}n, {CurrentElement.Electrons}e");
            OnElementCompleted?.Invoke(false, 0);
        }

        private void CompleteGame()
        {
            isPlaying = false;
            OnGameCompleted?.Invoke();
            OnFeedback?.Invoke($"🎉 Congratulations! You built all {allElements.Count} elements! " +
                $"Final Score: {score}, Best Streak: {bestStreak}");
        }

        public string GetHint()
        {
            if (CurrentElement == null) return "";

            var hints = new List<string>
            {
                $"Atomic number is {CurrentElement.AtomicNumber}",
                $"Protons = Atomic Number for neutral atoms",
                $"Electrons = Protons for neutral atoms",
                $"Mass Number = Protons + Neutrons",
                $"This element is in period {GetPeriod(CurrentElement.AtomicNumber)}",
                $"Look for {CurrentElement.Symbol} on the periodic table"
            };

            return hints[Random.Range(0, hints.Count)];
        }

        private int GetPeriod(int atomicNumber)
        {
            if (atomicNumber <= 2) return 1;
            if (atomicNumber <= 10) return 2;
            if (atomicNumber <= 18) return 3;
            if (atomicNumber <= 36) return 4;
            if (atomicNumber <= 54) return 5;
            if (atomicNumber <= 86) return 6;
            return 7;
        }

        public ElementBuildData GetElementAt(int index)
        {
            if (index >= 0 && index < allElements.Count)
                return allElements[index];
            return null;
        }

        // Save/Load progress
        public void SaveProgress()
        {
            PlayerPrefs.SetInt("AtomBuilder_CurrentElement", currentElementIndex);
            PlayerPrefs.SetInt("AtomBuilder_TotalCompleted", totalElementsCompleted);
            PlayerPrefs.SetInt("AtomBuilder_Score", score);
            PlayerPrefs.SetInt("AtomBuilder_BestStreak", bestStreak);
            PlayerPrefs.Save();
        }

        public void LoadProgress()
        {
            currentElementIndex = PlayerPrefs.GetInt("AtomBuilder_CurrentElement", 0);
            totalElementsCompleted = PlayerPrefs.GetInt("AtomBuilder_TotalCompleted", 0);
            score = PlayerPrefs.GetInt("AtomBuilder_Score", 0);
            bestStreak = PlayerPrefs.GetInt("AtomBuilder_BestStreak", 0);
        }

        public void ResetProgress()
        {
            PlayerPrefs.DeleteKey("AtomBuilder_CurrentElement");
            PlayerPrefs.DeleteKey("AtomBuilder_TotalCompleted");
            PlayerPrefs.DeleteKey("AtomBuilder_Score");
            PlayerPrefs.DeleteKey("AtomBuilder_BestStreak");
            currentElementIndex = 0;
            totalElementsCompleted = 0;
            score = 0;
            bestStreak = 0;
        }
    }

    /// <summary>
    /// Data structure for each element in the atom builder game
    /// </summary>
    [System.Serializable]
    public class ElementBuildData
    {
        public string Symbol;
        public string Name;
        public int AtomicNumber;
        public int Protons;
        public int Neutrons;
        public int Electrons;
        public int[] ElectronShells; // Electrons per shell (e.g., [2, 8, 1] for Sodium)

        public int MassNumber => Protons + Neutrons;
        
        public string GetElectronConfiguration()
        {
            if (ElectronShells == null || ElectronShells.Length == 0)
                return "N/A";
            
            return string.Join(", ", ElectronShells);
        }
    }
}

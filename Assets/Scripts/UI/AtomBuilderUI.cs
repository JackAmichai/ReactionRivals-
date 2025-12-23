using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace ReactionRivals
{
    /// <summary>
    /// UI for the Lightning Round Atom Builder mini-game.
    /// Players place protons, neutrons, and electrons to build each element.
    /// </summary>
    public class AtomBuilderUI : MonoBehaviour
    {
        [Header("Main Panels")]
        [SerializeField] private GameObject atomBuilderPanel;
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private GameObject victoryPanel;

        [Header("Element Info Display")]
        [SerializeField] private TextMeshProUGUI elementNameText;
        [SerializeField] private TextMeshProUGUI elementSymbolText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI hintText;

        [Header("Timer")]
        [SerializeField] private Slider timerSlider;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Image timerFill;
        [SerializeField] private Color timerNormalColor = Color.green;
        [SerializeField] private Color timerWarningColor = Color.yellow;
        [SerializeField] private Color timerDangerColor = Color.red;

        [Header("Particle Counters")]
        [SerializeField] private TextMeshProUGUI protonCountText;
        [SerializeField] private TextMeshProUGUI neutronCountText;
        [SerializeField] private TextMeshProUGUI electronCountText;

        [Header("Particle Buttons")]
        [SerializeField] private Button addProtonButton;
        [SerializeField] private Button removeProtonButton;
        [SerializeField] private Button addNeutronButton;
        [SerializeField] private Button removeNeutronButton;
        [SerializeField] private Button addElectronButton;
        [SerializeField] private Button removeElectronButton;

        [Header("Action Buttons")]
        [SerializeField] private Button submitButton;
        [SerializeField] private Button skipButton;
        [SerializeField] private Button hintButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button retryButton;

        [Header("Start Panel")]
        [SerializeField] private Button startNewGameButton;
        [SerializeField] private Button continueGameButton;
        [SerializeField] private Button selectLevelButton;
        [SerializeField] private TextMeshProUGUI savedProgressText;

        [Header("Result Panel")]
        [SerializeField] private TextMeshProUGUI resultTitleText;
        [SerializeField] private TextMeshProUGUI resultMessageText;
        [SerializeField] private TextMeshProUGUI resultScoreText;
        [SerializeField] private TextMeshProUGUI resultStreakText;
        [SerializeField] private Image resultElementImage;

        [Header("Victory Panel")]
        [SerializeField] private TextMeshProUGUI victoryScoreText;
        [SerializeField] private TextMeshProUGUI victoryStreakText;
        [SerializeField] private TextMeshProUGUI victoryTimeText;
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button mainMenuButton;

        [Header("Scoring Display")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI streakText;

        [Header("Atom Visualization")]
        [SerializeField] private Transform nucleusContainer;
        [SerializeField] private Transform electronShellContainer;
        [SerializeField] private GameObject protonPrefab;
        [SerializeField] private GameObject neutronPrefab;
        [SerializeField] private GameObject electronPrefab;
        [SerializeField] private GameObject electronShellPrefab;

        [Header("Level Selector")]
        [SerializeField] private GameObject levelSelectorPanel;
        [SerializeField] private Transform levelButtonContainer;
        [SerializeField] private GameObject levelButtonPrefab;

        [Header("Visual Settings")]
        [SerializeField] private Color protonColor = new Color(1f, 0.3f, 0.3f);
        [SerializeField] private Color neutronColor = new Color(0.5f, 0.5f, 0.5f);
        [SerializeField] private Color electronColor = new Color(0.3f, 0.5f, 1f);

        // Visualized particles
        private List<GameObject> visualProtons = new List<GameObject>();
        private List<GameObject> visualNeutrons = new List<GameObject>();
        private List<GameObject> visualElectrons = new List<GameObject>();
        private List<GameObject> electronShells = new List<GameObject>();

        private AtomBuilder atomBuilder;
        private float gameStartTime;
        private int hintsUsed = 0;

        private void Awake()
        {
            // Create AtomBuilder if it doesn't exist
            atomBuilder = FindObjectOfType<AtomBuilder>();
            if (atomBuilder == null)
            {
                GameObject go = new GameObject("AtomBuilder");
                atomBuilder = go.AddComponent<AtomBuilder>();
            }
        }

        private void Start()
        {
            SetupButtons();
            SubscribeToEvents();
            ShowStartPanel();
        }

        private void SetupButtons()
        {
            // Particle buttons
            if (addProtonButton) addProtonButton.onClick.AddListener(() => atomBuilder.AddProton());
            if (removeProtonButton) removeProtonButton.onClick.AddListener(() => atomBuilder.RemoveProton());
            if (addNeutronButton) addNeutronButton.onClick.AddListener(() => atomBuilder.AddNeutron());
            if (removeNeutronButton) removeNeutronButton.onClick.AddListener(() => atomBuilder.RemoveNeutron());
            if (addElectronButton) addElectronButton.onClick.AddListener(() => atomBuilder.AddElectron());
            if (removeElectronButton) removeElectronButton.onClick.AddListener(() => atomBuilder.RemoveElectron());

            // Action buttons
            if (submitButton) submitButton.onClick.AddListener(() => atomBuilder.SubmitAtom());
            if (skipButton) skipButton.onClick.AddListener(() => atomBuilder.SkipElement());
            if (hintButton) hintButton.onClick.AddListener(ShowHint);
            if (continueButton) continueButton.onClick.AddListener(OnContinueClicked);
            if (retryButton) retryButton.onClick.AddListener(OnRetryClicked);

            // Start panel buttons
            if (startNewGameButton) startNewGameButton.onClick.AddListener(StartNewGame);
            if (continueGameButton) continueGameButton.onClick.AddListener(ContinueGame);
            if (selectLevelButton) selectLevelButton.onClick.AddListener(ShowLevelSelector);

            // Victory panel buttons
            if (playAgainButton) playAgainButton.onClick.AddListener(StartNewGame);
            if (mainMenuButton) mainMenuButton.onClick.AddListener(ShowStartPanel);
        }

        private void SubscribeToEvents()
        {
            atomBuilder.OnElementStarted += OnElementStarted;
            atomBuilder.OnElementCompleted += OnElementCompleted;
            atomBuilder.OnGameCompleted += OnGameCompleted;
            atomBuilder.OnFeedback += OnFeedback;
            atomBuilder.OnTimeUp += OnTimeUp;
            atomBuilder.OnParticleAdded += UpdateParticleDisplay;
        }

        private void OnDestroy()
        {
            if (atomBuilder != null)
            {
                atomBuilder.OnElementStarted -= OnElementStarted;
                atomBuilder.OnElementCompleted -= OnElementCompleted;
                atomBuilder.OnGameCompleted -= OnGameCompleted;
                atomBuilder.OnFeedback -= OnFeedback;
                atomBuilder.OnTimeUp -= OnTimeUp;
                atomBuilder.OnParticleAdded -= UpdateParticleDisplay;
            }
        }

        private void Update()
        {
            if (atomBuilder != null && atomBuilder.IsPlaying)
            {
                UpdateTimer();
            }
        }

        private void ShowStartPanel()
        {
            if (startPanel) startPanel.SetActive(true);
            if (gamePanel) gamePanel.SetActive(false);
            if (resultPanel) resultPanel.SetActive(false);
            if (victoryPanel) victoryPanel.SetActive(false);
            if (levelSelectorPanel) levelSelectorPanel.SetActive(false);

            // Check for saved progress
            atomBuilder.LoadProgress();
            bool hasProgress = atomBuilder.CurrentLevel > 1;
            
            if (continueGameButton)
                continueGameButton.gameObject.SetActive(hasProgress);
            
            if (savedProgressText && hasProgress)
            {
                savedProgressText.text = $"Progress: Level {atomBuilder.CurrentLevel}/118\n" +
                    $"Score: {atomBuilder.Score}\nBest Streak: {atomBuilder.BestStreak}";
                savedProgressText.gameObject.SetActive(true);
            }
            else if (savedProgressText)
            {
                savedProgressText.gameObject.SetActive(false);
            }
        }

        private void StartNewGame()
        {
            atomBuilder.ResetProgress();
            hintsUsed = 0;
            gameStartTime = Time.time;
            atomBuilder.StartGame();
            
            if (startPanel) startPanel.SetActive(false);
            if (victoryPanel) victoryPanel.SetActive(false);
            if (levelSelectorPanel) levelSelectorPanel.SetActive(false);
            if (gamePanel) gamePanel.SetActive(true);
        }

        private void ContinueGame()
        {
            gameStartTime = Time.time;
            atomBuilder.LoadProgress();
            atomBuilder.StartFromElement(atomBuilder.CurrentLevel - 1);
            
            if (startPanel) startPanel.SetActive(false);
            if (gamePanel) gamePanel.SetActive(true);
        }

        private void ShowLevelSelector()
        {
            if (levelSelectorPanel) levelSelectorPanel.SetActive(true);
            if (startPanel) startPanel.SetActive(false);
            
            PopulateLevelButtons();
        }

        private void PopulateLevelButtons()
        {
            if (levelButtonContainer == null) return;

            // Clear existing buttons
            foreach (Transform child in levelButtonContainer)
            {
                Destroy(child.gameObject);
            }

            // Create buttons for all 118 elements
            for (int i = 0; i < 118; i++)
            {
                int levelIndex = i; // Capture for closure
                var element = atomBuilder.GetElementAt(i);
                
                GameObject buttonGO;
                if (levelButtonPrefab != null)
                {
                    buttonGO = Instantiate(levelButtonPrefab, levelButtonContainer);
                }
                else
                {
                    buttonGO = new GameObject($"Level{i + 1}Button");
                    buttonGO.transform.SetParent(levelButtonContainer);
                    buttonGO.AddComponent<Image>();
                    var btn = buttonGO.AddComponent<Button>();
                    var txt = new GameObject("Text").AddComponent<TextMeshProUGUI>();
                    txt.transform.SetParent(buttonGO.transform);
                    txt.text = $"{i + 1}. {element.Symbol}";
                    txt.alignment = TextAlignmentOptions.Center;
                    txt.fontSize = 14;
                }

                var button = buttonGO.GetComponent<Button>();
                var text = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
                
                if (text && element != null)
                {
                    text.text = $"{i + 1}. {element.Symbol}";
                }

                button.onClick.AddListener(() => StartFromLevel(levelIndex));
            }
        }

        private void StartFromLevel(int levelIndex)
        {
            hintsUsed = 0;
            gameStartTime = Time.time;
            atomBuilder.StartFromElement(levelIndex);
            
            if (levelSelectorPanel) levelSelectorPanel.SetActive(false);
            if (gamePanel) gamePanel.SetActive(true);
        }

        private void OnElementStarted(ElementBuildData element)
        {
            if (resultPanel) resultPanel.SetActive(false);
            if (gamePanel) gamePanel.SetActive(true);

            UpdateElementDisplay(element);
            UpdateParticleDisplay();
            UpdateScoreDisplay();
            ClearAtomVisualization();
            CreateElectronShells(element);

            if (hintText) hintText.text = "";
        }

        private void UpdateElementDisplay(ElementBuildData element)
        {
            if (elementNameText)
                elementNameText.text = $"Build: {element.Name}";
            
            if (elementSymbolText)
                elementSymbolText.text = element.Symbol;
            
            if (levelText)
                levelText.text = $"Level {atomBuilder.CurrentLevel} / {atomBuilder.TotalLevels}";
        }

        private void UpdateTimer()
        {
            float timeRemaining = atomBuilder.TimeRemaining;
            float timeLimit = atomBuilder.TimeLimit;
            float ratio = timeRemaining / timeLimit;

            if (timerSlider)
            {
                timerSlider.value = ratio;
            }

            if (timerText)
            {
                timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
            }

            if (timerFill)
            {
                if (ratio > 0.5f)
                    timerFill.color = timerNormalColor;
                else if (ratio > 0.25f)
                    timerFill.color = timerWarningColor;
                else
                    timerFill.color = timerDangerColor;
            }
        }

        private void UpdateParticleDisplay()
        {
            if (protonCountText)
                protonCountText.text = $"Protons: {atomBuilder.PlacedProtons}";
            
            if (neutronCountText)
                neutronCountText.text = $"Neutrons: {atomBuilder.PlacedNeutrons}";
            
            if (electronCountText)
                electronCountText.text = $"Electrons: {atomBuilder.PlacedElectrons}";

            // Update visual representation
            UpdateAtomVisualization();
        }

        private void UpdateScoreDisplay()
        {
            if (scoreText)
                scoreText.text = $"Score: {atomBuilder.Score}";
            
            if (streakText)
            {
                if (atomBuilder.Streak > 0)
                    streakText.text = $"🔥 Streak: {atomBuilder.Streak}";
                else
                    streakText.text = "";
            }
        }

        private void ClearAtomVisualization()
        {
            foreach (var p in visualProtons) Destroy(p);
            foreach (var n in visualNeutrons) Destroy(n);
            foreach (var e in visualElectrons) Destroy(e);
            foreach (var s in electronShells) Destroy(s);
            
            visualProtons.Clear();
            visualNeutrons.Clear();
            visualElectrons.Clear();
            electronShells.Clear();
        }

        private void CreateElectronShells(ElementBuildData element)
        {
            if (electronShellContainer == null || electronShellPrefab == null) return;

            // Create shells based on electron configuration
            int shellCount = element.ElectronShells?.Length ?? 1;
            for (int i = 0; i < shellCount; i++)
            {
                var shell = Instantiate(electronShellPrefab, electronShellContainer);
                float scale = 1f + (i * 0.5f);
                shell.transform.localScale = Vector3.one * scale;
                electronShells.Add(shell);
            }
        }

        private void UpdateAtomVisualization()
        {
            if (nucleusContainer == null) return;

            // Update protons
            while (visualProtons.Count < atomBuilder.PlacedProtons)
            {
                var proton = CreateParticle(protonPrefab, protonColor, nucleusContainer);
                PositionInNucleus(proton, visualProtons.Count + visualNeutrons.Count);
                visualProtons.Add(proton);
            }
            while (visualProtons.Count > atomBuilder.PlacedProtons)
            {
                Destroy(visualProtons[visualProtons.Count - 1]);
                visualProtons.RemoveAt(visualProtons.Count - 1);
            }

            // Update neutrons
            while (visualNeutrons.Count < atomBuilder.PlacedNeutrons)
            {
                var neutron = CreateParticle(neutronPrefab, neutronColor, nucleusContainer);
                PositionInNucleus(neutron, visualProtons.Count + visualNeutrons.Count);
                visualNeutrons.Add(neutron);
            }
            while (visualNeutrons.Count > atomBuilder.PlacedNeutrons)
            {
                Destroy(visualNeutrons[visualNeutrons.Count - 1]);
                visualNeutrons.RemoveAt(visualNeutrons.Count - 1);
            }

            // Update electrons
            if (electronShellContainer != null)
            {
                while (visualElectrons.Count < atomBuilder.PlacedElectrons)
                {
                    var electron = CreateParticle(electronPrefab, electronColor, electronShellContainer);
                    visualElectrons.Add(electron);
                }
                while (visualElectrons.Count > atomBuilder.PlacedElectrons)
                {
                    Destroy(visualElectrons[visualElectrons.Count - 1]);
                    visualElectrons.RemoveAt(visualElectrons.Count - 1);
                }
                PositionElectrons();
            }
        }

        private GameObject CreateParticle(GameObject prefab, Color color, Transform parent)
        {
            GameObject particle;
            if (prefab != null)
            {
                particle = Instantiate(prefab, parent);
            }
            else
            {
                // Create simple sphere if no prefab
                particle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                particle.transform.SetParent(parent);
                particle.transform.localScale = Vector3.one * 0.2f;
                var renderer = particle.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = color;
                }
            }
            return particle;
        }

        private void PositionInNucleus(GameObject particle, int index)
        {
            // Position particles in a cluster pattern
            float angle = index * 137.5f * Mathf.Deg2Rad; // Golden angle
            float radius = Mathf.Sqrt(index) * 0.15f;
            float height = (index % 3 - 1) * 0.1f;
            
            particle.transform.localPosition = new Vector3(
                Mathf.Cos(angle) * radius,
                height,
                Mathf.Sin(angle) * radius
            );
        }

        private void PositionElectrons()
        {
            var element = atomBuilder.CurrentElement;
            if (element?.ElectronShells == null) return;

            int electronIndex = 0;
            for (int shell = 0; shell < element.ElectronShells.Length && electronIndex < visualElectrons.Count; shell++)
            {
                int electronsInShell = Mathf.Min(
                    element.ElectronShells[shell],
                    visualElectrons.Count - electronIndex
                );
                
                float shellRadius = 1f + (shell * 0.5f);
                
                for (int e = 0; e < electronsInShell && electronIndex < visualElectrons.Count; e++)
                {
                    float angle = (e / (float)electronsInShell) * Mathf.PI * 2f;
                    visualElectrons[electronIndex].transform.localPosition = new Vector3(
                        Mathf.Cos(angle) * shellRadius,
                        0,
                        Mathf.Sin(angle) * shellRadius
                    );
                    electronIndex++;
                }
            }
        }

        private void OnElementCompleted(bool success, int pointsEarned)
        {
            atomBuilder.SaveProgress();
            
            if (resultPanel) resultPanel.SetActive(true);

            var element = atomBuilder.CurrentElement ?? atomBuilder.GetElementAt(atomBuilder.CurrentLevel - 2);
            
            if (resultTitleText)
            {
                resultTitleText.text = success ? "✓ Correct!" : "✗ Try Again";
                resultTitleText.color = success ? Color.green : Color.red;
            }

            if (resultMessageText && element != null)
            {
                if (success)
                {
                    resultMessageText.text = $"You successfully built {element.Name}!\n" +
                        $"+{pointsEarned} points";
                }
                else
                {
                    resultMessageText.text = $"{element.Name} requires:\n" +
                        $"• {element.Protons} Protons\n" +
                        $"• {element.Neutrons} Neutrons\n" +
                        $"• {element.Electrons} Electrons";
                }
            }

            if (resultScoreText)
                resultScoreText.text = $"Total Score: {atomBuilder.Score}";
            
            if (resultStreakText)
                resultStreakText.text = atomBuilder.Streak > 1 ? $"🔥 {atomBuilder.Streak} in a row!" : "";

            // Show appropriate button
            if (continueButton) continueButton.gameObject.SetActive(success);
            if (retryButton) retryButton.gameObject.SetActive(!success);
        }

        private void OnGameCompleted()
        {
            if (gamePanel) gamePanel.SetActive(false);
            if (resultPanel) resultPanel.SetActive(false);
            if (victoryPanel) victoryPanel.SetActive(true);

            float totalTime = Time.time - gameStartTime;

            if (victoryScoreText)
                victoryScoreText.text = $"Final Score: {atomBuilder.Score}";
            
            if (victoryStreakText)
                victoryStreakText.text = $"Best Streak: {atomBuilder.BestStreak}";
            
            if (victoryTimeText)
            {
                int minutes = Mathf.FloorToInt(totalTime / 60);
                int seconds = Mathf.FloorToInt(totalTime % 60);
                victoryTimeText.text = $"Time: {minutes}:{seconds:D2}";
            }

            // Clear saved progress since game is complete
            atomBuilder.ResetProgress();
        }

        private void OnFeedback(string message)
        {
            // Could show a toast notification or update a feedback text
            Debug.Log($"[AtomBuilder] {message}");
        }

        private void OnTimeUp()
        {
            // Visual/audio feedback for time running out
            if (timerFill)
                timerFill.color = Color.red;
        }

        private void ShowHint()
        {
            hintsUsed++;
            string hint = atomBuilder.GetHint();
            
            if (hintText)
                hintText.text = $"💡 Hint: {hint}";
        }

        private void OnContinueClicked()
        {
            atomBuilder.ContinueToNext();
        }

        private void OnRetryClicked()
        {
            atomBuilder.RetryCurrentElement();
        }

        public void Show()
        {
            if (atomBuilderPanel) atomBuilderPanel.SetActive(true);
            ShowStartPanel();
        }

        public void Hide()
        {
            if (atomBuilderPanel) atomBuilderPanel.SetActive(false);
            atomBuilder.SaveProgress();
        }

        public void Toggle()
        {
            if (atomBuilderPanel != null)
            {
                if (atomBuilderPanel.activeSelf)
                    Hide();
                else
                    Show();
            }
        }
    }
}

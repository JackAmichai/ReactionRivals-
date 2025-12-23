using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// UI for the chemistry quiz mini-game.
/// Appears between rounds to reinforce learning.
/// </summary>
public class ChemistryQuizUI : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject quizPanel;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Question Display")]
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI streakText;
    [SerializeField] private TextMeshProUGUI rewardText;

    [Header("Answer Buttons")]
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private TextMeshProUGUI[] answerTexts;
    [SerializeField] private Image[] answerBackgrounds;

    [Header("Result Display")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI explanationText;
    [SerializeField] private Button continueButton;

    [Header("Colors")]
    [SerializeField] private Color normalColor = new Color(0.2f, 0.3f, 0.4f);
    [SerializeField] private Color correctColor = new Color(0.2f, 0.7f, 0.3f);
    [SerializeField] private Color wrongColor = new Color(0.7f, 0.2f, 0.2f);
    [SerializeField] private Color selectedColor = new Color(0.3f, 0.5f, 0.7f);

    [Header("Rewards")]
    [SerializeField] private int baseReward = 1;      // ATP for correct answer
    [SerializeField] private int streakBonus = 1;     // Extra ATP per streak

    // State
    private QuizQuestion currentQuestion;
    private int correctStreak = 0;
    private int totalCorrect = 0;
    private int totalQuestions = 0;
    private HashSet<int> unlockedElements = new HashSet<int>();
    private int playerLevel = 1;

    // Events
    public System.Action<int> OnRewardEarned;       // ATP earned
    public System.Action OnQuizComplete;

    private void Awake()
    {
        // Setup answer button listeners
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinue);

        Hide();
    }

    /// <summary>
    /// Initialize the quiz with player's current progress
    /// </summary>
    public void Initialize(int level, HashSet<int> elements)
    {
        playerLevel = level;
        unlockedElements = elements;
    }

    /// <summary>
    /// Show a new quiz question
    /// </summary>
    public void ShowQuiz()
    {
        quizPanel.SetActive(true);
        if (resultPanel != null)
            resultPanel.SetActive(false);

        GenerateNewQuestion();
        UpdateStreakDisplay();
    }

    private void GenerateNewQuestion()
    {
        currentQuestion = ChemistryQuiz.GenerateQuestion(playerLevel, unlockedElements);
        
        if (categoryText != null)
            categoryText.text = $"📚 {currentQuestion.Category}";
        
        if (questionText != null)
            questionText.text = currentQuestion.Question;

        // Setup answer buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.Answers.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].interactable = true;
                answerTexts[i].text = currentQuestion.Answers[i];
                answerBackgrounds[i].color = normalColor;
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }

        // Show potential reward
        int potentialReward = baseReward + (correctStreak * streakBonus);
        if (rewardText != null)
            rewardText.text = $"Reward: {potentialReward} ATP";
    }

    private void OnAnswerSelected(int index)
    {
        // Disable all buttons
        foreach (var button in answerButtons)
            button.interactable = false;

        totalQuestions++;
        bool isCorrect = currentQuestion.IsCorrect(index);

        // Visual feedback
        answerBackgrounds[index].color = isCorrect ? correctColor : wrongColor;
        
        // Always show the correct answer
        answerBackgrounds[currentQuestion.CorrectIndex].color = correctColor;

        if (isCorrect)
        {
            correctStreak++;
            totalCorrect++;
            int reward = baseReward + ((correctStreak - 1) * streakBonus);
            
            ShowResult(true, reward);
            OnRewardEarned?.Invoke(reward);
        }
        else
        {
            correctStreak = 0;
            ShowResult(false, 0);
        }

        UpdateStreakDisplay();
    }

    private void ShowResult(bool correct, int reward)
    {
        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (resultText != null)
        {
            if (correct)
            {
                string streakMsg = correctStreak > 1 ? $" 🔥 {correctStreak} streak!" : "";
                resultText.text = $"<color=#00ff00>✓ Correct!</color>{streakMsg}\n+{reward} ATP";
            }
            else
            {
                resultText.text = $"<color=#ff4444>✗ Not quite!</color>\nThe answer was: {currentQuestion.GetCorrectAnswer()}";
            }
        }

        if (explanationText != null)
            explanationText.text = currentQuestion.Explanation;
    }

    private void UpdateStreakDisplay()
    {
        if (streakText != null)
        {
            string streakIcon = correctStreak >= 5 ? "🔥🔥" : correctStreak >= 3 ? "🔥" : "";
            streakText.text = $"Streak: {correctStreak} {streakIcon} | Score: {totalCorrect}/{totalQuestions}";
        }
    }

    private void OnContinue()
    {
        Hide();
        OnQuizComplete?.Invoke();
    }

    public void Hide()
    {
        if (quizPanel != null)
            quizPanel.SetActive(false);
    }

    /// <summary>
    /// Reset quiz statistics
    /// </summary>
    public void ResetStats()
    {
        correctStreak = 0;
        totalCorrect = 0;
        totalQuestions = 0;
    }

    /// <summary>
    /// Get current accuracy percentage
    /// </summary>
    public float GetAccuracy()
    {
        if (totalQuestions == 0) return 0;
        return (float)totalCorrect / totalQuestions * 100f;
    }
}

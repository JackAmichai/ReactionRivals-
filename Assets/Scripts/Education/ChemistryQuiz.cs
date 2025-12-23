using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Chemistry quiz system for educational mini-games between rounds.
/// Questions are based on actual chemistry facts and adapt to unlocked elements.
/// </summary>
public static class ChemistryQuiz
{
    /// <summary>
    /// Question types for variety
    /// </summary>
    public enum QuestionType
    {
        SymbolToName,       // What element has symbol "Fe"?
        NameToSymbol,       // What is the symbol for Iron?
        AtomicNumber,       // What is the atomic number of Carbon?
        ValenceElectrons,   // How many valence electrons does Oxygen have?
        Family,             // What family does Sodium belong to?
        Discovery,          // Who discovered Oxygen?
        TrueFalse,          // True or False: Gold never rusts
        BondingType,        // What type of bond forms between Na and Cl?
        MoleculeFormula,    // What is the formula for water?
        RealWorldUse        // What element powers your smartphone battery?
    }

    private static System.Random random = new System.Random();

    /// <summary>
    /// Generate a quiz question appropriate for the player's level
    /// </summary>
    public static QuizQuestion GenerateQuestion(int playerLevel, HashSet<int> unlockedElements)
    {
        // Filter to elements player has seen
        List<PeriodicElementInfo> availableElements = new List<PeriodicElementInfo>();
        foreach (int atomicNum in unlockedElements)
        {
            var element = PeriodicTable.GetElement(atomicNum);
            if (element != null)
                availableElements.Add(element);
        }

        if (availableElements.Count < 4)
        {
            // Not enough elements for good questions
            return GenerateBasicQuestion();
        }

        // Pick a random question type
        QuestionType type = (QuestionType)random.Next(0, System.Enum.GetValues(typeof(QuestionType)).Length);
        
        return GenerateQuestionOfType(type, availableElements);
    }

    private static QuizQuestion GenerateBasicQuestion()
    {
        return new QuizQuestion
        {
            Question = "What is the chemical symbol for water?",
            Answers = new string[] { "H₂O", "CO₂", "O₂", "NaCl" },
            CorrectIndex = 0,
            Explanation = "Water is made of 2 hydrogen atoms and 1 oxygen atom, hence H₂O.",
            Category = "Molecules"
        };
    }

    private static QuizQuestion GenerateQuestionOfType(QuestionType type, List<PeriodicElementInfo> elements)
    {
        var correctElement = elements[random.Next(elements.Count)];
        
        switch (type)
        {
            case QuestionType.SymbolToName:
                return GenerateSymbolToName(correctElement, elements);
            
            case QuestionType.NameToSymbol:
                return GenerateNameToSymbol(correctElement, elements);
            
            case QuestionType.AtomicNumber:
                return GenerateAtomicNumber(correctElement, elements);
            
            case QuestionType.ValenceElectrons:
                return GenerateValenceQuestion(correctElement);
            
            case QuestionType.Family:
                return GenerateFamilyQuestion(correctElement, elements);
            
            case QuestionType.Discovery:
                return GenerateDiscoveryQuestion(correctElement);
            
            case QuestionType.TrueFalse:
                return GenerateTrueFalseQuestion();
            
            case QuestionType.RealWorldUse:
                return GenerateRealWorldQuestion();
            
            default:
                return GenerateSymbolToName(correctElement, elements);
        }
    }

    private static QuizQuestion GenerateSymbolToName(PeriodicElementInfo correct, List<PeriodicElementInfo> pool)
    {
        var wrongAnswers = GetRandomWrongAnswers(correct, pool, 3, e => e.Name);
        
        var answers = new List<string> { correct.Name };
        answers.AddRange(wrongAnswers);
        int correctIndex = 0;
        ShuffleAnswers(answers, ref correctIndex);

        return new QuizQuestion
        {
            Question = $"What element has the symbol '{correct.Symbol}'?",
            Answers = answers.ToArray(),
            CorrectIndex = correctIndex,
            Explanation = $"{correct.Symbol} is the symbol for {correct.Name} (atomic number {correct.AtomicNumber}).",
            Category = "Symbols"
        };
    }

    private static QuizQuestion GenerateNameToSymbol(PeriodicElementInfo correct, List<PeriodicElementInfo> pool)
    {
        var wrongAnswers = GetRandomWrongAnswers(correct, pool, 3, e => e.Symbol);
        
        var answers = new List<string> { correct.Symbol };
        answers.AddRange(wrongAnswers);
        int correctIndex = 0;
        ShuffleAnswers(answers, ref correctIndex);

        return new QuizQuestion
        {
            Question = $"What is the chemical symbol for {correct.Name}?",
            Answers = answers.ToArray(),
            CorrectIndex = correctIndex,
            Explanation = $"The symbol {correct.Symbol} comes from {(correct.AtomicNumber == 26 ? "Latin 'ferrum'" : correct.AtomicNumber == 79 ? "Latin 'aurum'" : "its name")}.",
            Category = "Symbols"
        };
    }

    private static QuizQuestion GenerateAtomicNumber(PeriodicElementInfo correct, List<PeriodicElementInfo> pool)
    {
        var answers = new List<string> { correct.AtomicNumber.ToString() };
        
        // Generate plausible wrong answers near the correct one
        HashSet<int> usedNumbers = new HashSet<int> { correct.AtomicNumber };
        while (answers.Count < 4)
        {
            int offset = random.Next(-5, 6);
            if (offset == 0) offset = 1;
            int wrongNum = Mathf.Clamp(correct.AtomicNumber + offset, 1, 118);
            if (!usedNumbers.Contains(wrongNum))
            {
                answers.Add(wrongNum.ToString());
                usedNumbers.Add(wrongNum);
            }
        }
        
        int correctIndex = 0;
        ShuffleAnswers(answers, ref correctIndex);

        return new QuizQuestion
        {
            Question = $"What is the atomic number of {correct.Name}?",
            Answers = answers.ToArray(),
            CorrectIndex = correctIndex,
            Explanation = $"{correct.Name} has {correct.AtomicNumber} protons, giving it atomic number {correct.AtomicNumber}.",
            Category = "Atomic Structure"
        };
    }

    private static QuizQuestion GenerateValenceQuestion(PeriodicElementInfo correct)
    {
        var answers = new List<string>();
        HashSet<int> usedVals = new HashSet<int>();
        
        answers.Add(correct.ValenceElectrons.ToString());
        usedVals.Add(correct.ValenceElectrons);
        
        int[] possibleValues = { 1, 2, 3, 4, 5, 6, 7, 8 };
        foreach (int val in possibleValues)
        {
            if (answers.Count >= 4) break;
            if (!usedVals.Contains(val))
            {
                answers.Add(val.ToString());
                usedVals.Add(val);
            }
        }

        int correctIndex = 0;
        ShuffleAnswers(answers, ref correctIndex);

        return new QuizQuestion
        {
            Question = $"How many valence electrons does {correct.Name} have?",
            Answers = answers.ToArray(),
            CorrectIndex = correctIndex,
            Explanation = $"{correct.Name} has {correct.ValenceElectrons} valence electrons in its outer shell.",
            Category = "Electron Configuration"
        };
    }

    private static QuizQuestion GenerateFamilyQuestion(PeriodicElementInfo correct, List<PeriodicElementInfo> pool)
    {
        string correctFamily = FormatFamilyName(correct.Family);
        
        var answers = new List<string> { correctFamily };
        string[] allFamilies = { "Alkali Metal", "Noble Gas", "Halogen", "Transition Metal", "Nonmetal", "Metalloid" };
        
        foreach (string family in allFamilies)
        {
            if (answers.Count >= 4) break;
            if (family != correctFamily)
                answers.Add(family);
        }

        int correctIndex = 0;
        ShuffleAnswers(answers, ref correctIndex);

        return new QuizQuestion
        {
            Question = $"What family does {correct.Name} belong to?",
            Answers = answers.ToArray(),
            CorrectIndex = correctIndex,
            Explanation = $"{correct.Name} is a {correctFamily.ToLower()}, found in Group {correct.Group} of the periodic table.",
            Category = "Element Families"
        };
    }

    private static QuizQuestion GenerateDiscoveryQuestion(PeriodicElementInfo correct)
    {
        var history = ElementHistory.GetInfo(correct.AtomicNumber);
        
        if (history == null || string.IsNullOrEmpty(history.Discoverer) || history.Discoverer == "Various scientists")
        {
            // Fall back to a different question type
            return GenerateTrueFalseQuestion();
        }

        var answers = new List<string> { history.Discoverer };
        string[] fakeDiscoverers = { "Marie Curie", "Albert Einstein", "Dmitri Mendeleev", "Antoine Lavoisier", 
                                     "Robert Boyle", "Michael Faraday", "Niels Bohr", "Ernest Rutherford" };
        
        foreach (string name in fakeDiscoverers)
        {
            if (answers.Count >= 4) break;
            if (!name.Contains(history.Discoverer.Split(' ')[0]))
                answers.Add(name);
        }

        int correctIndex = 0;
        ShuffleAnswers(answers, ref correctIndex);

        return new QuizQuestion
        {
            Question = $"Who is credited with discovering {correct.Name}?",
            Answers = answers.ToArray(),
            CorrectIndex = correctIndex,
            Explanation = $"{correct.Name} was discovered by {history.Discoverer} in {history.GetFormattedYear()} in {history.DiscoveryLocation}.",
            Category = "History of Chemistry"
        };
    }

    private static QuizQuestion GenerateTrueFalseQuestion()
    {
        // Pool of true/false chemistry facts
        var facts = new (string statement, bool isTrue, string explanation)[]
        {
            ("Gold never rusts or tarnishes", true, "Gold is extremely unreactive and doesn't oxidize, which is why ancient gold artifacts look perfect today."),
            ("Diamond and graphite are both pure carbon", true, "They're allotropes - same element, different atomic arrangements. Diamond is hard, graphite is soft."),
            ("Helium was discovered on the Sun before Earth", true, "In 1868, scientists found helium in the Sun's spectrum during an eclipse, 27 years before finding it on Earth."),
            ("Water is the most common molecule in the universe", false, "Hydrogen (H₂) is the most common molecule. Water is common on Earth but rare in the universe."),
            ("Your body contains about 4 grams of iron", true, "Most of it is in hemoglobin, carrying oxygen in your blood."),
            ("Noble gases can form compounds", true, "While extremely unreactive, some noble gases (like Xenon) can form compounds under special conditions."),
            ("Mercury is the only metal that's liquid at room temperature", true, "Mercury melts at -39°C. Gallium is close but melts at 30°C (almost room temperature)."),
            ("The human body contains gold", true, "The average person has about 0.2mg of gold, mostly in the blood."),
            ("Oxygen makes up most of the air we breathe", false, "Nitrogen makes up 78% of air. Oxygen is only about 21%."),
            ("Aluminum was once more valuable than gold", true, "In the 1850s, before electrolysis made it cheap to produce, aluminum was a precious metal."),
        };

        var fact = facts[random.Next(facts.Length)];
        
        return new QuizQuestion
        {
            Question = $"True or False: {fact.statement}",
            Answers = new string[] { "True", "False" },
            CorrectIndex = fact.isTrue ? 0 : 1,
            Explanation = fact.explanation,
            Category = "Chemistry Facts"
        };
    }

    private static QuizQuestion GenerateRealWorldQuestion()
    {
        var questions = new (string question, string[] answers, int correct, string explanation)[]
        {
            ("What element powers most smartphone batteries?", new[] { "Lithium", "Copper", "Silver", "Iron" }, 0,
             "Lithium-ion batteries power almost all modern smartphones and electric vehicles."),
            
            ("What element makes rubies red and emeralds green?", new[] { "Chromium", "Iron", "Copper", "Gold" }, 0,
             "Chromium impurities give rubies their red color and emeralds their green color."),
            
            ("What element is in your toothpaste to prevent cavities?", new[] { "Fluorine", "Chlorine", "Bromine", "Iodine" }, 0,
             "Fluoride (from fluorine) strengthens tooth enamel and prevents decay."),
            
            ("What element is added to table salt to prevent thyroid problems?", new[] { "Iodine", "Iron", "Zinc", "Selenium" }, 0,
             "Iodized salt contains iodine, essential for thyroid hormone production."),
            
            ("What element makes blood red?", new[] { "Iron", "Copper", "Zinc", "Calcium" }, 0,
             "Iron in hemoglobin gives blood its red color. Interestingly, horseshoe crab blood is blue (copper-based)."),
            
            ("What element is the main component of computer chips?", new[] { "Silicon", "Carbon", "Copper", "Gold" }, 0,
             "Silicon is the semiconductor that makes modern electronics possible - hence 'Silicon Valley'."),
            
            ("What two elements make table salt?", new[] { "Sodium and Chlorine", "Calcium and Carbon", "Potassium and Chlorine", "Sodium and Iodine" }, 0,
             "Table salt is sodium chloride (NaCl) - one atom of sodium bonded to one atom of chlorine."),
            
            ("What noble gas is used in party balloons?", new[] { "Helium", "Neon", "Argon", "Krypton" }, 0,
             "Helium is lighter than air, making balloons float. It's also used because it's safe (unlike hydrogen)."),
        };

        var q = questions[random.Next(questions.Length)];
        
        int correctIndex = q.correct;
        var answers = new List<string>(q.answers);
        ShuffleAnswers(answers, ref correctIndex);

        return new QuizQuestion
        {
            Question = q.question,
            Answers = answers.ToArray(),
            CorrectIndex = correctIndex,
            Explanation = q.explanation,
            Category = "Real World Chemistry"
        };
    }

    private static List<string> GetRandomWrongAnswers(PeriodicElementInfo correct, List<PeriodicElementInfo> pool, 
        int count, System.Func<PeriodicElementInfo, string> selector)
    {
        var wrong = new List<string>();
        var shuffled = new List<PeriodicElementInfo>(pool);
        
        // Fisher-Yates shuffle
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            var temp = shuffled[i];
            shuffled[i] = shuffled[j];
            shuffled[j] = temp;
        }

        foreach (var element in shuffled)
        {
            if (element.AtomicNumber != correct.AtomicNumber && wrong.Count < count)
            {
                string value = selector(element);
                if (!wrong.Contains(value))
                    wrong.Add(value);
            }
        }

        return wrong;
    }

    private static void ShuffleAnswers(List<string> answers, ref int correctIndex)
    {
        string correctAnswer = answers[correctIndex];
        
        // Fisher-Yates shuffle
        for (int i = answers.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            var temp = answers[i];
            answers[i] = answers[j];
            answers[j] = temp;
        }

        // Find new correct index
        correctIndex = answers.IndexOf(correctAnswer);
    }

    private static string FormatFamilyName(ElementFamily family)
    {
        switch (family)
        {
            case ElementFamily.Hydrogen: return "Nonmetal";
            case ElementFamily.Alkali: return "Alkali Metal";
            case ElementFamily.AlkalineEarth: return "Alkaline Earth Metal";
            case ElementFamily.TransitionMetal: return "Transition Metal";
            case ElementFamily.PostTransitionMetal: return "Post-Transition Metal";
            case ElementFamily.Metalloid: return "Metalloid";
            case ElementFamily.NonMetal: return "Nonmetal";
            case ElementFamily.Halogen: return "Halogen";
            case ElementFamily.NobleGas: return "Noble Gas";
            case ElementFamily.Lanthanide: return "Lanthanide";
            case ElementFamily.Actinide: return "Actinide";
            default: return family.ToString();
        }
    }
}

/// <summary>
/// A single quiz question
/// </summary>
[System.Serializable]
public class QuizQuestion
{
    public string Question;
    public string[] Answers;
    public int CorrectIndex;
    public string Explanation;
    public string Category;

    public string GetCorrectAnswer()
    {
        return Answers[CorrectIndex];
    }

    public bool IsCorrect(int selectedIndex)
    {
        return selectedIndex == CorrectIndex;
    }
}

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    public QuestionManager questionManager;
    public UIManager uiManager;
    public AudioManager audioManager;
    public GameOverUI gameOverUI;

    [Header("Điểm số")]
    public int pointsOnMatch = 1;
    public int pointsOnMismatch = 1;
    public int score = 0;

    [Header("Thua sau bao nhiêu lần sai")]
    public int maxWrong = 3;
    private int wrongCount = 0;

    [Header("Độ trễ chuyển câu sau khi trả lời")]
    [SerializeField] private float answerDelay = 0.8f;

    private bool isRevealing = false;
    private bool isPlaying = false;
    private int correctCount = 0;  // ✅ Đếm số câu đúng

    private QuestionAsset currentQuestion;

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        score = 0;
        wrongCount = 0;
        correctCount = 0;
        isPlaying = true;
        isRevealing = false;

        questionManager.BuildDeck();
        uiManager.UpdateProgress(0, questionManager.TotalRounds);
        LoadNext();

        audioManager?.PlayBGM();
    }

    private void LoadNext()
    {
        currentQuestion = questionManager.GetNext();
        if (currentQuestion == null)
        {
            Win();
            return;
        }

        uiManager.UpdateProgress(questionManager.CurrentIndex, questionManager.TotalRounds);
        uiManager.ShowQuestion(currentQuestion);
        uiManager.SetButtonsInteractable(true);
    }

    public void OnChooseLegal() => PlayerAnswer(true);
    public void OnChooseScam() => PlayerAnswer(false);

    private void PlayerAnswer(bool choseLegal)
    {
        if (!isPlaying || currentQuestion == null || isRevealing) return;

        isRevealing = true;
        uiManager.SetButtonsInteractable(false);

        bool match = (currentQuestion.IsLegal == choseLegal);
        ApplyScore(match);
        uiManager.ShowReason(currentQuestion);

        if (match)
            audioManager?.PlayCorrect();
        else
            audioManager?.PlayWrong();

        StartCoroutine(NextAfter(match, answerDelay));
    }

    private void ApplyScore(bool match)
    {
        if (match)
        {
            score += pointsOnMatch;
            correctCount++; // ✅ Tăng số câu đúng
        }
        else
        {
            wrongCount++;
            score -= pointsOnMismatch;
        }
    }

    private IEnumerator NextAfter(bool match, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (wrongCount > maxWrong)
        {
            GameOver();
            yield break;
        }

        isRevealing = false;
        LoadNext();
    }

    private void GameOver()
    {
        isPlaying = false;
        uiManager.SetButtonsInteractable(false);
        audioManager?.StopBGM();

        Debug.Log("[GameManager] GAME OVER");

        int totalQuestions = questionManager.TotalRounds; // ✅ luôn là tổng số câu gốc
        gameOverUI?.ShowGameOver(false, correctCount, totalQuestions); // ❌ Thua
    }

    private void Win()
    {
        isPlaying = false;
        uiManager.SetButtonsInteractable(false);
        audioManager?.StopBGM();

        Debug.Log("[GameManager] YOU WIN");

        int totalQuestions = questionManager.TotalRounds;
        gameOverUI?.ShowGameOver(true, correctCount, totalQuestions); // ✅ Thắng
    }
}

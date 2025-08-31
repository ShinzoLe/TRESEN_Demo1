using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    public QuestionManager questionManager;
    public UIManager uiManager;
    public AudioManager audioManager; // optional

    [Header("Điểm số")]
    public int pointsOnMatch = 1;     // + điểm khi chọn trùng nhãn
    public int pointsOnMismatch = 1;  // - điểm khi chọn khác nhãn
    public int score = 0;

    [Header("Luật ‘sai quá 3’")]
    public int maxWrong = 3;          // NGƯỠNG: sai quá 3 (tức 4) thì thua
    private int wrongCount = 0;

    [Header("Thời gian & nhịp hiển thị")]
    [SerializeField] private float reasonRevealDelay = 1.25f; // giữ bảng lí do trước khi sang câu
    private bool isRevealing = false;                         // đang hiển thị lý do -> tạm dừng timer
    private bool isPlaying = false;

    private QuestionAsset currentQuestion;
    private float timeRemaining;

    void Start() { StartGame(); }

    void Update()
    {
        if (!isPlaying || isRevealing) return;

        timeRemaining -= Time.deltaTime;
        uiManager.UpdateTimer(timeRemaining);

        if (timeRemaining <= 0f)
        {
            // Hết giờ: coi như CHỌN KHÁC NHÃN -> trừ điểm, tăng wrongCount
            OnTimeout();
        }
    }

    public void StartGame()
    {
        score = 0;
        wrongCount = 0;
        isPlaying = true;
        LoadNext();
    }

    void LoadNext()
    {
        if (!questionManager.HasNext())
        {
            isPlaying = false;
            uiManager.ShowGameWin();
            audioManager?.Play("Victory");
            return;
        }

        currentQuestion = questionManager.NextQuestion();

        // Timer theo mốc 20/40/60
        int idx = questionManager.CurrentIndex;
        timeRemaining = (idx <= 20) ? 60f : (idx <= 40 ? 40f : 20f);

        uiManager.ShowQuestion(currentQuestion, idx, questionManager.TotalRounds);
        uiManager.UpdateScore(score);
        uiManager.SetButtonsInteractable(true);
    }

    // Gọi từ 2 nút: HỢP PHÁP(true) / LỪA ĐẢO(false)
    public void PlayerAnswer(bool choseLegal)
    {
        if (!isPlaying || currentQuestion == null || isRevealing) return;

        bool match = (currentQuestion.IsLegal == choseLegal);

        // Luôn hiển thị NHÃN ĐÚNG của câu hỏi + lý do:
        uiManager.ShowReason(currentQuestion);

        StartCoroutine(NextAfter(match, reasonRevealDelay));
    }

    private void OnTimeout()
    {
        // Hết giờ vẫn show NHÃN ĐÚNG của câu:
        uiManager.ShowReason(currentQuestion);
        StartCoroutine(NextAfter(match: false, reasonRevealDelay));
    }

    private IEnumerator NextAfter(bool match, float delay)
    {
        isRevealing = true;
        uiManager.SetButtonsInteractable(false);

        ApplyScore(match);
        if (!match) wrongCount++;

        // Giữ bảng lý do cho người chơi đọc
        yield return new WaitForSeconds(delay);

        // Kiểm tra ngưỡng sai: "sai QUÁ 3" => > 3
        if (wrongCount > maxWrong)
        {
            GameOver();
            yield break;
        }

        isRevealing = false;
        LoadNext();
    }

    private void ApplyScore(bool match)
    {
        if (match)
        {
            score += pointsOnMatch;
            audioManager?.Play("Correct");
        }
        else
        {
            score -= pointsOnMismatch;
            audioManager?.Play("Wrong");
        }
        uiManager.UpdateScore(score);
    }

    private void GameOver()
    {
        isPlaying = false;
        uiManager.ShowGameOver();
        audioManager?.Play("GameOver");
        uiManager.SetButtonsInteractable(false);
    }
}

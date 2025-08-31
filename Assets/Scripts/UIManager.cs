using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements (Legacy Text)")]
    public Text txtQuestion;  // khung lớn bên trái
    public Text txtCategory;  // ô "Loại:" (tuỳ chọn)
    public Text txtProgress;  // "n/60"
    public Text txtTimer;     // đếm ngược
    public Text txtReason;    // "Bảng lí do"
    public Text txtScore;     // ô hiển thị Điểm (nếu có 1 text riêng cho điểm)

    public Button btnLegal;   // HỢP PHÁP
    public Button btnScam;    // LỪA ĐẢO

    [Header("Timer colors")]
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;

    void Start()
    {
        if (txtReason) txtReason.text = "";
    }

    public void ShowQuestion(QuestionAsset q, int index, int total)
    {
        if (txtQuestion) txtQuestion.text = q.questionText;
        if (txtCategory) txtCategory.text = string.IsNullOrEmpty(q.category) ? "" : q.category;
        if (txtProgress) txtProgress.text = $"{index}/{total}";
        if (txtReason) txtReason.text = "";

        // reset màu timer
        if (txtTimer) txtTimer.color = normalColor;
    }

    public void UpdateTimer(float time)
    {
        if (!txtTimer) return;
        int sec = Mathf.CeilToInt(Mathf.Max(time, 0f));
        txtTimer.text = sec.ToString();
        txtTimer.color = (sec <= 10) ? warningColor : normalColor;
    }

    // choseLegal = true -> "HỢP PHÁP", false -> "LỪA ĐẢO"
    public void ShowReason(bool isLegalAnswer, string[] reasons)
    {
        if (!txtReason) return;

        var sb = new System.Text.StringBuilder();
        // Hiển thị theo NHÃN CỦA CÂU HỎI, không liên quan người chơi chọn gì
        sb.AppendLine(isLegalAnswer ? "✔ HỢP PHÁP" : "⚠ LỪA ĐẢO");

        if (reasons != null && reasons.Length > 0)
        {
            foreach (var r in reasons)
            {
                if (!string.IsNullOrWhiteSpace(r))
                    sb.AppendLine("• " + r);
            }
        }

        txtReason.text = sb.ToString();
    }
    public void ShowReason(QuestionAsset q)
    {
        ShowReason(q != null && q.IsLegal, q != null ? q.reasons : null);
    }

    public void UpdateScore(int score)
    {
        if (txtScore) txtScore.text = $"Điểm: {score}";
        // Nếu bạn muốn gộp vào Tiến độ, có thể làm:
        // if (txtScore == null && txtProgress != null) txtProgress.text += $"\nĐiểm: {score}";
    }
    public void SetButtonsInteractable(bool state)
    {
        if (btnLegal) btnLegal.interactable = state;
        if (btnScam) btnScam.interactable = state;
    }
    public void ShowGameWin()
    {
        if (txtQuestion) txtQuestion.text = "Chúc mừng! Bạn đã hoàn thành tất cả tình huống.";
        if (txtReason) txtReason.text = "";
    }
    public void ShowGameOver()
    {
        if (txtQuestion) txtQuestion.text = "Trò chơi kết thúc! Bạn đã trả lời sai quá nhiều.";
        if (txtReason) txtReason.text = "";
    }    
}

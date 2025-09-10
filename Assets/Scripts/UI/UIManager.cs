using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class UIManager : MonoBehaviour
{
    [Header("Text")]
    public Text titleText;        // tiêu đề câu hỏi
    public Text descriptionText;  // nội dung câu hỏi
    public Text progressText;     // 3/10
    public Text reasonText;       // bullet lý do sau khi trả lời

    [Header("Buttons")]
    public Button legalButton;    // nút HỢP PHÁP
    public Button scamButton;     // nút LỪA ĐẢO

    // Lên câu mới
    public void ShowQuestion(QuestionAsset q)
    {
        if (!q) return;

        if (titleText) titleText.text = q.title ?? "";
        if (descriptionText) descriptionText.text = q.description ?? "";

        if (reasonText) reasonText.text = ""; // clear lý do cũ

        SetButtonsInteractable(true);
    }

    // Sau khi người chơi trả lời: hiện Reason
    public void ShowReason(QuestionAsset q)
    {
        if (!q || !reasonText) return;

        var sb = new StringBuilder();
        if (q.reasons != null)
        {
            foreach (var line in q.reasons)
            {
                if (!string.IsNullOrWhiteSpace(line))
                    sb.AppendLine("• " + line);
            }
        }
        reasonText.text = sb.ToString();
    }

    public void UpdateProgress(int current, int total)
    {
        if (progressText) progressText.text = $"{current}/{total}";
    }

    public void SetButtonsInteractable(bool value)
    {
        if (legalButton) legalButton.interactable = value;
        if (scamButton) scamButton.interactable = value;
    }
}

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Quiz/Question Asset", fileName = "Q_Asset")]
public class QuestionAsset : ScriptableObject
{
    public enum LegalLabel { Legal, Scam }

    [Header("Nội dung")]
    [TextArea(2, 4)] public string title;
    [TextArea(3, 8)] public string description;

    [Header("Phân loại hiển thị")]
    public string category;                 // <-- THÊM: Nhóm nội dung (Email/SMS/Website...)

    [Header("Nhãn hợp pháp / lừa đảo")]
    public LegalLabel label = LegalLabel.Scam;

    [Header("Lý do (bullet)")]
    [TextArea(2, 4)] public List<string> reasons = new List<string>();

    // Helper
    public bool IsLegal => label == LegalLabel.Legal;
    public string LabelText => IsLegal ? "HỢP PHÁP" : "LỪA ĐẢO";
}

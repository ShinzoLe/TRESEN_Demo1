using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum LegalLabel
{
    [InspectorName("Hợp pháp")] HopPhap,
    [InspectorName("Lừa đảo")] LuaDao
}

[CreateAssetMenu(fileName = "Q_", menuName = "Quiz/Question (single)")]
public class QuestionAsset : ScriptableObject
{
    [TextArea(2, 6)] public string questionText;

    // Dữ liệu cũ (bool) – chỉ dùng 1 lần để migrate
    [FormerlySerializedAs("correctAnswer")]
    [SerializeField, HideInInspector] private bool _isLegalLegacy = true;

    [Header("Nhãn tình huống")]
    public LegalLabel label = LegalLabel.HopPhap;   // Dropdown: Hợp pháp / Lừa đảo

    [TextArea(2, 8)] public string[] reasons;       // Bảng lí do
    public string category;                          // Loại (tuỳ chọn)

    // Helper cho code khác dùng
    public bool IsLegal => label == LegalLabel.HopPhap;
    public string LabelText => label == LegalLabel.HopPhap ? "HỢP PHÁP" : "LỪA ĐẢO";

    // Cờ để chỉ migrate 1 lần, tránh ghi đè lựa chọn của bạn sau này
    [SerializeField, HideInInspector] private bool _migrated = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!_migrated)
        {
            label = _isLegalLegacy ? LegalLabel.HopPhap : LegalLabel.LuaDao;
            _migrated = true;
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}

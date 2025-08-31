using System;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

public enum LegalLabel
{
    [InspectorName("Hợp pháp")] HopPhap,
    [InspectorName("Lừa đảo")] LuaDao
}

[Serializable]
public class QuestionAsset
{
    // ---- Nội dung chính ----
    [TextArea(2, 6)] public string questionText;
    public LegalLabel label = LegalLabel.HopPhap;              // Trạng thái chuẩn
    [TextArea(2, 8)] public string[] reasons = Array.Empty<string>();
    public string category;

    // ---- Helper (không ghi vào XML) ----
    [XmlIgnore] public bool IsLegal => label == LegalLabel.HopPhap;
    [XmlIgnore] public string LabelText => IsLegal ? "HỢP PHÁP" : "LỪA ĐẢO";

    // Cho code cũ nếu còn dùng: true = Hợp pháp, false = Lừa đảo (không serialize)
    [XmlIgnore]
    public bool correctAnswer
    {
        get => IsLegal;
        set => label = value ? LegalLabel.HopPhap : LegalLabel.LuaDao;
    }

    // ---- Dữ liệu legacy cho Unity serialization cũ ----
    [FormerlySerializedAs("correctAnswer")]
    [SerializeField, HideInInspector] private bool _isLegalLegacy = true;

    // ---- Cầu nối cho XML cũ: <correctAnswer>true/false</correctAnswer> ----
    // Được dùng khi DESERIALIZE file cũ; khi SERIALIZE file mới sẽ KHÔNG ghi ra.
    [XmlElement("correctAnswer")]
    public bool LegacyCorrectAnswerXml
    {
        get => correctAnswer;                // nếu buộc phải ghi, vẫn nhất quán
        set { _isLegalLegacy = value; _hasLegacyFromXml = true; }
    }
    public bool ShouldSerializeLegacyCorrectAnswerXml() => false; // không ghi ra XML mới

    [XmlIgnore] private bool _hasLegacyFromXml = false;
    [XmlIgnore] private bool _migrated = false;

    // Gọi sau khi đọc dữ liệu (XML cũ hoặc Unity serialize cũ)
    public void MigrateIfNeeded()
    {
        if (_migrated) return;

        // Nếu đã có enum sẵn thì thôi; nếu đến từ dữ liệu bool cũ thì map sang enum
        label = _isLegalLegacy ? LegalLabel.HopPhap : LegalLabel.LuaDao;

        _migrated = true;
    }
}

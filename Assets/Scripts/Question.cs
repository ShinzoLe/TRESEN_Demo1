using System;
using UnityEngine;

[Serializable]
public class Question
{
    [TextArea(2, 5)] public string questionText;   // Nội dung câu hỏi
    public bool correctAnswer;                     // Đáp án đúng: true = Hợp pháp, false = Lừa đảo
    [TextArea(2, 6)] public string[] reasons;      // Giải thích / bảng lý do
}

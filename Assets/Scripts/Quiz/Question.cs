using UnityEngine;

// Legacy simplified question (kept for compatibility if something still references it)
[System.Serializable]
public class Question
{
    [TextArea(2, 4)] public string text;
    public bool correctAnswer; // true = Legal, false = Scam
}

using UnityEngine;
using System.Collections.Generic;

// Legacy container for old pipeline. Prefer using QuestionAsset instead.
[CreateAssetMenu(menuName = "Quiz/Question Bank (Legacy)", fileName = "Q_Bank")]
public class QuestionBank : ScriptableObject
{
    public List<Question> questions = new List<Question>();
}

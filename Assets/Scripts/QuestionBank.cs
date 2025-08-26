using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionBank", menuName = "Quiz/QuestionBank")]
public class QuestionBank : ScriptableObject
{
    public List<Question> questions = new List<Question>();
}

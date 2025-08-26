using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private int totalRounds = 60;   // số câu muốn chơi

    private List<QuestionAsset> _deck;
    private int _index = -1;

    public int CurrentIndex => _index + 1;
    public int TotalRounds => Mathf.Min(totalRounds, _deck?.Count ?? 0);

    void Awake()
    {
        // Load toàn bộ asset QuestionAsset trong Resources/Questions
        var all = Resources.LoadAll<QuestionAsset>("Questions");
        if (all == null || all.Length == 0)
        {
            Debug.LogError("Không tìm thấy câu hỏi nào trong Resources/Questions");
            _deck = new List<QuestionAsset>();
            return;
        }

        // Trộn + rút đúng số lượng cần
        _deck = all.OrderBy(_ => Random.value).Take(totalRounds).ToList();
        _index = -1;
    }

    public bool HasNext() => (_index + 1) < (_deck?.Count ?? 0);

    public QuestionAsset NextQuestion()
    {
        if (!HasNext()) return null;
        _index++;
        return _deck[_index];
    }
}

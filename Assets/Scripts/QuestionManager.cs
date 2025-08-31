using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;


public class QuestionManager : MonoBehaviour
{
    [SerializeField] private int totalRounds = 60;   // số câu muốn chơi

    private List<QuestionAsset> _deck;
    private int _index = -1;

    public int CurrentIndex => _index + 1;
    public int TotalRounds => Mathf.Min(totalRounds, _deck?.Count ?? 0);

    private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(Data));

    void Awake()
    {
        _deck = new List<QuestionAsset>();

        // Load tất cả file XML như TextAsset trong Assets/Resources/Questions
        TextAsset[] files = Resources.LoadAll<TextAsset>("Questions");
        if (files == null || files.Length == 0)
        {
            Debug.LogError("Không tìm thấy file XML nào trong Resources/Questions");
            return;
        }

        foreach (var ta in files)
        {
            try
            {
                using (var reader = new StringReader(ta.text))
                {
                    var data = (Data)_serializer.Deserialize(reader);
                    if (data?.Questions != null)
                    {
                        foreach (var q in data.Questions)
                        {
                            q?.MigrateIfNeeded(); // nếu có logic migrate bool -> enum
                            _deck.Add(q);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Lỗi đọc XML '{ta.name}': {ex.Message}");
            }
        }

        // Trộn bộ câu hỏi (Fisher–Yates) và cắt đúng số lượng cần
        for (int i = 0; i < _deck.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, _deck.Count);
            (_deck[i], _deck[j]) = (_deck[j], _deck[i]);
        }
        if (_deck.Count > totalRounds) _deck = _deck.Take(totalRounds).ToList();

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

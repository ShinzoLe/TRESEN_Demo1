using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestionManager : MonoBehaviour
{
    [Header("Nguồn dữ liệu (Resources/Questions)")]
    public string resourcesFolder = "Questions";

    [Header("Số câu cho 1 ván")]
    public int totalRounds = 20;

    private readonly List<QuestionAsset> deck = new List<QuestionAsset>();
    private int index = -1;

    public int TotalRounds => deck.Count;
    public int CurrentIndex => index + 1;

    void Awake()
    {
        BuildDeck();
    }

    public void BuildDeck()
    {
        deck.Clear();
        var all = Resources.LoadAll<QuestionAsset>(resourcesFolder);
        var list = new List<QuestionAsset>(all);
        // Shuffle
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        if (totalRounds > 0) list = list.Take(totalRounds).ToList();
        deck.AddRange(list);
        index = -1;
    }

    public QuestionAsset GetNext()
    {
        index++;
        if (index < 0 || index >= deck.Count) return null;
        return deck[index];
    }
}

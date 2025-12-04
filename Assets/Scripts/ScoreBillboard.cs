using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreBillboard : MonoBehaviour
{
    [SerializeField] private int max_scores_printed = 10;
    [SerializeField] private TextMeshProUGUI player_text, score_text;
    void Start()
    {
        UpdateScores();
    }


    public void UpdateScores() 
    {
        Score[] scores = ScoresFileController.LoadScores();

        List<Score> scores_list = new List<Score>(scores);
        scores_list = new List<Score>(scores_list.OrderByDescending(x => x.score).ToArray());
        //scores = scores_list.ToArray();

        if(scores_list.Count > max_scores_printed) {
            RemoveAllBelow(scores_list, max_scores_printed);
            ScoresFileController.SaveScores(scores_list.ToArray());
        }
        scores = scores_list.ToArray();
        int len = max_scores_printed < scores.Length ? max_scores_printed : scores.Length;
        player_text.text = score_text.text = string.Empty;
        for(int i = 0;i < len; i++) {
            player_text.text += $"{scores[i].player_name}:\n";
            score_text.text += $"{scores[i].score}\n";
        }
    }

    private void RemoveAllBelow(List<Score> list, int max) {
        int tmp = list.Count;
        int diff = tmp - max;
        for(int i = 0; i < diff; i++) {
            list.RemoveAt(tmp-diff);
        }
    }
}

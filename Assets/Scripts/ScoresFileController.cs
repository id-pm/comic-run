
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoresFileController
{
    private const string string_name = "scoreboard";    

    public static void SaveScores(Score[] scores) {
        string jsonString = JsonHelper.ToJson(scores);
        PlayerPrefs.SetString(string_name, jsonString);
        PlayerPrefs.Save();
    }
    public static void SaveScore(int score, string player_name) {
        Score[] scores_array = LoadScores();

        List<Score> scores;
        if(scores_array == null) {
            scores = new List<Score>();
        }else {
            scores = new List<Score>(LoadScores());
        }

        scores.Add(new Score(score, player_name));        

        string jsonString = JsonHelper.ToJson(scores.ToArray());

        PlayerPrefs.SetString(string_name, jsonString);
        PlayerPrefs.Save();
        
    }

    public static Score[] LoadScores() {
        string json = PlayerPrefs.GetString(string_name);
        Debug.Log(json);
        if(json == string.Empty) {
            return null;
        }
        return JsonHelper.FromJson<Score>(json);
    } 

    

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}

[Serializable] public class Score {
        public int score;
        public string player_name;
        public Score(int score, string player_name) {
            this.score = score;
            this.player_name = player_name;
        }
    }
